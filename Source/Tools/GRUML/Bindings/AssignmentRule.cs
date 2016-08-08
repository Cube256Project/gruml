using Common.ParserFramework;
using Common.Tokens;
using GRUML.Model;

namespace GRUML.Bindings
{
    class AssignmentRule : BindingSyntaxReductionRule
    {
        private static readonly Predicate[] Pattern = new Predicate[]
        {
            ClassType(typeof(BindingSyntax)),
            ClassType(typeof(Identifier)),
            ExactType(typeof(EqualSign)),
            ClassType(typeof(GeneralString))
        };

        public override bool Apply(ReductionStack s, object la)
        {
            if(s.MatchRight(Pattern))
            {
                var binding = s.Get<BindingSyntax>(-4);
                binding.SetProperty(s.Get<Identifier>(-3).Value, s.Get<Token>(-1));
                s.PopRight(3);
                return true;
            }

            return false;
        }
    }
}
