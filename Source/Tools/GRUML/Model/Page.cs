using System;
using System.Xml;

namespace GRUML.Model
{
    public class Page : ClassElement
    {
        public override bool Load(XmlElement e)
        {
            if(e.HasAttribute("name"))
            {
                Name = e.GetAttribute("name");
            }
            else
            {
                throw new Exception("page element requires 'name' attribute.");
            }

            return base.Load(e);
        }
    }
}
