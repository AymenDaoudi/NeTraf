using System;
using System.Collections.Generic;
using System.IO;
using static NeTraf.HelperMethods;
using static System.Console;

namespace NeTraf
{
    public static class TrafficDataPrinter
    {
        public static void PrintToOutputFile(List<TrafficDataRowSet> trafficDataRowSets, 
                                             string outputFilePath, 
                                             TrafficDataType trafficDataType, 
                                             TrafficMeasurementType trafficMeasurementType)
        {
            var stream = File.OpenWrite(outputFilePath);
            using (System.IO.StreamWriter file =  new System.IO.StreamWriter(stream))
            {
                var unit = GetBytesConvertionTargetUnit(trafficDataRowSets,trafficDataType,trafficMeasurementType);
                trafficDataRowSets.ForEach(trafficDataRowSet => 
                {
                    file.WriteLine(TrafficDataToOutput(trafficDataRowSet, 
                                                       trafficDataRowSet.CumulatedRunningTime, 
                                                       trafficDataType,
                                                       trafficMeasurementType, 
                                                       unit));
                });
            }
        }

        private static string TrafficDataToOutput(TrafficDataRowSet trafficDataRowSet, 
                                                  double cumulatedRunningTime, 
                                                  TrafficDataType trafficDataType,
                                                  TrafficMeasurementType trafficMeasurementType, 
                                                  TrafficUnitType trafficUnitType)
        {
            switch (trafficDataType)
            {
                case TrafficDataType.TotalData : 
                {
                    switch (trafficMeasurementType)
                    {
                        case TrafficMeasurementType.Packets : return $"{trafficDataRowSet.TotalTotalTrafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                        case TrafficMeasurementType.Bytes   : return $"{String.Format("{0:0.00}", ConvertBytes(trafficDataRowSet.TotalTotalTrafficData.Item2, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                        case TrafficMeasurementType.Rate    : return $"{String.Format("{0:0.00}", ConvertBytes(trafficDataRowSet.TotalTotalTrafficData.Item3, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                        default: return $"{trafficDataRowSet.TotalIncomingTrafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                    }
                }
                case TrafficDataType.IncomingData : 
                {
                    switch (trafficMeasurementType)
                    {
                        case TrafficMeasurementType.Packets : return $"{trafficDataRowSet.TotalIncomingTrafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                        case TrafficMeasurementType.Bytes   : return $"{String.Format("{0:0.00}", ConvertBytes(trafficDataRowSet.TotalIncomingTrafficData.Item2, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                        case TrafficMeasurementType.Rate    : return $"{String.Format("{0:0.00}", ConvertBytes(trafficDataRowSet.TotalIncomingTrafficData.Item3, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                        default: return $"{trafficDataRowSet.TotalIncomingTrafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                    }
                }
                case TrafficDataType.OutgoingData : 
                {
                    switch (trafficMeasurementType)
                    {
                        case TrafficMeasurementType.Packets : return $"{trafficDataRowSet.TotalOutgoingTrafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                        case TrafficMeasurementType.Bytes   : return $"{String.Format("{0:0.00}", ConvertBytes(trafficDataRowSet.TotalOutgoingTrafficData.Item2, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                        case TrafficMeasurementType.Rate    : return $"{String.Format("{0:0.00}", ConvertBytes(trafficDataRowSet.TotalOutgoingTrafficData.Item3, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                        default: return $"{trafficDataRowSet.TotalOutgoingTrafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                    }
                }
                default: 
                {
                    switch (trafficMeasurementType)
                    {
                        case TrafficMeasurementType.Packets : return $"{trafficDataRowSet.TotalTotalTrafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                        case TrafficMeasurementType.Bytes   : return $"{String.Format("{0:0.00}", ConvertBytes(trafficDataRowSet.TotalTotalTrafficData.Item2, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                        case TrafficMeasurementType.Rate    : return $"{String.Format("{0:0.00}", ConvertBytes(trafficDataRowSet.TotalTotalTrafficData.Item3, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                        default: return $"{trafficDataRowSet.TotalTotalTrafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                    }
                }
            }
        }

        public static void PrintToConsole(List<TrafficDataRowSet> trafficDataRowSets)
        {
            var bytesUnit = TrafficUnitType.Bytes;
            var rateUnit  = TrafficUnitType.Bytes; 

            trafficDataRowSets.ForEach(trafficDataRowSet => 
            {
                WriteLine();
                WriteLine($"During {ConvertTime(trafficDataRowSet.RunningTime)} :");
                WriteLine();

                foreach (TrafficDataType trafficDataType in Enum.GetValues(typeof(TrafficDataType)))
                {
                    bytesUnit = GetBytesConvertionTargetUnit(trafficDataRowSets,trafficDataType,TrafficMeasurementType.Bytes);
                    rateUnit  = GetBytesConvertionTargetUnit(trafficDataRowSets,trafficDataType,TrafficMeasurementType.Rate);
                    ForegroundColor = ConsoleColor.White;
                    switch (trafficDataType)
                    {
                        case TrafficDataType.TotalData    : Write("TotalTrafficData : "); break; 
                        case TrafficDataType.IncomingData : ForegroundColor = ConsoleColor.Green; Write("IncomingTrafficData : "); break; 
                        case TrafficDataType.OutgoingData : ForegroundColor = ConsoleColor.Red; Write("OutgoingTrafficData : "); break; 
                    }
                    WriteLine(trafficDataRowSet.Print(trafficDataType,bytesUnit,rateUnit ,false));
                }
                ForegroundColor = ConsoleColor.White; 
                WriteLine("__________________________________________________________________________");
            });
        }
    }
}