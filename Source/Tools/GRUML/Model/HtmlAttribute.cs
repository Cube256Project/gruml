namespace GRUML.Model
{
    public abstract class HtmlAttribute
    {
        public string Name { get; set; }
    }

    class StaticAttribute : HtmlAttribute
    {
        public string Value { get; set; }
    }

    class BindingAttribute : HtmlAttribute
    {
        public BindingSyntax Binding { get; set; }
    }
}
