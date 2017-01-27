using System.Diagnostics;

namespace NeTraf
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
                if (AdditionalArgument == "") CorrespondingProcess.StartInfo.Arguments = $"-{SwitchCharacter} {Command}";
                else
                {
                    CorrespondingProcess.StartInfo.Arguments = $"-{SwitchCharacter} {Command} {AdditionalArgument}";
                }
                CorrespondingProcess.StartInfo.UseShellExecute = false; 
                CorrespondingProcess.StartInfo.RedirectStandardOutput = true;
                CorrespondingProcess.Start ();
            }

            public static void Stop(string commandName)
            {
                var processes = Process.GetProcessesByName(commandName);
                foreach (var process in processes)
                {
                    process.Kill();
                    process.WaitForExit();
                } 
            }
        #endregion
    }  
}