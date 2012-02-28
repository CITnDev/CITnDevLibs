using System;
using System.Diagnostics;
using System.IO;
using CitnDev.System;
using NUnit.Framework;

namespace CitnDev.System_Test
{
    [TestFixture]
    public class AdvancedTraceTest
    {
        [Test]
        public void SimpleTrace()
        {
            var listener = new ConsoleTraceListener();
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.Information, listener);

            AdvancedTrace.TraceInformation("Default information");
            AdvancedTrace.TraceInformation("Information with exception", new FileNotFoundException("file.ext"));
            AdvancedTrace.TraceInformation("Information on a category", "Category2");
            AdvancedTrace.TraceInformation("Information with exception on a category", new Exception("Base exception"), "Category1");

            AdvancedTrace.TraceDatabase("Database");
            AdvancedTrace.TraceError("Error");
            AdvancedTrace.TraceFatal("Fatal");
            AdvancedTrace.TraceDebug("Debug");
            AdvancedTrace.TraceSQL("SQL");
            AdvancedTrace.TraceProblem("Problem");
            AdvancedTrace.TraceWarning("Warning");
        }
    }
}
