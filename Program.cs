using System.IO;
using static System.Console;
using static NeTraf.TrafficDataPrinter;
using static NeTraf.HelperMethods;
using static NeTraf.FileManager;
using static NeTraf.Plotter;
using static System.IO.Path;
using System.Collections.Generic;

namespace NeTraf
{
    public class Program
    {

        #region Fields
            private static string _ratePlotSettingsFilePath ;
            private static string _dataPlotSettingsFilePath ;
            private static string _netstatOutputFilePath    ;
            private static string _iptrafOutputFilePath     ;
            private static string _totalTrafficPacketsRawOutputFilePath    ;
            private static string _totalTrafficBytesRawOutputFilePath     ;
            private static string _totalTrafficRateRawOutputFilePath       ;
            private static string _incomingTrafficPacketsRawOutputFilePath ;
            private static string _outgoingTrafficPacketsRawOutputFilePath ;
            private static string _incomingTrafficBytesRawOutputFilePath   ;
            private static string _outgoingTrafficBytesRawOutputFilePath   ;
            private static string _incomingTrafficRateRawOutputFilePath    ;
            private static string _outgoingTrafficRateRawOutputFilePath    ;

            private static string _totalTrafficPacketsGraphicalOutputFilePath    ;
            private static string _totalTrafficBytesGraphicalOutputFilePath     ;
            private static string _totalTrafficRateGraphicalOutputFilePath       ;
            private static string _incomingTrafficPacketsGraphicalOutputFilePath ;
            private static string _outgoingTrafficPacketsGraphicalOutputFilePath ;
            private static string _incomingTrafficBytesGraphicalOutputFilePath   ;
            private static string _outgoingTrafficBytesGraphicalOutputFilePath   ;
            private static string _incomingTrafficRateGraphicalOutputFilePath    ;
            private static string _outgoingTrafficRateGraphicalOutputFilePath    ;

        #endregion

        public static void Main(string[] args)
        {    
            // string rootOutputFolderPath = "/home/aymendaoudi/Desktop/Output/Skype/VideoCall/";
            // string processName = "skype";
            
            // _ratePlotSettingsFilePath =  GetPlotSettingsFilePaths(RatePlotSettingsFileName);
            // _dataPlotSettingsFilePath =  GetPlotSettingsFilePaths(DataPlotSettingsFileName);

            // CreateFolderIfInexists(rootOutputFolderPath);
            // CreateFolderIfInexists(Path.Combine(rootOutputFolderPath,RawOutputFilesFolderName));
            // CreateFolderIfInexists(Path.Combine(rootOutputFolderPath,GraphicalOutputFilesFolderName));

            // CreateFolderIfInexists(Path.Combine(rootOutputFolderPath,GraphicalOutputFilesFolderName));
            
            // SetRawOutputFilePaths(rootOutputFolderPath);
            // SetGraphicalOutputFilePaths(rootOutputFolderPath);

            // //2) Parse Netstat
            // var netstatParser = new NetstatParser(_netstatOutputFilePath);

            //     // 3) Parse Iptraf
            // var iptrafParser = new IptrafParser(_iptrafOutputFilePath);

                
            // // 4) Get the corresponding process' ports from Netstat output
            // var netstatPorts = netstatParser.GetPortsFromNetstatOutput();
            // // 5) Join Iptraf output with Netstat filtered ports and get logged row sets for each time interval
            // var trafficDataRowSets = iptrafParser.GetIptrafTrafficDataSets(netstatPorts);
            // // 6) Print to Console
            // //PrintToConsole(trafficDataRowSets);
            // // 6) Print raw output files 
            // PrintRawOutputFiles(trafficDataRowSets);
            // //7) plot beybeyyyy !
            // PrintGraphicalOutputFiles(trafficDataRowSets);


            string interfaceNetwork = "";
            string processName = "";
            string rootOutputFolderPath = "";

            if (args.Length == 0)
            {
                Write("Interface network : "); interfaceNetwork = ReadLine();
                Write("Process Name  : "); processName = ReadLine();
                Write("Output Folder : "); rootOutputFolderPath = ReadLine();
            }
            else
            {
                interfaceNetwork = args[0];
                processName = args[1];
                rootOutputFolderPath = args[2];
            }

            _ratePlotSettingsFilePath =  GetPlotSettingsFilePaths(RatePlotSettingsFileName);
            _dataPlotSettingsFilePath =  GetPlotSettingsFilePaths(DataPlotSettingsFileName);

            CreateFolderIfInexists(rootOutputFolderPath);
            CreateFolderIfInexists(Path.Combine(rootOutputFolderPath,RawOutputFilesFolderName));
            CreateFolderIfInexists(Path.Combine(rootOutputFolderPath,GraphicalOutputFilesFolderName));

            CreateFolderIfInexists(Path.Combine(rootOutputFolderPath,GraphicalOutputFilesFolderName));
            
            SetRawOutputFilePaths(rootOutputFolderPath);
            SetGraphicalOutputFilePaths(rootOutputFolderPath);

            var netstatParser = new NetstatParser(_netstatOutputFilePath);

            var iptrafParser = new IptrafParser(_iptrafOutputFilePath);

            // 1) Execute Netstat + Iptraf

            var netstatCommand = new ShellCommand("/bin/bash",'c', $"\"netstat -c --all --tcp --udp --program | grep {processName} > {_netstatOutputFilePath}\"", "");
            netstatCommand.Execute();

            var iptrafCommand = new ShellCommand("/bin/bash",'c', $"\"iptraf -B -u -s {interfaceNetwork} -L {_iptrafOutputFilePath}\"", "");
            iptrafCommand.Execute();

            WriteLine("Start parsing ? (y/n) ");

            var isParsingAllowed = false;

            while (true)
            {
                if (isParsingAllowed = (ReadKey().Key == System.ConsoleKey.Y))
                {
                    ShellCommand.Stop("iptraf");
                    ShellCommand.Stop("netstat");
                    break;
                }
                else if (ReadKey().Key == System.ConsoleKey.N)
                {
                    break;
                }
                else
                {
                    WriteLine("Start parsing ? (y/n) ");
                }
            }
            
            if (isParsingAllowed)
            {   
                // 4) Get the corresponding process' ports from Netstat output
                var netstatPorts = netstatParser.GetPortsFromNetstatOutput();
                // 5) Join Iptraf output with Netstat filtered ports and get logged row sets for each time interval
                var trafficDataRowSets = iptrafParser.GetIptrafTrafficDataSets(netstatPorts);
                // 6) Print to Console
                //PrintToConsole(trafficDataRowSets);
                // 6) Print raw output files 
                PrintRawOutputFiles(trafficDataRowSets);
                //7) plot beybeyyyy !
                PrintGraphicalOutputFiles(trafficDataRowSets);
            }
            
            CleanIptraf();
        }


