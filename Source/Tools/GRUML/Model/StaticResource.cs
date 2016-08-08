using Common.Tokens;
using System;

namespace GRUML.Model
{
    /// <summary>
    /// Refers to a <see cref="Resource"/> object.
    /// </summary>
    class StaticResource : BindingSyntax
    {
        public string Key { get; set; }

        public override void SetDefault(Token value)
        {
            Key = value.Value;
        }

        public override void SetProperty(string name, Token value)
        {
            throw new NotImplementedException();
        }
    }
}
