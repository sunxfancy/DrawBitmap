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
    public class APITests
    {
        [TestMethod()]
        public void OpenImageFileTest()
        {
            String s = API.OpenImageFile();
            Assert.AreNotEqual(null, s);
        }

    }
}
