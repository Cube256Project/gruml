using System.Collections.Generic;

namespace GRUML.Model
{
    /// <summary>
    /// An element that will be converted into its own class.
    /// </summary>
    public abstract class ClassElement : ContainerElement
    {
        public string Name { get; protected set; }

        public string ScriptCode { get; protected set; }

        public List<EventBinding> EventBindings = new List<EventBinding>();

        protected ClassElement()
        {
            ScriptCode = string.Empty;
        }

        public override void AddChild(object child)
        {
            if (child is ScriptElement)
            {
                ScriptCode += ((ScriptElement)child).Code;
            }
            else if (child is EventBinding)
            {
                EventBindings.Add((EventBinding)child);
            }
            else
            {
                base.AddChild(child);
            }
        }

    }
}
