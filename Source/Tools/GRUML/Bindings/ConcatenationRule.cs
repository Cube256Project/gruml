using Common.ParserFramework;
using Common.Tokens;
using GRUML.Model;

namespace GRUML.Bindings
{
    class ConcatenationRule : BindingSyntaxReductionRule
    {
        private static readonly Predicate[] Pattern = new Predicate[]
        {
            ExactType(typeof(BindingSyntax)),
            ExactType(typeof(CommaSeparator)),
        };

        public override bool Apply(ReductionStack s, object la)
        {
            if (s.MatchRight(Pattern))
            {
                s.PopRight(1);
                return true;
            }

            return false;
        }
    }
}
