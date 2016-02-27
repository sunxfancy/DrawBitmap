using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawBitmap.MainClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MulticastNetWork;
namespace DrawBitmap.MainClass.Tests
{
    [TestClass()]
    public class ServerAPITests
    {
        public ServerAPITests()
        {
            ServerAPI.ServerIP = "111.195.198.161";
            ServerAPI.ServerPort = 9999;
            ServerAPI.client = new SendingClient(ServerAPI.ServerIP, ServerAPI.ServerPort);
        }

        [TestMethod()]
        public void RegisterTest()
        {
            string usertest = "sxf";
            string passtest = "12211037";
            string nicktest = "sxf";
            int expected = -1;//预期的值
            int actual;//实际的值
            actual = ServerAPI.Register(usertest, passtest, nicktest);
            Assert.AreEqual(expected, actual);
        }

        string usertest = "sxf";
        string passtest = "123";
        int id = 20;

        [TestMethod()]
        public void LoginTest()
        {
            LoginReturn actual;//实际的值
            actual = ServerAPI.Login(usertest, passtest);
            Assert.AreNotEqual(null, actual);
        }

        [TestMethod()]
        public void CheckingTest()
        {
            List<UserMessage> actual;//实际的值
            actual = ServerAPI.Checking(id);
        }


        [TestMethod()]
        public void InviteFriendsTest()
        {
            int plugin_id = 1;
            SortedSet<int> id_list = new SortedSet<int>();
            id_list.Add(44);
            var v = ServerAPI.InviteFriends(plugin_id, id, id_list);
            Assert.AreEqual(true, v);
        }

        [TestMethod()]
        public void TestNameTest()
        {
            int a = ServerAPI.TestName("sxf");
            int b = ServerAPI.TestName("www");
            Assert.AreEqual(-1, a);
            Assert.AreEqual(1, b);
        }

        [TestMethod()]
        public void SearchFriendsTest()
        {
            List<User> u = ServerAPI.SearchFriends("wax");
            List<User> p = ServerAPI.SearchFriends("www");
            Assert.AreEqual(1, u.Count);
            Assert.AreEqual(0, p.Count);
        }

        [TestMethod()]
        public void AddFriendTest()
        {
            int v_id = 51;
            bool v = ServerAPI.AddFriend(v_id,id);
            Assert.AreEqual(false,v);

            v_id = 519;
            v = ServerAPI.AddFriend(v_id, id);
            Assert.AreEqual(false, v);
        }

        [TestMethod()]
        public void UpdataMyInfoTest()
        {
            User u = new User();
            u.name = "wxs";
            u.user_id = 51;
            u.nickname = "旭日东升";
            u.ext = null;
            bool v = ServerAPI.UpdataMyInfo(u);
            Assert.AreEqual(true, v);

            u.name = "ppp";
            u.user_id = 199;
            u.nickname = "sss";
            u.ext = null;
            v = ServerAPI.UpdataMyInfo(u);
            Assert.AreEqual(false, v);

            u.name = null;
            u.user_id = 9000;
            u.nickname = "sss";
            u.ext = null;

            v = ServerAPI.UpdataMyInfo(u);
            Assert.AreEqual(false, v);

            u.name = "ggg";
            u.user_id = 0;
            u.nickname = "sss";
            u.ext = null;

            v = ServerAPI.UpdataMyInfo(u);
            Assert.AreEqual(false, v);
        }

        [TestMethod()]
        public void UpdataMyPasswordTest()
        {
            int id = 51;
            String opass = "ww";
            String npass = "www";

            int v = ServerAPI.UpdataMyPassword(id,opass,npass);
            Assert.AreEqual(1, v);

            ServerAPI.UpdataMyPassword(id, opass, npass);
            Assert.AreEqual(-1, v);

            v = ServerAPI.UpdataMyPassword(id, npass, opass);
            Assert.AreEqual(1, v);
        }

        [TestMethod()]
        public void GetFriendDataTest()
        {
            User v = ServerAPI.GetFriendData(20);
            Assert.AreEqual("sxf", v.name);
            v = ServerAPI.GetFriendData(293);
            Assert.AreEqual(null, v);
        }

        [TestMethod()]
        public void AgreeAddFriendTest()
        {
            int id_1 = 51;
            int id_2 = 20;
            bool v = ServerAPI.AgreeAddFriend(id_1,id_2,1);
            Assert.AreEqual(true, v);
            v = ServerAPI.AgreeAddFriend(id_1, id_2, 1);
            Assert.AreEqual(false, v);
            v = ServerAPI.AgreeAddFriend(id_1, id_2, 0);
            Assert.AreEqual(true, v);
        }
    }
}
