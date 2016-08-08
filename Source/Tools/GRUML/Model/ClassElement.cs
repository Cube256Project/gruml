namespace GRUML.Model
{
    /// <summary>
    /// An element that will be converted into its own class.
    /// </summary>
    public abstract class ClassElement : ContainerElement
    {
        public string Name { get; protected set; }

        public string ScriptCode { get; protected set; }

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
            else
            {
                base.AddChild(child);
            }
        }

    }
}
