using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GRUML.Model
{
    /// <summary>
    /// The definition of a control.
    /// </summary>
    public class ControlDefinition : ClassElement
    {
        public string BaseType { get; private set; }

        public override bool Load(XmlElement e)
        {
            if (e.NamespaceURI == DocumentReader.PresentationURI)
            {
                // template namespace -> go for 'name' attribute
                if (e.HasAttribute("name"))
                {
                    Name = e.GetAttribute("name");
                }
                else
                {
                    throw new Exception("control definition requires 'name' attribute.");
                }
            }
            else
            {
                // qualified namespace -> use localname
                Name = e.LocalName;
            }

            if (e.HasAttribute("basetype"))
            {
                BaseType = e.GetAttribute("basetype");
            }

            // let caller recurse
            return false;
        }
    }
}
