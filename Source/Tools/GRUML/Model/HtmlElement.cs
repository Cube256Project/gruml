using System.Xml;

namespace GRUML.Model
{
    /// <summary>
    /// Represent an HTML element.
    /// </summary>
    class HtmlElement : TemplateElement
    {
        public string Tag { get; private set; }

        /// <summary>
        /// Optinal namespace, to be set if not XHTML.
        /// </summary>
        public string NamespaceURI { get; private set; }

        public HtmlElement(string tag, string ns = null)
        {
            Tag = tag;
            NamespaceURI = ns;
        }

        public override bool Load(XmlElement e)
        {
            return base.Load(e);
        }
    }
}
