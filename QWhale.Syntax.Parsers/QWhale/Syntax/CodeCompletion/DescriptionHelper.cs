namespace QWhale.Syntax.CodeCompletion
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Xml;

    public class DescriptionHelper
    {
        private static Hashtable assemblies = new Hashtable();
        private static bool enabled = true;
        private static Regex regex = new Regex(@"((\r\n)|(\n)|(\r))(\s)+", RegexOptions.Multiline);
        private static string systemAssemblyFolder = string.Empty;

        public static string GetDescription(MemberInfo info)
        {
            if (!enabled)
            {
                return string.Empty;
            }
            return GetDescription(info, string.Empty);
        }

        public static string GetDescription(System.Reflection.ParameterInfo pinfo)
        {
            if (enabled && (pinfo.Member != null))
            {
                return GetDescription(pinfo.Member, ".param:" + pinfo.Name);
            }
            return string.Empty;
        }

        protected static string GetDescription(MemberInfo info, string paramName)
        {
            string str;
            string str2;
            if (GetParts(info, out str, out str2))
            {
                string str3;
                Type baseType = (info is Type) ? ((Type) info) : info.ReflectedType;
                if (((baseType == null) || !GetDescription(str, baseType, str2, paramName, out str3)) && ((info.DeclaringType == null) || !GetDescription(str, info.DeclaringType, str2, paramName, out str3)))
                {
                    while (baseType != null)
                    {
                        baseType = baseType.BaseType;
                        if ((baseType != null) && GetDescription(str, baseType, str2, paramName, out str3))
                        {
                            return str3;
                        }
                    }
                }
                else
                {
                    return str3;
                }
            }
            return string.Empty;
        }

        protected static bool GetDescription(string prefix, Type type, string postfix, string paramName, out string desc)
        {
            desc = string.Empty;
            if (type.Assembly != null)
            {
                DescriptionInfo descriptionInfo = GetDescriptionInfo(type.Assembly);
                if (descriptionInfo != null)
                {
                    object obj2 = descriptionInfo.Descriptions[prefix + ":" + type.FullName + ((postfix != string.Empty) ? ("." + postfix) : string.Empty) + paramName];
                    if (obj2 != null)
                    {
                        desc = regex.Replace(obj2.ToString().Trim(), " ");
                    }
                    return true;
                }
            }
            return false;
        }

        protected static DescriptionInfo GetDescriptionInfo(Assembly assembly)
        {
            object obj2 = assemblies[assembly];
            if (obj2 == null)
            {
                obj2 = LoadAssembly(assembly);
            }
            return (DescriptionInfo) obj2;
        }

        private static string GetInnerText(XmlNode node)
        {
            string innerText = string.Empty;
            if (node is XmlText)
            {
                innerText = node.InnerText;
                int index = innerText.IndexOf(":");
                if (index >= 0)
                {
                    innerText = innerText.Substring(index + 1);
                }
                index = innerText.IndexOf(".#ctor");
                if (index >= 0)
                {
                    innerText = innerText.Substring(0, index);
                }
                return innerText;
            }
            if (node.Attributes != null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    innerText = innerText + GetInnerText(attribute);
                }
            }
            if (node.HasChildNodes)
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    innerText = innerText + GetInnerText(node2);
                }
            }
            return innerText;
        }

        private static XmlNode GetNodeByName(XmlNode node, string nodeName)
        {
            if (node != null)
            {
                XmlNode nodeByName = (node.Name == nodeName) ? node : null;
                if (nodeByName != null)
                {
                    return nodeByName;
                }
                if (node.HasChildNodes)
                {
                    foreach (XmlNode node3 in node.ChildNodes)
                    {
                        nodeByName = GetNodeByName(node3, nodeName);
                        if (nodeByName != null)
                        {
                            return nodeByName;
                        }
                    }
                }
            }
            return null;
        }

        private static string GetNodeName(XmlNode node)
        {
            if (node.Attributes != null)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if (attribute.Name == "name")
                    {
                        return attribute.InnerText;
                    }
                }
            }
            return string.Empty;
        }

        private static bool GetNodePriority(XmlNode node, out int priority)
        {
            priority = 0;
            XmlNode nodeByName = GetNodeByName(node, "filterpriority");
            if (nodeByName != null)
            {
                try
                {
                    priority = int.Parse(nodeByName.InnerText) - 1;
                    return true;
                }
                catch
                {
                }
            }
            return false;
        }

        private static string GetNodeSummary(XmlNode node)
        {
            XmlNode nodeByName = GetNodeByName(node, "summary");
            if (nodeByName == null)
            {
                return string.Empty;
            }
            return GetInnerText(nodeByName);
        }

        protected static bool GetParts(MemberInfo info, out string prefix, out string postfix)
        {
            prefix = string.Empty;
            postfix = string.Empty;
            if (info.Name != null)
            {
                postfix = info.Name.Replace(".ctor", "#ctor");
                switch (info.MemberType)
                {
                    case MemberTypes.Property:
                        prefix = "P";
                        goto Label_00A9;

                    case MemberTypes.TypeInfo:
                    case MemberTypes.NestedType:
                        prefix = "T";
                        postfix = string.Empty;
                        goto Label_00A9;

                    case MemberTypes.Constructor:
                    case MemberTypes.Method:
                        prefix = "M";
                        goto Label_00A9;

                    case MemberTypes.Event:
                        prefix = "E";
                        goto Label_00A9;

                    case MemberTypes.Field:
                        prefix = "F";
                        goto Label_00A9;
                }
            }
            return false;
        Label_00A9:
            if (info is MethodInfo)
            {
                System.Reflection.ParameterInfo[] parameters = ((MethodInfo) info).GetParameters();
                if (parameters.Length > 0)
                {
                    string str = string.Empty;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        System.Reflection.ParameterInfo info2 = parameters[i];
                        string str2 = info2.ParameterType.ToString().Replace('&', '@');
                        str = (str == string.Empty) ? str2 : (str + "," + str2);
                    }
                    postfix = postfix + "(" + str + ")";
                }
            }
            return true;
        }

        public static int GetPriority(MemberInfo info)
        {
            if (!enabled)
            {
                return 0;
            }
            return GetPriority(info, string.Empty);
        }

        protected static int GetPriority(MemberInfo info, string paramName)
        {
            string str;
            string str2;
            if (GetParts(info, out str, out str2))
            {
                int num;
                Type baseType = (info is Type) ? ((Type) info) : info.ReflectedType;
                if (((baseType == null) || !GetPriority(str, baseType, str2, paramName, out num)) && ((info.DeclaringType == null) || !GetPriority(str, info.DeclaringType, str2, paramName, out num)))
                {
                    while (baseType != null)
                    {
                        baseType = baseType.BaseType;
                        if ((baseType != null) && GetPriority(str, baseType, str2, paramName, out num))
                        {
                            return num;
                        }
                    }
                }
                else
                {
                    return num;
                }
            }
            return 0;
        }

        protected static bool GetPriority(string prefix, Type type, string postfix, string paramName, out int priority)
        {
            priority = 0;
            if (type.Assembly != null)
            {
                DescriptionInfo descriptionInfo = GetDescriptionInfo(type.Assembly);
                if (descriptionInfo != null)
                {
                    object obj2 = descriptionInfo.Priorities[prefix + ":" + type.FullName + ((postfix != string.Empty) ? ("." + postfix) : string.Empty) + paramName];
                    if (obj2 != null)
                    {
                        priority = (int) obj2;
                    }
                    return true;
                }
            }
            return false;
        }

        public static DescriptionInfo LoadAssembly(Assembly assembly)
        {
            DescriptionInfo info = (DescriptionInfo) assemblies[assembly];
            if (info == null)
            {
                info = new DescriptionInfo();
                if ((assembly.Location != null) && (assembly.Location != string.Empty))
                {
                    try
                    {
                        FileInfo info2 = new FileInfo(Path.ChangeExtension(assembly.Location, "xml"));
                        if (!info2.Exists && (SystemAssemblyFolder != string.Empty))
                        {
                            info2 = new FileInfo(SystemAssemblyFolder + Path.ChangeExtension(Path.GetFileName(assembly.Location), "xml"));
                        }
                        if (info2.Exists)
                        {
                            LoadXmlFile(info2.FullName, info.Descriptions, info.Priorities);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            assemblies[assembly] = info;
            return info;
        }

        private static void LoadParameters(string name, XmlNode node, Hashtable descriptions)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    if (node2.Name == "param")
                    {
                        XmlAttribute attribute = node2.Attributes["name"];
                        if (attribute != null)
                        {
                            descriptions[name + ".param:" + attribute.Value] = GetInnerText(node2);
                        }
                    }
                }
            }
        }

        private static void LoadXmlFile(string fileName, Hashtable descriptions, Hashtable priorities)
        {
            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            XmlNode nodeByName = GetNodeByName(document, "members");
            if (nodeByName != null)
            {
                for (XmlNode node2 = nodeByName.FirstChild; node2 != null; node2 = node2.NextSibling)
                {
                    int num;
                    string nodeName = GetNodeName(node2);
                    descriptions[nodeName] = GetNodeSummary(node2);
                    if (GetNodePriority(node2, out num))
                    {
                        priorities[nodeName] = num;
                    }
                    if (node2.HasChildNodes)
                    {
                        LoadParameters(nodeName, node2, descriptions);
                    }
                }
            }
        }

        public static void UnloadAssembly(Assembly assembly)
        {
            if (assemblies.Contains(assembly))
            {
                assemblies.Remove(assembly);
            }
        }

        public static bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        public static string SystemAssemblyFolder
        {
            get
            {
                if (systemAssemblyFolder == string.Empty)
                {
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        AssemblyName name = assembly.GetName();
                        if ((name != null) && (string.Compare(name.Name, "mscorlib", true) == 0))
                        {
                            systemAssemblyFolder = string.Format(@"{0}\", Path.GetDirectoryName(assembly.Location));
                        }
                    }
                }
                return systemAssemblyFolder;
            }
        }

        public class DescriptionInfo
        {
            private Hashtable descriptions = new Hashtable();
            private Hashtable priorities = new Hashtable();

            public Hashtable Descriptions
            {
                get
                {
                    return this.descriptions;
                }
            }

            public Hashtable Priorities
            {
                get
                {
                    return this.priorities;
                }
            }
        }
    }
}

