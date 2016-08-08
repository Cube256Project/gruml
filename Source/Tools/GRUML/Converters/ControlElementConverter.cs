using Common;
using GRUML.Model;

namespace GRUML.Converters
{
    /// <summary>
    /// Converts a control element appearing inside a template, control or page.
    /// </summary>
    class ControlElementConverter : TemplateElementConverter
    {
        private ControlElement _e;

        public ControlElementConverter(ControlElement e) : base(e)
        {
            _e = e;
        }

        protected override void ConvertOverride()
        {
            // nested control, create exclusive container for it.
            // TODO: need not be a DIV, isn't it?
            ConvertDefault(_e.TagName);
        }


        protected override void ConvertAfterContainerAppended()
        {
            // construct control
            Writer.WriteLine("{");
            Writer.Indent();
            Writer.WriteLine("let c = new " + _e.ControlType + "();");
            Writer.WriteLine("c.parent = e;");
            Writer.WriteLine("c.dc = dc;");

            // OFNREJVN7V: associate the control object with the DOM element.
            Writer.WriteLine("e['__wrapper'] = function() { return c; }");
        }

        protected override void ConvertAfterAttributesAppended()
        {
            Writer.UnIndent();
            Writer.WriteLine("}");
        }

        protected override void ConvertStaticAttribute(StaticAttribute a)
        {
            switch(a.Name)
            {
                case "controltype":
                    break;

                case "command":
                    Writer.WriteLine("c.command = " + a.Value.Quote() + ";");
                    break;

                default:
                    base.ConvertStaticAttribute(a);
                    break;
            }
        }
    }
}
