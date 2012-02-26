//##########################################################
//
//  Author : Sebastien Curutchet
//  Last release : 17 nov. 2011
//  Company : CitnDev
//
//  Licensing : LGPL
//
//  History :
//      * 20111117 : 1st release version
//
//##########################################################


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CitnDev.System
{
    // The main static helper class
    public sealed class AdvancedTrace
    {
        private static readonly Dictionary<string, Dictionary<Type, List<TraceListener>>> Includes;
        private static readonly Dictionary<string, Dictionary<Type, List<TraceListener>>> Excludes;

        public class ListenerType
        {
            public const string All = "__ADVANCED_TRACE_ALL__";
            public const string Information = "__ADVANCED_TRACE_INFORMATION__";
            public const string Warning = "__ADVANCED_TRACE_WARNING__";
            public const string Error = "__ADVANCED_TRACE_ERROR__";
            public const string Problem = "__ADVANCED_TRACE_PROBLEM__";
            public const string Fatal = "__ADVANCED_TRACE_FATAL__";
            public const string Debug = "__ADVANCED_TRACE_DEBUG__";
            public const string Database = "__ADVANCED_TRACE_DATABASE__";
            public const string SQL = "__ADVANCED_TRACE_SQL__";
        }

        #region Default static constructor

        static AdvancedTrace()
        {
            Includes = new Dictionary<string, Dictionary<Type, List<TraceListener>>>();
            Excludes = new Dictionary<string, Dictionary<Type, List<TraceListener>>>();

            AddTraceType(ListenerType.All);
            AddTraceType(ListenerType.Information);
            AddTraceType(ListenerType.Warning);
            AddTraceType(ListenerType.Error);
            AddTraceType(ListenerType.Problem);
            AddTraceType(ListenerType.Fatal);
            AddTraceType(ListenerType.Debug);
            AddTraceType(ListenerType.Database);
            AddTraceType(ListenerType.SQL);
        }

        #endregion

        #region TraceListener management

        /// <summary>
        /// Add a trace type
        /// </summary>
        /// <param name="pstrType">String that represents the type</param>
        public static void AddTraceType(string pstrType)
        {
            lock (Includes)
            {
                if (!Includes.ContainsKey(pstrType))
                    Includes[pstrType] = new Dictionary<Type, List<TraceListener>>();
            }

            lock (Excludes)
            {
                if (!Excludes.ContainsKey(pstrType))
                    Excludes[pstrType] = new Dictionary<Type, List<TraceListener>>();
            }
        }

        // Add a TraceListener to a type
        public static void AddTraceListener(string pstrType, TraceListener poTraceListener)
        {
            lock (Includes[pstrType])
            {
                if (!Includes[pstrType].ContainsKey(poTraceListener.GetType()))
                    Includes[pstrType][poTraceListener.GetType()] = new List<TraceListener>();
            }

            lock (Includes[pstrType][poTraceListener.GetType()])
                Includes[pstrType][poTraceListener.GetType()].Add(poTraceListener);
        }

        // Remove a TraceListener from a type
        public static void RemoveTraceListener(string pstrType, TraceListener poTraceListener)
        {
            List<TraceListener> listeners = null;
            lock (Includes[pstrType])
                if (Includes[pstrType].ContainsKey(poTraceListener.GetType()))
                    listeners = Includes[pstrType][poTraceListener.GetType()];

            if (listeners != null && listeners.AsParallel().Contains(poTraceListener))
                lock (Includes[pstrType][poTraceListener.GetType()])
                    Includes[pstrType][poTraceListener.GetType()].Remove(poTraceListener);

            lock (Excludes[pstrType])
                if (!Excludes[pstrType].ContainsKey(poTraceListener.GetType()))
                    Excludes[pstrType][poTraceListener.GetType()] = new List<TraceListener>();

            lock (Excludes[pstrType][poTraceListener.GetType()])
                Excludes[pstrType][poTraceListener.GetType()].Add(poTraceListener);
        }

        // Remove a TraceListener from a type
        public static void RemoveAllTraceListener()
        {
            lock (Includes)
                Includes.Values.AsParallel().ForAll(dict => dict.Clear());

            lock (Excludes)
                Excludes.Values.AsParallel().ForAll(dict => dict.Clear());
        }

        #endregion

        #region Our trace methods

        public static void Trace(string pstrTrace, bool writeLine = true)
        {
            Trace(ListenerType.Information, pstrTrace, writeLine);
        }

        public static void Trace(string pstrType, string pstrTrace, bool writeLine = true)
        {
            Action<TraceListener> traceAction = listener =>
            {
                if (listener is AdvancedTraceListener)
                    if (writeLine)
                        ((AdvancedTraceListener)listener).WriteLineEx(pstrType, pstrTrace);
                    else
                        ((AdvancedTraceListener)listener).WriteEx(pstrType, pstrTrace);
                else
                    listener.WriteLine(pstrTrace);
            };

            CommonWrite(pstrType, traceAction);
        }

        public static void Trace(string pstrType, string pstrTrace, string pstrCategory, bool writeLine = true)
        {
            Action<TraceListener> traceAction = listener =>
            {
                if (listener is AdvancedTraceListener)
                    if (writeLine)
                        ((AdvancedTraceListener)listener).WriteLineEx(pstrType, pstrTrace, pstrCategory);
                    else
                        ((AdvancedTraceListener)listener).WriteEx(pstrType, pstrTrace, pstrCategory);
                else
                    listener.WriteLine(pstrTrace);
            };

            CommonWrite(pstrType, traceAction);
        }

        public static void Trace(string pstrType, object poTrace, bool writeLine = true)
        {
            Action<TraceListener> traceAction = listener =>
            {
                if (listener is AdvancedTraceListener)
                    if (writeLine)
                        ((AdvancedTraceListener)listener).WriteLineEx(pstrType, poTrace);
                    else
                        ((AdvancedTraceListener)listener).WriteEx(pstrType, poTrace);
                else
                    listener.WriteLine(poTrace);
            };

            CommonWrite(pstrType, traceAction);
        }

        public static void Trace(string pstrType, object poTrace, string pstrCategory, bool writeLine = true)
        {
            Action<TraceListener> traceAction = listener =>
            {
                if (listener is AdvancedTraceListener)
                    if (writeLine)
                        ((AdvancedTraceListener)listener).WriteLineEx(pstrType, poTrace, pstrCategory);
                    else
                        ((AdvancedTraceListener)listener).WriteEx(pstrType, poTrace, pstrCategory);
                else
                    listener.WriteLine(poTrace);
            };

            CommonWrite(pstrType, traceAction);
        }

        public static void Trace(string pstrType, string pstrTrace, Exception poException, bool writeLine = true)
        {
            Action<TraceListener> traceAction = listener =>
            {
                if (listener is AdvancedTraceListener)
                    if (writeLine)
                        ((AdvancedTraceListener)listener).WriteLineEx(pstrType, pstrTrace, poException);
                    else
                        ((AdvancedTraceListener)listener).WriteEx(pstrType, pstrTrace, poException);
                else
                    listener.WriteLine(pstrTrace + " " + poException.ToString());
            };

            CommonWrite(pstrType, traceAction);
        }

        public static void Trace(string pstrType, string pstrTrace, Exception poException, string pstrCategory, bool writeLine = true)
        {
            Action<TraceListener> traceAction = listener =>
            {
                if (listener is AdvancedTraceListener)
                    if (writeLine)
                        ((AdvancedTraceListener)listener).WriteLineEx(pstrType, pstrTrace, poException, pstrCategory);
                    else
                        ((AdvancedTraceListener)listener).WriteEx(pstrType, pstrTrace, poException, pstrCategory);
                else
                    listener.WriteLine(pstrTrace + " " + poException.ToString(), pstrCategory);
            };

            CommonWrite(pstrType, traceAction);
        }

        public static void Trace(string[] pstrTypeArray, string pstrTrace)
        {
            for (int i = 0; pstrTypeArray != null && i < pstrTypeArray.Length; i++)
                Trace(pstrTypeArray[i], pstrTrace);
        }

        public static void Trace(string[] pstrTypeArray, string pstrTrace, string pstrCategory)
        {
            for (int i = 0; pstrTypeArray != null && i < pstrTypeArray.Length; i++)
                Trace(pstrTypeArray[i], pstrTrace, pstrCategory);
        }

        public static void Trace(string[] pstrTypeArray, object poTrace)
        {
            for (int i = 0; pstrTypeArray != null && i < pstrTypeArray.Length; i++)
                Trace(pstrTypeArray[i], poTrace);
        }

        public static void Trace(string[] pstrTypeArray, object poTrace, string pstrCategory)
        {
            for (int i = 0; pstrTypeArray != null && i < pstrTypeArray.Length; i++)
                Trace(pstrTypeArray[i], poTrace, pstrCategory);
        }

        public static void Trace(string[] pstrTypeArray, string pstrTrace, Exception poException)
        {
            if (poException == null)
            {
                Trace(pstrTypeArray, pstrTrace);
                return;
            }

            for (int i = 0; pstrTypeArray != null && i < pstrTypeArray.Length; i++)
                Trace(pstrTypeArray[i], pstrTrace, poException);
        }

        public static void Trace(string[] pstrTypeArray, string pstrTrace, Exception poException, string pstrCategory)
        {
            if (poException == null)
            {
                Trace(pstrTypeArray, pstrTrace, pstrCategory);
                return;
            }

            for (int i = 0; pstrTypeArray != null && i < pstrTypeArray.Length; i++)
                Trace(pstrTypeArray[i], pstrTrace, poException, pstrCategory);
        }

        public static void Flush()
        {
            lock (Includes)
                Includes.SelectMany(trace => trace.Value).SelectMany(type => type.Value).AsParallel().ForAll(listener => listener.Flush());
        }

        #endregion

        // Legacy Debug and Trace methods

        #region Debug.XXX / Trace.XXX

        public static void Write(string pstrTrace)
        {
            Trace(pstrTrace, false);
        }

        public static void Write(string pstrTrace, string pstrCategory)
        {
            Trace(pstrTrace, pstrCategory, false);
        }

        public static void Write(object poTrace)
        {
            Trace(ListenerType.Information, poTrace, false);
        }

        public static void Write(object poTrace, string pstrCategory)
        {
            Trace(ListenerType.Information, poTrace, pstrCategory, false);
        }

        public static void WriteLine(string pstrTrace)
        {
            Trace(ListenerType.Information, pstrTrace);
        }

        public static void WriteLine(string pstrTrace, string pstrCategory)
        {
            Trace(ListenerType.Information, pstrTrace, pstrCategory);
        }

        public static void WriteLine(object poTrace)
        {
            Trace(ListenerType.Information, poTrace);
        }

        public static void WriteLine(object poTrace, string pstrCategory)
        {
            Trace(ListenerType.Information, poTrace, pstrCategory);
        }

        #endregion

        #region Trace.TraceXXX

        public static void TraceInformation(string pstrTrace)
        {
            Trace(ListenerType.Information, pstrTrace);
        }

        public static void TraceWarning(string pstrTrace)
        {
            Trace(ListenerType.Warning, pstrTrace);
        }

        public static void TraceError(string pstrTrace)
        {
            Trace(ListenerType.Error, pstrTrace);
        }

        #endregion

        // Extended Trace methods

        #region TraceInformation

        public static void TraceInformation(string pstrTrace, string pstrCategory)
        {
            Trace(ListenerType.Information, pstrTrace, pstrCategory);
        }

        public static void TraceInformation(string pstrTrace, Exception poException)
        {
            Trace(ListenerType.Information, pstrTrace, poException);
        }

        public static void TraceInformation(string pstrTrace, Exception poException, string pstrCategory)
        {
            Trace(ListenerType.Information, pstrTrace, poException, pstrCategory);
        }

        #endregion

        #region TraceWarning

        public static void TraceWarning(string pstrTrace, string pstrCategory)
        {
            Trace(ListenerType.Warning, pstrTrace, pstrCategory);
        }

        public static void TraceWarning(string pstrTrace, Exception poException)
        {
            Trace(ListenerType.Warning, pstrTrace, poException);
        }

        public static void TraceWarning(string pstrTrace, Exception poException, string pstrCategory)
        {
            Trace(ListenerType.Warning, pstrTrace, poException, pstrCategory);
        }

        #endregion

        #region TraceError

        public static void TraceError(string pstrTrace, string pstrCategory)
        {
            Trace(ListenerType.Error, pstrTrace, pstrCategory);
        }

        public static void TraceError(string pstrTrace, Exception poException)
        {
            Trace(ListenerType.Error, pstrTrace, poException);
        }

        public static void TraceError(string pstrTrace, Exception poException, string pstrCategory)
        {
            Trace(ListenerType.Error, pstrTrace, poException, pstrCategory);
        }

        #endregion

        #region TraceProblem

        public static void TraceProblem(string pstrTrace)
        {
            Trace(ListenerType.Problem, pstrTrace);
        }

        public static void TraceProblem(string pstrTrace, string pstrCategory)
        {
            Trace(ListenerType.Problem, pstrTrace, pstrCategory);
        }

        public static void TraceProblem(string pstrTrace, Exception poException)
        {
            Trace(ListenerType.Problem, pstrTrace, poException);
        }

        public static void TraceProblem(string pstrTrace, Exception poException, string pstrCategory)
        {
            Trace(ListenerType.Problem, pstrTrace, poException, pstrCategory);
        }

        #endregion

        #region TraceFatal

        public static void TraceFatal(string pstrTrace)
        {
            Trace(ListenerType.Fatal, pstrTrace, string.Empty);
        }

        public static void TraceFatal(string pstrTrace, string pstrCategory)
        {
            Trace(ListenerType.Fatal, pstrTrace, pstrCategory);
        }

        public static void TraceFatal(string pstrTrace, Exception poException)
        {
            Trace(ListenerType.Fatal, pstrTrace, poException);
        }

        public static void TraceFatal(string pstrTrace, Exception poException, string pstrCategory)
        {
            Trace(ListenerType.Fatal, pstrTrace, poException, pstrCategory);
        }

        #endregion

        #region TraceDebug

        public static void TraceDebug(string pstrTrace)
        {
            Trace(ListenerType.Debug, pstrTrace);
        }

        public static void TraceDebug(string pstrTrace, string pstrCategory)
        {
            Trace(ListenerType.Debug, pstrTrace, pstrCategory);
        }

        public static void TraceDebug(string pstrTrace, Exception poException)
        {
            Trace(ListenerType.Debug, pstrTrace, poException);
        }

        public static void TraceDebug(string pstrTrace, Exception poException, string pstrCategory)
        {
            Trace(ListenerType.Debug, pstrTrace, poException, pstrCategory);
        }

        #endregion

        #region TraceDatabase

        public static void TraceDatabase(string pstrTrace)
        {
            Trace(ListenerType.Database, pstrTrace);
        }

        public static void TraceDatabase(string pstrTrace, string pstrCategory)
        {
            Trace(ListenerType.Database, pstrTrace, pstrCategory);
        }

        #endregion

        #region TraceSQL

        public static void TraceSQL(string pstrTrace)
        {
            Trace(ListenerType.SQL, pstrTrace);
        }

        public static void TraceSQL(string pstrTrace, string pstrCategory)
        {
            Trace(ListenerType.SQL, pstrTrace, pstrCategory);
        }

        #endregion

        private static void CommonWrite(string pstrType, Action<TraceListener> traceAction)
        {
            if (Includes.ContainsKey(pstrType))
            {
                // Trace listeners added to Information type
                lock (Includes[pstrType])
                    Includes[pstrType].AsParallel().ForAll(dict => dict.Value.ForEach(traceAction));
            }

            // Trace listeners added to All type
            List<TraceListener> excludes;
            if (Excludes.ContainsKey(pstrType))
            {
                lock (Excludes[pstrType])
                    excludes = Excludes[pstrType].SelectMany(type => type.Value).ToList();
            }
            else
                excludes = new List<TraceListener>();


            List<TraceListener> includesAll;
            lock (Includes[ListenerType.All])
                includesAll = Includes[ListenerType.All].SelectMany(type => type.Value).ToList();
            includesAll.Except(excludes).AsParallel().ForAll(traceAction);
        }
    }

    // Our base implementation of the TraceListener -> Used to build custom listener
    public abstract class AdvancedTraceListener : TraceListener
    {
        // The standard write methods will never be called on the AdvancedTraceListener
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Write(string pstrMessage) { }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Write(string pstrMessage, string pstrCategory) { }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Write(object poTrace) { }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Write(object poTrace, string pstrCategory) { }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void WriteLine(string pstrMessage) { }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void WriteLine(string pstrMessage, string pstrCategory) { }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void WriteLine(object poTrace) { }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void WriteLine(object poTrace, string pstrCategory) { }

        // Advanced trace method
        public virtual void WriteEx(string pstrType, string pstrTrace) { }
        public virtual void WriteEx(string pstrType, string pstrTrace, string pstrCategory) { }
        public virtual void WriteEx(string pstrType, object poTrace) { }
        public virtual void WriteEx(string pstrType, object poTrace, string pstrCategory) { }
        public virtual void WriteEx(string pstrType, string pstrMessage, Exception poException) { }
        public virtual void WriteEx(string pstrType, string pstrMessage, Exception poException, string pstrUserCategory) { }
        public virtual void WriteLineEx(string pstrType, string pstrTrace) { }
        public virtual void WriteLineEx(string pstrType, string pstrTrace, string pstrCategory) { }
        public virtual void WriteLineEx(string pstrType, object poTrace) { }
        public virtual void WriteLineEx(string pstrType, object poTrace, string pstrCategory) { }
        public virtual void WriteLineEx(string pstrType, string pstrMessage, Exception poException) { }
        public virtual void WriteLineEx(string pstrType, string pstrMessage, Exception poException, string pstrUserCategory) { }
    }
}
