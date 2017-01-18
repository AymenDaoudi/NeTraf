using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {            
            var netstatPorts = GetPortsFromNetstatOutput("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/Netstatoutput"); 
            var loggedDataSets = ParseIptrafData("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/Iptrafoutput", netstatPorts);       
            loggedDataSets.ForEach(loggedDataSet => 
            {
                WriteLine("TotalCollectedData : " + loggedDataSet.TotalTotalCollectedData + 
                          "IncomingCollectedData : " + loggedDataSet.TotalIncomingCollectedData + 
                          "OutgoingCollectedData : " + loggedDataSet.TotalOutgoingCollectedData);
                WriteLine("_________________________");
            });
        }

        public static List<List<string>> GetCollectedDataGroups(List<string> loggedLines)
        {
            DeleteEmptyLines(ref loggedLines);
            return GetLoggedDataGroups(loggedLines);   
        }

        public static void DeleteEmptyLines(ref List<string> loggedLines)
        {
            loggedLines.RemoveAll(line => line == string.Empty);
        }

        public static List<List<string>> GetLoggedDataGroups(List<string> loggedLines)
        {
            var loggedDataGroups = new List<List<string>>();
            var loggedDataGroup = new List<string>();
            loggedDataGroups.Add(loggedDataGroup);
            
            for (int i = 0; i < loggedLines.Count; i++)
            {
                if (loggedLines[i].Contains("Running time"))
                {
                    loggedDataGroup = new List<string>();
                    loggedDataGroups.Add(loggedDataGroup);
                    continue;
                }            
                if (loggedLines[i].Contains("***")) continue;                  
                loggedDataGroup.Add(loggedLines[i]);
            }

            loggedDataGroups.Remove(loggedDataGroups.First(_ => _.Count()==0));
            return loggedDataGroups;
        }

        public static List<LoggedDataSet> ParseIptrafData(string path, List<uint> netstatPorts)
        {
            var loggedLines = File.ReadAllLines(path).ToList();
            
            var collectedDataGroups = GetCollectedDataGroups(loggedLines);

            var loggedDataSets = new List<LoggedDataSet>();
            
            collectedDataGroups.ForEach(_ => loggedDataSets.Add(new LoggedDataSet(_)));         

            loggedDataSets.ForEach(loggedDataSet => loggedDataSet.LoggedDatas.RemoveAll(loggedData => !netstatPorts.Contains(loggedData.PortNumber)));
            loggedDataSets.ForEach(loggedDataSet => loggedDataSet.CalculateTotals());

            foreach (var loggedDataSet in loggedDataSets)
            {
                const int timeLapse = 120;
                if(loggedDataSets.IndexOf(loggedDataSet) == 0) continue;

                var addedPackets = loggedDataSet.AccumulatedTotalTotalCollectedData.Item1 - 
                                   loggedDataSets[loggedDataSets.IndexOf(loggedDataSet)-1].AccumulatedTotalTotalCollectedData.Item1;

                var addedBytes = loggedDataSet.AccumulatedTotalTotalCollectedData.Item2 - 
                                 loggedDataSets[loggedDataSets.IndexOf(loggedDataSet)-1].AccumulatedTotalTotalCollectedData.Item2;
                
                var rate = addedBytes/timeLapse;

                loggedDataSet.TotalTotalCollectedData = new Tuple<double,double,double>(addedPackets,addedBytes,rate);

                addedPackets = loggedDataSet.AccumulatedTotalIncomingCollectedData.Item1 - 
                               loggedDataSets[loggedDataSets.IndexOf(loggedDataSet)-1].AccumulatedTotalIncomingCollectedData.Item1;

                addedBytes = loggedDataSet.AccumulatedTotalIncomingCollectedData.Item2 - 
                             loggedDataSets[loggedDataSets.IndexOf(loggedDataSet)-1].AccumulatedTotalIncomingCollectedData.Item2;
                
                rate = addedBytes/timeLapse;

                loggedDataSet.TotalIncomingCollectedData = new Tuple<double,double,double>(addedPackets,addedBytes,rate);                                                        

                addedPackets = loggedDataSet.AccumulatedTotalOutgoingCollectedData.Item1 - 
                               loggedDataSets[loggedDataSets.IndexOf(loggedDataSet)-1].AccumulatedTotalOutgoingCollectedData.Item1;

                addedBytes = loggedDataSet.AccumulatedTotalOutgoingCollectedData.Item2 - 
                             loggedDataSets[loggedDataSets.IndexOf(loggedDataSet)-1].AccumulatedTotalOutgoingCollectedData.Item2;
                
                rate = addedBytes/timeLapse;

                loggedDataSet.TotalOutgoingCollectedData = new Tuple<double,double,double>(addedPackets,addedBytes,rate);                                                                                                                
            }
            return loggedDataSets;
        }

        public static uint GetPortNumber(string sourceInfo)
        {
            string port ="";
            uint portNumber;

            try
            {
                for (int i = sourceInfo.Length - 1; sourceInfo[i] != ':'; i--)
                {
                    port = port.Insert(0,sourceInfo[i].ToString());
                }
                uint.TryParse(port,out portNumber);
            }
            catch (System.IndexOutOfRangeException)
            {
                return 0;
            }

            return portNumber;
        }

        public static List<uint> GetPortsFromNetstatOutput(string path)
        {
            var loggedLines = File.ReadAllLines(path).ToList();

            var collectedData = new List<List<string>>();

            loggedLines.ForEach(line => collectedData.Add(Regex.Split(line, @"\s{2,}").ToList()));

            var trafficSourceInfo = collectedData.Select(_ => _[2].Split(' ').Last()).ToList();
            var ports = trafficSourceInfo.Select(sourceInfo => GetPortNumber(sourceInfo)).Distinct().ToList();

            return ports;
        }
    }
}
