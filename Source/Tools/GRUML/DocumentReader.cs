using Common;
using GRUML.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GRUML
{
    /// <summary>
    /// Loads a GRUML document.
    /// </summary>
    public class DocumentReader
    {
        #region Namespace Identifiers

        public const string PresentationURI = "cfs:schema:presentation:template";
        public const string ControlsURI = "cfs:collection:presentation:control:default";
        public const string XHTMLURI = "http://www.w3.org/1999/xhtml";
        public const string SVGURI = "http://www.w3.org/2000/svg";

        #endregion

        #region Private

        private Stack<Element> _stack = new Stack<Element>();
        private HashSet<string> _files = new HashSet<string>();

        #endregion

        #region Properties

        public enum ReaderState
        {
            initial,
            dictionary,
            template,
            control,
            project
        }

        public ReaderState State { get; private set; }

        internal ContainerElement Container { get { return _stack.OfType<ContainerElement>().First(); } }

        public string BaseDirectory { get; set; }

        #endregion

        #region Public Methods

        public Element Load(XmlDocument dom)
        {
            return Load(dom.DocumentElement);
        }

        public Element Load(Uri uri)
        {
            if (uri.Scheme == "file")
            {
                SetFileProcessed(uri.LocalPath);
            }

            var dom = new XmlDocument();
            dom.Load(uri.ToString());
            return Load(dom);
        }

        #endregion

        #region Internal Methods

        internal bool SetFileProcessed(string path)
        {
            return _files.Add(path);
        }

        #endregion

        #region Private Methods

        private Element Load(XmlText e)
        {
            return new TextElement(e.InnerText);
        }

        private Element Load(XmlElement e)
        {
            // create corresponding object ...
            var result = CreateElement(e);

            // capture previous state
            var previous = State;
            var pushed = false;
            try
            {
                // can we have it here?
                if (!Validate(result))
                {
                    throw new Exception("unexpected element " + e.Name.Quote() + " [" + result + "] in state " + State + ".");
                }

                result.Context = this;

                // expose container, if, to make it available to element Load overrides.
                _stack.Push(result);
                pushed = true;

                // try to load
                if (!result.Load(e))
                {
                    // did not process children.
                    foreach (var child in e.ChildNodes.OfType<XmlNode>())
                    {
                        Element u;
                        if (child is XmlElement)
                        {
                            u = Load((XmlElement)child);
                        }
                        else if (child is XmlText)
                        {
                            u = Load((XmlText)child);
                        }
                        else if(child is XmlComment)
                        {
                            // ignore
                            continue;
                        }
                        else
                        {
                            Log.Warning("ignored [{0}]", child.GetType().Name);
                            continue;
                        }

                        if (u.IsChild)
                        {
                            result.AddChild(u);
                        }
                    }
                }
            }
            finally
            {
                if (pushed) _stack.Pop();

                // restore previous state
                SetState(previous);
            }


            return result;
        }

        private bool Validate(Element e)
        {
            var result = false;
            switch (State)
            {
                case ReaderState.initial:
                    if (e is ResourceDictionary)
                    {
                        SetState(ReaderState.dictionary);
                        result = true;
                    }
                    else if (e is ControlDefinition)
                    {
                        SetState(ReaderState.control);
                        result = true;
                    }
                    else if (e is Project)
                    {
                        SetState(ReaderState.project);
                        result = true;
                    }
                    break;

                case ReaderState.project:
                    if (e is Include)
                    {
                        result = true;
                    }
                    else if (e is ControlDefinition)
                    {
                        SetState(ReaderState.control);
                        result = true;
                    }
                    else if (e is Page)
                    {
                        SetState(ReaderState.control);
                        result = true;
                    }
                    else if (e is ResourceDictionary)
                    {
                        SetState(ReaderState.dictionary);
                        result = true;
                    }
                    break;

                case ReaderState.control:
                case ReaderState.template:
                    if (e is TemplateElement)
                    {
                        SetState(ReaderState.template);
                        result = true;
                    }
                    else if (e is ResourceDictionary)
                    {
                        SetState(ReaderState.dictionary);
                        result = true;
                    }
                    else if (e is ScriptElement && State == ReaderState.control)
                    {
                        result = true;
                    }
                    break;

                case ReaderState.dictionary:
                    if (e is DataTemplate)
                    {
                        SetState(ReaderState.template);
                        return true;
                    }
                    else if (e is Resource)
                    {
                        SetState(ReaderState.template);
                        return true;
                    }
                    break;

                default:
                    break;
            }

            return result;
        }

        private void SetState(ReaderState newstate)
        {
            if(State != newstate)
            {
                State = newstate;
            }
        }

        private Element CreateElement(XmlElement e)
        {
            Element result;
            var ns = e.NamespaceURI;
            if (ns == PresentationURI)
            {
                // special elements and HTML
                switch (e.LocalName)
                {
                    case "dictionary":
                        result = new ResourceDictionary();
                        break;

                    case "template":
                        result = new DataTemplate();
                        break;

                    case "control":
                        result = CreateControlElement(e);
                        break;

                    case "script":
                        result = new ControlScriptElement();
                        break;

                    case "project":
                        result = new Project();
                        break;

                    case "include":
                        result = new Include();
                        break;

                    case "page":
                        result = new Page();
                        break;

                    case "resource":
                        result = new Resource();
                        break;

                    default:
                        result = CreateHtmlElement(e);
                        break;
                }
            }
            else if (ns == XHTMLURI)
            {
                // HTML only
                result = CreateHtmlElement(e);
            }
            else if (ns == ControlsURI)
            {
                // explicit control
                result = CreateControlElement(e);
            }
            else if (ns == SVGURI)
            {
                result = CreateHtmlElement(e);
            }
            else
            {
                throw new Exception("unsupported namespace " + ns.Quote() + ".");
            }

            return result;
        }

        private static Element CreateHtmlElement(XmlElement e)
        {
            if (e.NamespaceURI == XHTMLURI || e.NamespaceURI == PresentationURI)
            {
                switch (e.LocalName)
                {
                    /*case "button":
                        return new ControlElement("ButtonControl", "button");*/

                    default:
                        return new HtmlElement(e.LocalName);
                }
            }
            else
            {
                return new HtmlElement(e.LocalName, e.NamespaceURI);
            }
        }

        private Element CreateControlElement(XmlElement e)
        {
            switch (State)
            {
                case ReaderState.initial:
                case ReaderState.project:
                    return new ControlDefinition();

                default:
                    return new ControlElement();
            }
        }

        #endregion
    }
}
