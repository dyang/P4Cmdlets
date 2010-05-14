using NUnit.Framework;
using P4Cmdlets.Core;
using P4Cmdlets.Test.Fixture;

namespace P4Cmdlets.Test
{
    [TestFixture]
    public class ConnectionTest
    {
        private P4Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new P4Fixture();
            _fixture.SetUpServer();
            _fixture.SetUpClient();
        }

        [TearDown]
        public void TearDown()
        {
            _fixture.Dispose();
        }

        [Test]
        public void ShouldConnectAndDisconnect()
        {
            using (P4 p4 = P4.Connect(P4Fixture.Host, P4Fixture.Port, P4Fixture.User, P4Fixture.Password, P4Fixture.Client))
            {
                Assert.IsTrue(p4.IsValidConnection);
            }
        }

        [Test]
        public void ShouldCreateChangelist()
        {
            using (P4 p4 = P4.Connect(P4Fixture.Host, P4Fixture.Port, P4Fixture.User, P4Fixture.Password, P4Fixture.Client))
            {
                Changelist pending = p4.CreateChangelist("Description");
                Assert.AreEqual("Description", pending.Description);
                Assert.IsTrue(pending.Id > 0);
            }
        }

        [Test]
        public void ShouldAddFileToPendingChangelist()
        {
            using (P4 p4 = P4.Connect(P4Fixture.Host, P4Fixture.Port, P4Fixture.User, P4Fixture.Password, P4Fixture.Client))
            {
                Changelist pending = p4.CreateChangelist("Description");
                Assert.AreEqual(0, p4.Describe(pending).NumberOfFiles);

                p4.AddFile(pending, _fixture.Touch("file"));
                 Assert.AreEqual(1, p4.Describe(pending).NumberOfFiles);
            }
        }
    }
}