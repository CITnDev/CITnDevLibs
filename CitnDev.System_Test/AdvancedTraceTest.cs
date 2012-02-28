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
            var listener = new TestListener();
            var advListener = new TestAdvancedTraceListener();
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.Information, listener);
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.Information, advListener);

            AdvancedTrace.TraceInformation("Default information");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception", new FileNotFoundException("file.ext"));
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information on a category", "Category2");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception on a category", new Exception("Base exception"), "Category1");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());

            AdvancedTrace.TraceDatabase("Database");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceError("Error");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceFatal("Fatal");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceDebug("Debug");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceSQL("SQL");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceProblem("Problem");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceWarning("Warning");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());

            AdvancedTrace.TraceInformation("Finish");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
        }

        [Test]
        public void MultipleTrace()
        {
            var listener = new TestListener();
            var advListener = new TestAdvancedTraceListener();
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.Information, listener);
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.Warning, listener);
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.Information, advListener);
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.Problem, advListener);

            AdvancedTrace.TraceInformation("Default information");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception", new FileNotFoundException("file.ext"));
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information on a category", "Category2");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception on a category", new Exception("Base exception"), "Category1");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());

            AdvancedTrace.TraceDatabase("Database");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceError("Error");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceFatal("Fatal");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceDebug("Debug");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceSQL("SQL");
            Assert.False(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());
            AdvancedTrace.TraceProblem("Problem");
            Assert.False(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceWarning("Warning");
            Assert.True(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());

            AdvancedTrace.TraceInformation("Finish");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
        }

        [Test]
        public void TraceAll()
        {
            var listener = new TestListener();
            var advListener = new TestAdvancedTraceListener();
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.All, listener);
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.All, advListener);

            AdvancedTrace.TraceInformation("Default information");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception", new FileNotFoundException("file.ext"));
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information on a category", "Category2");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception on a category", new Exception("Base exception"), "Category1");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());

            AdvancedTrace.TraceDatabase("Database");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceError("Error");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceFatal("Fatal");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceDebug("Debug");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceSQL("SQL");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceProblem("Problem");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceWarning("Warning");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());

            AdvancedTrace.TraceInformation("Finish");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
        }

        [Test]
        public void TraceAllWithRemoveType()
        {
            var listener = new TestListener();
            var advListener = new TestAdvancedTraceListener();
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.All, listener);
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.All, advListener);
            AdvancedTrace.RemoveTraceListener(AdvancedTrace.ListenerType.Warning, advListener);

            AdvancedTrace.TraceInformation("Default information");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception", new FileNotFoundException("file.ext"));
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information on a category", "Category2");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception on a category", new Exception("Base exception"), "Category1");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());

            AdvancedTrace.TraceDatabase("Database");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceError("Error");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceFatal("Fatal");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceDebug("Debug");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceSQL("SQL");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceProblem("Problem");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceWarning("Warning");
            Assert.True(listener.IsNewMessage());
            Assert.False(advListener.IsNewMessage());

            AdvancedTrace.TraceInformation("Finish");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
        }

        [Test]
        public void TraceAllWithNewType()
        {
            const string newType = "__NEW_TYPE__";

            var listener = new TestListener();
            var advListener = new TestAdvancedTraceListener();
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.All, listener);
            AdvancedTrace.AddTraceListener(AdvancedTrace.ListenerType.All, advListener);
            AdvancedTrace.AddTraceType(newType);

            AdvancedTrace.TraceInformation("Default information");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception", new FileNotFoundException("file.ext"));
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information on a category", "Category2");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceInformation("Information with exception on a category", new Exception("Base exception"), "Category1");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());

            AdvancedTrace.TraceDatabase("Database");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceError("Error");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceFatal("Fatal");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceDebug("Debug");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceSQL("SQL");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceProblem("Problem");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
            AdvancedTrace.TraceWarning("Warning");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());


            AdvancedTrace.Trace(newType, "New type");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());

            AdvancedTrace.TraceInformation("Finish");
            Assert.True(listener.IsNewMessage());
            Assert.True(advListener.IsNewMessage());
        }
    }


}
