using NUnit.Framework;
using P4Cmdlets.Core;
using P4Cmdlets.Test.Fixture;

namespace P4Cmdlets.Test
{
    [TestFixture]
    public class ConnectionTest
    {
        [Test]
        public void ShouldLoginAndLogout()
        {
            using (var p4Fixture = new P4Fixture())
            {
                p4Fixture.SetUpServer();
                P4 p4 = new P4();
                Connection conn = p4.Login("username", "password", "client");
                Assert.IsTrue(conn.IsValidConnection);

                p4.Logout(conn);
                Assert.IsFalse(conn.IsValidConnection);
            }
        }
    }
}
