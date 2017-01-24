using System.Diagnostics;

namespace ConsoleApplication
{
    public class ShellCommand
    {
        #region Properties
            public Process CorrespondingProcess { get; private set;}
            public string Command { get; }
            public string Host { get; }
            public char SwitchCharacter { get; }
            public string AdditionalArgument { get; }
        #endregion

        public ShellCommand(string host, char switchCharacter, string command, string additionalArgument)
        {
            Host = host;
            SwitchCharacter = switchCharacter;
            Command = command;
            AdditionalArgument = additionalArgument;
        }

        #region Methods
            public void Execute()
            {
                CorrespondingProcess = new Process();
                CorrespondingProcess.StartInfo.FileName = Host;
                CorrespondingProcess.StartInfo.Arguments = $"-{SwitchCharacter} {Command} {AdditionalArgument}";
                CorrespondingProcess.StartInfo.UseShellExecute = false; 
                CorrespondingProcess.StartInfo.RedirectStandardOutput = true;
                CorrespondingProcess.Start ();
            }

            public void Stop()
            {
                CorrespondingProcess = new Process();
                CorrespondingProcess.StartInfo.FileName = "/bin/bash";
                CorrespondingProcess.StartInfo.Arguments = $"-c killall \"{CorrespondingProcess.ProcessName} \"";
                CorrespondingProcess.StartInfo.UseShellExecute = false;
                CorrespondingProcess.StartInfo.RedirectStandardOutput = true;
                CorrespondingProcess.Start();
            }
        #endregion
    }  
}