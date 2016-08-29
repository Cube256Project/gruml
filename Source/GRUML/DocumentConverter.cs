using Common;
using Common.Tokenization;
using GRUML.Converters;
using GRUML.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GRUML
{
    public class DocumentConverter
    {
        #region Private

        private Stack<Element> _stack = new Stack<Element>();

        #endregion

        #region Properties

        internal TokenWriter Writer { get; set; }

        internal int Depth { get { return _stack.Count; } }

        internal IEnumerable<Element> Stack {  get { return _stack; } }

        internal bool IsControl
        {
            get
            {
                foreach (var ancestor in _stack)
                {
                    if (ancestor is DataTemplate)
                    {
                        break;
                    }
                    else if (ancestor is ClassElement)
                    {
                        return true;
                    }
                }

                return false;
            }
        }


        #endregion

        public DocumentConverter(TokenWriter writer)
        {
            Writer = writer;
        }

        public void Convert(Element e)
        {
            if (!_stack.Any())
            {
                Writer.WriteLine();
            }

            try
            {
                _stack.Push(e);

                CreateConverter(e).Convert();
            }
            finally
            {
                _stack.Pop();
            }
        }

        private ElementConverter CreateConverter(Element e)
        {
            ElementConverter result;
            if (e is ResourceDictionary)
            {
                result = new ResourceDictionaryConverter((ResourceDictionary)e);
            }
            else if (e is ClassElement)
            {
                result = new ControlDefinitionConverter((ClassElement)e);
            }
            else if (e is DictionaryElement)
            {
                result = new DataTemplateConverter((DictionaryElement)e);
            }
            else if (e is HtmlElement)
            {
                result = HtmlElementConverter.Create((HtmlElement)e);
            }
            else if (e is ControlElement)
            {
                result = new ControlElementConverter((ControlElement)e);
            }
            else if (e is TextElement)
            {
                result = new TextElementConverter((TextElement)e);
            }
            else if (e is Project)
            {
                result = new ProjectConverter((Project)e);
            }
            else
            {
                throw new NotSupportedException("no converter for [" + e + "].");
            }

            result.Context = this;
            result.Writer = Writer;
            return result;
        }
    }
}
