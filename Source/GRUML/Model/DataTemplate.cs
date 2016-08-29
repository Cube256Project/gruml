using System.Xml;

namespace GRUML.Model
{
    class DataTemplate : DictionaryElement
    {
        public override bool Load(XmlElement e)
        {
            if (e.HasAttribute("key"))
            {
                _tags.Add(new NameTag(e.GetAttribute("key")));
            }

            if (e.HasAttribute("datatype"))
            {
                _tags.Add(new TypeTag(e.GetAttribute("datatype")));
            }

            return base.Load(e);
        }
    }
}
