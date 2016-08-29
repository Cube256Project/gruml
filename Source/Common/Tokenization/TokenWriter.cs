using System.IO;
using System.Text;

namespace Common.Tokenization
{
    public class TokenWriter
    {
        #region Private

        private StringBuilder _text = new StringBuilder();
        private int _indent = 0;
        private bool _ateol = true;
        private int _linechars = 0;

        #endregion

        #region Properties

        public string Text { get { return _text.ToString(); } }

        #endregion

        #region Basic Writing

        public void Write(string text)
        {
            if (_ateol)
            {
                _text.Append(new string(' ', _indent * 4));
                _ateol = false;
            }

            _text.Append(text);
            _linechars += text.Length;

            /*if (_linechars > 120)
            {
                _text.AppendLine(); _linechars = 0;
            }*/ 
        }

        public void WriteLine()
        {
            _text.AppendLine();
            _ateol = true;
        }

        public void WriteLine(string text)
        {
            if(null != text) Write(text);
            WriteLine();
        }

        public void WriteMultiLine(string text)
        {
            var reader = new StringReader(text);
            var first = true;

            while (true)
            {
                var line = reader.ReadLine();
                if (null == line) break;

                if (first) first = false;
                else WriteLine();

                Write(line);
            }
        }

        public void Indent()
        {
            _indent++;
        }

        public void UnIndent()
        {
            _indent--;
        }

        #endregion
    }
}
