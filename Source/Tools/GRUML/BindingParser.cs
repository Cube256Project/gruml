using Common;
using Common.ParserFramework;
using Common.Tokenization;
using Common.Tokens;
using GRUML.Bindings;
using GRUML.Model;
using System;
using System.Text.RegularExpressions;

namespace GRUML
{
    /// <summary>
    /// Parses a binding definitions.
    /// </summary>
    class BindingParser
    {
        #region Private

        private static Regex _bindingexpression = new Regex(@"^\{.*\}$");

        #endregion

        /// <summary>
        /// Parse binding attribute value.
        /// </summary>
        /// <param name="value">The binding string, including curly brackets.</param>
        /// <returns>The converted binding object.</returns>
        public BindingSyntax Parse(string value)
        {
            var reader = new TokenReader();
            var stack = new ReductionStack(new BindingSyntaxReductionRules());

            foreach (var token in reader.Read(value))
            {
                stack.Reduce(token);
                stack.Push(token);

                if(token is EndOfFile)
                {
                    // final reduction
                    stack.Reduce(null);
                }

                // Log.Debug("stack: |{0}|", stack.ToSeparatorList("|"));
            }

            if(stack.Count != 1)
            {
                throw new Exception("failed to parse binding expression " + value.Quote() + ".");
            }

            return stack.Get<BindingSyntax>(0);
        }

        /// <summary>
        /// Tests whether a string is enclosed in curly brackets.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBindingExpression(string value)
        {
            return _bindingexpression.IsMatch(value);
        }

    }
}
