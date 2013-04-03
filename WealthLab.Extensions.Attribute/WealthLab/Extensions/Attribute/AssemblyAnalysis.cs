namespace WealthLab.Extensions.Attribute
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;

    internal class AssemblyAnalysis : MarshalByRefObject
    {
        private string _attributeAssemblyDir;

        private Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }

        private Assembly GetAssembly(string fileName)
        {
            Assembly assembly;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(this.CurrentDomain_ReflectionOnlyAssemblyResolve);
            Assembly.ReflectionOnlyLoadFrom(string.Concat(new object[] { this._attributeAssemblyDir, Path.DirectorySeparatorChar, Assembly.GetExecutingAssembly().GetName().Name, ".dll" }));
            try
            {
                assembly = Assembly.ReflectionOnlyLoadFrom(fileName);
            }
            catch (BadImageFormatException)
            {
                return null;
            }
            return assembly;
        }

        public ExtensionInfoAttribute GetAttribute(Assembly asm)
        {
            if (asm != null)
            {
                foreach (CustomAttributeData data in CustomAttributeData.GetCustomAttributes(asm))
                {
                    if (data.Constructor.DeclaringType.Name == "ExtensionInfoAttribute")
                    {
                        ExtensionInfoAttribute attribute = new ExtensionInfoAttribute();
                        for (int i = 0; i < data.ConstructorArguments.Count; i++)
                        {
                            CustomAttributeTypedArgument argument = data.ConstructorArguments[i];
                            switch (i)
                            {
                                case 0:
                                    attribute._type = (ExtensionType) argument.Value;
                                    break;

                                case 1:
                                    attribute._strongName = (string) argument.Value;
                                    break;

                                case 2:
                                    attribute._displayName = (string) argument.Value;
                                    break;

                                case 3:
                                    attribute._description = (string) argument.Value;
                                    break;

                                case 4:
                                    attribute._version = (string) argument.Value;
                                    break;

                                case 5:
                                    attribute._publisher = (string) argument.Value;
                                    break;

                                case 6:
                                    attribute._glyph16x16Name = (string) argument.Value;
                                    break;

                                case 7:
                                    attribute._licence = (ExtensionLicence) argument.Value;
                                    break;

                                case 8:
                                    if (argument.Value is ReadOnlyCollection<CustomAttributeTypedArgument>)
                                    {
                                        List<string> list2 = new List<string>();
                                        foreach (CustomAttributeTypedArgument argument2 in argument.Value as ReadOnlyCollection<CustomAttributeTypedArgument>)
                                        {
                                            list2.Add((string) argument2.Value);
                                        }
                                        attribute._deleteFiles = list2.ToArray();
                                    }
                                    break;
                            }
                        }
                        foreach (CustomAttributeNamedArgument argument3 in data.NamedArguments)
                        {
                            CustomAttributeTypedArgument typedValue = argument3.TypedValue;
                            switch (argument3.MemberInfo.Name)
                            {
                                case "LicencePrice":
                                    attribute.LicencePrice = (string) typedValue.Value;
                                    break;

                                case "PublisherUrl":
                                    attribute.PublisherUrl = (string) typedValue.Value;
                                    break;

                                case "Glyph100x100Name":
                                    attribute.Glyph100x100Name = (string) typedValue.Value;
                                    break;

                                case "PostInstallBatch":
                                    attribute.PostInstallBatch = (string) typedValue.Value;
                                    break;

                                case "PostUninstallBatch":
                                    attribute.PostUninstallBatch = (string) typedValue.Value;
                                    break;

                                case "PreInstallBatch":
                                    attribute.PreInstallBatch = (string) typedValue.Value;
                                    break;

                                case "PreUninstallBatch":
                                    attribute.PreUninstallBatch = (string) typedValue.Value;
                                    break;

                                case "MinProVersion":
                                    attribute.MinProVersion = (string) typedValue.Value;
                                    break;

                                case "MaxProVersion":
                                    attribute.MaxProVersion = (string) typedValue.Value;
                                    break;

                                case "MinDeveloperVersion":
                                    attribute.MinDeveloperVersion = (string) typedValue.Value;
                                    break;

                                case "MaxDeveloperVersion":
                                    attribute.MaxDeveloperVersion = (string) typedValue.Value;
                                    break;

                                case "HostApp":
                                    attribute.HostApp = (ExtensionHostApp) typedValue.Value;
                                    break;
                            }
                        }
                        return attribute;
                    }
                }
            }
            return null;
        }

        public ExtensionInfoAttribute GetExtensionInfo(string fileName, string attributeAssemblyDir)
        {
            this._attributeAssemblyDir = attributeAssemblyDir;
            Assembly asm = this.GetAssembly(fileName);
            ExtensionInfoAttribute attribute = this.GetAttribute(asm);
            if ((asm != null) && (attribute != null))
            {
                using (Stream stream = asm.GetManifestResourceStream(attribute.Glyph16x16Name))
                {
                    if (stream != null)
                    {
                        attribute._glyph16x16Base64 = ExtensionInfoAttribute.ConvertStreamToBase64(stream);
                    }
                }
            }
            return attribute;
        }

        public string GetGlyphBase64(string fileName, string glyphName, string attributeAssemblyDir)
        {
            string str = null;
            this._attributeAssemblyDir = attributeAssemblyDir;
            Assembly assembly = this.GetAssembly(fileName);
            if (assembly != null)
            {
                using (Stream stream = assembly.GetManifestResourceStream(glyphName))
                {
                    if (stream != null)
                    {
                        str = ExtensionInfoAttribute.ConvertStreamToBase64(stream);
                    }
                }
            }
            return str;
        }
    }
}

