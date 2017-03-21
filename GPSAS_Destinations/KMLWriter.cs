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


        public KMLWriter(DataPoint[] arrData, String dir, String fileName)
        {
            Logger.Log("Generating KML for file: " + fileName);

            try
            {
                gererateKML(arrData, dir, fileName, true);
                gererateKML(arrData, dir, fileName, false);
            }
            catch(Exception ex)
            {
                Logger.Log("Unable to generate KML File.");
                Logger.Log(ex.ToString());
                Logger.Log(ex.StackTrace.ToString());
            }
        }

        private void gererateKML(DataPoint[] arrData, String dir, String fileName, Boolean withNoise)
        {
            Document doc = new Document();

            Dictionary<int, List<Placemark>> placemarks = new Dictionary<int, List<Placemark>>();

            for (int i = 0; i < arrData.Length; i++)
            {
                Point point = new Point();
                DataPoint dataPoint = arrData[i];

                point.Coordinate = new Vector(dataPoint.LAT, dataPoint.LON);

                Placemark placemark = new Placemark();
                placemark.Geometry = point;
                if (dataPoint.AID == ClusterComputer.NOISE)
                    placemark.Name = "NOISE";
                else
                    placemark.Name = String.Format("{0} - {1}", dataPoint.AID.ToString(), dataPoint.IID.ToString());

                if(withNoise)
                    doc.AddFeature(placemark);
                else
                    if(dataPoint.AID != ClusterComputer.NOISE)
                        doc.AddFeature(placemark);

            }

            KmlFile file = KmlFile.Create(doc, true);

            String fName;
            if (withNoise)
                fName = String.Format(@"\{0}_With_Noise.kml", fileName);
            else
                fName = String.Format(@"\{0}.kml", fileName);

            FileStream outStream = new FileStream(dir + fName, FileMode.Create, FileAccess.ReadWrite);

            file.Save(outStream);

            outStream.Close();
        }


    }
}
