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
            if (s is Binding)
            {
                ConvertDataBinding(prop, (Binding)s);
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

        public void ConvertDataBinding(string prop, Binding b)
        {
            string target;
            var path = b.Path;

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
                path = "dc." + path;
            }
            else
            {
                target = "dc";
            }


            var source = "new ObjectBinding(" + target + ", " + path.Quote();
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
