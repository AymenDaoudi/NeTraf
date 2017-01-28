using System;
using System.Collections.Generic;
using System.IO;
using static NeTraf.HelperMethods;

namespace NeTraf
{
    public static class ExtensionMethods
    {
        public static string ToSimpleString(this TrafficUnitType trafficUnitType)
        {
          switch(trafficUnitType)
          {
            case TrafficUnitType.Bytes     : return "Bytes";
            case TrafficUnitType.KiloBytes : return "KB";
            case TrafficUnitType.MegaBytes : return "MB";
            default: return "Bytes";
          }
        }
        public static string Print(this Tuple<double, double, double> tuple, TrafficUnitType bytesUnitType, TrafficUnitType rateUnitType, bool dataOnly)
        {
            Tuple<double,string> convertedBytes = ConvertBytes(tuple.Item2,bytesUnitType);
            Tuple<double,string> convertedRate = ConvertBytes(tuple.Item3,rateUnitType);
            if (dataOnly)
            {
                return $" {tuple.Item1},{String.Format("{0:0.00}", convertedBytes.Item1)},{String.Format("{0:0.00}", convertedRate.Item1)}";
            }
            return $" Packets : {tuple.Item1} packets,"+ 
                   $" Bytes : {String.Format("{0:0.00}", convertedBytes.Item1)} {convertedBytes.Item2} ," +
                   $" Rate : {String.Format("{0:0.00}", convertedRate.Item1)} {convertedRate.Item2}/s .";
        }

        public static IEnumerable<string> ReadAllLines(this StreamReader streamReader)
        {
            string line;
            while ((line = streamReader.ReadLine()) !=  null)
            {
                yield return line;
            }
        }
    }
}