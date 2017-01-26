using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Console;

namespace NeTraf
{
    public class IptrafParser
    {
        #region Fields
            private List<string> _loggedRows;
            private const string _unsignedIntegerPattern = @"\d+";
            private FileStream _fileStream;
        #endregion

        #region Properties
            public string FilePath { get; }
        #endregion

        public IptrafParser(string filePath)
        {
            FilePath = filePath;
            _fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }


        #region Methods
            public List<TrafficDataRowSet> GetIptrafTrafficDataSets(List<uint> netstatPorts)
            {
                var trafficDataRowSets = new List<TrafficDataRowSet>();
                try
                {
                    using (_fileStream)
                    {
                        using (var streamReader = new StreamReader(_fileStream, System.Text.Encoding.ASCII)) 
                        {
                            _loggedRows = streamReader.ReadAllLines().ToList();
                        }
                    }
                    DeleteEmptyRows(ref _loggedRows);

                    var trafficDataGroups = ToTrafficDataGroups(_loggedRows);                    
                    
                    trafficDataGroups.ForEach(_ => trafficDataRowSets.Add(new TrafficDataRowSet(_.Item1,_.Item2,_.Item3))); 
                    
                    FilterConcernedTrafficData(ref trafficDataRowSets,netstatPorts);

                    CalculateTrafficDataSetsTotals(ref trafficDataRowSets);

                    GetRealTrafficData(ref trafficDataRowSets);
                    
                    //Just to simulate a start from 0
                    trafficDataRowSets.Insert(0,new TrafficDataRowSet(0,0,new Tuple<double,double,double>(0,0,0),
                                                                          new Tuple<double,double,double>(0,0,0),
                                                                          new Tuple<double,double,double>(0,0,0)));
                    
                    
                }
                catch (FileNotFoundException exception)
                {
                    trafficDataRowSets = null;
                    WriteLine($"The Iptraf ouput file : {exception.FileName} was not found !");
                }
                
                return trafficDataRowSets;
            }

            public static void DeleteEmptyRows(ref List<string> loggedRows)
            {
                loggedRows.RemoveAll(row => row == string.Empty);
            }

            public List<Tuple<double,double,List<string>>> ToTrafficDataGroups(List<string> loggedLines)
            {
                var integerRegex = new Regex(_unsignedIntegerPattern);

                var loggedRowGroups = new List<Tuple<double,double,List<string>>>();
                var loggedDataTrafficGroup = new List<string>();
                var runningTime = 0d;
                var previousRunningTime = 0d;
                for (int i = 0; i < loggedLines.Count; i++)
                {
                    if (loggedLines[i].Contains("Running time"))
                    {
                        previousRunningTime = runningTime;
                        double.TryParse(integerRegex.Match(loggedLines[i]).Groups[0].Value, out runningTime);
                        loggedRowGroups.Add(new Tuple<double, double, List<string>>(runningTime - previousRunningTime,runningTime,loggedDataTrafficGroup));
                        loggedDataTrafficGroup = new List<string>();
                        continue;
                    }            
                    if (loggedLines[i].Contains("***")) continue;                  
                    loggedDataTrafficGroup.Add(loggedLines[i]);
                }

                return loggedRowGroups;
            }

            private void FilterConcernedTrafficData(ref List<TrafficDataRowSet> trafficDataRowSets, List<uint> netstatPorts) => trafficDataRowSets.ForEach(trafficDataRowSet
                                                                                                                             => trafficDataRowSet.TrafficDataRows.RemoveAll(loggedData 
                                                                                                                             => !netstatPorts.Contains(loggedData.PortNumber)));        
            private void CalculateTrafficDataSetsTotals(ref List<TrafficDataRowSet> trafficDataSets)  => trafficDataSets.ForEach(trafficDataSet 
                                                                                                      => trafficDataSet.CalculateTotals());

            //Tight coupled with TrafficDataRowSet class !!
            public Tuple<double,double,double> UnaccumulateTrafficData(Tuple<double,double> AccumulatedColledData, 
                                                                       Tuple<double,double> NextAccumulatedColledData,
                                                                       double timeLapse)
            {
                var addedPackets = NextAccumulatedColledData.Item1 - AccumulatedColledData.Item1;

                var addedBytes = NextAccumulatedColledData.Item2 - AccumulatedColledData.Item2;

                var rate = addedBytes/timeLapse;

                return new Tuple<double,double,double>(addedPackets,addedBytes,rate);
            }

            //Tight coupled with TrafficDataRowSet class !!
            private void GetRealTrafficData(ref List<TrafficDataRowSet> trafficDataRowSets)
            {
                foreach (var trafficDataRowSet in trafficDataRowSets)
                    {
                        if(trafficDataRowSets.IndexOf(trafficDataRowSet) == 0) 
                        {
                            trafficDataRowSet.TotalTotalTrafficData = UnaccumulateTrafficData(new Tuple<double, double>(0,0),
                                                                                    trafficDataRowSet.AccumulatedTotalTotalTrafficData,
                                                                                    trafficDataRowSet.RunningTime);
                            trafficDataRowSet.TotalIncomingTrafficData = UnaccumulateTrafficData(new Tuple<double, double>(0,0),
                                                                                    trafficDataRowSet.AccumulatedTotalIncomingTrafficData,
                                                                                    trafficDataRowSet.RunningTime);
                            trafficDataRowSet.TotalOutgoingTrafficData = UnaccumulateTrafficData(new Tuple<double, double>(0,0),
                                                                                    trafficDataRowSet.AccumulatedTotalOutgoingTrafficData,
                                                                                    trafficDataRowSet.RunningTime);
                            continue;
                        }

                        trafficDataRowSet.TotalTotalTrafficData = UnaccumulateTrafficData(trafficDataRowSets[trafficDataRowSets.IndexOf(trafficDataRowSet)-1].AccumulatedTotalTotalTrafficData,
                                                                                          trafficDataRowSet.AccumulatedTotalTotalTrafficData,
                                                                                          trafficDataRowSet.RunningTime);
                                                                                 
                        trafficDataRowSet.TotalIncomingTrafficData = UnaccumulateTrafficData(trafficDataRowSets[trafficDataRowSets.IndexOf(trafficDataRowSet)-1].AccumulatedTotalIncomingTrafficData,
                                                                                             trafficDataRowSet.AccumulatedTotalIncomingTrafficData,
                                                                                             trafficDataRowSet.RunningTime);
                                                                                    
                        trafficDataRowSet.TotalOutgoingTrafficData = UnaccumulateTrafficData(trafficDataRowSets[trafficDataRowSets.IndexOf(trafficDataRowSet)-1].AccumulatedTotalOutgoingTrafficData,
                                                                                             trafficDataRowSet.AccumulatedTotalOutgoingTrafficData,
                                                                                             trafficDataRowSet.RunningTime);                                                                                                                
                    }
            }

            public static double GetAverageRunningTime(List<TrafficDataRowSet> trafficDataRowSets) => trafficDataRowSets.Last().CumulatedRunningTime / trafficDataRowSets.Count();

        #endregion
    }
}