        private static void PrintRawOutputFiles(List<TrafficDataRowSet> trafficDataRowSets)
        {
            PrintToOutputFile(trafficDataRowSets , _totalTrafficPacketsRawOutputFilePath    , TrafficDataType.TotalData    , TrafficMeasurementType.Packets);
            PrintToOutputFile(trafficDataRowSets , _totalTrafficBytesRawOutputFilePath      , TrafficDataType.TotalData    , TrafficMeasurementType.Bytes);
            PrintToOutputFile(trafficDataRowSets , _totalTrafficRateRawOutputFilePath       , TrafficDataType.TotalData    , TrafficMeasurementType.Rate);
            PrintToOutputFile(trafficDataRowSets , _incomingTrafficPacketsRawOutputFilePath , TrafficDataType.IncomingData , TrafficMeasurementType.Packets);
            PrintToOutputFile(trafficDataRowSets , _outgoingTrafficPacketsRawOutputFilePath , TrafficDataType.OutgoingData , TrafficMeasurementType.Packets);
            PrintToOutputFile(trafficDataRowSets , _incomingTrafficBytesRawOutputFilePath   , TrafficDataType.IncomingData , TrafficMeasurementType.Bytes);
            PrintToOutputFile(trafficDataRowSets , _outgoingTrafficBytesRawOutputFilePath   , TrafficDataType.OutgoingData , TrafficMeasurementType.Bytes);
            PrintToOutputFile(trafficDataRowSets , _incomingTrafficRateRawOutputFilePath    , TrafficDataType.IncomingData , TrafficMeasurementType.Rate);
            PrintToOutputFile(trafficDataRowSets , _outgoingTrafficRateRawOutputFilePath    , TrafficDataType.OutgoingData , TrafficMeasurementType.Rate);
        }

