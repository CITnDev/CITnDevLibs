//##########################################################
//
//  Author : Sebastien Curutchet
//  Last release : Feb. 28,  2012
//  Company : CitnDev
//
//  Licensing : LGPL
//
//  History :
//      * 20120228 : fix hot add/remove trace type
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
        private static readonly Dictionary<string, Dictionary<Type, List<TraceListener>>> Tracers;
        private static readonly List<TraceListener> TraceAll;

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
            Tracers = new Dictionary<string, Dictionary<Type, List<TraceListener>>>();
            TraceAll = new List<TraceListener>();

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
        /// <param name="type">String that represents the type</param>
        public static void AddTraceType(string type)
        {
            if (type == ListenerType.All)
                return;

            lock (Tracers)
            {
                if (!Tracers.ContainsKey(type))
                    Tracers[type] = new Dictionary<Type, List<TraceListener>>();
            }

            lock (Tracers[type])
            {
                lock (TraceAll)
                {
                    foreach (var listener in TraceAll)
                    {
                        InternalAddListener(type, listener);
                    }
                }
            }

        }

        // Add a TraceListener to a type
        public static void AddTraceListener(string type, TraceListener traceListener)
        {
            if (type == ListenerType.All)
            {
                lock (Tracers)
                {
                    foreach (var tracerKey in Tracers.Keys)
                    {
                        InternalAddListener(tracerKey, traceListener);
                    }
                }

                lock (TraceAll)
                {
                    TraceAll.Add(traceListener);
                }
            }
            else
            {
                lock (Tracers[type])
                {
                    InternalAddListener(type, traceListener);
                }
            }
        }

        // Remove a TraceListener from a type
        public static void RemoveTraceListener(string type, TraceListener traceListener)
        {
            var listenerType = traceListener.GetType();
            if (type == ListenerType.All)
            {
                lock (TraceAll)
                {
                    TraceAll.Remove(traceListener);
                }
                lock (Tracers)
                {
                    foreach (var tracerKey in Tracers.Keys)
                    {
                        lock (Tracers[tracerKey])
                        {
                            Tracers[tracerKey][listenerType].Remove(traceListener);
                        }
                    }
                }
            }
            else
            {
                lock (Tracers[type])
                {
                    Tracers[type][listenerType].Remove(traceListener);
                }
            }
        }

        // Remove a TraceListener from a type
        public static void RemoveAllTraceListener()
        {
            lock(Tracers)
            {
                Tracers.Clear();
            }
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
            lock (Tracers)
            {
                Tracers.SelectMany(type => type.Value).SelectMany(listenerType => listenerType.Value).AsParallel().ForAll(listener => listener.Flush());
            }
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
            if (Tracers.ContainsKey(pstrType))
            {
                // Trace listeners added to Information type
                lock (Tracers[pstrType])
                    Tracers[pstrType].AsParallel().ForAll(dict => dict.Value.ForEach(traceAction));
            }
        }

        private static void InternalAddListener(string type, TraceListener traceListener)
        {
            var listenerType = traceListener.GetType();

            if (!Tracers[type].ContainsKey(listenerType))
            {
                Tracers[type].Add(listenerType, new List<TraceListener>());
            }

            Tracers[type][listenerType].Add(traceListener);
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
