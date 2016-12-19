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
            var exp = @"C:\Users\admin\AppData\Roaming\Thunderbird\profiles.ini"; 
            var act = Thunderbird.Address.ThunderbirdProfileChoicer;
            Assert.AreEqual(exp, act);
        }
        [TestMethod]
        public void Test_ThunderbirdProfileSettingFolder()
        {
            var exp = @"C:\Users\admin\AppData\Roaming\Thunderbird\";
            var act = Thunderbird.Address.ThunderbirdProfileSettingFolder;
            Assert.AreEqual(exp, act);
        }
    }
}
