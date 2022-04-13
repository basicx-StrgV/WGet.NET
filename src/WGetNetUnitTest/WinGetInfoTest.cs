using NUnit.Framework;
using WGetNET;

namespace WGetNetUnitTest
{
    public class WinGetInfoTest
    {
        private WinGetInfo _winGetInfo;

        [SetUp]
        public void Setup()
        {
            _winGetInfo = new WinGetInfo();
        }

        [Test]
        public void WinGetInstalled()
        {
            //WinGet needs to be installed.
            Assert.IsTrue(_winGetInfo.WinGetInstalled);
        }

        [Test]
        public void WinGetVersion()
        {
            //A version number needs to be set.
            Assert.AreNotEqual(string.Empty, _winGetInfo.WinGetVersion);
        }
    }
}