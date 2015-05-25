using System;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Linq;

namespace Tests
{
    internal static class Utils
    {
        /// <summary>
        /// Check if provided code implements given interface
        /// </summary>
        /// <param name="code">Implementation code</param>
        /// <param name="i">Interface to implement</param>
        /// <param name="implementationClassName">Class, which implements given interface</param>
        /// <returns></returns>
        public static bool CompileImplementation(string code, Type i, string implementationClassName)
        {
            var cp = new CompilerParameters { GenerateInMemory = true };
          
            // reference Test assemly with test types
            cp.ReferencedAssemblies.Add(Path.GetFileNameWithoutExtension(typeof(Utils).Assembly.Location) + ".dll");

            // try to compile
            CompilerResults cr = new CSharpCodeProvider().CompileAssemblyFromSource(cp, code);

            if (cr.Errors.Count > 0)
                return false;

            // get resulting type
            var type = cr.CompiledAssembly.GetType(implementationClassName);
            
            // check if interface is really implemented
            return i.IsAssignableFrom(type);
        }
    }
}
