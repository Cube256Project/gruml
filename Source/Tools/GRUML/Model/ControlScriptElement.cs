using System.Xml;

namespace GRUML.Model
{
    /// <summary>
    /// A script associated with a control class.
    /// </summary>
    class ControlScriptElement : ScriptElement
    {
        public override bool Load(XmlElement e)
        {
            Code = e.InnerText;

            // no children
            return true;
        }

    }
}
