using System.Collections.Generic;
using System.Linq;
using CitnDev.System.Extensions;
using NUnit.Framework;

namespace CitnDev.System_Test.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void List100Page20()
        {
            var list = new List<int>();
            for (int i = 0; i < 100; i++) { list.Add(i); }

            var result = list.ToPages(20).ToList();

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(20, result[0].Count());
            Assert.AreEqual(20, result[1].Count());
            Assert.AreEqual(20, result[2].Count());
            Assert.AreEqual(20, result[3].Count());
            Assert.AreEqual(20, result[4].Count());
        }

        [Test]
        public void List99Page20()
        {
            var list = new List<int>();
            for (int i = 0; i < 99; i++) { list.Add(i); }

            var result = list.ToPages(20).ToList();

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(20, result[0].Count());
            Assert.AreEqual(20, result[1].Count());
            Assert.AreEqual(20, result[2].Count());
            Assert.AreEqual(20, result[3].Count());
            Assert.AreEqual(19, result[4].Count());
        }

        [Test]
        public void List2Page20()
        {
            var list = new List<int>();
            for (int i = 0; i < 2; i++) { list.Add(i); }

            var result = list.ToPages(20).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0].Count());
        }

        [Test]
        public void ListEmptyPage20()
        {
            var list = new List<int>();

            var result = list.ToPages(20).ToList();

            Assert.AreEqual(0, result.Count);
        }
    }
}
