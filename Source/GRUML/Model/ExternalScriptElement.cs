using System.IO;

namespace GRUML.Model
{
    public abstract class ExternalScriptElement : ScriptElement
    {
        public string Location { get; private set; }

        public virtual string FileName {  get { return System.IO.Path.GetFileName(Location); } }

        public ExternalScriptElement()
        { }

        public ExternalScriptElement(string path)
        {
            Location = path;
            Code = File.ReadAllText(path);
        }
    }
}
