using System;
using System.IO;
using P4Cmdlets.Test.Util;

namespace P4Cmdlets.Test.Fixture
{
    public class P4Fixture : IDisposable
    {
        public const string Host = "127.0.0.1";
        public const string Port = "1667";
        public const string P4Port = Host + ":" + Port;
        public const string User = "Administrator";
        public const string Password = "password";
        public const string Client = "P4CmdletsClient";
        private DirectoryInfo _clientPathDir;
        private DirectoryInfo _serverPathDir;

        #region IDisposable Members

        public void Dispose()
        {
            TearDownClient();
            TearDownServer();
        }

        #endregion

        public void SetUpServer()
        {
            // create test repo folder
            _serverPathDir = FileUtil.CreateTempDirectory();

            // copy db to test repo folder
            string dbPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Data\P4");
            FileUtil.CopyDirectory(new DirectoryInfo(dbPath), _serverPathDir);

            // execute p4d -r -p cmd to start p4d
            Command.Named("p4d").WithArguments("-r", _serverPathDir.FullName, "-p", Port).Run();

            // verify p4d is started
            if (!Command.Named("p4").WithArguments("-p", P4Port, "info").WaitForCommandToSucceed())
                throw new Exception(
                    "Unable to start p4 server in one minute.  Check your test data & environment manually please");
        }

        public void SetUpClient()
        {
            _clientPathDir = FileUtil.CreateTempDirectory();
            string clientSpec = "Client:    " + Client + "\n"
                                + "Host:    127.0.0.1\n"
                                + "Root:    " + _clientPathDir.FullName + "\n"
                                + "Options:    noallwrite noclobber nocompress unlocked nomodtime normdir\n"
                                + "SubmitOptions:    submitunchanged\n"
                                + "LineEnd:    local\n"
                                + "View:\n"
                                + @"    //depot/... //" + Client + "/...";

            var result = Command.Named("p4").WithArguments("-p", P4Port, "client", "-i").WithInput(clientSpec).RunAndWaitForExit();
            if (!result.IsSuccessful)
            {
                throw new Exception(string.Format("Unable to set up client.  Exit code: {0}, error: {1}",
                                                  result.ExitCode, result.Error));
            }
        }

        public void TearDownServer()
        {
            Command.Named("p4").WithArguments("-p", P4Port, "admin", "stop").RunAndWaitForExit();

            if (_serverPathDir != null && _serverPathDir.Exists)
                Command.WaitForCommandToSucceed(DeleteServerPath);
        }

        // DY - refactor out the duplication below
        private bool DeleteServerPath()
        {
            try
            {
                _serverPathDir.Delete(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void TearDownClient()
        {
            if (_clientPathDir != null && _clientPathDir.Exists)
                Command.WaitForCommandToSucceed(new Func<bool>(DeleteClientPath));
        }

        private bool DeleteClientPath()
        {
            try
            {
                _clientPathDir.Delete(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}