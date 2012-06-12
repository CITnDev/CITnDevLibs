using System;
using NUnit.Framework;

namespace CitnDev.System_Test
{
    public class DumpObjectTests
    {
        public class ObjectWithValueTypeTest
        {
            public ObjectWithValueTypeTest()
            {
                PublicProperty2 = null;

                PublicProperty = "publicProp";
                PublicField = "publicField";

                ProtectedProperty = 10;
                ProtectedField = -10;

                PrivateProperty = DateTime.Today;
                _privateField = DateTime.Today.AddDays(-1);
            }

            public string PublicProperty { get; set; }
            public decimal? PublicProperty2 { get; set; }
            protected long ProtectedProperty { get; set; }
            private DateTime PrivateProperty { get; set; }

            public string PublicField;
            protected long ProtectedField;
            private DateTime _privateField;
        }

        public class ObjectWithObjectTest
        {
            public ObjectWithObjectTest()
            {
                PublicProperty = "publicProp";
                Instance = new ObjectWithValueTypeTest();
            }

            public string PublicProperty { get; set; }
            public ObjectWithValueTypeTest Instance { get; set; }
            public ObjectWithValueTypeTest NullValue { get; set; }
        }

        [Test]
        public void DumpObjectWithValueTyeTest()
        {
            var expectedString = "+ " + typeof(ObjectWithValueTypeTest) + Environment.NewLine +
                                 "\t- PublicProperty = publicProp" + Environment.NewLine +
                                 "\t- PublicProperty2 = " + CitnDev.System.DumpObject.NullRepresentation;

            var valueTest = new ObjectWithValueTypeTest();
            var dumpText = CitnDev.System.DumpObject.Dump(valueTest, 0, "\t");

            Assert.AreEqual(expectedString, dumpText);
        }

        [Test]
        public void DumpObjectWithObjectTest()
        {
            var expectedString = "+ " + typeof(ObjectWithObjectTest) + Environment.NewLine +
                                 "\t- PublicProperty = publicProp" + Environment.NewLine +
                                 "\t+ Instance = " + typeof(ObjectWithValueTypeTest) + Environment.NewLine +
                                 "\t\t- PublicProperty = publicProp" + Environment.NewLine +
                                 "\t\t- PublicProperty2 = <null>" + Environment.NewLine +
                                 "\t+ NullValue = <null>";

            var valueTest = new ObjectWithObjectTest();
            var dumpText = CitnDev.System.DumpObject.Dump(valueTest, 0, "\t");

            Assert.AreEqual(expectedString, dumpText);
        }
    }
}
