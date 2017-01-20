using System.Diagnostics;

namespace ConsoleApplication
{
    public class ShellCommand
    {
        #region Properties
            public string Command { get; }
            public Process CorrespondingProcess { get; private set;}
        #endregion

        public ShellCommand(string command)
        {
            Command = command;
        }

        #region Methods
            public void Execute()
            {
                CorrespondingProcess = new Process();
                CorrespondingProcess.StartInfo.FileName = "/bin/bash";
                CorrespondingProcess.StartInfo.Arguments = "-c \" " + Command + " \"";
                CorrespondingProcess.StartInfo.UseShellExecute = false; 
                CorrespondingProcess.StartInfo.RedirectStandardOutput = true;
                CorrespondingProcess.Start ();
            }

            private void Execute(string command)
            {
                CorrespondingProcess = new Process();
                CorrespondingProcess.StartInfo.FileName = "/bin/bash";
                CorrespondingProcess.StartInfo.Arguments = "-c \" " + command + " \"";
                CorrespondingProcess.StartInfo.UseShellExecute = false; 
                CorrespondingProcess.StartInfo.RedirectStandardOutput = true;
                CorrespondingProcess.Start ();
            }

            public void Stop()
            {
                Execute($"killall {CorrespondingProcess.ProcessName}");
            }
        #endregion
    }  
}