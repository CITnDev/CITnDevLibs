using System;
using System.Diagnostics;
using CitnDev.System;

namespace CitnDev.System_Test
{
    public class TestListener : TraceListener
    {
        private string _previousMessage;

        private string _lastMessage;
        public string LastMessage
        {
            get { return _lastMessage; }
            private set { _lastMessage = value; Console.WriteLine(value); }
        }

        public bool IsNewMessage()
        {
            var result = _previousMessage != _lastMessage;
            _previousMessage = _lastMessage;

            return result;
        }

        public override void Write(string message)
        {
            LastMessage = message;
        }

        public override void WriteLine(string message)
        {
            LastMessage = "NL " + message;
        }
    }

    public class TestAdvancedTraceListener : AdvancedTraceListener
    {
        private string _previousMessage;

        private string _lastMessage;
        public string LastMessage
        {
            get { return _lastMessage; }
            private set { _lastMessage = value; Console.WriteLine(value); }
        }

        public bool IsNewMessage()
        {
            var result = _previousMessage != _lastMessage;
            _previousMessage = _lastMessage;

            return result;
        }


        public override void Write(string message)
        {
            LastMessage = message;
        }

        public override void WriteLine(string message)
        {
            LastMessage = message;
        }

        public override void Write(string message, string category)
        {
            if (string.IsNullOrEmpty(category))
                LastMessage = message;
            else
                LastMessage = category + " : " + message;
        }

        public override void Write(object o, string category)
        {
            if (string.IsNullOrEmpty(category))
                LastMessage = o.ToString();
            else
                LastMessage = category + " : " + o;
        }

        public override void WriteLine(string message, string category)
        {
            if (string.IsNullOrEmpty(category))
                LastMessage = message;
            else
                LastMessage = category + " : " + message;
        }

        public override void WriteLine(object o, string category)
        {
            if (string.IsNullOrEmpty(category))
                LastMessage = o.ToString();
            else
                LastMessage = category + " : " + o;
        }


        public override void WriteEx(string type, string message) { WriteEx(type, message, null, string.Empty); }
        public override void WriteEx(string type, string message, string category) { WriteEx(type, message, null, category); }
        public override void WriteEx(string type, object value) { WriteEx(type, value.ToString(), null, string.Empty); }
        public override void WriteEx(string type, object value, string category) { WriteEx(type, value.ToString(), null, category); }
        public override void WriteEx(string type, string message, Exception exception) { WriteEx(type, message, exception, string.Empty); }
        public override void WriteEx(string type, string message, Exception exception, string category)
        {
            LastMessage = "[Type : " + type + "] "
                          + (string.IsNullOrWhiteSpace(category) ? "" : " - [Category : " + category + "] ")
                          + message
                          + (exception == null ? "" : " (Exception : " + exception + ") ");
        }
        public override void WriteLineEx(string type, string message) { WriteLineEx(type, message, null, string.Empty); }
        public override void WriteLineEx(string type, string message, string category) { WriteLineEx(type, message, null, category); }
        public override void WriteLineEx(string type, object value) { WriteLineEx(type, value.ToString(), null, string.Empty); }
        public override void WriteLineEx(string type, object value, string category) { WriteLineEx(type, value.ToString(), null, category); }
        public override void WriteLineEx(string type, string message, Exception exception) { WriteLineEx(type, message, exception, string.Empty); }
        public override void WriteLineEx(string type, string message, Exception exception, string category)
        {
            LastMessage = "NL [Type : " + type + "] "
                          + (string.IsNullOrWhiteSpace(category) ? "" : " - [Category : " + category + "] ")
                          + message
                          + (exception == null ? "" : " (Exception : " + exception + ") ");            
        }
    }
}
