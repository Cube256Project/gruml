using Common.ParserFramework;
using System.Collections.Generic;

namespace GRUML.Bindings
{
    class BindingSyntaxReductionRules : ReductionRuleCollection
    {
        public override IEnumerable<ReductionRule> Select(ReductionStack stack, object lookahead)
        {
            yield return new StartRule();
            yield return new AssignmentRule();
            yield return new ConcatenationRule();
            yield return new EndRule();
        }
    }
}
