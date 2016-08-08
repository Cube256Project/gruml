using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Common
{
    public static class LogMessageExtensions
    {
        public static ILogMessage WithHeader(this ILogMessage log, string format, params object[] args)
        {
            log.Header = string.Format(format, args);
            return log;
        }

        public static ILogMessage WithData(this ILogMessage log, object data)
        {
            log.AddData(data);
            return log;
        }

        public static ILogMessage WithSeverity(this ILogMessage log, LogSeverity severity)
        {
            log.Severity = severity;
            return log;
        }

        public static ILogMessage WithExceptionData(this ILogMessage log, Exception ex)
        {
            var first = true;
            var sb = new StringBuilder();
            while (null != ex)
            {
                if (first) first = false;
                else sb.AppendLine();

                sb.AppendLine("[" + ex.GetType().FullName + "]");
                sb.AppendLine(ex.ToString());

                ex = ex.InnerException;
            }

            log.AddData(sb.ToString());

            return log;
        }

        internal static void SerializeContext(this ILogMessage log)
        {
        }

        internal static void SerializeContext(this ILogMessage log, IEnumerable<object> context)
        {
            var set = new HashSet<object>();
            var list = new List<string>();

            foreach (var e in context)
            {
                if (null != e && !set.Contains(e))
                {
                    set.Add(e);
                    list.Add(ConvertToString(e));
                }
            }

            log.Context = list.ToArray();
        }

        private static string ConvertToString(object e)
        {
            if (e is string)
            {
                return (string)e;
            }
            else
            {
                var type = e.GetType();
                var source = type.GetCustomAttribute<LogSourceAttribute>(true);
                if (null != source)
                {
                    try
                    {
                        // get context property from object given by dotted propertypath.
                        var parts = source.PropertyPath.Split('.');
                        foreach (var part in parts)
                        {
                            var prop = type.GetProperty(part);
                            if (null == prop)
                                break;

                            e = prop.GetValue(e);
                            if (null == e)
                                break;

                            type = e.GetType();
                        }

                        return e.ToString();
                    }
                    catch (Exception)
                    { 
                    }
                }

                return type.FullName;
            }
        }
    }
}
