using System;
using P4API;

namespace P4Cmdlets.Core
{
    public class Connection
    {
        private readonly P4Connection _p4;

        public Connection(P4Connection p4)
        {
            _p4 = p4;
        }

        public bool IsValidConnection
        {
            get { return _p4.IsValidConnection(true, true);}
        }

        public void Disconnect()
        {
            _p4.Disconnect();
        }
    }
}
