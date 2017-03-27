using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SharpKml.Dom;
using SharpKml.Base;
using SharpKml.Engine;

namespace GPSAS_Destinations
{
    public class KMLWriter
    {
        public static Boolean kmlEnabled { get; set; }
        public static decimal minAreaTime { get; set; }

        public KMLWriter(DataPoint[] arrData, String dir, String fileName, Dictionary<int, Double> areaTimes)
        {
            if (!kmlEnabled)
            {
                Logger.Log("KML is disabled. Not generating KML file.");
                return;
            }

            Logger.Log("Generating KML for file: " + fileName);

            try
            {
                gererateKML(arrData, dir, fileName, areaTimes);
            }
            catch(Exception ex)
            {
                Logger.Log("Unable to generate KML File.");
                Logger.Log(ex.ToString());
                Logger.Log(ex.StackTrace.ToString());
            }
        }

        private void gererateKML(DataPoint[] arrData, String dir, String fileName, Dictionary<int, Double> areaTimes)
        {
            Document doc = new Document();

            Dictionary<int, List<Placemark>> placemarks = new Dictionary<int, List<Placemark>>();

            for (int i = 0; i < arrData.Length; i++)
            {
                DataPoint dataPoint = arrData[i];

                double areaTime;

                // Check if point meets min area time threshold, and check that it is not noise
                if ((areaTimes.TryGetValue(dataPoint.AID, out areaTime)) && (areaTime >= (double) minAreaTime) && (dataPoint.AID != ClusterComputer.NOISE))
                {
                    Point point = new Point();
                    point.Coordinate = new Vector(dataPoint.LAT, dataPoint.LON);

                    Placemark placemark = new Placemark();
                    placemark.Geometry = point;
                    placemark.Name = String.Format("{0} - {1}", dataPoint.AID.ToString(), dataPoint.IID.ToString());

                    doc.AddFeature(placemark);
                }
            }

            KmlFile file = KmlFile.Create(doc, true);

            String fName;

            fName = String.Format(@"\{0}_MinAreaTime_{1}.kml", fileName, minAreaTime.ToString());

            FileStream outStream = new FileStream(dir + fName, FileMode.Create, FileAccess.ReadWrite);

            file.Save(outStream);

            outStream.Close();
        }
    }
}
