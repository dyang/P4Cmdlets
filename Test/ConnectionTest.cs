using NUnit.Framework;
using P4Cmdlets.Core;
using P4Cmdlets.Test.Fixture;

namespace P4Cmdlets.Test
{
    [TestFixture]
    public class ConnectionTest
    {
        [Test]
        public void ShouldConnectAndDisconnect()
        {
            using (var p4Fixture = new P4Fixture())
            {
                p4Fixture.SetUpServer();
                p4Fixture.SetUpClient();
                P4 p4 = new P4();
                Connection conn = p4.Connect(P4Fixture.Host, P4Fixture.Port,  P4Fixture.User, P4Fixture.Password, P4Fixture.Client);
                Assert.IsTrue(conn.IsValidConnection);

                p4.Disconnect(conn);
            }
        }
    }
}
