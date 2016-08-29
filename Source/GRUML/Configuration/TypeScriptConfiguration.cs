using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRUML.Configuration
{
    class TypeScriptCompilerOptions
    {
        public string target = "es5";
        public string outFile;
    }

    class TypeScriptConfiguration
    {
        public TypeScriptCompilerOptions compilerOptions = new TypeScriptCompilerOptions();
        public List<string> files = new List<string>();
    }
}
