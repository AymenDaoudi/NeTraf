using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Console;

namespace NeTraf
{
    public class NetstatParser
    {
        #region Fields
            private const string _moreThanTwoSpacesPattern = @"\s{2,}";
            private List<string> _loggedLines;
            private FileStream _fileStream;
        #endregion
        #region Properties
            public string FilePath { get; }
        #endregion

        public NetstatParser(string filePath)
        {
            FilePath = filePath;
            _fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }


        #region Methods
            public List<uint> GetPortsFromNetstatOutput()
            {
                List<uint> ports = new List<uint>();
                try
                {
                    using (_fileStream)
                    {
                        using (var streamReader = new StreamReader(_fileStream, System.Text.Encoding.ASCII)) 
                        {
                            _loggedLines = streamReader.ReadAllLines().ToList();
                        }
                    }
                
                    var collectedData = SplitLinesByCollumns();
                    
                    var trafficSourceInfo = GetTrafficSourceColumn(collectedData);

                    ports = trafficSourceInfo.Select(sourceInfo => ExtractPortNumber(sourceInfo)).Distinct().ToList();
                }
                catch (FileNotFoundException exception)
                {
                    ports = null;
                    WriteLine($"The Netstat ouput file : {exception.FileName} was not found !");
                }
                
                return ports;
                
            }
                
            private List<List<string>> SplitLinesByCollumns()
            {
                var collectedData = new List<List<string>>();
                _loggedLines.ForEach(line => collectedData.Add(Regex.Split(line, _moreThanTwoSpacesPattern).ToList()));
                return collectedData;
            } 

            private List<string> GetTrafficSourceColumn(List<List<string>> collectedData)
            {
                return collectedData.Select(_ => _[2].Split(' ').Last()).ToList();
            }

            private static uint ExtractPortNumber(string sourceInfo)
            {
                string port = "";
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

        #endregion
    }
}