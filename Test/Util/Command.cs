using System;
using System.Diagnostics;
using System.IO;

namespace P4Cmdlets.Test.Util
{
    public class Command
    {
        private readonly string _command;
        private string[] _args;
        private string _input;
        private const int TWENTY_SECONDS = 20 * 1000;
        private const int SLEEP_INTERVAL = 400;

        private Command(string command)
        {
            _command = command;
        }

        public static Command Named(string command)
        {
            return new Command(command);
        }

        public Command WithArguments(params string[] args)
        {
            _args = args;
            return this;
        }

        public Command WithInput(string input)
        {
            _input = input;
            return this;
        }

        public void Run()
        {
            using (Process process = Process.Start(CreateStartInfo(_command, _args)))
            {
                if (!string.IsNullOrEmpty(_input))
                {
                    using (StreamWriter inputWriter = process.StandardInput)
                    {
                        inputWriter.Write(_input);
                    }
                }    
            }
        }

        public CommandResult RunAndWaitForExit()
        {
            CommandResult result = new CommandResult();
            using (Process process = Process.Start(CreateStartInfo(_command, _args)))
            {
                if (!string.IsNullOrEmpty(_input))
                {
                    using (StreamWriter inputWriter = process.StandardInput)
                    {
                        inputWriter.Write(_input);
                    }
                }

                process.WaitForExit();
                result.ExitCode = process.ExitCode;
                using (StreamReader outputReader = process.StandardOutput)
                {
                    result.Output = outputReader.ReadToEnd();
                }
                using (StreamReader errorReader = process.StandardError)
                {
                    result.Error = errorReader.ReadToEnd();
                }
                return result;
            }
        }

        private ProcessStartInfo CreateStartInfo(string command, string[] args)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = command;
            startInfo.Arguments = string.Join(" ", args);
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            return startInfo;
        }

        public class CommandResult
        {
            public string Output { get; set; }
            public string Error { get; set; }
            public int ExitCode { get; set; }

            public bool IsSuccessful
            {
                get { return ExitCode == 0; }
            }
        }

        // DY - see if there is anyway to remove the dup below
        public bool WaitForCommandToSucceed()
        {
            int baseline = Environment.TickCount;
            while (Environment.TickCount - baseline <= TWENTY_SECONDS)
            {
                if (RunAndWaitForExit().IsSuccessful)
                    return true;
                System.Threading.Thread.Sleep(SLEEP_INTERVAL);
            }
            return false;
        }

        // DY - see if there is anyway to remove the dup
        public static bool WaitForCommandToSucceed(Func<bool> command)
        {
            int baseline = Environment.TickCount;
            while (Environment.TickCount - baseline <= TWENTY_SECONDS)
            {
                if (command()) return true;
                System.Threading.Thread.Sleep(SLEEP_INTERVAL);
            }
            return false;
        }

    }
}
