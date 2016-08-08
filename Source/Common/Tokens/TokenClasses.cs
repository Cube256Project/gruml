﻿
namespace Common.Tokens
{
    public interface TokenClass { }

    /// <summary>
    /// Interface implemented by tokens that represent strings.
    /// </summary>
    public interface GeneralString : TokenClass
    {
        string Value { get; }
    }

    public interface InternetString : TokenClass { }

    interface NonDefault : TokenClass { }

    interface XmlSimpleName : TokenClass { }

}
