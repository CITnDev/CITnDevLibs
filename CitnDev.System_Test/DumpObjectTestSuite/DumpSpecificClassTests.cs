using System.Globalization;
using CitnDev.System;
using NUnit.Framework;

namespace CitnDev.System_Test.DumpObjectTestSuite
{
    [TestFixture]
    public class DumpSpecificClassTests
    {
        [Test]
// ReSharper disable InconsistentNaming
        public void DumpCultureInfo_fr()
// ReSharper restore InconsistentNaming
        {
            var culture = CultureInfo.GetCultureInfo("fr");
            string dumpText = DumpObject.Dump("Culture",culture, 0, "\t");
            Assert.AreEqual("- Culture = fr", dumpText);
        }

        [Test]
// ReSharper disable InconsistentNaming
        public void DumpCultureInfo_frFR()
// ReSharper restore InconsistentNaming
        {
            var culture = CultureInfo.GetCultureInfo("fr-FR");
            string dumpText = DumpObject.Dump("Culture", culture, 0, "\t");
            Assert.AreEqual("- Culture = fr-FR", dumpText);
        }
    }
}