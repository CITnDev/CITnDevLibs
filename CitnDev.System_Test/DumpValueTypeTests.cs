using System;
using CitnDev.System;
using NUnit.Framework;

namespace CitnDev.System_Test
{
    [TestFixture]
    public class DumpValueTypeTests
    {
        [Test]
        public void DumpBooleanValueType()
        {
            string dumpIntText = DumpObject.Dump(true, 0, "\t");
            Assert.AreEqual("- True", dumpIntText);
        }

        [Test]
        public void DumpDateTimeValueType()
        {
            string dumpIntText = DumpObject.Dump(DateTime.Today, 0, "\t");
            Assert.AreEqual("- " + DateTime.Today, dumpIntText);
        }

        [Test]
        public void DumpIntValueType()
        {
            string dumpIntText = DumpObject.Dump(5, 0, "\t");
            Assert.AreEqual("- 5", dumpIntText);
        }

        [Test]
        public void DumpNullableWithValue()
        {
            int? value = 10;
            string dumpIntText = DumpObject.Dump(value, 0, "\t");
            Assert.AreEqual("- 10", dumpIntText);
        }

        [Test]
        public void DumpNullableWithoutValue()
        {
            int? value = null;
// ReSharper disable ExpressionIsAlwaysNull
            string dumpIntText = DumpObject.Dump(value, 0, "\t");
// ReSharper restore ExpressionIsAlwaysNull
            Assert.AreEqual("- " + DumpObject.NullRepresentation, dumpIntText);
        }

        [Test]
        public void DumpStringValueType()
        {
            string dumpIntText = DumpObject.Dump("coucou", 0, "\t");
            Assert.AreEqual("- coucou", dumpIntText);
        }
    }
}