using Common.ParserFramework;
using Common.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tokenization.Rules
{
    class UrlCombinationRule : ReductionRule
    {
        private static readonly Predicate[] Pattern = new Predicate[] 
        {
            ClassType(typeof(UrlString)),
            ClassType(typeof(Token))
        };

        public override bool Apply(ReductionStack s, object la)
        {
            if(s.MatchRight(Pattern))
            {
                var url = s.Get<UrlString>(-2);
                var token = s.Get<Token>(-1);

                if (!(token is Whitespace))
                {
                    url.Extend(token);
                    s.Replace(2, url);
                    return true;
                }
            }

            return false;
        }
    }
}