        private static void SetRawOutputFilePaths(string rootOutputFolderPath)
        {
            _netstatOutputFilePath = Combine(rootOutputFolderPath , RawOutputFilesFolderName , NetstatOuputFileName);
            _iptrafOutputFilePath =  Combine(rootOutputFolderPath , RawOutputFilesFolderName , IptrafOutputFileName);

            CreateFileIfInexists(_netstatOutputFilePath);
            CreateFileIfInexists(_iptrafOutputFilePath);

            _totalTrafficPacketsRawOutputFilePath    = Combine(rootOutputFolderPath , RawOutputFilesFolderName , TotalTrafficPacketsOutputFileName);
            _totalTrafficBytesRawOutputFilePath      = Combine(rootOutputFolderPath , RawOutputFilesFolderName , TotalTrafficBytesOutputFileName);
            _totalTrafficRateRawOutputFilePath       = Combine(rootOutputFolderPath , RawOutputFilesFolderName , TotalTrafficRateOutputFileName);
            _incomingTrafficPacketsRawOutputFilePath = Combine(rootOutputFolderPath , RawOutputFilesFolderName , IncomingTrafficPacketsOutputFileName);
            _outgoingTrafficPacketsRawOutputFilePath = Combine(rootOutputFolderPath , RawOutputFilesFolderName , OutgoingTrafficPacketsOutputFileName);
            _incomingTrafficBytesRawOutputFilePath   = Combine(rootOutputFolderPath , RawOutputFilesFolderName , IncomingTrafficBytesOutputFileName);
            _outgoingTrafficBytesRawOutputFilePath   = Combine(rootOutputFolderPath , RawOutputFilesFolderName , OutgoingTrafficBytesOutputFileName);
            _incomingTrafficRateRawOutputFilePath    = Combine(rootOutputFolderPath , RawOutputFilesFolderName , IncomingTrafficRateOutputFileName);
            _outgoingTrafficRateRawOutputFilePath    = Combine(rootOutputFolderPath , RawOutputFilesFolderName , OutgoingTrafficRateOutputFileName);
        }

        private static void SetGraphicalOutputFilePaths(string rootOutputFolderPath)
        {
            _totalTrafficPacketsGraphicalOutputFilePath    = Combine(rootOutputFolderPath , GraphicalOutputFilesFolderName , TotalTrafficPacketsOutputFileName    + ".png");
            _totalTrafficBytesGraphicalOutputFilePath      = Combine(rootOutputFolderPath , GraphicalOutputFilesFolderName , TotalTrafficBytesOutputFileName      + ".png");
            _totalTrafficRateGraphicalOutputFilePath       = Combine(rootOutputFolderPath , GraphicalOutputFilesFolderName , TotalTrafficRateOutputFileName       + ".png");
            _incomingTrafficPacketsGraphicalOutputFilePath = Combine(rootOutputFolderPath , GraphicalOutputFilesFolderName , IncomingTrafficPacketsOutputFileName + ".png");
            _outgoingTrafficPacketsGraphicalOutputFilePath = Combine(rootOutputFolderPath , GraphicalOutputFilesFolderName , OutgoingTrafficPacketsOutputFileName + ".png");
            _incomingTrafficBytesGraphicalOutputFilePath   = Combine(rootOutputFolderPath , GraphicalOutputFilesFolderName , IncomingTrafficBytesOutputFileName   + ".png");
            _outgoingTrafficBytesGraphicalOutputFilePath   = Combine(rootOutputFolderPath , GraphicalOutputFilesFolderName , OutgoingTrafficBytesOutputFileName   + ".png");
            _incomingTrafficRateGraphicalOutputFilePath    = Combine(rootOutputFolderPath , GraphicalOutputFilesFolderName , IncomingTrafficRateOutputFileName    + ".png");
            _outgoingTrafficRateGraphicalOutputFilePath    = Combine(rootOutputFolderPath , GraphicalOutputFilesFolderName , OutgoingTrafficRateOutputFileName    + ".png");
        }

