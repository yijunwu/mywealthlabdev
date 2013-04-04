namespace Fidelity.Components
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;

    [ToolboxBitmap(typeof(AssemblyLoader), "AssemblyLoader")]
    public class AssemblyLoader : Component, IComparer<Assembly>, IComparer<Type>
    {
        private static double double_0;
        private static double double_1;
        private static double double_2 = new DateTime(0x7d8, 9, 9).ToOADate();
        private IContainer icontainer_0;
        private List<Type> list_0;
        private List<Assembly> list_1;
        private static List<string> list_2 = new List<string>();
        public const string LogStartedKey = "LogStarted";
        public const string SessionEndKey = "LastSessionEnd";
        public const string SessionStartKey = "LastSessionStart";
        private static SettingsManager settingsManager_0 = new SettingsManager();
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private static string string_4 = "";
        private string string_5;
        private static string string_6;

        public AssemblyLoader()
        {
            this.string_2 = "*.dll";
            this.list_0 = new List<Type>();
            this.list_1 = new List<Assembly>();
            this.string_3 = "";
            this.string_5 = "";
            this.method_2();
        }

        public AssemblyLoader(IContainer container)
        {
            this.string_2 = "*.dll";
            this.list_0 = new List<Type>();
            this.list_1 = new List<Assembly>();
            this.string_3 = "";
            this.string_5 = "";
            container.Add(this);
            this.method_2();
        }

        public static void CloseLog()
        {
            bool flag = false;
            foreach (string str in list_2)
            {
                smethod_2(ref flag, str);
            }
            settingsManager_0.Set("LastSessionEnd", getCurrentOADateTime());
            settingsManager_0.SaveSettings();
        }

        public int Compare(Assembly assembly_0, Assembly assembly_1)
        {
            return this.GetAssemblyDescription(assembly_0).CompareTo(this.GetAssemblyDescription(assembly_1));
        }

        public int Compare(Type type_0, Type type_1)
        {
            return type_0.Name.CompareTo(type_1.Name);
        }

        public object CreateInstance(string typeName)
        {
            object obj2;
            using (List<Type>.Enumerator enumerator = this.Types.GetEnumerator())
            {
                Type current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Name == typeName)
                    {
                        goto Label_0030;
                    }
                }
                return null;
            Label_0030:
                obj2 = Activator.CreateInstance(current);
            }
            return obj2;
        }

        public object CreateInstance(Type type_0)
        {
            return Activator.CreateInstance(type_0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GetAssemblyDescription(Assembly assembly_0)
        {
            if (Attribute.IsDefined(assembly_0, typeof(AssemblyDescriptionAttribute)))
            {
                AssemblyDescriptionAttribute customAttribute = (AssemblyDescriptionAttribute) Attribute.GetCustomAttribute(assembly_0, typeof(AssemblyDescriptionAttribute));
                if (customAttribute != null)
                {
                    return customAttribute.Description;
                }
            }
            return assembly_0.GetName().Name;
        }

        ///WYJ fix: original signature: private void method_0()
        private void loadAssemblies()
        {
            Assembly assembly;
            Type[] types;
            if (!base.DesignMode)
            {
                this.list_0.Clear();
                this.list_1.Clear();
                if ((this.Interface != "" || this.BaseClass != "") && this.Path != "" && this.Path != null && Directory.Exists(this.Path))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(this.Path);
                    FileInfo[] files = directoryInfo.GetFiles(this.PathMask);
                    string[] strArrays = new string[] { "MetaLib.dll", "WealthLab.DataProviders.WL4Files.Win32DBClient.dll" };
                    string[] strArrays1 = strArrays;
                    bool flag = false;
                    FileInfo[] fileInfoArray = files;
                    for (int i = 0; i < (int)fileInfoArray.Length; i++)
                    {
                        FileInfo fileInfo = fileInfoArray[i];
                        try
                        {
                            if (Array.IndexOf<string>(strArrays1, fileInfo.Name) == -1)
                            {
                                if (this.string_5 != "")
                                {
                                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name);
                                    if (fileNameWithoutExtension.ToUpper() != this.string_5.ToUpper())
                                    {
                                        goto Label0;
                                    }
                                }
                                assembly = Assembly.LoadFile(System.IO.Path.Combine(directoryInfo.FullName, fileInfo.Name));
                            }
                            else
                            {
                                goto Label0;
                            }
                        }
                        catch
                        {
                            //object obj = obj1;
                            assembly = null;
                        }
                        if (assembly != null)
                        {
                            try
                            {
                                types = assembly.GetTypes();
                            }
                            catch
                            {
                                //object obj2 = obj3;
                                goto Label0;
                            }
                            bool flag1 = false;
                            Type[] typeArray = types;
                            for (int j = 0; j < (int)typeArray.Length; j++)
                            {
                                Type type = typeArray[j];
                                if ((this.Interface == null || !(this.Interface != "") || !(type.GetInterface(this.Interface) == null)) && (this.BaseClass == null || !(this.BaseClass != "") || (!type.IsAbstract || !(this.BaseClass != "Object")) && this.method_1(type)))
                                {
                                    this.list_0.Add(type);
                                    if (!flag1 && !AssemblyLoader.list_2.Contains(assembly.FullName))
                                    {
                                        AssemblyLoader.smethod_2(ref flag, assembly.FullName);
                                        AssemblyLoader.list_2.Add(assembly.FullName);
                                        flag1 = flag;
                                    }
                                    if (!this.list_1.Contains(assembly))
                                    {
                                        this.list_1.Add(assembly);
                                    }
                                }
                            }
                        }
                    Label0:
                        if (flag)
                        {
                            lock (AssemblyLoader.settingsManager_0)
                            {
                                AssemblyLoader.settingsManager_0.SaveSettings();
                            }
                        }
                    }
                    
                }
                this.list_1.Sort(this);
                this.list_0.Sort(this);
                return;
            }
            else
            {
                return;
            }
        }

        private bool method_1(Type type_0)
        {
            if (this.BaseClass == "Object")
            {
                return true;
            }
            if (type_0.Name == this.BaseClass)
            {
                return true;
            }
            if (type_0.BaseType == null)
            {
                return false;
            }
            return this.method_1(type_0.BaseType);
        }

        private void method_2()
        {
            this.icontainer_0 = new Container();
        }

        ///WYJ fix: original signature: private static DateTime smethod_0()
        private static DateTime getNow()
        {
            return DateTime.Now.ToUniversalTime();
        }

        ///WYJ fix: original signature: private static double smethod_1()
        private static double getCurrentOADateTime()
        {
            return getNow().ToOADate();
        }

        private static void smethod_2(ref bool bool_0, string string_7)
        {
            string_7 = string_7.Replace("=", ":");
            string str = settingsManager_0.Get(string_7, "");
            StringBuilder builder = new StringBuilder();
            if (str != "")
            {
                string[] strArray = str.Split(new char[] { '|' });
                if ((((strArray.Length % 2) <= 0) && (strArray[strArray.Length - 1] != null)) && (strArray[strArray.Length - 1] != ""))
                {
                    double num;
                    for (int i = 0; i < (strArray.Length - 1); i++)
                    {
                        builder.Append(strArray[i]);
                        builder.Append("|");
                    }
                    if (!double.TryParse(strArray[strArray.Length - 1], out num))
                    {
                        num = 0.0;
                    }
                    if (num < double_0)
                    {
                        builder.Append(strArray[strArray.Length - 1]);
                        builder.Append("|");
                        builder.Append(getCurrentOADateTime().ToString());
                        builder.Append("|");
                    }
                }
                else
                {
                    string str2 = strArray[0];
                    for (int j = 0; j < (strArray.Length - 1); j++)
                    {
                        builder.Append(strArray[j]);
                        builder.Append("|");
                        str2 = strArray[j];
                    }
                    builder.Append(str2);
                    builder.Append("|");
                    builder.Append(str2);
                    builder.Append("|");
                }
            }
            else
            {
                builder.Append(getCurrentOADateTime().ToString());
                builder.Append("|");
            }
            builder.Append(getCurrentOADateTime().ToString());
            lock (settingsManager_0)
            {
                settingsManager_0.Set(string_7, builder.ToString());
                bool_0 = true;
            }
        }

        public List<Type> TypesInAssembly(Assembly assembly_0)
        {
            List<Type> list = new List<Type>();
            foreach (Type type in this.Types)
            {
                if (type.Assembly == assembly_0)
                {
                    list.Add(type);
                }
            }
            list.Sort(this);
            return list;
        }

        [Browsable(false)]
        public List<Assembly> Assemblies
        {
            get
            {
                return this.list_1;
            }
        }

        public string BaseClass
        {
            get
            {
                return this.string_3;
            }
            set
            {
                this.string_3 = value;
                this.loadAssemblies();
            }
        }

        public string DLLNameFilter
        {
            get
            {
                return this.string_5;
            }
            set
            {
                this.string_5 = value;
            }
        }

        public string Interface
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
                this.loadAssemblies();
            }
        }

        public static string LogFileName
        {
            get
            {
                return string_4;
            }
            set
            {
                if ((value != null) && (value != ""))
                {
                    string_4 = value;
                    int startIndex = value.LastIndexOf(@"\");
                    if (startIndex > 0)
                    {
                        LogFilePath = value.Substring(0, ++startIndex);
                        string_4 = value.Substring(startIndex, value.Length - startIndex);
                    }
                    settingsManager_0.IsEncrypted = true;
                    settingsManager_0.FileName = string_4;
                    double num2 = getCurrentOADateTime();
                    if (settingsManager_0.Get("LogStarted", double_2) == double_2)
                    {
                        settingsManager_0.Set("LogStarted", num2);
                    }
                    double_0 = settingsManager_0.Get("LastSessionStart", double_2);
                    settingsManager_0.Set("LastSessionStart", num2);
                    double_1 = settingsManager_0.Get("LastSessionEnd", double_2);
                    settingsManager_0.SaveSettings();
                }
            }
        }

        public static string LogFilePath
        {
            get
            {
                return string_6;
            }
            set
            {
                string_6 = value;
                settingsManager_0.RootPath = string_6;
            }
        }

        public string Path
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
                this.loadAssemblies();
            }
        }

        public string PathMask
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        [Browsable(false)]
        public List<Type> Types
        {
            get
            {
                return this.list_0;
            }
        }
    }
}

