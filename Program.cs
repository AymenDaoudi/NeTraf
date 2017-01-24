using System.IO;
using static System.Console;
using static ConsoleApplication.TrafficDataPrinter;
using static ConsoleApplication.HelperMethods;

namespace ConsoleApplication
{
    public class Program
    {
        #region Fields
            private const string _incomingTrafficRate = "Incoming Traffic Rate";
            private const string _outgoingTrafficRate = "Outgoing Traffic Rate";
            private const string _incomingTrafficBytes = "Incoming Traffic Bytes";
            private const string _outgoingTrafficBytes = "Outgoing Traffic Bytes";
            private const string _incomingTrafficPackets = "Incoming Traffic Packets";
            private const string _outgoingTrafficPackets = "Outgoing Traffic Packets";
        #endregion

        
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

            // //6) Print to output file 
            PrintToOutputFile(trafficDataRowSets,"/home/aymendaoudi/Desktop/Output/Ring/VideoCall/IncomingTrafficRateRawOutput.txt", TrafficDataType.IncomingData, TrafficMeasurementType.Rate);
            PrintToOutputFile(trafficDataRowSets,"/home/aymendaoudi/Desktop/Output/Ring/VideoCall/OutgoingTrafficRateRawOutput.txt", TrafficDataType.OutgoingData, TrafficMeasurementType.Rate);
            PrintToOutputFile(trafficDataRowSets,"/home/aymendaoudi/Desktop/Output/Ring/VideoCall/IncomingTrafficBytesRawOutput.txt", TrafficDataType.IncomingData, TrafficMeasurementType.Bytes);
            PrintToOutputFile(trafficDataRowSets,"/home/aymendaoudi/Desktop/Output/Ring/VideoCall/OutgoingTrafficBytesRawOutput.txt", TrafficDataType.OutgoingData, TrafficMeasurementType.Bytes);
            PrintToOutputFile(trafficDataRowSets,"/home/aymendaoudi/Desktop/Output/Ring/VideoCall/IncomingTrafficPacketsRawOutput.txt", TrafficDataType.IncomingData, TrafficMeasurementType.Packets);
            PrintToOutputFile(trafficDataRowSets,"/home/aymendaoudi/Desktop/Output/Ring/VideoCall/OutgoingTrafficPacketsRawOutput.txt", TrafficDataType.OutgoingData, TrafficMeasurementType.Packets);

            if (args.Length == 0)
            {
                //ask for args
            }
            else
            {
                FileChecker("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DownTrafficRate.png");
                FileChecker("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/UpTrafficRate.png");
                FileChecker("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DownTrafficPackets.png");
                FileChecker("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/UpTrafficPackets.png");
                FileChecker("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DownTrafficBytes.png");
                FileChecker("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/UpTrafficBytes.png");
            }
            
            //7) plot beybeyyyy !
            var incomingRatePlotter = new Plotter("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/IncomingTrafficRateRawOutput.txt",
                                      _incomingTrafficRate,
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DownTrafficRate.png",
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/RatePlotSettings",
                                      _incomingTrafficRate,
                                      "Time",
                                      "Speed(KB/s)",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));
        
             var outgoingRatePlotter = new Plotter("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/OutgoingTrafficRateRawOutput.txt",
                                      _outgoingTrafficRate,
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/UpTrafficRate.png",
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/RatePlotSettings",
                                      _outgoingTrafficRate,
                                      "Time",
                                      "Speed(KB/s)",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

             var incomingBytesPlotter = new Plotter("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/IncomingTrafficBytesRawOutput.txt",
                                      _incomingTrafficBytes,
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DownTrafficBytes.png",
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DataPlotSettings",
                                      _incomingTrafficBytes,
                                      "Time",
                                      "MBytes",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

             var outgoingBytesPlotter = new Plotter("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/OutgoingTrafficBytesRawOutput.txt",
                                      _outgoingTrafficBytes,
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/UpTrafficBytes.png",
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DataPlotSettings",
                                      _outgoingTrafficBytes,
                                      "Time",
                                      "MBytes",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

             var incomingPacketsPlotter = new Plotter("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/IncomingTrafficPacketsRawOutput.txt",
                                      _incomingTrafficPackets,
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DownTrafficPackets.png",
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DataPlotSettings",
                                      _incomingTrafficPackets,
                                      "Time",
                                      "Packets",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

             var outgoingPacketsPlotter = new Plotter("/home/aymendaoudi/Desktop/Output/Ring/VideoCall/OutgoingTrafficPacketsRawOutput.txt",
                                      _outgoingTrafficPackets,
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/UpTrafficPackets.png",
                                      "/home/aymendaoudi/Desktop/Output/Ring/VideoCall/DataPlotSettings",
                                      _outgoingTrafficPackets,
                                      "Time",
                                      "Packets",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));
            incomingRatePlotter.Plot();
            outgoingRatePlotter.Plot();
            incomingBytesPlotter.Plot();
            outgoingBytesPlotter.Plot();
            incomingPacketsPlotter.Plot();
            outgoingPacketsPlotter.Plot();
        }
    }
}