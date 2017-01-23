using System;

namespace ConsoleApplication
{
    public static class HelperMethods
    {
        public static Tuple<double,string> ConvertBytes(double bytes)
        {
            string unit = "Bytes";
            double convertedBytes = bytes;
            if (bytes > 1048576)
            {
                convertedBytes = bytes / 1048576;
                unit = "MB";
            }
            else if (bytes > 1024)
            {
                convertedBytes = bytes / 1024;
                unit = "KB";
            }

            return new Tuple<double,string>(convertedBytes,unit);
        }


        public static string ConvertTime(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time .ToString(@"hh\:mm\:ss");
        }

    }
}