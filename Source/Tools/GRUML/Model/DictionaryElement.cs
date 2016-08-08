using System.Collections.Generic;

namespace GRUML.Model
{
    /// <summary>
    /// Tagged element can be part of dictionary.
    /// </summary>
    public abstract class DictionaryElement : Element
    {
        protected List<TemplateElement> _elements = new List<TemplateElement>();
        protected List<ResourceTag> _tags = new List<ResourceTag>();

        public IEnumerable<ResourceTag> Tags { get { return _tags; } }

        public IEnumerable<TemplateElement> Elements { get { return _elements; } }

        public override void AddChild(object child)
        {
            if (child is TemplateElement)
            {
                _elements.Add((TemplateElement)child);
            }
            else
            {
                base.AddChild(child);
            }
        }

    }
}
