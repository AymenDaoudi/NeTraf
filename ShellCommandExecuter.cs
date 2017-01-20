using System.Diagnostics;

namespace ConsoleApplication
{
    public class ShellCommandExecuter
    {
        #region Properties
            public string Command { get; }
            public Process correspondingProcess { get; private set;}
        #endregion

        public ShellCommandExecuter(string command)
        {
            Command = command;
        }

        #region Methods
            public void Execute()
            {
                correspondingProcess = new Process();
                correspondingProcess.StartInfo.FileName = "/bin/bash";
                correspondingProcess.StartInfo.Arguments = "-c \" " + Command + " \"";
                correspondingProcess.StartInfo.UseShellExecute = false; 
                correspondingProcess.StartInfo.RedirectStandardOutput = true;
                correspondingProcess.Start ();
            }

            private void Execute(string command)
            {
                correspondingProcess = new Process();
                correspondingProcess.StartInfo.FileName = "/bin/bash";
                correspondingProcess.StartInfo.Arguments = "-c \" " + command + " \"";
                correspondingProcess.StartInfo.UseShellExecute = false; 
                correspondingProcess.StartInfo.RedirectStandardOutput = true;
                correspondingProcess.Start ();
            }

            public void Stop()
            {
                Execute($"killall {correspondingProcess.ProcessName}");
            }
        #endregion
    }  
}