using Common.ParserFramework;
using Common.Tokens;

namespace Common.Tokenization.Rules
{
    class UrlTerminalRule : ReductionRule
    {
        public override bool Apply(ReductionStack s, object la)
        {
            if (IsTerminator(la))
            {
                var stack = (TokenizerReductionStack)s;
                stack.PopRules();
                return true;
            }

            return false;
        }

        private bool IsTerminator(object token)
        {
            return
                token is Semicolon
                || token is CommaSeparator
                || token is VerticalBar
                || token is Whitespace
                || token is EndOfFile;
        }
    }
}
