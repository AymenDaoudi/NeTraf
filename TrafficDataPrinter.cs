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
        public static void PrintToOutputFile(List<TrafficDataRowSet> trafficDataRowSets, string outputFilePath, TrafficDataType trafficDataType, DataUnitType dataUnitType)
        {
            var stream = File.OpenWrite(outputFilePath);
            using (System.IO.StreamWriter file =  new System.IO.StreamWriter(stream))
            {
                trafficDataRowSets.ForEach(trafficDataRowSet => 
                {
                    switch (trafficDataType)
                    {
                        case TrafficDataType.TotalData : file.WriteLine(TrafficDataToOutput(trafficDataRowSet.TotalTotalTrafficData, trafficDataRowSet.CumulatedRunningTime, dataUnitType));
                        break;
                        case TrafficDataType.IncomingData : file.WriteLine(TrafficDataToOutput(trafficDataRowSet.TotalIncomingTrafficData, trafficDataRowSet.CumulatedRunningTime, dataUnitType));
                        break;
                        case TrafficDataType.OutgoingData : file.WriteLine(TrafficDataToOutput(trafficDataRowSet.TotalOutgoingTrafficData, trafficDataRowSet.CumulatedRunningTime, dataUnitType));
                        break;
                        default: file.WriteLine(TrafficDataToOutput(trafficDataRowSet.TotalTotalTrafficData, trafficDataRowSet.CumulatedRunningTime, dataUnitType));
                        break;
                    }
                });
            }
        }

        private static string TrafficDataToOutput(Tuple<double,double,double> trafficData,double cumulatedRunningTime, DataUnitType dataUnitType)
        {
            switch (dataUnitType)
            {
                case DataUnitType.Packets : return $"{trafficData.Item1},{ConvertTime(cumulatedRunningTime)}"; 
                case DataUnitType.Bytes : return $"{String.Format("{0:0.00}", ConvertBytes(trafficData.Item2).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                case DataUnitType.Rate : return $"{String.Format("{0:0.00}", ConvertBytes(trafficData.Item3).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
                default: return $"{String.Format("{0:0.00}", ConvertBytes(trafficData.Item1).Item1)},{ConvertTime(cumulatedRunningTime)}"; 
            }
        }

        public static void PrintToConsole(List<TrafficDataRowSet> trafficDataRowSets)
        {
            trafficDataRowSets.ForEach(trafficDataRowSet => 
            {
                WriteLine();
                WriteLine($"During {ConvertTime(trafficDataRowSet.RunningTime)} :");
                WriteLine();
                ForegroundColor = ConsoleColor.White;
                WriteLine($"TotalTrafficData : {trafficDataRowSet.TotalTotalTrafficData.Print(false)}" );
                ForegroundColor = ConsoleColor.Green;
                WriteLine($"IncomingTrafficData : {trafficDataRowSet.TotalIncomingTrafficData.Print(false)}");
                ForegroundColor = ConsoleColor.Red;
                WriteLine($"OutgoingTrafficData : {trafficDataRowSet.TotalOutgoingTrafficData.Print(false)}");
                ForegroundColor = ConsoleColor.White; 
                WriteLine("__________________________________________________________________________");
            });
        }

        
    }
}