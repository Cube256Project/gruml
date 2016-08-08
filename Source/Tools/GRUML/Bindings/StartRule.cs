using Common.ParserFramework;
using Common.Tokens;
using GRUML.Model;
using System;

namespace GRUML.Bindings
{
    class StartRule : BindingSyntaxReductionRule
    {
        private static readonly Predicate[] Pattern = new Predicate[]
        {
            ExactType(typeof(StartOfFile)),
            ExactType(typeof(CurlyBracketLeft)),
            ClassType(typeof(GeneralString))
        };


        public override bool Apply(ReductionStack s, object la)
        {
            if (s.MatchLeft(Pattern))
            {
                var id = s.Get<Token>(-1);
                var kind = id.Value.ToLower();

                object replace;
                switch(kind)
                {
                    case "binding":
                        replace = new Binding();
                        break;

                    case "resource":
                        replace = new StaticResource();
                        break;

                    default:
                        // assume binding name
                        replace = new Binding { Path = kind };
                        break;
                }

                s.Replace(3, replace);

                return true;
            }

            return false;
        }
    }
}
