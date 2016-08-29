using Common.Tokenization;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRUML.Model;

namespace GRUML.Converters
{
    class BindingConverter
    {
        public TokenWriter Writer { get; private set; }

        public bool IsControl { get; set; }

        public BindingConverter(TokenWriter writer)
        {
            Writer = writer;
        }

        public void Convert(string prop, BindingSyntax s)
        {
            if (s is DataBinding)
            {
                ConvertDataBinding(prop, (DataBinding)s);
            }
            else if (s is StaticResource)
            {
                ConvertResourceBinding(prop, (StaticResource)s);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void ConvertDataBinding(string prop, DataBinding b)
        {
            string target;

            var path =  new List<string>(null == b.Path ? new string[0] : b.Path.Split('.'));

            if (null != b.Element)
            {
                // TODO: resolve element name to ...
                throw new NotImplementedException();
            }

            if (IsControl)
            {

                // datacontext is default target
                target = "this";

                // just prefix the path for that
                path.Insert(0, "dc");
            }
            else
            {
                target = "dc";
            }

            var patharg = path.Count > 0 ? path.ToSeparatorList(".").Quote() : "null";

            var source = "new ObjectBinding(" + target + ", " + patharg;
            if (null != b.Converter)
            {
                source += ", new " + b.Converter + "()";
            }

            source += ")";

            Writer.WriteLine("BindingOperations.setBinding(e, " + prop.Quote() + ", " + source + ")");

        }

        public void ConvertResourceBinding(string prop, StaticResource s)
        {
            Writer.WriteLine("BindingOperations.setBinding(e, " + prop.Quote() + ", new ResourceBinding(e, " + s.Key.Quote() + "))");
        }
    }
}
