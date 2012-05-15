using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitnDev.System.Extensions;
using NUnit.Framework;

namespace CitnDev.System_Test.Extensions
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        public enum Enum1 { Ok = 0, Ko = 1 }
        public enum Enum2 { Ok = 0, Ko = 1 }

        public class BaseClass1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Enum1 Status { get; set; }
        }

        public class Class1 : BaseClass1
        {
            public List<string> Data { get; set; }
            public Dictionary<long, string> Dict { get; set; }
        }

        public class Class2
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Enum1 Status { get; set; }
            public Dictionary<long, string> Dict { get; set; }            
        }

        [Test]
        public void ObjetctExtensionToTest()
        {
            var class2 = new Class2 { Id = 1234, Name = "Class2", Dict = new Dictionary<long, string>(), Status = Enum1.Ko };
            var class1 = class2.To<Class1>();

            Assert.True(class1.Dict == class2.Dict);
            Assert.True(class1.Id == class2.Id);
            Assert.True(class1.Name == class2.Name);
            Assert.True((int)class1.Status == (int)class2.Status);
        }

    }
}
