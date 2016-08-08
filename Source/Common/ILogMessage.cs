
using System;

namespace Common
{
    /// <summary>
    /// Interface provided by the log message representation.
    /// </summary>
    public interface ILogMessage
    {
        /// <summary>
        /// Unique per message identification.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// URI describing the event.
        /// </summary>
        string EventCode { get; set; }

        DateTime EventTime { get; set; }

        /// <summary>
        /// Severity must be at least 'none'.
        /// </summary>
        LogSeverity Severity { get; set; }

        /// <summary>
        /// The log message header.
        /// </summary>
        string Header { get; set; }

        /// <summary>
        /// The body of the log message.
        /// </summary>
        string Body { get; }

        object[] Context { get; set; }

        /// <summary>
        /// Adds a data object to the log message.
        /// </summary>
        /// <remarks>
        /// <para>The object can be a string or a StringBuilder.</para></remarks>
        /// <param name="data">The data object to data.</param>
        void AddData(object data);

        /// <summary>
        /// Adds a context object to the message.
        /// </summary>
        /// <param name="obj"></param>
        void AddContext(object obj);

        void Submit();
    }
}
