using P4API;

namespace P4Cmdlets.Core
{
    public class P4
    {
        public Connection Connect(string host, string port, string username, string password, string clientName)
        {
            var p4 = new P4Connection();
            p4.Host = host;
            p4.Port = port;
            p4.User = username;
            p4.Password = password;
            p4.Client = clientName;
            p4.Connect();
            return new Connection(p4);
        }

        public void Disconnect(Connection conn)
        {
            conn.Disconnect();
        }
    }
}
