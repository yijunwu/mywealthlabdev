namespace WealthLab
{
    using Fidelity.Components;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(StrategyManager), "StrategyManager")]
    public class StrategyManager : Component, IComparer<Strategy>
    {
        private AssemblyLoader assemblyLoader_0;
        private Dictionary<string, List<string>> dictionary_0;
        private Dictionary<string, Bitmap> dictionary_1;
        private IContainer icontainer_0;
        private static readonly ILog ilog_0 = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private List<string> list_0;
        private List<string> list_1;
        private List<Strategy> list_2;
        private SettingsManager settingsManager_0;
        private string string_0;
        private WealthScriptCompiler wealthScriptCompiler_0;

        public StrategyManager()
        {
            this.dictionary_0 = new Dictionary<string, List<string>>();
            this.list_0 = new List<string>();
            this.list_1 = new List<string>();
            this.list_2 = new List<Strategy>();
            this.dictionary_1 = new Dictionary<string, Bitmap>();
            this.wealthScriptCompiler_0 = new WealthScriptCompiler();
            this.settingsManager_0 = new SettingsManager();
            this.method_3();
        }

        public StrategyManager(IContainer container)
        {
            this.dictionary_0 = new Dictionary<string, List<string>>();
            this.list_0 = new List<string>();
            this.list_1 = new List<string>();
            this.list_2 = new List<Strategy>();
            this.dictionary_1 = new Dictionary<string, Bitmap>();
            this.wealthScriptCompiler_0 = new WealthScriptCompiler();
            this.settingsManager_0 = new SettingsManager();
            container.Add(this);
            this.method_3();
        }

        public bool AddFolder(string folderName, bool messageOnError)
        {
            string str2;
            bool flag;
            string str = folderName.ToUpper();
            using (IEnumerator<string> enumerator = this.FolderNames.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    if (current.ToUpper() == str)
                    {
                        ///goto  Label_0035;  ///WYJ fix, simplify the flow
                        if (messageOnError)
                        {
                            MessageBox.Show("A Folder with this name already exists");
                        }
                        return false;
                    }
                }
            }
            str2 = this.RootPath + @"\Strategies\" + folderName;
            try
            {
                Directory.CreateDirectory(str2);
                this.FolderNames.Add(folderName);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                flag = false;
            }
            return flag;
        }

        public bool AddFolderToNetworkPath(string networkPath, string folder)
        {
            try
            {
                string path = networkPath;
                if (!path.EndsWith(@"\"))
                {
                    path = path + @"\";
                }
                path = path + folder;
                if (this.dictionary_0[networkPath].Contains(folder))
                {
                    throw new ArgumentException("A folder named \"" + folder + @"\ already exists.");
                }
                Directory.CreateDirectory(path);
                this.dictionary_0[networkPath].Add(folder);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed to add folder to Network Path: " + exception.Message, "Create Folder");
                return false;
            }
        }

        public bool AddStrategy(Strategy strategy_0, string folderName, bool allowDuplicates, ref string reason)
        {
            strategy_0.Folder = folderName;
            string path = this.method_1(strategy_0);
            bool flag = true;
            if (!allowDuplicates)
            {
                if (File.Exists(path))
                {
                    flag = false;
                    reason = "Strategy already exists";
                }
                else
                {
                    using (List<Strategy>.Enumerator enumerator = this.list_2.GetEnumerator())
                    {
                        Strategy current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (strategy_0.ID == current.ID)
                            {
                                ///goto  Label_005D; ///WYJ fix, simplify the flow
                                flag = false;
                                reason = "Strategy ID already exists as " + current.Folder + @"\" + strategy_0.Name;
                                break;
                            }
                        }
                    }
                }
            }
            if (flag)
            {
                try
                {
                    this.AddFolder(folderName, false);
                    this.SaveStrategy(strategy_0, folderName, false, "");
                }
                catch
                {
                    reason = "Error writing to " + strategy_0.Folder;
                    flag = false;
                }
            }
            return flag;
        }

        public int Compare(Strategy strategy_0, Strategy strategy_1)
        {
            return strategy_0.Name.CompareTo(strategy_1.Name);
        }

        public bool DeleteFolder(string folderName, string networkPath)
        {
            string path = this.RootPath + @"\Strategies\" + folderName;
            if (networkPath != "")
            {
                path = networkPath;
                if (!path.EndsWith(@"\"))
                {
                    path = path + @"\";
                }
                path = path + folderName;
            }
            if (Directory.Exists(path))
            {
                int count = this.StrategiesInFolder(folderName, networkPath).Count;
                if ((count > 0) && (MessageBox.Show("There are " + count + " Strategies in this Folder.  Delete it anyway?", "Delete Folder", MessageBoxButtons.YesNo) == DialogResult.No))
                {
                    return false;
                }
                string str2 = folderName.ToUpper();
                for (int i = this.list_2.Count - 1; i >= 0; i--)
                {
                    Strategy strategy = this.list_2[i];
                    if ((this.method_2(strategy).ToUpper() == str2) && (strategy.NetworkDrivePath == networkPath))
                    {
                        this.DeleteStrategy(strategy);
                    }
                }
                Directory.Delete(path);
            }
            if (networkPath != "")
            {
                if (this.dictionary_0.ContainsKey(networkPath))
                {
                    List<string> list = this.dictionary_0[networkPath];
                    if (list.Contains(folderName))
                    {
                        list.Remove(folderName);
                    }
                }
            }
            else if (this.FolderNames.Contains(folderName))
            {
                this.FolderNames.Remove(folderName);
            }
            return true;
        }

        public void DeleteStrategy(Strategy strategy_0)
        {
            if (File.Exists(strategy_0.FileName))
            {
                File.Delete(strategy_0.FileName);
            }
            this.list_2.Remove(strategy_0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public Bitmap GetCustomGlyph(string libraryName)
        {
            if (!this.dictionary_1.ContainsKey(libraryName))
            {
                return null;
            }
            return this.dictionary_1[libraryName];
        }

        public List<string> GetNetworkPathFolders(string path)
        {
            if (this.dictionary_0.ContainsKey(path))
            {
                return this.dictionary_0[path];
            }
            return new List<string>();
        }

        public WealthScript GetWealthScriptObject(Strategy strategy_0)
        {
            if (strategy_0.StrategyType != StrategyType.Compiled)
            {
                WealthScriptCompiler compiler = new WealthScriptCompiler {
                    SourceCode = strategy_0.Code
                };
                return compiler.CompileSource(strategy_0.References);
            }
            if (this.assemblyLoader_0 == null)
            {
                this.LoadStrategies();
            }
            return (WealthScript) this.assemblyLoader_0.CreateInstance(strategy_0.WealthScriptType);
        }

        public void LoadStrategies()
        {
            this.list_2.Clear();
            this.list_0.Clear();
            this.list_1.Clear();
            this.settingsManager_0.RootPath = this.RootPath;
            this.settingsManager_0.FileName = "PrecompiledStrategyAcctNumbers.txt";
            this.settingsManager_0.LoadSettings();
            string path = this.RootPath + @"\Strategies\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            this.method_0(path, "");
            this.assemblyLoader_0 = new AssemblyLoader();
            this.assemblyLoader_0.BaseClass = "StrategyHelper";
            this.assemblyLoader_0.Path = Path.GetDirectoryName(Application.ExecutablePath);
            foreach (Assembly assembly in this.assemblyLoader_0.Assemblies)
            {
                string assemblyDescription = this.assemblyLoader_0.GetAssemblyDescription(assembly);
                this.list_1.Add(assemblyDescription);
                bool flag = false;
                foreach (System.Type type in this.assemblyLoader_0.TypesInAssembly(assembly))
                {
                    Strategy strategy;
                    if (!flag)
                    {
                        Stream manifestResourceStream = assembly.GetManifestResourceStream(type, "glyph.bmp");
                        if (manifestResourceStream != null)
                        {
                            Bitmap bitmap = new Bitmap(manifestResourceStream);
                            this.dictionary_1.Add(assemblyDescription, bitmap);
                        }
                        flag = true;
                    }
                    StrategyHelper strategyHelper = (StrategyHelper)this.assemblyLoader_0.CreateInstance(type);

                    strategy = new Strategy();
                    strategy.Name = strategyHelper.Name;
                    strategy.ID = strategyHelper.ID;
                    strategy.Description = strategyHelper.Description;
                    strategy.FileName = assemblyDescription;
                    strategy.Folder = assemblyDescription;
                    strategy.StrategyType = StrategyType.Compiled;
                    strategy.Author = strategyHelper.Author;
                    strategy.CreationDate = strategyHelper.CreationDate;
                    strategy.LastModified = strategy.CreationDate;
                    strategy.WealthScriptType = strategyHelper.WealthScriptType;
                    strategy.URL = strategyHelper.URL;

                    if (this.settingsManager_0.ContainsKey(strategy.Name))
                    {
                        strategy.AccountNumber = this.settingsManager_0.Get(strategy.Name, "");
                    }
                    this.list_2.Add(strategy);
                }
            }
        }

        public void LoadStrategiesFromNetworkPath(string networkPath)
        {
            try
            {
                string[] directories = Directory.GetDirectories(networkPath);
                List<string> list = new List<string>();
                foreach (string str in directories)
                {
                    string[] strArray3 = str.Split(new char[] { '\\' });
                    string item = strArray3[strArray3.Length - 1];
                    list.Add(item);
                }
                this.dictionary_0[networkPath] = list;
                this.method_0(networkPath, networkPath);
            }
            catch
            {
            }
        }

        public bool LoadStrategyParameters(Strategy strategy_0, WealthScript wealthScript_0)
        {
            if (wealthScript_0 == null)
            {
                return false;
            }
            if (strategy_0.ParameterValues.Count != wealthScript_0.Parameters.Count)
            {
                return false;
            }
            for (int i = 0; i < strategy_0.ParameterValues.Count; i++)
            {
                wealthScript_0.Parameters[i].Value = strategy_0.ParameterValues[i];
            }
            return true;
        }

        public Strategy Lookup(string name)
        {
            Strategy strategy2;
            string str = name.ToUpper();
            using (List<Strategy>.Enumerator enumerator = this.list_2.GetEnumerator())
            {
                Strategy current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Name.ToUpper() == str)
                    {
                        ///goto  Label_003C;  ///WYJ fix, simplify the flow
                        strategy2 = current;
                        return strategy2;
                    }
                }
                return null;
            }
        }

        public Strategy Lookup(string name, string folder, string networkPath)
        {
            Strategy strategy2;
            string str = name.ToUpper();
            string str2 = folder.ToUpper();
            using (List<Strategy>.Enumerator enumerator = this.list_2.GetEnumerator())
            {
                Strategy current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (((current.Name.ToUpper() == str) && (current.Folder.ToUpper() == str2)) && (current.NetworkDrivePath == networkPath))
                    {
                        ///goto  Label_0064;  ///WYJ fix, simplify the flow
                        strategy2 = current;
                        return strategy2;
                    }
                }
                return null;
            }
        }

        public Strategy LookupID(string ID)
        {
            Strategy strategy2;
            using (List<Strategy>.Enumerator enumerator = this.list_2.GetEnumerator())
            {
                Strategy current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.ID.ToString() == ID)
                    {
                        ///goto  Label_003E; ///WYJ fix, simplify the flow
                        strategy2 = current;
                        return strategy2;
                    }
                }
                return null;
            }
        }

        ///WYJ fix, code from Reflector, workable, but deprecated because of having too many goto statements. Try version from ILSpy
        /*
        private void method_0(string string_1, string string_2)
        {
            string[] directories = Directory.GetDirectories(string_1);
            string text = "Strategy could not be loaded due to an error in the XML source file \n";
            string str2 = string.Empty;
            string[] strArray2 = directories;
            int index = 0;
        Label_0018:
            if (index < strArray2.Length)
            {
                string path = strArray2[index];
                string item = path.Substring(string_1.Length);
                if (string_2 == "")
                {
                    this.list_0.Add(item);
                }
                string[] files = Directory.GetFiles(path, "*.xml");
                int num2 = 0;
                while (true)
                {
                    if (num2 < files.Length)
                    {
                        string fileName = files[num2];
                        try
                        {
                            Strategy strategy = Strategy.FromFile(fileName);
                            strategy.FileName = fileName;
                            strategy.Folder = item;
                            strategy.NetworkDrivePath = string_2;
                            this.list_2.Add(strategy);
                        }
                        catch (Exception)
                        {
                            str2 = str2 + "\"" + fileName + "\"";
                        }
                    }
                    else
                    {
                        index++;
                        goto Label_0018;
                    }
                    num2++;
                }
            }
            if (str2.Length > 0)
            {
                text = text + str2;
                ilog_0.Error("Error loading strategies - " + text);
                MessageBox.Show(text, "Error Loading Strategies", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        } */

        ///WYJ fix, code from ILSpy
        private void method_0(string string_1, string string_2)
        {
            string[] directories = Directory.GetDirectories(string_1);
            string text = "Strategy could not be loaded due to an error in the XML source file \n";
            string text2 = string.Empty;
            string[] array = directories;
            for (int i = 0; i < array.Length; i++)
            {
                string text3 = array[i];
                string text4 = text3.Substring(string_1.Length);
                if (string_2 == "")
                {
                    this.list_0.Add(text4);
                }
                string[] files = Directory.GetFiles(text3, "*.xml");
                string[] array2 = files;
                for (int j = 0; j < array2.Length; j++)
                {
                    string text5 = array2[j];
                    try
                    {
                        Strategy strategy = Strategy.FromFile(text5);
                        strategy.FileName = text5;
                        strategy.Folder = text4;
                        strategy.NetworkDrivePath = string_2;
                        this.list_2.Add(strategy);
                    }
                    catch (Exception)
                    {
                        text2 = text2 + "\"" + text5 + "\"";
                    }
                }
            }
            if (text2.Length > 0)
            {
                text += text2;
                StrategyManager.ilog_0.Error("Error loading strategies - " + text);
                MessageBox.Show(text, "Error Loading Strategies", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private string method_1(Strategy strategy_0)
        {
            string str = SymbolFileNameConverter.SymbolToFileName(strategy_0.Name);
            if ((strategy_0.NetworkDrivePath != "") && (strategy_0.NetworkDrivePath != null))
            {
                string networkDrivePath = strategy_0.NetworkDrivePath;
                if (!networkDrivePath.EndsWith(@"\"))
                {
                    networkDrivePath = networkDrivePath + @"\";
                }
                string str3 = networkDrivePath;
                return (str3 + strategy_0.Folder + @"\" + str + ".xml");
            }
            return (this.RootPath + @"\Strategies\" + strategy_0.Folder + @"\" + str + ".xml");
        }

        private string method_2(Strategy strategy_0)
        {
            if (strategy_0.StrategyType == StrategyType.Compiled)
            {
                return strategy_0.FileName;
            }
            string[] strArray = Path.GetDirectoryName(strategy_0.FileName).Split(new char[] { '\\' });
            return strArray[strArray.Length - 1];
        }

        private void method_3()
        {
            this.icontainer_0 = new Container();
        }

        public void RemoveNetworkPath(string networkPath)
        {
            this.dictionary_0.Remove(networkPath);
            for (int i = this.list_2.Count - 1; i >= 0; i--)
            {
                if (this.list_2[i].NetworkDrivePath == networkPath)
                {
                    this.list_2.RemoveAt(i);
                }
            }
        }

        public void SaveParameterValues(Strategy strategy_0, WealthScript wealthScript_0)
        {
            strategy_0.ParameterValues.Clear();
            foreach (StrategyParameter parameter in wealthScript_0.Parameters)
            {
                strategy_0.ParameterValues.Add(parameter.Value);
            }
            this.SaveStrategy(strategy_0);
        }

        public void SaveStrategy(Strategy strategy_0)
        {
            this.SaveStrategy(strategy_0, strategy_0.Folder, true, strategy_0.NetworkDrivePath);
        }

        public void SaveStrategy(Strategy strategy_0, string folderName, string networkPath)
        {
            this.SaveStrategy(strategy_0, folderName, true, networkPath);
        }

        public void SaveStrategy(Strategy strategy_0, string folderName, bool lastModIsNow, string networkPath)
        {
            if (lastModIsNow)
            {
                strategy_0.LastModified = DateTime.Now;
            }
            strategy_0.Folder = folderName;
            strategy_0.NetworkDrivePath = networkPath;
            strategy_0.FileName = this.method_1(strategy_0);
            strategy_0.SaveToFile(strategy_0.FileName);
            if (this.LookupID(strategy_0.ID.ToString()) == null)
            {
                this.list_2.Add(strategy_0);
            }
        }

        public void SetAccountNumberForPrecompiledStrategy(Strategy strategy_0, string accountNumber)
        {
            this.settingsManager_0.Set(strategy_0.Name, accountNumber);
            this.settingsManager_0.SaveSettings();
        }

        public IList<Strategy> StrategiesInFolder(string folderName, string networkPath)
        {
            List<Strategy> list = new List<Strategy>();
            foreach (Strategy strategy in this.list_2)
            {
                if ((this.method_2(strategy) == folderName) && (strategy.NetworkDrivePath == networkPath))
                {
                    list.Add(strategy);
                }
            }
            list.Sort(this);
            return list;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IList<string> FolderNames
        {
            get
            {
                return this.list_0;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public IList<string> LibraryNames
        {
            get
            {
                return this.list_1;
            }
        }

        public string RootPath
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
                if ((this.string_0 != null) && (this.string_0 != ""))
                {
                    this.LoadStrategies();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<Strategy> Strategies
        {
            get
            {
                return this.list_2;
            }
        }
    }
}

