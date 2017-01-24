using System.Diagnostics;

namespace ConsoleApplication
{
    //Suports only one graph for the meantime
    public class Plotter
    {
        #region Fields
            

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
            DataFilePath = dataFilePath;
            KeyTitle = keyTitle;
            OutputFilePath = outputFilePath;
            SettingsFilePath = settingsFilePath;
            Title = title;
            XLabel = xLabel;
            YLabel = yLabel;
            XTics = xTics;
        }

        #region Methods

        public Process Plot()
        {
            var commandString = $"\"filename='{DataFilePath}';" +
                                  $"keyTitle='{KeyTitle}';" +
                                  $"output='{OutputFilePath}';" +
                                  $"xLabel='{XLabel}';" +
                                  $"yLabel='{YLabel}';" +
                                  $"title='{Title}';" +
                                  $"xTics='{XTics}'\"";

            var gnuplotCommand = new ShellCommand("/usr/bin/gnuplot",'e', commandString, SettingsFilePath);

            gnuplotCommand.Execute();
            return gnuplotCommand.CorrespondingProcess;
        }   
        #endregion
    }
}