using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GRUML.Model
{
    class TextElement : TemplateElement
    {
        public string Text { get; private set; }

        public TextElement(string text)
        {
            Text = new Regex(@"\s+").Replace(text, m => " ");
        }
    }
}
