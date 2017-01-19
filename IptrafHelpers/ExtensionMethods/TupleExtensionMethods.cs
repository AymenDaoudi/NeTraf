using System;

namespace ConsoleApplication
{
    public static class TupleExtensionMethods
    {
        public static string Print(this Tuple<double, double, double> tuple)
        {
            double rate = tuple.Item3;
            string unit = "Bytes/s";
            if (tuple.Item3 > 1048576)
            {
                rate = tuple.Item3 / 1048576;
                unit = "MB/s";
            }
            else if (tuple.Item3 > 1024)
            {
                rate = tuple.Item3 / 1024;
                unit = "KB/s";
            }
            return $" Packets : {tuple.Item1} , Bytes : {tuple.Item2} , Rate : {rate} {unit} .";
        }
    }
}