using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExternalMailServerChange001;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_ThunderbirdProfileChoicer()
        {


            var exp = true;
            var act = true;
            Assert.AreEqual(exp, act);
        }
    }
}
