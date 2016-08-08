using GRUML.Model;

namespace GRUML.Converters
{
    class DataTemplateConverter : ElementConverter
    {
        private DictionaryElement _template;

        public DataTemplateConverter(DictionaryElement template)
        {
            _template = template;
        }

        protected override void ConvertOverride()
        {
            Writer.WriteLine("// [template] " + _template.SequenceNumber);

            Writer.WriteLine("function " + _template.UniqueName + "(dc: any, e: any): void {");

            BeginRender();

            foreach (var e in _template.Elements)
            {
                Context.Convert(e);
            }

            EndRender();
        }
    }
}
