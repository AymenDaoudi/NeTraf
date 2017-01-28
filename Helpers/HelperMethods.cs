using System;
using System.Collections.Generic;
using System.Linq;

namespace NeTraf
{
    public static class HelperMethods
    {
        public static TrafficUnitType GetBytesConvertionTargetUnit(List<TrafficDataRowSet> trafficDataRowSets, 
                                                                   TrafficDataType trafficDataType, 
                                                                   TrafficMeasurementType trafficMeasurementType)
        {
            List<double> allBytesValues ;

            switch (trafficMeasurementType)
            {
                case TrafficMeasurementType.Bytes :
                {
                    switch (trafficDataType)
                    {
                        case TrafficDataType.TotalData    : allBytesValues = trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item2).ToList();    break;
                        case TrafficDataType.IncomingData : allBytesValues = trafficDataRowSets.Select(_ => _.TotalIncomingTrafficData.Item2).ToList(); break;
                        case TrafficDataType.OutgoingData : allBytesValues = trafficDataRowSets.Select(_ => _.TotalOutgoingTrafficData.Item2).ToList(); break;
                        default : allBytesValues = trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item2).ToList();    break;
                    }
                }
                break;
                case TrafficMeasurementType.Rate  :
                {
                    switch (trafficDataType)
                    {
                        case TrafficDataType.TotalData    : allBytesValues = trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item3).ToList();    break;
                        case TrafficDataType.IncomingData : allBytesValues = trafficDataRowSets.Select(_ => _.TotalIncomingTrafficData.Item3).ToList(); break;
                        case TrafficDataType.OutgoingData : allBytesValues = trafficDataRowSets.Select(_ => _.TotalOutgoingTrafficData.Item3).ToList(); break;
                        default : allBytesValues = trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item3).ToList();    break;
                    }
                }
                break;
                default:
                {
                    switch (trafficDataType)
                    {
                        case TrafficDataType.TotalData    : allBytesValues = trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item2).ToList();    break;
                        case TrafficDataType.IncomingData : allBytesValues = trafficDataRowSets.Select(_ => _.TotalIncomingTrafficData.Item2).ToList(); break;
                        case TrafficDataType.OutgoingData : allBytesValues = trafficDataRowSets.Select(_ => _.TotalOutgoingTrafficData.Item2).ToList(); break;
                        default : allBytesValues = trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item2).ToList();    break;
                    }
                }
                break;
            }

            var trafficInMBytes = allBytesValues.Count(_ => _ > 1048576);
            var trafficInKBytes = allBytesValues.Count(_ => _ > 1024 && _ < 1048576);
            var trafficInBytes  = allBytesValues.Count(_ => _ < 1024);

            var maxCount = new List<int>(){trafficInBytes,trafficInKBytes,trafficInMBytes}.Max();

            if (maxCount == trafficInBytes) return TrafficUnitType.Bytes;
            else if (maxCount == trafficInKBytes) return TrafficUnitType.KiloBytes;
            else if (maxCount == trafficInMBytes) return TrafficUnitType.MegaBytes;
            else return TrafficUnitType.Bytes;
        }

        public static Tuple<double,string> ConvertBytes(double bytes, TrafficUnitType trafficUnitType)
        {
            var unit = "Bytes";
            var convertedBytes = bytes;
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
            var timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}