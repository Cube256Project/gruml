using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;  

namespace Common
{
    class BasicLogMessage : ILogMessage
    {
        #region Private

        private StringBuilder _text;

        #endregion

        #region Properties

        public string ID { get; private set; }

        public string EventCode { get; set; }

        public DateTime EventTime { get; set; }

        public LogSeverity Severity { get; set; }

        public string Header { get; set; }

        public string Body { get { return null == _text ? null : _text.ToString(); } }

        public object[] Context { get; set; }

        #endregion

        #region Construction

        public BasicLogMessage()
        {
            Severity = LogSeverity.none;
            Context = new object[0];

            // TODO: CTRMIEWANN: this is only one option to do it.
            var id = new byte[32];
            RandomNumberGenerator.Create().GetBytes(id);
            ID = id.ToBase32();
        }

        #endregion

        public void AddData(object obj)
        {
            if (null != obj)
            {
                if (null == _text) _text = new StringBuilder();

                if (obj is string)
                {
                    _text.Append((string)obj);
                }
                else if (obj is byte[])
                {
                    _text.Append(HexDump.Convert((byte[])obj));
                }
                else if (obj is StringBuilder)
                {
                    _text.Append(obj.ToString());
                }
                else
                {
                    _text.Append("[" + obj.GetType().AssemblyQualifiedName + "]");
                }
            }
        }

        public void AddContext(object obj)
        {
            Context = Context.Concat(new[] { obj }).ToArray();
        }

        public void Submit()
        {
            // CTRMIEWANN: integration optional?
            EventTime = UniqueTimeSource.GetUniqueTime();

            // submit to current context.
            LogContext.Current.Submit(this);
        }
    }
}
