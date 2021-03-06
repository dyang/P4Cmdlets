﻿using System;
using P4API;

namespace P4Cmdlets.Core
{
    public class P4 : IDisposable
    {
        private readonly P4Connection _p4;

        private P4(P4Connection p4)
        {
            _p4 = p4;
        }

        public bool IsValidConnection
        {
            get { return _p4.IsValidConnection(true, true); }
        }

        public static P4 Connect(string host, string port, string username, string password, string clientName)
        {
            var p4 = new P4Connection();
            p4.Host = host;
            p4.Port = port;
            p4.User = username;
            p4.Password = password;
            p4.Client = clientName;
            p4.Connect();
            P4 wrapper = new P4(p4);
            return wrapper;
        }

        public Changelist CreateChangelist(string description)
        {
            return new Changelist(_p4.CreatePendingChangelist(description));
        }

        public void Dispose()
        {
            _p4.Dispose();
        }

        public void AddFile(Changelist pending, string file)
        {
            _p4.Run("add", "-c", pending.Id.ToString(), file);
        }

        public Description Describe(Changelist changelist)
        {
            P4UnParsedRecordSet result = _p4.RunUnParsed("describe", changelist.Id.ToString());
            return new Description(result.Messages);
        }
    }
}
