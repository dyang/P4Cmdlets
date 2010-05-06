using System;
using System.Diagnostics;
using System.IO;

namespace P4Cmdlets.Test.Util
{
    public class Command
    {
        private const int TWENTY_SECONDS = 20 * 1000;
        private const int SLEEP_INTERVAL = 400;

        public static void Run(string command, params string[] args)
        {
            Process.Start(CreateStartInfo(command, args));
        }

        public static CommandResult RunAndWaitForExit(string command, params string[] args)
        {
            // DY - redirect error/warning?
             CommandResult result = new CommandResult();

            using (Process process = Process.Start(CreateStartInfo(command, args)))
            {
                using (StreamReader outputReader = process.StandardOutput)
                {
                    process.WaitForExit();
                    result.Output = outputReader.ReadToEnd();
                    result.ExitCode = process.ExitCode;

                    Debug.WriteLine(string.Format("Running command {0} with arguments: {1}", command, string.Join(" ", args)));
                    Debug.WriteLine("Output: " + result.Output);
                    Debug.WriteLine("ExitCode: " + result.ExitCode);
                }
            }
            return result;
        }

        private static ProcessStartInfo CreateStartInfo(string command, string[] args)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = command;
            startInfo.Arguments = string.Join(" ", args);
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            return startInfo;
        }

        public class CommandResult
        {
            public string Output { get; set; }
            public int ExitCode { get; set; }

            public bool IsSuccessful
            {
                get { return ExitCode == 0; }
            }
        }

        // DY - see if there is anyway to remove the dup below
        public static bool WaitForCommandToSucceed(string command, params string[] args)
        {
            int baseline = System.Environment.TickCount;
            while (System.Environment.TickCount - baseline <= TWENTY_SECONDS)
            {
                if (Command.RunAndWaitForExit(command, args).IsSuccessful)
                    return true;
                System.Threading.Thread.Sleep(SLEEP_INTERVAL);
            }
            return false;
        }

        // DY - see if there is anyway to remove the dup
        public static bool WaitForCommandToSucceed(Func<bool> command)
        {
            int baseline = System.Environment.TickCount;
            while (System.Environment.TickCount - baseline <= TWENTY_SECONDS)
            {
                if (command()) return true;
                System.Threading.Thread.Sleep(SLEEP_INTERVAL);
            }
            return false;
        }


    }
}
