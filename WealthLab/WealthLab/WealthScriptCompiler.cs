namespace WealthLab
{
    using Fidelity.Components;
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(WealthScriptCompiler), "WealthScriptCompiler")]
    public class WealthScriptCompiler : Component
    {
        private CompilerErrorCollection compilerErrorCollection_0;
        private IContainer icontainer_0;
        private string string_0;

        public WealthScriptCompiler()
        {
            this.method_0();
        }

        public WealthScriptCompiler(IContainer container)
        {
            container.Add(this);
            this.method_0();
        }

        public WealthScript CompileSource(string references)
        {
            CodeDomProvider provider = new CSharpCodeProvider();
            CompilerParameters options = new CompilerParameters {
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = false
            };
            AssemblyLoader loader = new AssemblyLoader {
                BaseClass = "Object",
                Path = Path.GetDirectoryName(Application.ExecutablePath)
            };
            options.ReferencedAssemblies.Add("System.dll");
            options.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            options.ReferencedAssemblies.Add("System.Drawing.dll");
            foreach (Assembly assembly in loader.Assemblies)
            {
                if (!options.ReferencedAssemblies.Contains(assembly.Location))
                {
                    options.ReferencedAssemblies.Add(assembly.Location);
                }
            }
            foreach (string str in references.Split(new char[] { ';' }))
            {
                if (str.Trim() != "")
                {
                    string str2 = str;
                    if (!str2.ToUpper().EndsWith(".DLL"))
                    {
                        str2 = str2 + ".dll";
                    }
                    if (!options.ReferencedAssemblies.Contains(str2))
                    {
                        options.ReferencedAssemblies.Add(str2);
                    }
                }
            }
            CompilerResults results = provider.CompileAssemblyFromSource(options, new string[] { this.SourceCode });
            this.compilerErrorCollection_0 = results.Errors;
            if (results.Errors.HasErrors)
            {
                return null;
            }
            Assembly compiledAssembly = results.CompiledAssembly;
            foreach (System.Type type in compiledAssembly.GetTypes())
            {
                if ((!type.IsAbstract && (type.BaseType != null)) && ((type.BaseType.Name == "WealthScript") || (type.BaseType.Name == "WealthScriptTL")))
                {
                    return (Activator.CreateInstance(type) as WealthScript);
                }
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public CompilerErrorCollection CompilerErrors
        {
            get
            {
                return this.compilerErrorCollection_0;
            }
        }

        public string SourceCode
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }
    }
}

