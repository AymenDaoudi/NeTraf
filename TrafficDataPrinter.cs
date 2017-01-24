using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ConsoleApplication.HelperMethods;
using static System.Console;

namespace ConsoleApplication
{
    public static class TrafficDataPrinter
    {
        public static void PrintToOutputFile(List<TrafficDataRowSet> trafficDataRowSets, string outputFilePath, TrafficDataType trafficDataType, TrafficMeasurementType trafficMeasurementType)
        {
            var stream = File.OpenWrite(outputFilePath);
            using (System.IO.StreamWriter file =  new System.IO.StreamWriter(stream))
            {
                trafficDataRowSets.ForEach(trafficDataRowSet => 
                {
                    switch (trafficDataType)
                    {
                        case TrafficDataType.TotalData : 
                        { 
                            TrafficUnitType unit = TrafficUnitType.Bytes;
                            switch (trafficMeasurementType)
                            {
                                case TrafficMeasurementType.Bytes : unit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item2).ToList()); break;
                                case TrafficMeasurementType.Rate : unit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item3).ToList()); break;
                            }
                            file.WriteLine(TrafficDataToOutput(trafficDataRowSet.TotalTotalTrafficData, trafficDataRowSet.CumulatedRunningTime, trafficMeasurementType, unit));
                        }
                        break;
                        case TrafficDataType.IncomingData : 
                        { 
                            TrafficUnitType unit = TrafficUnitType.Bytes;
                            switch (trafficMeasurementType)
                            {
                                case TrafficMeasurementType.Bytes : unit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalIncomingTrafficData.Item2).ToList()); break;
                                case TrafficMeasurementType.Rate : unit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalIncomingTrafficData.Item3).ToList()); break;
                            }
                            file.WriteLine(TrafficDataToOutput(trafficDataRowSet.TotalIncomingTrafficData, trafficDataRowSet.CumulatedRunningTime, trafficMeasurementType, unit));
                        }
                        break;
                        case TrafficDataType.OutgoingData : 
                        {
                            TrafficUnitType unit = TrafficUnitType.Bytes;
                            switch (trafficMeasurementType)
                            {
                                case TrafficMeasurementType.Bytes : unit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalOutgoingTrafficData.Item2).ToList()); break;
                                case TrafficMeasurementType.Rate : unit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalOutgoingTrafficData.Item3).ToList()); break;
                            }
                            file.WriteLine(TrafficDataToOutput(trafficDataRowSet.TotalOutgoingTrafficData, trafficDataRowSet.CumulatedRunningTime, trafficMeasurementType, unit));
                        }
                        break;
                        default: 
                        {
                            TrafficUnitType unit = TrafficUnitType.Bytes;
                            switch (trafficMeasurementType)
                            {
                                case TrafficMeasurementType.Bytes : unit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item2).ToList()); break;
                                case TrafficMeasurementType.Rate : unit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item3).ToList()); break;
                            }
                            file.WriteLine(TrafficDataToOutput(trafficDataRowSet.TotalTotalTrafficData, trafficDataRowSet.CumulatedRunningTime, trafficMeasurementType, unit));
                        }
                        break;
                    }
                });
            }
        }

        private static string TrafficDataToOutput(Tuple<double,double,double> trafficData, double cumulatedRunningTime, TrafficMeasurementType trafficMeasurementType, TrafficUnitType trafficUnitType)
        {
            switch (trafficMeasurementType)
            {
                case TrafficMeasurementType.Packets : return $"{trafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                case TrafficMeasurementType.Bytes : return $"{String.Format("{0:0.00}", ConvertBytes(trafficData.Item2, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                case TrafficMeasurementType.Rate : return $"{String.Format("{0:0.00}", ConvertBytes(trafficData.Item3, trafficUnitType).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                default: return $"{trafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
            }
        }

        public static void PrintToConsole(List<TrafficDataRowSet> trafficDataRowSets)
        {
            var bytesUnit = TrafficUnitType.Bytes;
            var rateUnit = TrafficUnitType.Bytes; 
            trafficDataRowSets.ForEach(trafficDataRowSet => 
            {
                WriteLine();
                WriteLine($"During {ConvertTime(trafficDataRowSet.RunningTime)} :");
                WriteLine();
                
                bytesUnit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item2).ToList());
                rateUnit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalTotalTrafficData.Item3).ToList());
                ForegroundColor = ConsoleColor.White;
                WriteLine($"TotalTrafficData : {trafficDataRowSet.TotalTotalTrafficData.Print(bytesUnit,rateUnit ,false)}" );

                bytesUnit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalIncomingTrafficData.Item2).ToList());
                rateUnit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalIncomingTrafficData.Item3).ToList());
                ForegroundColor = ConsoleColor.Green;
                WriteLine($"IncomingTrafficData : {trafficDataRowSet.TotalIncomingTrafficData.Print(bytesUnit,rateUnit,false)}");

                bytesUnit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalOutgoingTrafficData.Item2).ToList());
                rateUnit = GetBytesConvertionTargetUnit(trafficDataRowSets.Select(_ => _.TotalOutgoingTrafficData.Item3).ToList());
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"OutgoingTrafficData : {trafficDataRowSet.TotalOutgoingTrafficData.Print(bytesUnit,rateUnit,false)}");
                ForegroundColor = ConsoleColor.White; 
                WriteLine("__________________________________________________________________________");
            });
        }

        
    }
}