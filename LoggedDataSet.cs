using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication
{
    public class LoggedDataSet
    {
        #region Properties
        public List<LoggedData> LoggedDatas { get; set; }
        public Tuple<double,double> AccumulatedTotalTotalCollectedData { get; set; }
        public Tuple<double,double> AccumulatedTotalIncomingCollectedData { get; set; }
        public Tuple<double,double> AccumulatedTotalOutgoingCollectedData { get; set; }
        public Tuple<double,double,double> TotalTotalCollectedData { get; set; }
        public Tuple<double,double,double> TotalIncomingCollectedData { get; set; }
        public Tuple<double,double,double> TotalOutgoingCollectedData { get; set; }
        
        #endregion

        public LoggedDataSet(List<string> loggedData)
        {
            LoggedDatas = loggedData.Select(_ => new LoggedData(_)).ToList();                                                       
        }

        public void CalculateTotals ()
        {
            AccumulatedTotalTotalCollectedData =  new Tuple<double,double>(LoggedDatas.Select( data => data.TotalCollectedData.Packets).Sum(),
                                                                       LoggedDatas.Select( data => data.TotalCollectedData.Bytes).Sum());

            AccumulatedTotalIncomingCollectedData =  new Tuple<double,double>(LoggedDatas.Select( data => data.IncomingCollectedData.Packets).Sum(),
                                                                          LoggedDatas.Select( data => data.IncomingCollectedData.Bytes).Sum());
                                                                          
            AccumulatedTotalOutgoingCollectedData =  new Tuple<double,double>(LoggedDatas.Select( data => data.OutgoingCollectedData.Packets).Sum(),
                                                                          LoggedDatas.Select( data => data.OutgoingCollectedData.Bytes).Sum()); 
        }
    }
}