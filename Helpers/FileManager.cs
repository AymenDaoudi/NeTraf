using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace NeTraf
{
    public static class FileManager
    {
        #region Files
            
            public const string NetstatOuputFileName = "NetstatOutput";
            public const string IptrafOutputFileName = "IptrafOutput";

            //This is unpleasing, to fix later !!
            public const string RatePlotSettingsFileName = "RatePlotSettings.txt";
            public const string DataPlotSettingsFileName = "DataPlotSettings.txt";

            public const string TotalTrafficPacketsOutputFileName    = "Total_Traffic_Packets";
            public const string TotalTrafficBytesOutputFileName      = "Total_Traffic_Bytes";
            public const string TotalTrafficRateOutputFileName       = "Total_Traffic_Rate";
            public const string IncomingTrafficPacketsOutputFileName = "Incoming_Traffic_Packets";
            public const string OutgoingTrafficPacketsOutputFileName = "Outgoing_Traffic_Packets";
            public const string IncomingTrafficBytesOutputFileName   = "Incoming_Traffic_Bytes";
            public const string OutgoingTrafficBytesOutputFileName   = "Outgoing_Traffic_Bytes";
            public const string IncomingTrafficRateOutputFileName    = "Incoming_Traffic_Rate";
            public const string OutgoingTrafficRateOutputFileName    = "Outgoing_Traffic_Rate";

        #endregion

        #region Folders

            public const string RawOutputFilesFolderName = "RawOutput";
            public const string GraphicalOutputFilesFolderName = "GraphicalOutput";

        #endregion

        #region Methods
            public static void CreateFileIfInexists(string filePath)
            {
               using (var fileStream = new FileStream(filePath, 
                                               FileMode.Create, 
                                               FileAccess.ReadWrite, 
                                               FileShare.ReadWrite))
                                               {

                                               }
            }

            public static void CreateFolderIfInexists(string folderPath)
            {
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            }

            public static string GetPlotSettingsFilePaths(string fileName)
            {
                var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;
                var applicationBaseDirectory = Directory.GetParent(applicationBasePath);
                return Path.Combine(applicationBasePath,"SettingFiles",fileName);
            }
        #endregion
    }
}