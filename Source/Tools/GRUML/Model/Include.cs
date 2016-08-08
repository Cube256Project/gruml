using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GRUML.Model
{
    /// <summary>
    /// Includes files from a file system.
    /// </summary>
    class Include : Element
    {
        public override bool IsChild {  get { return false; } }

        public override bool Load(XmlElement e)
        {
            string path;
            if (e.HasAttribute("source"))
            {
                path = e.GetAttribute("source");
                if (!Path.IsPathRooted(path))
                {
                    path = Path.GetFullPath(Path.Combine(Context.BaseDirectory, path));
                }
            }
            else
            {
                path = Context.BaseDirectory;
            }

            Regex filter;
            if (e.HasAttribute("filter"))
            {
                filter = WildcardFactory.BuildWildcardsFromList(e.GetAttribute("filter"));
            }
            else
            {
                filter = WildcardFactory.BuildWildcards("*");
            }

            int maxdepth = 1;
            if (e.HasAttribute("recurse"))
            {
                bool recursef;
                if (bool.TryParse(e.GetAttribute("recurse"), out recursef))
                {
                    maxdepth = int.MaxValue;
                }
            }

            Recurse(path, filter, maxdepth);
            
            return false;
        }

        #region Private Methods

        private void Recurse(string path, Regex filter, int maxdepth)
        {
            if (Directory.Exists(path))
            {
                if (maxdepth > 0)
                {
                    foreach (var subdir in Directory.GetDirectories(path))
                    {
                        Recurse(subdir, filter, maxdepth - 1);
                    }
                }

                foreach (var file in Directory.GetFiles(path))
                {
                    Recurse(file, filter, 0);
                }
            }
            else if (File.Exists(path))
            {
                if (filter.IsMatch(path))
                {
                    if (Context.SetFileProcessed(path))
                    {
                        LoadFile(path);
                    }
                }
            }
        }

        private void LoadFile(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            switch (ext)
            {
                case ".xml":
                    LoadXml(path);
                    break;

                case ".ts":
                case ".js":
                    LoadScript(path);
                    break;

                case ".css":
                    LoadStyle(path);
                    break;

                default:
                    Log.Warning("ignored file {0}.", path.Quote());
                    break;
            }
        }

        private void LoadStyle(string path)
        {
            Context.Container.AddChild(new CascadingStyleSheet(path));
        }

        private void LoadScript(string path)
        {
            Log.Trace("including {0} ...", path.Quote());
            Context.Container.AddChild(new TypeScriptUnit(path));
        }

        private void LoadXml(string path)
        {
            Log.Debug("loading {0} ...", path.Quote());
            var dom = new XmlDocument();
            dom.Load(path);

            Context.Container.AddChild(Context.Load(dom));
        }

        #endregion
    }
}
