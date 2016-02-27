using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawBitmap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace DrawBitmap.Tests
{
    [TestClass()]
    public class FriendTests
    {
        [TestMethod()]
        public void UpdatabyUserTest()
        {
            User u = new User();
            u.name = "sxf";
            u.nickname = "孙笑凡";
            u.ip = null;
            u.ext = new UserExt();
            u.ext.Age = 20;
            u.ext.Country = "中国";
            u.ext.Hometown = "河北唐山";
            u.ext.Introduce = "在下孙笑凡";
            u.ext.Level = 80;
            u.ext.Motto = "横笛笑看今古事";
            u.ext.Telephone = "13141474298";
            
            User p = new User(new Friend(u));
         //   Assert.AreEqual<User>(u,p);
            Assert.IsFalse(u.Equals(p));
        }
    }
}
