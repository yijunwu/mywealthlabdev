namespace WealthLab.IndexDefinitions
{
    using Fidelity.Components;
    using Microsoft.CSharp;
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using WealthLab;

    internal class IndicatorGenerator
    {
        private Assembly _asm;
        private Dictionary<string, DataSeries> _cached = new Dictionary<string, DataSeries>();
        private string _code = "";
        private static CompilerParameters _compileParams = new CompilerParameters();
        private MethodInfo _method;
        private static List<string> _namespaces = new List<string>();

        static IndicatorGenerator()
        {
            AssemblyLoader loader = new AssemblyLoader {
                BaseClass = "IndicatorHelper",
                Path = Path.GetDirectoryName(Application.ExecutablePath)
            };
            foreach (Assembly assembly in loader.Assemblies)
            {
                _compileParams.ReferencedAssemblies.Add(assembly.ManifestModule.Name);
                foreach (System.Type type in loader.TypesInAssembly(assembly))
                {
                    string item = type.Namespace;
                    if (!_namespaces.Contains(item))
                    {
                        _namespaces.Add(item);
                    }
                }
            }
            _namespaces.Add("WealthLab");
            _compileParams.ReferencedAssemblies.Add("WealthLab.dll");
            _compileParams.GenerateInMemory = true;
        }

        private string FixCode(string code)
        {
            string str = code.Trim();
            if (!str.EndsWith(";"))
            {
                str = str + ";";
            }
            string[] strArray = str.Split(new char[] { '(' });
            if (strArray.Length == 2)
            {
                string str2 = strArray[1].Trim();
                if (!str2.StartsWith("Bars"))
                {
                    str = strArray[0] + "(Bars." + str2;
                }
            }
            return str;
        }

        private void GenerateMethod()
        {
            this._asm = null;
            this._method = null;
            this._cached.Clear();
            CodeMemberMethod method = new CodeMemberMethod {
                Name = "Series",
                Attributes = MemberAttributes.Public | MemberAttributes.Static
            };
            method.Parameters.Add(new CodeParameterDeclarationExpression(typeof(Bars), "Bars"));
            method.ReturnType = new CodeTypeReference(typeof(DataSeries));
            method.Statements.Add(new CodeSnippetExpression("try { return " + this._code + "} catch { return null; }"));
            CodeTypeDeclaration declaration = new CodeTypeDeclaration("tmpClass") {
                Attributes = MemberAttributes.Public | MemberAttributes.Static
            };
            declaration.Members.Add(method);
            CodeNamespace namespace2 = new CodeNamespace("tmpNameSpace");
            namespace2.Types.Add(declaration);
            foreach (string str in _namespaces)
            {
                namespace2.Imports.Add(new CodeNamespaceImport(str));
            }
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(namespace2);
            CompilerResults results = new CSharpCodeProvider().CompileAssemblyFromDom(_compileParams, new CodeCompileUnit[] { unit });
            if ((results.Errors != null) && (results.Errors.Count > 0))
            {
                throw new Exception("Error: Invalid indicator settings.");
            }
            this._asm = results.CompiledAssembly;
            foreach (System.Type type in this._asm.GetTypes())
            {
                if ((type.Name == "tmpClass") && (type.Namespace == "tmpNameSpace"))
                {
                    this._method = type.GetMethod("Series");
                    break;
                }
            }
        }

        public DataSeries GetIndicator(Bars symBars)
        {
            if (!this._cached.ContainsKey(symBars.Symbol))
            {
                DataSeries series = null;
                if (this._method != null)
                {
                    object[] parameters = new object[] { symBars };
                    try
                    {
                        series = (DataSeries) this._method.Invoke(null, parameters);
                    }
                    catch
                    {
                        series = new DataSeries("Invalid Indicator");
                    }
                }
                else
                {
                    series = new DataSeries("Invalid Indicator");
                }
                this._cached.Add(symBars.Symbol, series);
            }
            return this._cached[symBars.Symbol];
        }

        public string Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = this.FixCode(value);
                this.GenerateMethod();
            }
        }
    }
}

