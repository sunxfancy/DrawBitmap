using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DrawBitmap.MainClass;
using DrawBitmap;
using MulticastNetWork;

namespace UnitTestProject
{
    [TestClass]
    public class NetWorkTest
    {
        public NetWorkTest()
        {
            InitAppData();
        }

        private void InitAppData()
        {
            ServerAPI.ServerIP = "172.16.49.192";
            ServerAPI.ServerIP = "127.0.0.1";
            //ServerAPI.ServerIP = "172.16.120.30";
            ServerAPI.ServerPort = 9999;
            ServerAPI.client = new SendingClient(ServerAPI.ServerIP, ServerAPI.ServerPort);
        }

        [TestMethod]
        public void TestRegister()
        {
            
            string usertest = "孙笑凡";
            string passtest = "12211037";
            string nicktest = "sxf";
            int expected = 0;//预期的值
            int actual;//实际的值
            actual = ServerAPI.Register(usertest, passtest, nicktest);
            Assert.AreEqual(expected, actual);

        }


    }
}
