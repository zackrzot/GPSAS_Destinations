using System;
using System.Collections.Generic;
using System.IO;
using Excel;
using System.Data;
using System.Globalization;

namespace GPSAS_Destinations
{
    public static class ExcelManager
    {
        private static int dateTimeRow { get; set; }
        private static int latitudeRow { get; set; }
        private static int longitudeRow { get; set; }
        private static int settingRow { get; set; }
        private static int idRow { get; set; }

        private static String dateTimeString = "DateTimeS";
        private static String latitudeString = "Latitude";
        private static String longitudeString = "Longitude";
        private static String settingString = "setting";
        private static String idString = "id";

        public class ExcelParseExceptin : Exception { }

        /// <summary>
        /// Reads the provided excel file into a data set.
        /// </summary>
        /// <param name="parentInstance">Instance of parent form</param>
        /// <param name="fileName">File name to be parsed.</param>
        public static void ReadData(GPSAS_DestinationsForm parentInstance, String fileName)
        {
            parentInstance.SetStatusText("Reading file");
            // Create file stream
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            // Create excel reader instance
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            // Column names ARE in first row, but they are needed to determine column names
            excelReader.IsFirstRowAsColumnNames = false;
            // Assign data to DataSet
            DataSet dataSet = excelReader.AsDataSet();
            // Clean up
            excelReader.Close();
            stream.Close();
            // Parse data
            parseDataSet(dataSet);
        }

        /// <summary>
        /// Assigns the column numbers of the required items. Throws ExcelParseExceptin if unable to identify all names.
        /// </summary>
        /// <param name="dataRow">Data row that contains column names.</param>
        private static void assignColumnNumbers(DataRow dataRow)
        {
            dateTimeRow = latitudeRow = longitudeRow = settingRow = idRow  = -1;
            for (int col = 0; col < 50; col++)
            {
                try
                {
                    if (dataRow.ItemArray[col].ToString().ToLower() == dateTimeString.ToLower())
                        dateTimeRow = col;
                    if (dataRow.ItemArray[col].ToString().ToLower() == latitudeString.ToLower())
                        latitudeRow = col;
                    if (dataRow.ItemArray[col].ToString().ToLower() == longitudeString.ToLower())
                        longitudeRow = col;
                    if (dataRow.ItemArray[col].ToString().ToLower() == settingString.ToLower())
                        settingRow = col;
                    if (dataRow.ItemArray[col].ToString().ToLower() == idString.ToLower())
                        idRow = col;
                }
                catch { break; }
            }
            Logger.Log("dateTimeRow col was: " + dateTimeRow.ToString());
            Logger.Log("latitudeRow col was: " + latitudeRow.ToString());
            Logger.Log("longitudeRow col was: " + longitudeRow.ToString());
            Logger.Log("settingRow col was: " + settingRow.ToString());
            Logger.Log("idRow col was: " + idRow.ToString());
            if ((dateTimeRow == -1) ||
                (latitudeRow == -1) ||
                (longitudeRow == -1) ||
                (settingRow == -1) ||
                (idRow == -1))
                throw new ExcelParseExceptin();
        }

        /// <summary>
        /// Parses the data set.
        /// </summary>
        /// <param name="dataSet"></param>
        private static void parseDataSet(DataSet dataSet)
        {
            int c = 1;
            var fmt = new NumberFormatInfo();
            fmt.NegativeSign = "-";
            fmt.NumberDecimalSeparator = ".";
            Boolean firstrow = true;
            // Clear ClusterComputer data point list
            ClusterComputer.DataPoints = new List<DataPoint>();
            // Parse data set entries
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                if (firstrow)
                {
                    assignColumnNumbers(dataRow);
                    firstrow = false;
                    continue;
                }
                String id = null;
                try
                {
                    id = dataRow.ItemArray[idRow].ToString();
                    String latStr = dataRow.ItemArray[latitudeRow].ToString().Replace("−", "-");
                    Double lat = Double.Parse(latStr, fmt);
                    String lonStr = dataRow.ItemArray[longitudeRow].ToString().Replace("−", "-");
                    Double lon = Double.Parse(lonStr, fmt);
                    String setting = dataRow.ItemArray[settingRow].ToString();
                    DateTime dateTime = DateTime.Parse(dataRow.ItemArray[dateTimeRow].ToString());
                    // Create new data point
                    DataPoint dataItem = new DataPoint(id, lat, lon, setting, dateTime);
                    // Append datapoint to set
                    ClusterComputer.DataPoints.Add(dataItem);
                }
                catch (Exception ex)
                {
                    Logger.Log("Unable to parse row in dataset. Error: " + ex.ToString() + " Row: " + c.ToString());
                }
                if(String.IsNullOrEmpty(id))
                    c++;
                if(c > 10)
                {
                    Logger.Log("EOF detected.");
                    return;
                }
                
            }
        }

    }
}
