using Common;
using GRUML.Model;

namespace GRUML.Converters
{
    class TextElementConverter : ElementConverter
    {
        private TextElement _e;

        public TextElementConverter(TextElement e)
        {
            _e = e;
        }

        protected override void ConvertOverride()
        {
            Writer.WriteLine("e.appendChild(document.createTextNode(" + _e.Text.CQuote() + "));");
        }
    }
}
