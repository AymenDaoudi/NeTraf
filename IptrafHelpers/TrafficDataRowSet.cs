using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;
using static ConsoleApplication.HelperMethods;

namespace ConsoleApplication
{
    public class TrafficDataRowSet
    {
        #region Properties
            public double RunningTime { get;}
            public double CumulatedRunningTime { get;}
            public List<TrafficDataRow> TrafficDataRows { get; set; }
            public Tuple<double,double> AccumulatedTotalTotalTrafficData { get; set; }
            public Tuple<double,double> AccumulatedTotalIncomingTrafficData { get; set; }
            public Tuple<double,double> AccumulatedTotalOutgoingTrafficData { get; set; }
            public Tuple<double,double,double> TotalTotalTrafficData { get; set; }
            public Tuple<double,double,double> TotalIncomingTrafficData { get; set; }
            public Tuple<double,double,double> TotalOutgoingTrafficData { get; set; }
        
        #endregion

        public TrafficDataRowSet(double runningTime,double cumulatedRunningTime,List<string> loggedRows)
        {
            TrafficDataRows = loggedRows.Select(trafficDataRow => new TrafficDataRow(trafficDataRow)).ToList();  
            RunningTime = runningTime;
            CumulatedRunningTime = cumulatedRunningTime;
        }

        public TrafficDataRowSet(double runningTime,
                                 double cumulatedRunningTime,
                                 Tuple<double,double,double> totalTotalTrafficData,
                                 Tuple<double,double,double> totalIncomingTrafficData,
                                 Tuple<double,double,double> totalOutgoingTrafficData)
        {
            TrafficDataRows = new List<TrafficDataRow>();
            RunningTime = runningTime;
            CumulatedRunningTime = cumulatedRunningTime;
            TotalTotalTrafficData = totalTotalTrafficData;
            TotalIncomingTrafficData = totalIncomingTrafficData;
            TotalOutgoingTrafficData = totalOutgoingTrafficData;
            AccumulatedTotalTotalTrafficData = new Tuple<double,double>(0,0);
            AccumulatedTotalIncomingTrafficData = new Tuple<double,double>(0,0);
            AccumulatedTotalOutgoingTrafficData = new Tuple<double,double>(0,0);
        }

        public void CalculateTotals ()
        {
            AccumulatedTotalTotalTrafficData =  new Tuple<double,double>(TrafficDataRows.Select(data => data.TotalTrafficData.Packets).Sum(),
                                                                         TrafficDataRows.Select(data => data.TotalTrafficData.Bytes).Sum());

            AccumulatedTotalIncomingTrafficData =  new Tuple<double,double>(TrafficDataRows.Select(data => data.IncomingTrafficData.Packets).Sum(),
                                                                            TrafficDataRows.Select(data => data.IncomingTrafficData.Bytes).Sum());
                                                                          
            AccumulatedTotalOutgoingTrafficData =  new Tuple<double,double>(TrafficDataRows.Select(data => data.OutgoingTrafficData.Packets).Sum(),
                                                                            TrafficDataRows.Select(data => data.OutgoingTrafficData.Bytes).Sum()); 
        }        
    }
}