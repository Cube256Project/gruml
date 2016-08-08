using Common;
using GRUML.Model;
using System;
using System.Linq;

namespace GRUML.Converters
{
    abstract class TemplateElementConverter : ElementConverter
    {
        private ContainerElement _e;

        public TemplateElementConverter(ContainerElement e)
        {
            _e = e;
        }

        protected void ConvertDefault(string tag, string ns = null)
        {
            Writer.WriteLine("// ++" + _e.SequenceNumber + " " + tag);

            // create DOM element
            if (null == ns)
            {
                Writer.WriteLine("e = document.createElement(" + tag.Quote() + ");");
            }
            else
            {
                Writer.WriteLine("e = document.createElementNS(" + ns.Quote() + ", " + tag.Quote() + ");");
            }

            // append it to the current container
            Writer.WriteLine("L[0].appendChild(e);");

            if (null != _e.ID)
            {
                if (Context.IsControl)
                {
                    // TODO: check if in a control context!
                    Writer.WriteLine("this." + _e.ID + " = e;");
                }
                else
                {
                    throw new Exception("named elements are not allowed in data templates.");
                }
            }

            ConvertAfterContainerAppended();

            ConvertAttributes();

            ConvertAfterAttributesAppended();

            ConvertElements();

            Writer.WriteLine("// --" + _e.SequenceNumber);
        }

        protected virtual void ConvertAfterContainerAppended()
        {
            if (null != _e.Dictionary)
            {
                // WXQ7XZ4EKL: associate resource dictionary with DOM element.
                Writer.WriteLine("e.setAttribute('__dict', " + _e.Dictionary.UniqueName.Quote() + ");");
            }
        }

        protected virtual void ConvertAfterAttributesAppended()
        {
        }

        protected virtual void ConvertElements()
        {
            if (_e.Elements.Any())
            {
                foreach (var child in _e.Elements)
                {
                    Push();
                    Context.Convert(child);
                    Pop();
                }
            }
        }

        protected virtual void ConvertAttributes()
        {
            foreach (var h in _e.Attributes)
            {
                var name = h.Name;
                // event attribute??

                if (h is BindingAttribute)
                {
                    ConvertBindingAttribute((BindingAttribute)h);
                }
                else if (h is StaticAttribute)
                {
                    if (name.StartsWith("on"))
                    {
                        ConvertFunctionAttribute((StaticAttribute)h);
                    }
                    else
                    {
                        ConvertStaticAttribute((StaticAttribute)h);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void ConvertFunctionAttribute(StaticAttribute a)
        {
            // skip on.. prefix
            var name = a.Name.Substring(2);

            // CZQWOA74T2: event listener handler indirection
            Writer.WriteLine("e.addEventListener(" + name.Quote() + ", function(e) { " + a.Value + " });");
        }

        protected virtual void ConvertStaticAttribute(StaticAttribute a)
        {
            Writer.WriteLine("e.setAttribute(" + a.Name.Quote() + ", " + a.Value.Quote() + ");");
        }

        protected void ForAllElements(Element e, Action<Element> action)
        {
            action(e);

            if (e is ContainerElement)
            {
                var container = (ContainerElement)e;
                foreach (var child in container.Elements)
                {
                    ForAllElements(child, action);
                }
            }
        }

        /// <summary>
        /// Emits dictionaries defined by a template element.
        /// </summary>
        /// <param name="e"></param>
        protected void EmitDictionaries(Element e)
        {
            if (e is ContainerElement)
            {
                var container = (ContainerElement)e;
                foreach (var child in container.Elements)
                {
                    EmitDictionaries(child);
                }

                if (null != container.Dictionary)
                {
                    Context.Convert(container.Dictionary);
                }
            }
        }


        private void ConvertBindingAttribute(BindingAttribute h)
        {
            var c = new BindingConverter(Writer);

            c.IsControl = Context.IsControl;
            c.Convert(h.Name, h.Binding);
        }
    }
}
