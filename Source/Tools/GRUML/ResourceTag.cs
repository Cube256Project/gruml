namespace GRUML
{
    public abstract class ResourceTag
    {
        public string Value { get; protected set; }

        public override string ToString()
        {
            return Value;
        }
    }

    /// <summary>
    /// VITIJBLJY2: construct a datatype resource tag.
    /// </summary>
    class TypeTag : ResourceTag
    {
        public TypeTag(string typename)
        {
            Value = "template:class:" + typename;
        }
    }

    class NameTag : ResourceTag
    {
        public NameTag(string name)
        {
            Value = "template:name:" + name;
        }
    }
}
