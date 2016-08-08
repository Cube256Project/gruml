using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GRUML.Model
{
    class Resource : DictionaryElement
    {
        public override bool Load(XmlElement e)
        {
            if (e.HasAttribute("key"))
            {
                _tags.Add(new NameTag(e.GetAttribute("key")));
            }
            else
            {
                throw new ArgumentException("resource element requires 'key' attribute.");
            }

            return base.Load(e);
        }
    }
}
