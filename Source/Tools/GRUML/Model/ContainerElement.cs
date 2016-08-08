using System;
using System.Collections.Generic;

namespace GRUML.Model
{
    /// <summary>
    /// An element containing other elements and attributes.
    /// </summary>
    public abstract class ContainerElement : Element
    {
        #region Private

        private List<TemplateElement> _elements = new List<TemplateElement>();
        private List<HtmlAttribute> _attributes = new List<HtmlAttribute>();

        #endregion

        #region Properties

        public IEnumerable<TemplateElement> Elements { get { return _elements; } }

        public IEnumerable<HtmlAttribute> Attributes { get { return _attributes; } }

        /// <summary>
        /// Optional dictionary associated with the element.
        /// </summary>
        public ResourceDictionary Dictionary { get; protected set; }

        #endregion

        protected void AddElement(TemplateElement e)
        {
            _elements.Add(e);
        }

        protected void AddAttribute(HtmlAttribute a)
        {
            _attributes.Add(a);
        }

        public override void AddChild(object child)
        {
            if (child is ResourceDictionary)
            {
                if (null == Dictionary)
                {
                    Dictionary = (ResourceDictionary)child;
                }
                else
                {
                    throw new ArgumentException("a dictionary is already associated with the element.");
                }
            }
            else if (child is TemplateElement)
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
