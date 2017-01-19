using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace ConsoleApplication
{
    public class IptrafParser
    {
        #region Fields
            private const int timeLapse = 120;
            private List<string> _loggedRows;
        #endregion

        #region Properties
            public string FilePath { get; }
        #endregion

        public IptrafParser(string filePath)
        {
            FilePath = filePath;
        }


        #region Methods
            public List<TrafficDataRowSet> GetIptrafTrafficDataSets(List<uint> netstatPorts)
            {
                var trafficDataSets = new List<TrafficDataRowSet>();
                try
                {
                    _loggedRows = File.ReadAllLines(FilePath).ToList();
                    DeleteEmptyRows(ref _loggedRows);

                    var trafficDataGroups = ToTrafficDataGroups(_loggedRows);                    
                    trafficDataGroups.ForEach(_ => trafficDataSets.Add(new TrafficDataRowSet(_))); 
                    
                    FilterConcernedTrafficData(ref trafficDataSets,netstatPorts);

                    CalculateTrafficDataSetsTotals(ref trafficDataSets);

                    GetRealTrafficData(ref trafficDataSets);
                    
                }
                catch (FileNotFoundException exception)
                {
                    trafficDataSets = null;
                    WriteLine($"The Iptraf ouput file : {exception.FileName} was not found !");
                }
                
                return trafficDataSets;
            }

            public static void DeleteEmptyRows(ref List<string> loggedRows)
            {
                loggedRows.RemoveAll(row => row == string.Empty);
            }

            public List<List<string>> ToTrafficDataGroups(List<string> loggedLines)
            {
                var loggedRowGroups = new List<List<string>>();
                var loggedRowGroup = new List<string>();
                loggedRowGroups.Add(loggedRowGroup);

                for (int i = 0; i < loggedLines.Count; i++)
                {
                    if (loggedLines[i].Contains("Running time"))
                    {
                        loggedRowGroup = new List<string>();
                        loggedRowGroups.Add(loggedRowGroup);
                        continue;
                    }            
                    if (loggedLines[i].Contains("***")) continue;                  
                    loggedRowGroup.Add(loggedLines[i]);
                }

                loggedRowGroups.Remove(loggedRowGroups.First(_ => _.Count()==0));
                return loggedRowGroups;
            }


            private void FilterConcernedTrafficData(ref List<TrafficDataRowSet> trafficDataRowSets, List<uint> netstatPorts) => trafficDataRowSets.ForEach(trafficDataRowSet => trafficDataRowSet.TrafficDataRows.RemoveAll(loggedData => !netstatPorts.Contains(loggedData.PortNumber)));        
            

            private void CalculateTrafficDataSetsTotals(ref List<TrafficDataRowSet> trafficDataSets) 
            {
                trafficDataSets.ForEach(trafficDataSet => trafficDataSet.CalculateTotals());
            } 

            public Tuple<double,double,double> UnaccumulateTrafficData(Tuple<double,double> AccumulatedColledData, 
                                                                    Tuple<double,double> NextAccumulatedColledData,
                                                                    int timeLapse)
            {
                var addedPackets = NextAccumulatedColledData.Item1 - AccumulatedColledData.Item1;

                var addedBytes = NextAccumulatedColledData.Item2 - AccumulatedColledData.Item2;

                var rate = addedBytes/timeLapse;

                return new Tuple<double,double,double>(addedPackets,addedBytes,rate);
            }

            private void GetRealTrafficData(ref List<TrafficDataRowSet> trafficDataRowSets)
            {
                foreach (var trafficDataRowSet in trafficDataRowSets)
                    {
                        if(trafficDataRowSets.IndexOf(trafficDataRowSet) == 0) 
                        {
                            trafficDataRowSet.TotalTotalTrafficData = UnaccumulateTrafficData(new Tuple<double, double>(0,0),
                                                                                    trafficDataRowSet.AccumulatedTotalTotalTrafficData,
                                                                                    timeLapse);
                            trafficDataRowSet.TotalIncomingTrafficData = UnaccumulateTrafficData(new Tuple<double, double>(0,0),
                                                                                    trafficDataRowSet.AccumulatedTotalIncomingTrafficData,
                                                                                    timeLapse);
                            trafficDataRowSet.TotalOutgoingTrafficData = UnaccumulateTrafficData(new Tuple<double, double>(0,0),
                                                                                    trafficDataRowSet.AccumulatedTotalOutgoingTrafficData,
                                                                                    timeLapse);
                            continue;
                        }

                        trafficDataRowSet.TotalTotalTrafficData = UnaccumulateTrafficData(trafficDataRowSets[trafficDataRowSets.IndexOf(trafficDataRowSet)-1].AccumulatedTotalTotalTrafficData,
                                                                                    trafficDataRowSet.AccumulatedTotalTotalTrafficData,
                                                                                    timeLapse);
                                                                                 
                        trafficDataRowSet.TotalIncomingTrafficData = UnaccumulateTrafficData(trafficDataRowSets[trafficDataRowSets.IndexOf(trafficDataRowSet)-1].AccumulatedTotalIncomingTrafficData,
                                                                                       trafficDataRowSet.AccumulatedTotalIncomingTrafficData,
                                                                                       timeLapse);
                                                                                    
                        trafficDataRowSet.TotalOutgoingTrafficData = UnaccumulateTrafficData(trafficDataRowSets[trafficDataRowSets.IndexOf(trafficDataRowSet)-1].AccumulatedTotalOutgoingTrafficData,
                                                                                       trafficDataRowSet.AccumulatedTotalOutgoingTrafficData,
                                                                                       timeLapse);                                                                                                                
                    }
            }

        #endregion
    }
}