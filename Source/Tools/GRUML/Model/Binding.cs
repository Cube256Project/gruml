﻿using Common;
using Common.Tokens;
using System;

namespace GRUML.Model
{
    /// <summary>
    /// A binding object.
    /// </summary>
    public class Binding : BindingSyntax
    {
        public string Path;

        public object Converter;

        public string Element;

        public override void SetProperty(string name, Token value)
        {
            if (name == "path")
            {
                Path = value.Value;
            }
            else if (name == "converter")
            {
                Converter = value.Value;
            }
            else if (name == "element")
            {
                Element = value.Value;
            }
            else
            {
                throw new Exception("binding does not support property " + name.Quote() + ".");
            }
        }

        public override void SetDefault(Token value)
        {
            throw new NotImplementedException();
        }
    }
}
