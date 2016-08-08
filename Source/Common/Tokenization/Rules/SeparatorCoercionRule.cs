using Common.ParserFramework;
using Common.Tokens;
using System.Linq;

namespace Common.Tokenization.Rules
{
    class SeparatorCoercionRule : ReductionRule
    {
        #region Private

        private static readonly Predicate[] Pattern = new Predicate[]
        {
            ClassType(typeof(GeneralString)),
            ClassType(typeof(Separator)), 
            ClassType(typeof(GeneralString))
        };

        #endregion

        public override bool Apply(ReductionStack s, object la)
        {
            if(s.MatchRight(Pattern))
            {
                // separator priority must be greater equal reduction.
                var separator = s.Get<Separator>(-2);
                var lapr = ((Token)la).GetSeparatorPriority();

                if (separator.GetSeparatorPriority() >= lapr)
                {
                    var replace = separator.CombineTokens(s.Skip(s.Count - 3).OfType<Token>());
                    s.Replace(3, replace);
                    return true;
                }
            }

            return false;
        }
    }
}
