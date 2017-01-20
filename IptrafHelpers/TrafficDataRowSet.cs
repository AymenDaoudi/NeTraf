using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace ConsoleApplication
{
    public class TrafficDataRowSet
    {
        #region Properties
        public List<TrafficDataRow> TrafficDataRows { get; set; }
        public Tuple<double,double> AccumulatedTotalTotalTrafficData { get; set; }
        public Tuple<double,double> AccumulatedTotalIncomingTrafficData { get; set; }
        public Tuple<double,double> AccumulatedTotalOutgoingTrafficData { get; set; }
        public Tuple<double,double,double> TotalTotalTrafficData { get; set; }
        public Tuple<double,double,double> TotalIncomingTrafficData { get; set; }
        public Tuple<double,double,double> TotalOutgoingTrafficData { get; set; }
        
        #endregion

        public TrafficDataRowSet(List<string> loggedRows)
        {
            TrafficDataRows = loggedRows.Select(trafficDataRow => new TrafficDataRow(trafficDataRow)).ToList();                                                       
        }

        public void CalculateTotals ()
        {
            AccumulatedTotalTotalTrafficData =  new Tuple<double,double>(TrafficDataRows.Select( data => data.TotalTrafficData.Packets).Sum(),
                                                                       TrafficDataRows.Select( data => data.TotalTrafficData.Bytes).Sum());

            AccumulatedTotalIncomingTrafficData =  new Tuple<double,double>(TrafficDataRows.Select( data => data.IncomingTrafficData.Packets).Sum(),
                                                                          TrafficDataRows.Select( data => data.IncomingTrafficData.Bytes).Sum());
                                                                          
            AccumulatedTotalOutgoingTrafficData =  new Tuple<double,double>(TrafficDataRows.Select( data => data.OutgoingTrafficData.Packets).Sum(),
                                                                          TrafficDataRows.Select( data => data.OutgoingTrafficData.Bytes).Sum()); 
        }

        public static void PrintTrafficData(Tuple<double,double,double> trafficData, TrafficDataType trafficDataType)
        {
            switch (trafficDataType)
            {
                case TrafficDataType.TotalData : ForegroundColor = ConsoleColor.White; break;
                case TrafficDataType.IncomingData : ForegroundColor = ConsoleColor.Green; break;
                case TrafficDataType.OutgoingData : ForegroundColor = ConsoleColor.Red; break;
                default: ForegroundColor = ConsoleColor.White; break;
            }
            WriteLine($"{trafficDataType} : " + trafficData.Print(false));
            ForegroundColor = ConsoleColor.White; 
        }
    }
}