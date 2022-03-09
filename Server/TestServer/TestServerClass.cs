using NUnit.Framework;
using Server.Models;
using System.Net;

namespace TestServer
{
    [TestFixture]
    public class TestServerClass
    {
        [Test]
        public void TestCanServerBeCreated ()
        {
            IServer server = new HTTPServer(12345,"0.0.0.0");
            Assert.IsNotNull(server);
            Assert.AreEqual(server.Port, 12345);
            Assert.AreEqual(server.Address, IPAddress.Parse("0.0.0.0"));
        }
    }
}
