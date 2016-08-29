using System.Linq;
using System.Xml;
using Common;

namespace GRUML.Model
{
    /// <summary>
    /// Element of a data template or control.
    /// </summary>
    public abstract class TemplateElement : ContainerElement
    {
        public string Name { get; private set; }

        public override bool Load(XmlElement e)
        {
            // process attributes
            LoadAttributes(e);

            // and the rest
            return base.Load(e);
        }

        private void LoadAttributes(XmlElement e)
        {
            foreach (var a in e.Attributes.OfType<XmlAttribute>())
            {
                if (a.Name == "name")
                {
                    // name attribute is special
                    Name = a.Value;
                }
                else if (a.Name == "id")
                {
                    ID = a.Value;
                }
                else if (a.Name == "xmlns")
                {
                    // omit
                    continue;
                }
                else if (a.Name == "command")
                {
                    AddAttribute(new StaticAttribute
                    {
                        Name = "onclick",
                        Value = "Control.SendRoutedCommand(this, " + a.Value.CQuote() + ");"
                    });
                }
                else
                {
                    HtmlAttribute u;
                    var value = a.Value;
                    if (BindingParser.IsBindingExpression(value))
                    {
                        u = new BindingAttribute
                        {
                            Name = a.Name,
                            Binding = new BindingParser().Parse(value)
                        };
                    }
                    else
                    {
                        u = new StaticAttribute { Name = a.Name, Value = a.Value };
                    }

                    AddAttribute(u);
                }
            }
        }
    }
}
