using NUnit.Framework;
using System.Collections.Generic;
using System.Security;
using WGetNET;

namespace WGetNetUnitTest
{
    public class WinGetSourceManagerTest
    {
        private WinGetSourceManager _winGetSourceManager;

        [SetUp]
        public void Setup()
        {
            _winGetSourceManager = new WinGetSourceManager();
        }

        [Test]
        public void GetInstalledSources()
        {
            //There should allways be one source.
            Assert.GreaterOrEqual(_winGetSourceManager.GetInstalledSources().Count, 1);
        }

        [Test]
        public void AddSourceOne()
        {
            try
            {
                bool result = _winGetSourceManager.AddSource("test", "localhost");
                Assert.IsTrue(result);
            }
            catch (SecurityException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void AddSourceTwo()
        {
            try
            {
                bool result = _winGetSourceManager.AddSource("test", "localhost", "test");
                Assert.IsTrue(result);
            }
            catch (SecurityException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void AddSourceThree()
        {
            WinGetSource source = new WinGetSource()
            {
                SourceName = "test",
                SourceUrl = "localhost",
                SourceType = "test"
            };

            try
            {
                bool result = _winGetSourceManager.AddSource(source);
                Assert.IsTrue(result);
            }
            catch (SecurityException)
            {
                Assert.Pass();
            }
        }
    }
}