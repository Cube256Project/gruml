using Common;
using Common.Tokenization;
using GRUML.Configuration;
using GRUML.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GRUML
{
    /// <summary>
    /// Command line utility for GRUML, can also be used as an object.
    /// </summary>
    public class Utility
    {
        #region Properties

        public string InputDirectory { get; set; }

        public string OutputDirectory { get; set; }

        public string ConfigurationFile { get; set; }

        public string StageDirectory { get; set; }

        public Element Root { get; set; }

        public Project Project { get { return Root as Project; } }

        public Assembly Library { get; set; }

        public string BundleName { get; private set; }

        public bool UseLibrary { get { return null == Project ? true : Project.UseLibrary; } }

        public List<string> StyleSheets = new List<string>();
        private bool Verbose;

        #endregion

        static int Main(string[] args)
        {
            try
            {
                new Utility().Run(args);
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error("error: {0}", ex.Message);
                Log.Debug("{0}", ex.StackTrace);
                return 1;
            }
        }

        private static void SetupLog()
        {
            // TODO: this has issues with parallel implementations when used as a tool.
            Log.DefaultMinimumSeverity = LogSeverity.debug;

            LogContext.Default.AddFollower(new ConsoleLogFollower());
        }

        #region Construction

        public Utility()
        {
            SetupLog();

            InputDirectory = Directory.GetCurrentDirectory();
            Library = typeof(Library).Assembly;
        }

        #endregion

        public void Run(string[] args)
        {
            Log.Information("GRUML converter utility, v{0}", GetType().Assembly.GetName().Version);

            ParseArguments(args);

            Uri uri = ParseConfigurationSource();

            // load the project
            Root = new DocumentReader { BaseDirectory = InputDirectory }.Load(uri);

            // setup directories
            SetupStageDirectory();
            SetupOutputDirectory();

            // convert project
            GenerateCode();

            // generate tsconfig.json ...
            var tsc = new TypeScriptConfiguration();

            var bundlename = null == Project ? null : Project.BundleName;
            BundleName = bundlename ?? "bundle.js";

            tsc.compilerOptions.outFile = Path.Combine(OutputDirectory, BundleName);

            if (UseLibrary)
            {
                tsc.files.Add("Library.ts");
                InstallLibrary(Library);
            }

            tsc.files.Add("generated-output.ts");


            WriteTypeScriptSettings(tsc);

            var appdir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            Log.Debug("looking for NPM in {0} ...", appdir.Quote());

            var tooloutput = new TokenWriter();

            Log.Debug("converting typescript ...");
            var pr = new ProcessRunner();
            pr.WorkingDirectory = StageDirectory;
            pr.FileName = Path.Combine(appdir, @"npm\tsc.cmd");
            pr.OnOutput += (sender, e) => tooloutput.WriteLine("> " + e.Data);
            pr.Start();

            var exitcode = pr.Wait();
            Log.Debug("typescript => {0}", exitcode);
            if (0 != exitcode)
            {
                Log.Debug("{0}", tooloutput.Text);
            }


            // index template
            if (null != Project)
            {
                EmitPages();
            }
        }

        private void GenerateCode()
        {
            var writer = ConvertProject();

            var outputscript = Path.Combine(StageDirectory, "generated-output.ts");
            File.WriteAllText(outputscript, writer.Text);

            if (Verbose)
            {
                Log.Debug("generated code: {0}", writer.Text);
            }
        }

        private void InstallLibrary(Assembly library)
        {
            var numberofiles = 0;
            var filter = WildcardFactory.BuildWildcards("*.ts");
            foreach (var libitem in library.GetManifestResourceNames())
            {
                var parts = libitem.Split('.');
                var name = parts.Reverse().Take(2).Reverse().ToSeparatorList(".");

                if (filter.IsMatch(name))
                {
                    var stagepath = Path.Combine(StageDirectory, name);
                    using (var file = File.OpenWrite(stagepath))
                    {
                        library.GetManifestResourceStream(libitem).CopyTo(file);
                        numberofiles++;
                    }
                }
            }

            Log.Debug("copied {0} file(s) from {1}.", numberofiles, library.GetName().Name);
        }

        private TokenWriter ConvertProject()
        {
            // generate typescript code 
            var writer = new TokenWriter();
            var includes = new List<string>();

            if (UseLibrary)
            {
                var defaultincludes = new string[]
            {
                "TypeSystem.ts",
                "ResourceDictionary.ts",
                "Control.ts",
                "ItemsControl.ts",
                "Page.ts",
                "BindingOperations.ts"
            };

                includes.AddRange(defaultincludes);
            }

            if (null != Project)
            {
                foreach (var script in Project.Scripts)
                {
                    if (script is TypeScriptUnit)
                    {
                        // include into generated code ...
                        /*writer.WriteLine("// *** " + script.Location + " ***");
                        writer.WriteLine(script.Code);*/
                        var dest = Path.Combine(StageDirectory, script.FileName);
                        File.WriteAllText(dest, script.Code);

                        includes.Add(script.FileName);

                    }
                    else if (script is CascadingStyleSheet)
                    {
                        // copy to output
                        var dest = Path.Combine(OutputDirectory, script.FileName);
                        File.WriteAllText(dest, script.Code);

                        StyleSheets.Add(script.FileName);
                    }
                    else if (script is StyleSheetReference)
                    {
                        StyleSheets.Add(script.FileName);
                    }
                    else
                    {
                        Log.Warning("unabled to do [{0}].", script);
                    }
                }
            }

            foreach (var include in includes)
            {
                writer.WriteLine("/// <reference path=\"" + include + "\" />");
            }

            writer.WriteLine();

            writer.WriteLine("// *** generated code ***");

            // convert the root element
            new DocumentConverter(writer).Convert(Root);

            return writer;
        }

        private void WriteTypeScriptSettings(TypeScriptConfiguration tsc)
        {
            // serialize it JSON ...
            var settings = new JsonSerializerSettings();
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            settings.NullValueHandling = NullValueHandling.Ignore;

            // to file
            File.WriteAllText(Path.Combine(StageDirectory, "tsconfig.json"), JsonConvert.SerializeObject(tsc, settings));
        }

        private void EmitPages()
        {
            foreach (var page in Project.Pages)
            {
                var dom = new XmlDocument();
                dom.PreserveWhitespace = true;
                dom.Load(Library.GetResourceStream("page-template.xml"));
                var nm = new XmlNamespaceManager(dom.NameTable);
                nm.AddNamespace("e", DocumentReader.XHTMLURI);
                nm.AddNamespace("z", "gruml:inject");

                var output = Path.Combine(OutputDirectory, page.Name + ".html");

                InjectStyle(dom, nm);
                InjectScript(dom, nm);

                var node = dom.SelectSingleNode("//z:main", nm);
                if (null == node)
                {
                    throw new Exception("unable to inject 'z:main' was not found.");
                }

                var writer = new TokenWriter();

                if (null != Project.Dictionary)
                {
                    writer.WriteLine("ResourceDictionary.install(" + Project.Dictionary.UniqueName.Quote() + ", gizmo);");
                }

                writer.Write("let c = new " + page.Name + "();");
                writer.Write(" c.parent = gizmo; c.start();");
                // writer.Write("UserLog.Trace('foo');");

                var sibling = node.NextSibling;
                var parent = node.ParentNode;
                parent.RemoveChild(node);
                parent.InsertBefore(node.OwnerDocument.CreateTextNode(writer.Text), sibling);

                dom.Save(output);

                Log.Debug("page {0} created.", output.Quote());
            }
        }

        private void InjectStyle(XmlDocument dom, XmlNamespaceManager nm)
        {
            var stylenode = dom.SelectSingleNode("//z:style", nm);
            if (null == stylenode)
            {
                throw new Exception("unable to inject 'z:style' was not found.");
            }

            foreach (var style in StyleSheets)
            {
                var link = dom.CreateElement("link");
                link.SetAttribute("rel", "stylesheet");
                link.SetAttribute("href", style);
                stylenode.ParentNode.InsertAfter(link, stylenode);
            }

            stylenode.ParentNode.RemoveChild(stylenode);
        }

        private void InjectScript(XmlDocument dom, XmlNamespaceManager nm)
        {
            var stylenode = dom.SelectSingleNode("//z:script", nm);
            if (null == stylenode)
            {
                throw new Exception("unable to inject 'z:script' was not found.");
            }

            var script = dom.CreateElement("script");
            script.SetAttribute("src", BundleName);
            script.InnerText = " ";

            stylenode.ParentNode.ReplaceChild(script, stylenode);
        }

        private void SetupOutputDirectory()
        {
            if(null == OutputDirectory)
            {
                OutputDirectory = Path.Combine(InputDirectory, "output");
            }

            Directory.CreateDirectory(OutputDirectory);

            Log.Debug("output directory {0} ...", OutputDirectory.Quote());

        }

        private Uri ParseConfigurationSource()
        {
            if (null == ConfigurationFile)
            {
                ConfigurationFile = Path.Combine(InputDirectory, "gruml.xml");
            }

            var path = ConfigurationFile;
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(InputDirectory, path);
            }

            // validate-normalize
            path = Path.GetFullPath(path);

            Uri uri;
            if (!Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                throw new ArgumentException("specified configuration path is invalid.");
            }

            if (uri.Scheme == "file")
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("configuration file " + path.Quote() + " was not found.");
                }
            }

            return uri;
        }

        private void SetupStageDirectory()
        {
            if (null == StageDirectory)
            {
                StageDirectory = FileSystemUtilities.CreateTemporaryDirectory();
            }

            if (Directory.Exists(StageDirectory))
            {
                Directory.Delete(StageDirectory, true);
            }

            Directory.CreateDirectory(StageDirectory);

            Log.Debug("using stage {0} ...", StageDirectory.Quote());
        }

        private void ParseArguments(string[] args)
        {
            int i = 0, s = 0;

            for (; i < args.Length; ++i)
            {
                var a = args[i];
                switch(s)
                {
                    case 0:
                        if (a == "-d" || a == "--directory")
                        {
                            s = 1;
                        }
                        else if (a == "--output-directory")
                        {
                            s = 2;
                        }
                        else if (a == "-v" || a == "--verbose")
                        {
                            Log.DefaultMinimumSeverity = LogSeverity.unspecified;
                            Verbose = true;
                        }
                        else
                        {
                            throw new ArgumentException("unsupported option " + a.Quote() + ".");
                        }
                        break;

                    case 1:
                        InputDirectory = Path.GetFullPath(a);
                        s = 0;
                        break;

                    case 2:
                        OutputDirectory = Path.GetFullPath(a);
                        s = 0;
                        break;
                }
            }
        }
    }
}
