using Common.ParserFramework;
using Common.Tokens;
using GRUML.Model;

namespace GRUML.Bindings
{
    class EndRule : BindingSyntaxReductionRule
    {
        private static readonly Predicate[] Pattern = new Predicate[]
        {
            ClassType(typeof(BindingSyntax)),
            ExactType(typeof(CurlyBracketRight)),
            ExactType(typeof(EndOfFile))
        };

        private static readonly Predicate[] ShortPattern = new Predicate[]
{
            ClassType(typeof(BindingSyntax)),
            ClassType(typeof(GeneralString)),
            ExactType(typeof(CurlyBracketRight)),
            ExactType(typeof(EndOfFile))
};

        public override bool Apply(ReductionStack s, object la)
        {
            if (s.MatchRight(Pattern))
            {
                s.PopRight(2);
                return true;
            }
            else if (s.MatchRight(ShortPattern))
            {
                var b = s.Get<BindingSyntax>(-4);
                b.SetDefault(s.Get<Token>(-3));
                s.PopRight(3);
                return true;
            }

            return false;
        }
    }
}
