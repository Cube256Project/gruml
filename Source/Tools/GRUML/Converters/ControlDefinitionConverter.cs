using Common;
using GRUML.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GRUML.Converters
{
    class ControlDefinitionConverter : TemplateElementConverter
    {
        private ClassElement _e;
        private HashSet<string> _names = new HashSet<string>();
        private static Regex _rxscript = new Regex(@"#[a-z0-9_]+", RegexOptions.IgnoreCase);

        public ControlDefinitionConverter(ClassElement e) : base(e)
        {
            _e = e;
        }

        protected override void ConvertOverride()
        {
            // emit related dictionaries before the control
            EmitDictionaries(_e);

            // control definition, extend control or specified type
            Writer.WriteLine("// [control] " + _e.SequenceNumber);
            Writer.Write("class " + _e.Name);

            string basetype;
            if (_e is ControlDefinition)
            {
                basetype = ((ControlDefinition)_e).BaseType ?? "Control";
            }
            else if (_e is Page)
            {
                basetype = "Page";
            }
            else
            {
                basetype = "Control";
            }

            Writer.Write(" extends " + basetype);

            // class body
            Writer.WriteLine(" {");
            Writer.Indent();

            // TODO: '$type' is obsolete!?
            Writer.WriteLine("$type: string = " + _e.Name.Quote() + ";");

            // named elements declarations
            EmitElementDeclarations();

            // constructor
            Writer.WriteLine("constructor() { super(); }");

            Writer.WriteLine("protected render(e: any): void {");
            BeginRender();

            ConvertAfterContainerAppended();

            Writer.WriteLine("let dc = this.dc;");
            Writer.WriteLine("let self = this;");

            // Writer.WriteLine("UserLog.Trace('render: ' + " + _e.Name.Quote() + " + ' dc ' + dc);");

            Writer.WriteLine("// definition elements ..");

            ConvertElements();

            EndRender();

            // custom code
            EmitScriptCode();

            Writer.UnIndent();
            Writer.WriteLine("}");
        }

        private void EmitScriptCode()
        {
            var bad = new HashSet<string>();
            var code = _rxscript.Replace(_e.ScriptCode, m =>
            {
                var id = m.Value.Substring(1);
                if (_names.Contains(id))
                {
                    return "this." + id;
                }
                else
                {
                    bad.Add(id);
                    return "?invalid-reference?";
                }
            });

            if (bad.Any())
            {
                throw new Exception("unresolve references: " + bad.ToSeparatorList());
            }

            Writer.WriteLine(code);
        }

        private void EmitElementDeclarations()
        {
            ForAllElements(_e, e =>
            {
                if (null != e.ID)
                {
                    if (_names.Add(e.ID))
                    {
                        Writer.WriteLine(e.ID + ": any;");
                    }
                    else
                    {
                        throw new Exception("duplicate element identifier " + e.ID.Quote() + ".");
                    }
                }
            });
        }
    }
}
