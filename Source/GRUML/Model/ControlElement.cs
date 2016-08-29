using System;
using System.Xml;

namespace GRUML.Model
{
    /// <summary>
    /// Represents a control embedded in another control or template.
    /// </summary>
    class ControlElement : TemplateElement
    {
        public string ControlType { get; private set; }

        public string TagName { get; private set; }

        public ControlElement()
        {
            TagName = "div";
        }

        public ControlElement(string controltype, string tagname)
        {
            TagName = tagname;
            ControlType = controltype;
        }

        public override bool Load(XmlElement e)
        {
            if (e.NamespaceURI == DocumentReader.PresentationURI)
            {
                if (e.HasAttribute("type"))
                {
                    if (null == ControlType)
                    {
                        ControlType = e.GetAttribute("type");
                    }
                    else
                    {
                        throw new Exception("control type redefinition.");
                    }
                }
                else
                {
                    if (null == ControlType)
                    {
                        throw new Exception("control definition requires 'type' attribute.");
                    }
                }
            }
            else
            {
                ControlType = e.LocalName;
            }

            return base.Load(e);
        }

        public override void AddChild(object child)
        {
            if (child is TemplateElement)
            {
                AddElement((TemplateElement)child);
            }
            else
            {
                base.AddChild(child);
            }
        }
    }
}
