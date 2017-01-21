using System.IO;
using static System.Console;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {    
            //ExecuteCommand("netstat -c --all --tcp --udp --program | grep dring > /home/aymendaoudi/Desktop/Test");
            //ExecuteCommand("iptraf -B -u -s ens33 -L /home/aymendaoudi/Desktop/Test");
            
            var netstatParser = new NetstatParser("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/Netstatoutput");
            var iptrafParser = new IptrafParser("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/Iptrafoutput");
            
            var netstatPorts = netstatParser.GetPortsFromNetstatOutput();
     
            var trafficDataRowSets = iptrafParser.GetIptrafTrafficDataSets(netstatPorts);

            trafficDataRowSets.ForEach(trafficDataRowSet => 
            {
                TrafficDataRowSet.PrintTrafficData(trafficDataRowSet.TotalTotalTrafficData, TrafficDataType.TotalData);
                TrafficDataRowSet.PrintTrafficData(trafficDataRowSet.TotalIncomingTrafficData, TrafficDataType.IncomingData);
                TrafficDataRowSet.PrintTrafficData(trafficDataRowSet.TotalOutgoingTrafficData, TrafficDataType.OutgoingData);
                
                WriteLine();
                WriteLine(trafficDataRowSet.RunningTime);
                WriteLine("__________________________________________________________________________");
            });
            
            var stream = File.OpenWrite("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/NetTrafOutput.txt");
            using (System.IO.StreamWriter file =  new System.IO.StreamWriter(stream))
            {
                trafficDataRowSets.ForEach(trafficDataRowSet => 
                {
                    file.WriteLine(trafficDataRowSet.TotalTotalTrafficData.Print(true));
                    // file.WriteLine(trafficDataRowSet.TotalIncomingTrafficData.Print(true));
                    // file.WriteLine(trafficDataRowSet.TotalOutgoingTrafficData.Print(true));
                });
            }
        }
    }
}
