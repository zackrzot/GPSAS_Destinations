using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GPSAS_Destinations
{
    public static class Util
    {
        public static void AddAll<T>(this Stack<T> stack1, Stack<T> stack2)
        {
            T[] arr = new T[stack2.Count];
            stack2.CopyTo(arr, 0);

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                stack1.Push(arr[i]);
            }
        }
    }

    public static class ClusterComputer
    {
        #region ClusterComputer Public Variables

        public static int NOISE = 999999;                                   // Int to represent datapoint is considered noise
        public static int UNMARKED = 99999;                                 // Int to represent unmarked data point
        public static Double DELTA_DIST_THRESHOLD { get; set; }             // Maximum radius of a center data point to determine a cluster
        public static Double DELTA_TIME_THRESHOLD { get; set; }             // Maximum time difference between two entries within a location cluster to determine the same instance 
        public static int MINPTS { get; set; }                              // Min points in a location to be considered a cluster
        public static Boolean HaversineOn = true;                   // Whether to use haversine or basic distance formula
        public static List<DataPoint> DataPoints = new List<DataPoint>();   // Dynamic list that temporarily stores the entries to be parsed

        #endregion

        #region ClusterComputer Private Variables

        private static DataPoint[] arrData;                                 // Array of data points to be used in parsing
        private static String workingDirectory = null;                      // Working directory to be used by the cluster computer
        private static GPSAS_DestinationsForm GPSAS_DestinationsFormInstance;             // Instance of parent win form
        private static int areaID = 0;                                      // Area ID counter
        private static int instanceID = 0;                                  // Instance ID counter
        private static List<int> nearestNeighborIndexes = new List<int>();  // List that stores nearest neighbor indexes of neighbors

        #endregion

        #region ClusterComputer Main Computer

        public static void Start(GPSAS_DestinationsForm _instance, String _workingDirectory, String _fileName)
        {
            // Assign parent form instance
            GPSAS_DestinationsFormInstance = _instance;

            // Set working directory
            workingDirectory = _workingDirectory;

            // Load data point list into an array
            arrData = DataPoints.ToArray();
            Logger.Log("Number of valid data points: " + arrData.Length.ToString());

            // Displose of datapoint list
            DataPoints.Clear();

            // Log parameters
            logParameters();

            // Run main computer function
            try
            {
                computeClusters();
            }
            catch (Exception ex)
            {
                Logger.Log("Unable to compute clusters for file: " + _fileName);
                Logger.Log(ex.ToString());
                if (ex is OutOfMemoryException)
                    MessageBox.Show("OutOfMemoryException. Computations are too large for this machine.", "Failure");
                else if (ex is OverflowException)
                    MessageBox.Show("OverflowException. Bounds of types have been exceeded. Parameters must be reduced to compute.", "Failure");
                else
                    MessageBox.Show("Parse failed.", "Failure");
                return;
            }

            // Identify instance cluster times
            Dictionary<int, Double> clusterTimes = identifyClustersTimes();

            // Identify total times in area clusters
            Dictionary<int, Double> areaTimes = identifyAreaTimes(clusterTimes);

            // Write the results
            writeResults(_fileName, clusterTimes, areaTimes);
        }

        private static void computeClusters()
        {
            // First, identify area clusters
            computeLocationClusters();

            // Finally, identify time instances within those locations
            computeTimeInstanceClusters();
        }

        private static void computeLocationClusters()
        {
            GPSAS_DestinationsFormInstance.SetStatusText("Clustering area");

            areaID = 0;

            // For each data point in the array of data
            for (int i = 0; i < arrData.Length; i++)
            {
                // Update progress bar
                GPSAS_DestinationsFormInstance.UpdateProgressBarParse((int)Math.Round(((Double)i / (Double)arrData.Length) * 100));

                // If the data point location has not been processed yet
                if (arrData[i].AID == UNMARKED)
                {
                    // Retrieve the indexes of the location based neighbors for this datapoint
                    List<int> neighborIndexes = retrieveAreaNeighbours(i);

                    // If the number of neighbors to this data point does not meet the num neighbors requirement, mark as NOISE
                    if (neighborIndexes.Count < MINPTS)
                        arrData[i].AID = NOISE;
                    else
                    {
                        // Increment cluster ID
                        areaID = areaID + 1;

                        // Mark each neighbor data point with the coresponding neighbor index
                        foreach (int index in neighborIndexes)
                            arrData[index].AID = areaID;
                    }
                }
            }
            Logger.Log("There are this many area clusters: " + areaID.ToString());
        }

        private static void computeTimeInstanceClusters()
        {
            GPSAS_DestinationsFormInstance.SetStatusText("Clustering time");

            instanceID = 0;

            // For each data point in the array of data
            for (int i = 0; i < arrData.Length; i++)
            {
                // Update progress bar
                GPSAS_DestinationsFormInstance.UpdateProgressBarParse((int)Math.Round(((Double)i / (Double)arrData.Length) * 100));

                // If the instance ID is ummarked
                if (arrData[i].IID == UNMARKED)
                {
                    // Increment instance ID
                    instanceID = instanceID + 1;

                    // Clear out nearest neighbor list
                    nearestNeighborIndexes = new List<int>();

                    // Recursively find nearest neighbors based on time at a location
                    recursiveRetrieveNearestNeighbors(i);

                    // Mark each of the found data points with that index
                    foreach (int index in nearestNeighborIndexes)
                        arrData[index].IID = instanceID;
                }
            }
            Logger.Log("There are this many time clusters: " + instanceID.ToString());
        }

        #endregion

        #region ClusterComputer Neighbor Methods

        private static List<int> retrieveAreaNeighbours(int n)
        {
            List<int> neighborIndexes = new List<int>();

            // For each point in the dataset
            for (int i = 0; i < arrData.Length; i++)
            {
                Double dDist = deltaDist(arrData[n], arrData[i]);

                if (dDist <= DELTA_DIST_THRESHOLD)
                    neighborIndexes.Add(i);
            }
            return neighborIndexes;
        }

        private static void recursiveRetrieveNearestNeighbors(int i)
        {
            List<int> neighborIndexes = new List<int>();
            // Return all sequences of nearest neighbor times
            neighborIndexes = recursiveNeighbors(i);
            // Base case - no more nearest neighbor data points found
            if (neighborIndexes.Count == 0)
                return;
            else
            {
                // Otherwise, identify new leads
                List<int> newLeads = new List<int>();

                // Only look for neighbors of unsearched indexes
                foreach (int index in neighborIndexes)
                    if (!nearestNeighborIndexes.Contains(index))
                        newLeads.Add(index);

                // Add to cumulative list
                nearestNeighborIndexes.AddRange(newLeads);

                // Recursively search each new lead
                foreach (int index in newLeads)
                    recursiveRetrieveNearestNeighbors(index);
            }
        }

        private static List<int> recursiveNeighbors(int n)
        {
            List<int> neighborIndexes = new List<int>();

            // For each point in the dataset
            for (int i = 0; i < arrData.Length; i++)
            {
                if (arrData[i].IID == UNMARKED && arrData[i].AID == arrData[n].AID)
                {
                    Double dTime = deltaTime(arrData[n], arrData[i]);

                    if (dTime <= (Double)DELTA_TIME_THRESHOLD)
                    {
                        neighborIndexes.Add(i);
                    }
                }
            }
            return neighborIndexes;
        }

        #endregion

        #region ClusterComputer Find Times

        private static Dictionary<int, Double> identifyClustersTimes()
        {
            GPSAS_DestinationsFormInstance.SetStatusText("Identifying clusters and timespans");

            Dictionary<int, Double> clusterTimes = new Dictionary<int, Double>();

            // For each possible cluster label
            for (int id = 0; id <= instanceID; id++)
            {
                if (instanceID == 0)
                    GPSAS_DestinationsFormInstance.UpdateProgressBarParse(100);
                else
                    GPSAS_DestinationsFormInstance.UpdateProgressBarParse((int)Math.Round(((Double)id / (Double)instanceID) * 100));

                // This will not work in the year 9000
                DateTime minTime = DateTime.Parse("9000 - 08 - 10T16: 16:50Z");
                DateTime maxTime = DateTime.Parse("1000 - 08 - 10T16: 16:50Z");
                int clusterPointCount = 0;

                // Itterate through dataset looking for datapoints with the matching cluster ID
                for (int dataPointIndex = 0; dataPointIndex < arrData.Length; dataPointIndex++)
                {
                    DataPoint dataPoint = arrData[dataPointIndex];
                    if (dataPoint.IID == id && dataPoint.AID != NOISE)
                    {
                        if (dataPoint.DATETIME > maxTime)
                            maxTime = dataPoint.DATETIME;
                        if (dataPoint.DATETIME < minTime)
                            minTime = dataPoint.DATETIME;
                        clusterPointCount++;
                    }
                }

                // Append at least 2 points of data
                if (clusterPointCount > 1)
                {
                    Double difTime = (Double)(maxTime - minTime).TotalMinutes;
                    clusterTimes.Add(id, (Double)(maxTime - minTime).TotalMinutes);
                }
            }

            return clusterTimes;
        }

        private static Dictionary<int, Double> identifyAreaTimes(Dictionary<int, Double> clusterTimes) {

            Dictionary<int, Double> areaTimes = new Dictionary<int, Double>();

            // For each possible area label
            for (int id = 0; id <= areaID; id++)
            {
                if (areaID == 0)
                    GPSAS_DestinationsFormInstance.UpdateProgressBarParse(100);
                else
                    GPSAS_DestinationsFormInstance.UpdateProgressBarParse((int)Math.Round(((Double)id / (Double)areaID) * 100));

                List<int> foundCIDs = new List<int>();

                Double totalTime = 0;

                // Itterate through dataset looking for datapoints with the matching cluster ID
                for (int dataPointIndex = 0; dataPointIndex < arrData.Length; dataPointIndex++)
                {
                    DataPoint dataPoint = arrData[dataPointIndex];
                    if (dataPoint.AID == id && !foundCIDs.Contains(dataPoint.IID))
                    {
                        try
                        {
                            totalTime = totalTime + clusterTimes[dataPoint.IID];
                        }
                        catch { }
                        foundCIDs.Add(dataPoint.IID);
                    }
                }
                areaTimes.Add(id, totalTime);
            }
            return areaTimes;
        }

        #endregion

        #region ClusterComputer Output

        private static void writeResults(String originalFileName, Dictionary<int, Double> clusterTimes, Dictionary<int, Double> areaTimes)
        {
            originalFileName = originalFileName.Replace(".xlsx", "").Replace(".xls", ""); ;

            KMLWriter kmlw = new KMLWriter(arrData, workingDirectory, originalFileName, areaTimes);

            GPSAS_DestinationsFormInstance.SetStatusText("Writing results");
            try
            {
                Random generator = new Random();
                String rid = generator.Next(0, 1000000).ToString("D6");
                String fileName = originalFileName + "_" +
                    DELTA_DIST_THRESHOLD.ToString() + "_" +
                    DELTA_TIME_THRESHOLD.ToString() + "_" +
                    MINPTS.ToString() + "_" + rid + ".xlsx";

                String fullPath = workingDirectory + "\\" + fileName;
                new ExcelWriter(fullPath, arrData, clusterTimes, areaTimes, GPSAS_DestinationsFormInstance);
                try
                {
                    Logger.Log(fullPath);
                    System.Diagnostics.Process.Start(fullPath);
                }
                catch (Exception ex) { Logger.Log(ex.ToString()); }
            }
            catch { MessageBox.Show("Unable to write data to excel file.", "Error"); }
        }

        #endregion

        #region ClusterComputer Utility

        private static Double toRad(Double val)
        {
            return (Math.PI / 180) * val;
        }

        public static Double deltaDist(DataPoint x, DataPoint y)
        {
            if (HaversineOn)
            {
                var R = 6371e3; // metres
                var phi1 = toRad(x.LAT);
                var phi2 = toRad(y.LAT);
                var deltaPhi = toRad(y.LAT - x.LAT);
                var deltaLambda = toRad(y.LON - x.LON);
                var a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                        Math.Cos(phi1) * Math.Cos(phi2) *
                        Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return (R * c);
            }
            else
                return Math.Sqrt(Math.Pow((x.LAT - y.LAT), 2) + Math.Pow((x.LON - y.LON), 2)) * 100000;
        }

        private static Double deltaTime(DataPoint x, DataPoint y)
        {
            return Math.Abs((x.DATETIME - y.DATETIME).TotalMinutes);
        }

        private static void logParameters()
        {
            Logger.Log("DELTA_DIST_THRESHOLD: " + DELTA_DIST_THRESHOLD.ToString());
            Logger.Log("DELTA_TIME_THRESHOLD: " + DELTA_TIME_THRESHOLD.ToString());
            Logger.Log("MINPTS: " + MINPTS.ToString());
            if (HaversineOn)
                Logger.Log("Haversine: ON");
            else
                Logger.Log("Distance Formula: ON");
        }

        #endregion
    }
}
