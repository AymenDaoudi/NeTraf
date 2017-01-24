using System;
using static ConsoleApplication.HelperMethods;

namespace ConsoleApplication
{
    public static class ExtensionMethods
    {
        public static string Print(this Tuple<double, double, double> tuple, TrafficUnitType bytesUnitType, TrafficUnitType rateUnitType, bool dataOnly)
        {
            var convertedBytes = ConvertBytes(tuple.Item2,bytesUnitType);
            var convertedRate = ConvertBytes(tuple.Item3,rateUnitType);
            if (dataOnly)
            {
                return $" {tuple.Item1},{String.Format("{0:0.00}", convertedBytes.Item1)},{String.Format("{0:0.00}", convertedRate.Item1)}";
            }
            return $" Packets : {tuple.Item1} packets,"+ 
                   $" Bytes : {String.Format("{0:0.00}", convertedBytes.Item1)} {convertedBytes.Item2} ," +
                   $" Rate : {String.Format("{0:0.00}", convertedRate.Item1)} {convertedRate.Item2}/s .";
        }
    }
}