using System.Xml;

namespace GRUML.Model
{
    public class StyleSheetReference : ExternalScriptElement
    {
        private string _filename;

        public override string FileName {  get { return _filename; } }

        public StyleSheetReference(XmlElement e) : base()
        {
            _filename = e.GetAttribute("ref");
        }
    }
}
