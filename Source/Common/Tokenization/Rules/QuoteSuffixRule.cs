using Common.ParserFramework;
using Common.Tokens;
using System;
using System.Linq;

namespace Common.Tokenization.Rules
{
    class QuoteSuffixRule : ReductionRule
    {
        private Type _quotetype;

        public QuoteSuffixRule(Quote quote)
        {
            _quotetype = quote.GetType();
        }

        public override bool Apply(ReductionStack s, object la)
        {
            var stack = (TokenizerReductionStack)s;

            Quote quote;
            if(s.TryGet(-1, out quote))
            {
                if (quote.GetType() == _quotetype)
                {
                    var n = s.Count;
                    var j = n - 1;
                    while (j > 0)
                    {
                        --j;
                        if(stack[j].GetType() == _quotetype)
                        {
                            break;
                        }
                    }

                    var content = stack.GetRange(j, s.Count).OfType<Token>();
                    var replace = QuotedString.Create(content);

                    stack.Replace(n - j, replace);
                    stack.PopRules();
                    return true;
                }
            }

            return false;
        }
    }
}
