using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using sysIO = System.IO;
using cIO = CitnDev.System.IO;

namespace CitnDev.System_Test.IO
{
    [TestFixture]
    public class FileTest
    {
        [Test]
        public void Delete()
        {
            var folder = sysIO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                            "NUnit.System");

            if (!sysIO.Directory.Exists(folder))
                sysIO.Directory.CreateDirectory(folder);

            var testFile = sysIO.Path.Combine(folder, "test.txt");

            sysIO.File.WriteAllText(testFile, "000" + Environment.NewLine);

            cIO.File.Delete(testFile);

            using (var log = new TextWriterTraceListener(testFile))
            {
                log.WriteLine("11111");
            }

            var content = sysIO.File.ReadAllText(testFile);

            Assert.False(content.Contains("0"));

            sysIO.File.Delete(testFile);
        }

        [Test]
        public void DeleteStress()
        {
            var folder = sysIO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                            "NUnit.System");

            if (!sysIO.Directory.Exists(folder))
                sysIO.Directory.CreateDirectory(folder);

            var testFile = sysIO.Path.Combine(folder, "test.txt");

            int i = 0;
            while (i < 50)
            {
                using (var f = File.Create(testFile)) ;
                cIO.File.Delete(testFile);
                if (File.Exists(testFile))
                    Assert.Fail("File not deleted");

                i++;
            }
        }
    }
}
