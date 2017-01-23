using System.IO;
using static System.Console;
using static ConsoleApplication.TrafficDataPrinter;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {    
            // 1) Execute Netstat + Iptraf
            //ExecuteCommand("netstat -c --all --tcp --udp --program | grep dring > /home/aymendaoudi/Desktop/Test");
            //ExecuteCommand("iptraf -B -u -s ens33 -L /home/aymendaoudi/Desktop/Test");
            
            // 2) Parse Netstat
            var netstatParser = new NetstatParser("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/Netstatoutput");

            // 3) Parse Iptraf
            var iptrafParser = new IptrafParser("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/Iptrafoutput");
            
            // 4) Get the corresponding process' ports from Netstat output
            var netstatPorts = netstatParser.GetPortsFromNetstatOutput();

            // 5) Join Iptraf output with Netstat filtered ports and get logged row sets for each time interval
            var trafficDataRowSets = iptrafParser.GetIptrafTrafficDataSets(netstatPorts);

            // 6) Print to Console
            //PrintToConsole(trafficDataRowSets);

            //6) Print to output file 
            PrintToOutputFile(trafficDataRowSets,"/home/aymendaoudi/Desktop/Output/Ring/VideoCall/NetTrafOutput.txt", TrafficDataType.IncomingData, DataUnitType.Rate);
        }
    }
}