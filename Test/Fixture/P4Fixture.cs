﻿using System;
using System.IO;
using System.Threading;
using P4Cmdlets.Test.Util;

namespace P4Cmdlets.Test.Fixture
{
    public class P4Fixture : IDisposable
    {
        private DirectoryInfo _serverPathDir;
        private const string PORT = "1667";
        private const string P4PORT = "127.0.0.1:" + PORT;

        public void SetUpServer()
        {
            // create test repo folder
            string serverPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _serverPathDir = Directory.CreateDirectory(serverPath);
            if (_serverPathDir == null || !_serverPathDir.Exists) 
                throw new Exception("Unable to create directory at " + serverPath);

            // copy db to test repo folder
            string dbPath = Path.Combine(System.Environment.CurrentDirectory, @"..\..\Data\P4");
            FileUtil.CopyDirectory(new DirectoryInfo(dbPath), new DirectoryInfo(serverPath));

            // execute p4d -r -p cmd to start p4d
            Command.Run("p4d", "-r", serverPath, "-p", PORT);

            // verify p4d is started
            if (!Command.WaitForCommandToSucceed("p4", "-p", P4PORT, "info"))
                throw new Exception("Unable to start p4 server in one minute.  Check your test data & environment manually please");

        }

        public void TearDownServer()
        {
            Command.RunAndWaitForExit("p4", "-p",  P4PORT, "admin", "stop");

            if (_serverPathDir != null && _serverPathDir.Exists)
                Command.WaitForCommandToSucceed(new Func<bool>(DeleteServerPath));
        }

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


        public void Dispose()
        {
            TearDownServer();
        }
    }
}