        private static void PrintGraphicalOutputFiles(List<TrafficDataRowSet> trafficDataRowSets)
        {
            var totalRatePlotter = new Plotter(_totalTrafficRateRawOutputFilePath,
                                      _totalTrafficRatePlotTitle,
                                      _totalTrafficRateGraphicalOutputFilePath,
                                      _ratePlotSettingsFilePath,
                                      _totalTrafficRatePlotTitle,
                                      _xLabel,
                                      $"Speed({GetBytesConvertionTargetUnit(trafficDataRowSets,TrafficDataType.TotalData,TrafficMeasurementType.Rate).ToSimpleString()}/s)",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

            var totalBytesPlotter = new Plotter(_totalTrafficBytesRawOutputFilePath,
                                      _totalTrafficBytesPlotTitle,
                                      _totalTrafficBytesGraphicalOutputFilePath,
                                      _dataPlotSettingsFilePath,
                                      _totalTrafficBytesPlotTitle,
                                      _xLabel,
                                      $"{GetBytesConvertionTargetUnit(trafficDataRowSets,TrafficDataType.TotalData,TrafficMeasurementType.Bytes).ToSimpleString()}",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

            var totalPacketsPlotter = new Plotter(_totalTrafficPacketsRawOutputFilePath,
                                      _totalTrafficPacketsPlotTitle,
                                      _totalTrafficPacketsGraphicalOutputFilePath,
                                      _dataPlotSettingsFilePath,
                                      _totalTrafficPacketsPlotTitle,
                                      _xLabel,
                                      "Packets",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

            var incomingRatePlotter = new Plotter(_incomingTrafficRateRawOutputFilePath,
                                      _incomingTrafficRatePlotTitle,
                                      _incomingTrafficRateGraphicalOutputFilePath,
                                      _ratePlotSettingsFilePath,
                                      _incomingTrafficRatePlotTitle,
                                      _xLabel,
                                      $"Speed({GetBytesConvertionTargetUnit(trafficDataRowSets,TrafficDataType.IncomingData,TrafficMeasurementType.Rate).ToSimpleString()}/s)",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));
        
             var outgoingRatePlotter = new Plotter(_outgoingTrafficRateRawOutputFilePath,
                                      _outgoingTrafficRatePlotTitle,
                                      _outgoingTrafficRateGraphicalOutputFilePath,
                                      _ratePlotSettingsFilePath,
                                      _outgoingTrafficRatePlotTitle,
                                      _xLabel,
                                      $"Speed({GetBytesConvertionTargetUnit(trafficDataRowSets,TrafficDataType.OutgoingData,TrafficMeasurementType.Rate).ToSimpleString()}/s)",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

             var incomingBytesPlotter = new Plotter(_incomingTrafficBytesRawOutputFilePath,
                                      _incomingTrafficBytesPlotTitle,
                                      _incomingTrafficBytesGraphicalOutputFilePath,
                                      _dataPlotSettingsFilePath,
                                      _incomingTrafficBytesPlotTitle,
                                      _xLabel,
                                      $"{GetBytesConvertionTargetUnit(trafficDataRowSets,TrafficDataType.IncomingData,TrafficMeasurementType.Bytes).ToSimpleString()}",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

             var outgoingBytesPlotter = new Plotter(_outgoingTrafficBytesRawOutputFilePath,
                                      _outgoingTrafficBytesPlotTitle,
                                      _outgoingTrafficBytesGraphicalOutputFilePath,
                                      _dataPlotSettingsFilePath,
                                      _outgoingTrafficBytesPlotTitle,
                                      _xLabel,
                                      $"{GetBytesConvertionTargetUnit(trafficDataRowSets,TrafficDataType.OutgoingData,TrafficMeasurementType.Bytes).ToSimpleString()}",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

             var incomingPacketsPlotter = new Plotter(_incomingTrafficPacketsRawOutputFilePath,
                                      _incomingTrafficPacketsPlotTitle,
                                      _incomingTrafficPacketsGraphicalOutputFilePath,
                                      _dataPlotSettingsFilePath,
                                      _incomingTrafficPacketsPlotTitle,
                                      _xLabel,
                                      "Packets",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));

             var outgoingPacketsPlotter = new Plotter(_outgoingTrafficPacketsRawOutputFilePath,
                                      _outgoingTrafficPacketsPlotTitle,
                                      _outgoingTrafficPacketsGraphicalOutputFilePath,
                                      _dataPlotSettingsFilePath,
                                      _outgoingTrafficPacketsPlotTitle,
                                      _xLabel,
                                      "Packets",
                                      ConvertTime(IptrafParser.GetAverageRunningTime(trafficDataRowSets)));
            totalRatePlotter.Plot();
            totalBytesPlotter.Plot();
            totalPacketsPlotter.Plot();
            incomingRatePlotter.Plot();
            outgoingRatePlotter.Plot();
            incomingBytesPlotter.Plot();
            outgoingBytesPlotter.Plot();
            incomingPacketsPlotter.Plot();
            outgoingPacketsPlotter.Plot();
        }
    }
}