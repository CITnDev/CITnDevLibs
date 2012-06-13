using System;
using NUnit.Framework;

namespace CitnDev.System_Test.DumpObjectTestSuite
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
// ReSharper disable UnusedAutoPropertyAccessor.Local
            private DateTime PrivateProperty { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local

            public string PublicField;
            protected long ProtectedField;
// ReSharper disable NotAccessedField.Local
            private DateTime _privateField;
// ReSharper restore NotAccessedField.Local
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
            var expectedString = "+ Object = " + typeof(ObjectWithValueTypeTest) + Environment.NewLine +
                                 "\t- PublicProperty = publicProp" + Environment.NewLine +
// ReSharper disable RedundantNameQualifier
                                 "\t- PublicProperty2 = " + CitnDev.System.DumpObject.NullRepresentation;
// ReSharper restore RedundantNameQualifier

            var valueTest = new ObjectWithValueTypeTest();
// ReSharper disable RedundantNameQualifier
            var dumpText = CitnDev.System.DumpObject.Dump("Object",valueTest, 0, "\t");
// ReSharper restore RedundantNameQualifier

            Assert.AreEqual(expectedString, dumpText);
        }

        [Test]
        public void DumpObjectWithObjectTest()
        {
            var expectedString = "+ Object = " + typeof(ObjectWithObjectTest) + Environment.NewLine +
                                 "\t- PublicProperty = publicProp" + Environment.NewLine +
                                 "\t+ Instance = " + typeof(ObjectWithValueTypeTest) + Environment.NewLine +
                                 "\t\t- PublicProperty = publicProp" + Environment.NewLine +
                                 "\t\t- PublicProperty2 = <null>" + Environment.NewLine +
                                 "\t+ NullValue = <null>";

            var valueTest = new ObjectWithObjectTest();
// ReSharper disable RedundantNameQualifier
            var dumpText = CitnDev.System.DumpObject.Dump("Object", valueTest, 0, "\t");
// ReSharper restore RedundantNameQualifier

            Assert.AreEqual(expectedString, dumpText);
        }
    }
}
