using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplication
{
    public static class HelperMethods
    {
        public static TrafficUnitType GetBytesConvertionTargetUnit(List<double> allBytesValues)
        {
            var trafficInMBytes = allBytesValues.Count(_ => _ > 1048576);
            var trafficInKBytes = allBytesValues.Count(_ => _ > 1024 && _ < 1048576);
            var trafficInBytes = allBytesValues.Count(_ => _ < 1024);

            var maxCount = new List<int>(){trafficInBytes,trafficInKBytes,trafficInMBytes}.Max();

            if (maxCount == trafficInBytes) return TrafficUnitType.Bytes;
            else if (maxCount == trafficInKBytes) return TrafficUnitType.KiloBytes;
            else if (maxCount == trafficInMBytes) return TrafficUnitType.MegaBytes;
            else return TrafficUnitType.Bytes;
        }

        public static Tuple<double,string> ConvertBytes(double bytes, TrafficUnitType trafficUnitType)
        {
            string unit = "Bytes";
            double convertedBytes = bytes;
            switch (trafficUnitType)
            {
                case TrafficUnitType.KiloBytes : 
                {
                    convertedBytes = bytes / 1024;
                    unit = "KB";
                }
                break;
                case TrafficUnitType.MegaBytes : 
                {
                   convertedBytes = bytes / 1048576;
                unit = "MB";
                }
                break;
            }
            return new Tuple<double,string>(convertedBytes,unit);
        }

        public static string ConvertTime(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time .ToString(@"hh\:mm\:ss");
        }

        public static void FileChecker(string filePath)
        {
            if (!File.Exists(filePath)) File.Create(filePath);
        }

    }
}