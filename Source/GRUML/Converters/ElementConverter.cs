using Common.Tokenization;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GRUML.Converters
{
    abstract class ElementConverter
    {
        public TokenWriter Writer { get; set; }

        public DocumentConverter Context { get; set; }

        public void Convert()
        {
            ConvertOverride();
        }

        protected abstract void ConvertOverride();


        protected virtual void BeginRender()
        {
            Writer.Indent();
            Writer.WriteLine("let L: any[] = [];");
            Writer.WriteLine("L.unshift(e);");
        }

        protected void EndRender()
        {
            Writer.UnIndent();
            Writer.WriteLine("}");
            Writer.WriteLine();
        }

        protected void Push()
        {
            Writer.WriteLine("L.unshift(e);");
        }

        protected void Pop()
        {
            Writer.WriteLine("e = L.shift();");
        }

        protected void EnterBlock()
        {
            Writer.WriteLine("{");
            Writer.Indent();
        }

        protected void LeaveBlock()
        {
            Writer.UnIndent();
            Writer.WriteLine("}");
        }

        protected void WriteComment(string text)
        {
            Writer.WriteLine("// " + text);
        }
    }
}
