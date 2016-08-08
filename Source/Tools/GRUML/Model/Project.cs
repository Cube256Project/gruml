using System;
using System.Collections.Generic;
using System.Xml;

namespace GRUML.Model
{
    /// <summary>
    /// A project element encompasses the global dictionary, controls and external scripts.
    /// </summary>
    /// <remarks>
    /// <para>Use the <see cref="DocumentReader"/> to parse XML files.</para></remarks>
    public class Project : ContainerElement
    {
        public List<ExternalScriptElement> Scripts = new List<ExternalScriptElement>();

        public List<ControlDefinition> Controls = new List<ControlDefinition>();

        public List<Page> Pages = new List<Page>();

        /// <summary>
        /// Default control.
        /// </summary>
        public string DefaultControlType { get; set; }

        public override bool Load(XmlElement e)
        {
            if (e.HasAttribute("default"))
            {
                DefaultControlType = e.GetAttribute("default");
            }

            return base.Load(e);
        }

        public override void AddChild(object child)
        {
            if (child is ExternalScriptElement)
            {
                Scripts.Add((ExternalScriptElement)child);
            }
            else if (child is ControlDefinition)
            {
                Controls.Add((ControlDefinition)child);
            }
            else if (child is Page)
            {
                Pages.Add((Page)child);
            }
            else if (child is ResourceDictionary)
            {
                if (null == Dictionary)
                {
                    Dictionary = (ResourceDictionary)child;
                }
                else
                {
                    Dictionary.Merge((ResourceDictionary)child);
                }
            }
            else
            {
                base.AddChild(child);
            }
        }
    }
}
