using System;
using System.IO;
using System.Collections.Generic;

namespace GPSAS_Destinations
{
    public static class Logger
    {
        private static List<String> _logs = new List<String>();
        private static String _directory;
        private static Object _lockLogs = new Object();
        private static List<String> Logs
        {
            get
            {
                lock (_lockLogs)
                {
                    return _logs;
                }
            }
            set
            {
                lock (_lockLogs)
                {
                    _logs = value;
                }
            }
        }

        public static void Log(String text)
        {
            String dateTime = DateTime.Now.ToLongTimeString();
            Logs.Add(String.Format("{0}\t-\t{1}\n", dateTime, text));
        }

        public static void Initialize(String dir)
        {
            // Set output directory
            _directory = dir;
            // Clear previous
            Logs.Clear();
        }

        public static void Save()
        {
            String fullPath = _directory + "\\Log.txt";
            foreach (String log in Logs)
                File.AppendAllText(fullPath, log);
        }
    }
}
