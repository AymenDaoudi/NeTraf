using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;

namespace ConsoleApplication
{
    public class Program
    {
        // public static void ExecuteCommand(string command)
        // {
        //     Process proc = new System.Diagnostics.Process ();
        //     proc.StartInfo.FileName = "/bin/bash";
        //     proc.StartInfo.Arguments = "-c \" " + command + " \"";
        //     proc.StartInfo.UseShellExecute = false; 
        //     proc.StartInfo.RedirectStandardOutput = true;
        //     proc.Start ();
            
        //     while (true) 
        //     {
        //         var enteredCommand = Console.ReadLine();
        //         if (enteredCommand.ToLower() == "stop")
        //         {
        //             proc.Kill();
        //             //ExecuteCommand("killall iptraf");
        //             Environment.Exit(0);
        //         }
        //     }

        //     // while (!proc.StandardOutput.EndOfStream) 
        //     // {
        //     //     Console.WriteLine (proc.StandardOutput.ReadLine ());
        //     //     var enteredCommand = Console.ReadLine();
        //     //     if (enteredCommand.ToLower() == "stop")
        //     //     {
        //     //         proc.Kill();
        //     //         Environment.Exit(0);
        //     //     }
        //     // }
        // }
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
                WriteLine("__________________________________________________________________________");
            });
        }
    }
}
