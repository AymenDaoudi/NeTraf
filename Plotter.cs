using System.Diagnostics;

namespace NeTraf
{
    //Suports only one graph for the meantime
    public class Plotter
    {
        #region Consts
            public const string _totalTrafficRatePlotTitle = "Total Traffic Rate";
            public const string _totalTrafficBytesPlotTitle = "Total Traffic Bytes";
            public const string _totalTrafficPacketsPlotTitle = "Total Traffic Packets";
            public const string _incomingTrafficRatePlotTitle = "Incoming Traffic Rate";
            public const string _outgoingTrafficRatePlotTitle = "Outgoing Traffic Rate";
            public const string _incomingTrafficBytesPlotTitle = "Incoming Traffic Bytes";
            public const string _outgoingTrafficBytesPlotTitle = "Outgoing Traffic Bytes";
            public const string _incomingTrafficPacketsPlotTitle = "Incoming Traffic Packets";
            public const string _outgoingTrafficPacketsPlotTitle = "Outgoing Traffic Packets";
            public const string _xLabel = "Time";

        #endregion

        #region Properties
            public string DataFilePath { get; }
            public string KeyTitle { get; }
            public string OutputFilePath { get; }
            public string SettingsFilePath { get; }
            public string Title { get; }
            public string XLabel { get; }
            public string YLabel { get; }
            public string XTics { get; }
            
            //To support, later ... may be
            // public string YTics { get; }
        #endregion

        public Plotter(string dataFilePath, 
                       string keyTitle,
                       string outputFilePath,
                       string settingsFilePath,
                       string title, 
                       string xLabel,
                       string yLabel,
                       string xTics)
        {
            DataFilePath     = dataFilePath;
            KeyTitle         = keyTitle;
            OutputFilePath   = outputFilePath;
            SettingsFilePath = settingsFilePath;
            Title            = title;
            XLabel           = xLabel;
            YLabel           = yLabel;
            XTics            = xTics;
        }

        #region Methods
            public Process Plot()
            {
                var commandString = $"\"filename='{DataFilePath}';" +
                                    $"keyTitle='{KeyTitle}';"       +
                                    $"output='{OutputFilePath}';"   +
                                    $"xLabel='{XLabel}';"           +
                                    $"yLabel='{YLabel}';"           +
                                    $"title='{Title}';"             +
                                    $"xTics='{XTics}'\"";
                                
                var gnuplotCommand = new ShellCommand("/usr/bin/gnuplot",'e', commandString, SettingsFilePath);
                gnuplotCommand.Execute();
            
                return gnuplotCommand.CorrespondingProcess;
            }   
        #endregion
    }
}