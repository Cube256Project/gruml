using Common.Tokens;

namespace GRUML.Model
{
    public abstract class BindingSyntax
    {
        public abstract void SetDefault(Token value);

        public abstract void SetProperty(string name, Token value);
    }
}
