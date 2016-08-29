using Common;
using GRUML.Model;

namespace GRUML.Converters
{
    class ResourceDictionaryConverter : ElementConverter
    {
        private ResourceDictionary _dictionary;

        public ResourceDictionaryConverter(ResourceDictionary e)
        {
            _dictionary = e;
        }

        protected override void ConvertOverride()
        {
            // emit templates
            foreach (var e in _dictionary.Items)
            {
                Context.Convert(e.Value);
            }

            // WXQ7XZ4EKL: convert dictionary into a function.
            Writer.WriteLine("// [dictionary] " + _dictionary.SequenceNumber);
            Writer.WriteLine("function " + _dictionary.UniqueName + "(key: string): any {");
            Writer.Indent();
            Writer.WriteLine("switch(key) {");
            Writer.Indent();

            foreach (var e in _dictionary.Items)
            {
                Writer.WriteLine("case " + e.Key.Quote() + ": return " + e.Value.UniqueName + ";");
            }

            Writer.WriteLine("default: return null;");
            Writer.UnIndent();
            Writer.WriteLine("}");

            Writer.UnIndent();
            Writer.WriteLine("}");
            Writer.WriteLine();
        }
    }
}
