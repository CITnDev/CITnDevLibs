using System;
using System.Collections.Generic;
using CitnDev.System;
using NUnit.Framework;

namespace CitnDev.System_Test.DumpObjectTestSuite
{
    public class DumpCollectionTests
    {
        [Test]
        public void DumpDictionaryTests()
        {
            var dict = new Dictionary<int, int>();

            for (int i = 0; i < 10; i++)
            {
                dict[i] = 10 - i;
            }

            var dumpText = DumpObject.Dump("Dictionary", dict, 0, "\t");

            var expected = "- Dictionary = {" + dict.Count + " items}" + Environment.NewLine +
                           "\t- 0 = 10" + Environment.NewLine +
                           "\t- 1 = 9" + Environment.NewLine +
                           "\t- 2 = 8" + Environment.NewLine +
                           "\t- 3 = 7" + Environment.NewLine +
                           "\t- 4 = 6" + Environment.NewLine +
                           "\t- 5 = 5" + Environment.NewLine +
                           "\t- 6 = 4" + Environment.NewLine +
                           "\t- 7 = 3" + Environment.NewLine +
                           "\t- 8 = 2" + Environment.NewLine +
                           "\t- 9 = 1";

            Assert.AreEqual(expected, dumpText);
        }

        [Test]
        public void DumpListTests()
        {
            var list = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                list.Add(i);
            }

            var dumpText = DumpObject.Dump("List", list, 0, "\t");

            var expected = "- List = {" + list.Count + " items}" + Environment.NewLine +
                           "\t- [0] = 0" + Environment.NewLine +
                           "\t- [1] = 1" + Environment.NewLine +
                           "\t- [2] = 2" + Environment.NewLine +
                           "\t- [3] = 3" + Environment.NewLine +
                           "\t- [4] = 4" + Environment.NewLine +
                           "\t- [5] = 5" + Environment.NewLine +
                           "\t- [6] = 6" + Environment.NewLine +
                           "\t- [7] = 7" + Environment.NewLine +
                           "\t- [8] = 8" + Environment.NewLine +
                           "\t- [9] = 9";

            Assert.AreEqual(expected, dumpText);
        }
    }
}
