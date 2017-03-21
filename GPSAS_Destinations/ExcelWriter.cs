using System;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace GPSAS_Destinations
{
    public class ExcelWriter
    {
        private String path {get; set;}
        private DataPoint[] arrData { get; set; }
        private GPSAS_DestinationsForm instance { get; set; }
        private Dictionary<int, Double> clusterTimes { get; set; }
        private Dictionary<int, Double> areaTimes { get; set; }
        private _Application app = null;
        private _Workbook workBook = null;
        private _Worksheet workSheet = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="_path">Working directory to save report.</param>
        /// <param name="_arrData">Array of cluster data.</param>
        /// <param name="_numClusters">Number of clusters.</param>
        /// <param name="_instance">Instance of parent form.</param>
        public ExcelWriter(String _path, DataPoint[] _arrData, Dictionary<int, Double>  _clusterTimes, Dictionary<int, Double> _areaTimes, GPSAS_DestinationsForm _instance)
        {
            this.path = _path;
            this.arrData = _arrData;
            this.instance = _instance;
            this.clusterTimes = _clusterTimes;
            this.areaTimes = _areaTimes;
            this.app = new Application();
            this.workBook = workBook = app.Workbooks.Add(System.Reflection.Missing.Value);

            // Generate report
            generateReport();

            // Select sheet and save workbook
            workBook.Application.ActiveWorkbook.Sheets[1].Select();
            workBook.SaveAs(this.path, XlFileFormat.xlWorkbookDefault);

            // Release memory, close writer
            ReleaseObject(workBook);
            ReleaseObject(workSheet);
            app.Quit();
            ReleaseObject(app);
        }

        /// <summary>
        /// Generates the content of the excel report.
        /// </summary>
        private void generateReport()
        {
            workSheet = (Worksheet)workBook.Worksheets.get_Item(1);
            workSheet.Select();

            // Label column names
            writeColumnNames();

            int rowCount = 2;
            int counter = 1;

            foreach(var kvp in clusterTimes)
            {
                if (clusterTimes.Count == 0)
                    instance.UpdateProgressBarParse(100);
                else
                {
                    instance.UpdateProgressBarParse((int)Math.Round(((Double)counter / (Double)clusterTimes.Count) * 100));
                    // Itterate through dataset looking for datapoints with the matching cluster ID
                    for (int dataPointIndex = 0; dataPointIndex < arrData.Length; dataPointIndex++)
                    {
                        DataPoint dataPoint = arrData[dataPointIndex];
                        if (dataPoint.IID == kvp.Key)
                        {
                            workSheet.Cells[rowCount, 1].value = dataPoint.AID.ToString();
                            workSheet.Cells[rowCount, 2].value = kvp.Key.ToString();
                            workSheet.Cells[rowCount, 3].value = dataPoint.ID.ToString();
                            workSheet.Cells[rowCount, 4].value = dataPoint.LAT.ToString();
                            workSheet.Cells[rowCount, 5].value = dataPoint.LON.ToString();
                            workSheet.Cells[rowCount, 6].value = dataPoint.SETTING.ToString();
                            workSheet.Cells[rowCount, 7].value = dataPoint.DATETIME.ToString();
                            workSheet.Cells[rowCount, 8].value = kvp.Value.ToString();
                            workSheet.Cells[rowCount, 9].value = areaTimes[dataPoint.AID].ToString();
                            rowCount++;
                        }
                    }
                    counter++;
                }
            }
        }

        /// <summary>
        /// Writes the column names.
        /// </summary>
        private void writeColumnNames()
        {
            workSheet.Cells[1, 1].value = "AID";
            workSheet.Cells[1, 2].value = "CID";
            workSheet.Cells[1, 3].value = "ID";
            workSheet.Cells[1, 4].value = "LAT";
            workSheet.Cells[1, 5].value = "LON";
            workSheet.Cells[1, 6].value = "Setting";
            workSheet.Cells[1, 7].value = "DateTime";
            workSheet.Cells[1, 8].value = "InstanceTime";
            workSheet.Cells[1, 9].value = "AreaTime";
        }

        /// <summary>
        /// Helper class for disposing objects.
        /// </summary>
        /// <param name="obj">Object to be disposed.</param>
        internal static void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                }
            }
            catch { }
            finally
            {
                GC.Collect();
            }
        }
    }
}
