using System;
using CitnDev.System;
using NUnit.Framework;

namespace CitnDev.System_Test.DumpObjectTestSuite
{
    [TestFixture]
    public class DumpValueTypeTests
    {
        [Test]
        public void DumpBooleanValueType()
        {
            string dumpIntText = DumpObject.Dump("Bool", true, 0, "\t");
            Assert.AreEqual("- Bool = True", dumpIntText);
        }

        [Test]
        public void DumpDateTimeValueType()
        {
            string dumpIntText = DumpObject.Dump("Date",DateTime.Today, 0, "\t");
            Assert.AreEqual("- Date = " + DateTime.Today, dumpIntText);
        }

        [Test]
        public void DumpIntValueType()
        {
            string dumpIntText = DumpObject.Dump("Int", 5, 0, "\t");
            Assert.AreEqual("- Int = 5", dumpIntText);
        }

        [Test]
        public void DumpNullableWithValue()
        {
            int? value = 10;
            string dumpIntText = DumpObject.Dump("Nullable",value, 0, "\t");
            Assert.AreEqual("- Nullable = 10", dumpIntText);
        }

        [Test]
        public void DumpNullableWithoutValue()
        {
            int? value = null;
// ReSharper disable ExpressionIsAlwaysNull
            string dumpIntText = DumpObject.Dump("Nullable", value, 0, "\t");
// ReSharper restore ExpressionIsAlwaysNull
            Assert.AreEqual("- Nullable = " + DumpObject.NullRepresentation, dumpIntText);
        }

        [Test]
        public void DumpStringValueType()
        {
            string dumpIntText = DumpObject.Dump("String","coucou", 0, "\t");
            Assert.AreEqual("- String = coucou", dumpIntText);
        }
    }
}