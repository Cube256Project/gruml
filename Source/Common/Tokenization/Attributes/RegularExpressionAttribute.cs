using System;
using System.Text.RegularExpressions;

namespace Common.Tokenization.Attributes
{
    /// <summary>
    /// Attributes a regular expression to an atomic token.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RegularExpressionAttribute : Attribute
    {
        public string Expression { get; private set; }

        public int Priority { get; private set; }

        public RegularExpressionAttribute(string e, int priority = 0)
        {
            Expression = e;
            Priority = priority;
        }
    }
}
