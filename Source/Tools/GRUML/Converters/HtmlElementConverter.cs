using GRUML.Model;

namespace GRUML.Converters
{
    class HtmlElementConverter : TemplateElementConverter
    {
        private HtmlElement _e;

        protected HtmlElementConverter(HtmlElement e) : base(e)
        {
            _e = e;
        }

        public static HtmlElementConverter Create(HtmlElement e)
        {
            return new HtmlElementConverter(e);
        }

        protected override void ConvertOverride()
        {
            ConvertDefault(_e.Tag, _e.NamespaceURI);
        }
    }
}
