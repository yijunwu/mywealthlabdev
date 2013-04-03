namespace WealthLab.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.Extensions.Agent;
    using WealthLab.Extensions.Attribute;
    using WealthLab.Extensions.Properties;

    public class ExtensionManager
    {
        private bool bool_0;
        private Class40 class40_0 = new Class40();
        public static WealthLab.Extensions.Config Config;
        private CopyDataForm copyDataForm_0;
        private static Enum8 enum8_0;
        private ExtensionManagerForm extensionManagerForm_0;
        private static IAuthenticationHost iauthenticationHost_0;
        internal static List<string> list_0 = new List<string>();
        private UpdateExtensionsForm updateExtensionsForm_0;
        private static Version version_0;
        public static string WleFileFromOsClick;
        public static string WleFileFromStart;

        public event UpdateExtensionActionEventHandler UpdateExtensionAction;

        public event UpdateExtensionsCompletedHandler UpdateExtensionsCompleted;

        public event UpdateExtensionsStartHandler UpdateExtensionsStart;

        public ExtensionManager()
        {
            enum8_0 = this.method_0();
            version_0 = this.method_1();
            Config = WealthLab.Extensions.Config.Deserialize();
            if (!Config.CheckUpdates.HasValue)
            {
                Config.CheckUpdates = new bool?(enum8_0 != Enum8.const_0);
            }
            this.class40_0.method_4(new Class40.Delegate14(this.method_2));
            this.UpdateExtensions();
            this.copyDataForm_0 = new CopyDataForm();
            this.copyDataForm_0.method_0(new CopyDataForm.Delegate16(this.method_4));
            this.copyDataForm_0.Location = new Point(-2000, -2000);
            this.copyDataForm_0.Show();
            this.copyDataForm_0.Hide();
            WleFileFromStart = this.method_3();
        }

        public void CheckForUpdates()
        {
            if (Config.CheckUpdates.Value)
            {
                this.class40_0.method_15();
            }
            if (!string.IsNullOrEmpty(WleFileFromStart))
            {
                Delegate6 method = new Delegate6(this.method_6);
                this.copyDataForm_0.Invoke(method);
            }
        }

        public void CheckForUpdates(IAuthenticationHost authHost)
        {
            iauthenticationHost_0 = authHost;
            this.CheckForUpdates();
        }

        public void ClickHandler()
        {
            this.method_6();
        }

        public void ClickHandler(IAuthenticationHost authHost)
        {
            iauthenticationHost_0 = authHost;
            this.ClickHandler();
        }

        public ToolStripMenuItem GetMenuItem()
        {
            ToolStripMenuItem item = new ToolStripMenuItem("Extension Manager") {
                Image = Resources.package.ToBitmap()
            };
            item.Click += new EventHandler(this.method_5);
            return item;
        }

        public void MainFormShow()
        {
        }

        private Enum8 method_0()
        {
            string[] files = Directory.GetFiles(Application.StartupPath);
            string str = Path.Combine(Application.StartupPath, "WealthLabPro.exe");
            foreach (string str2 in files)
            {
                if (str2 == str)
                {
                    return Enum8.const_0;
                }
            }
            return Enum8.const_1;
        }

        private Version method_1()
        {
            return Assembly.GetEntryAssembly().GetName().Version;
        }

        private void method_2(object sender, EventArgs9 e)
        {
            if ((e.method_0() != (Enum9.flag_1 | Enum9.flag_0)) || !Config.CheckUpdates.Value)
            {
                return;
            }
            using (List<Class46>.Enumerator enumerator = this.class40_0.list_0.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Class46 current = enumerator.Current;
                    if (current.method_5() == Enum12.const_1)
                    {
                        ExtensionInfoAttribute attribute = current.method_7();
                        if (enum8_0 != Enum8.const_0)
                        {
                            goto Label_0094;
                        }
                        if (((attribute != null) && !string.IsNullOrEmpty(attribute.PreInstallBatch)) && attribute.PreInstallBatch.ToLower().Contains("[fidelityapproved=true]"))
                        {
                            goto Label_008B;
                        }
                    }
                }
                goto Label_00AB;
            Label_008B:
                this.bool_0 = true;
                goto Label_00AB;
            Label_0094:
                this.bool_0 = true;
            }
        Label_00AB:
            if (this.bool_0)
            {
                Delegate6 method = new Delegate6(this.method_6);
                this.copyDataForm_0.Invoke(method);
            }
        }

        private string method_3()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if (commandLineArgs.Length < 2)
            {
                return null;
            }
            string path = commandLineArgs[1];
            if (!File.Exists(path))
            {
                return null;
            }
            if (Path.GetExtension(path).ToLower() != ".wle")
            {
                return null;
            }
            return path;
        }

        private void method_4(object sender, EventArgs13 e)
        {
            if (string.IsNullOrEmpty(WleFileFromStart))
            {
                switch (this.class40_0.method_12())
                {
                    case Enum10.const_0:
                        WleFileFromOsClick = e.string_0;
                        this.method_6();
                        return;

                    case Enum10.const_1:
                        WleFileFromOsClick = e.string_0;
                        return;

                    case Enum10.const_2:
                        this.method_6();
                        this.class40_0.method_17(e.string_0);
                        WleFileFromOsClick = string.Empty;
                        return;
                }
            }
        }

        private void method_5(object sender, EventArgs e)
        {
            this.ClickHandler();
        }

        private void method_6()
        {
            if ((this.extensionManagerForm_0 != null) && !this.extensionManagerForm_0.IsDisposed)
            {
                this.extensionManagerForm_0.Show();
            }
            else
            {
                this.extensionManagerForm_0 = new ExtensionManagerForm();
                this.extensionManagerForm_0.method_2(this.class40_0);
                this.extensionManagerForm_0.Show();
                if (this.class40_0.method_12() == Enum10.const_2)
                {
                    this.class40_0.method_26();
                    if (this.bool_0)
                    {
                        this.extensionManagerForm_0.method_12();
                    }
                    else
                    {
                        this.extensionManagerForm_0.method_11();
                    }
                }
                if (this.class40_0.method_12() == Enum10.const_1)
                {
                    this.extensionManagerForm_0.method_8(new EventArgs7(this.class40_0.method_13()));
                }
            }
            if (this.extensionManagerForm_0.WindowState == FormWindowState.Minimized)
            {
                this.extensionManagerForm_0.WindowState = FormWindowState.Normal;
            }
            this.extensionManagerForm_0.Activate();
            if (!this.extensionManagerForm_0.method_0())
            {
                this.extensionManagerForm_0.method_2(this.class40_0);
            }
            if (this.class40_0.method_12() == Enum10.const_0)
            {
                this.class40_0.method_15();
            }
            Application.DoEvents();
        }

        public static void NavigateToThirdPartySite(string string_0)
        {
            bool flag = false;
            if (iauthenticationHost_0 != null)
            {
                if (iauthenticationHost_0.NavigateToThirdPartySite(string_0))
                {
                    flag = true;
                }
            }
            else
            {
                flag = true;
            }
            if (flag)
            {
                Process.Start(string_0);
            }
        }

        public static void SerializeConfig()
        {
            try
            {
                Config.Serialize();
            }
            catch
            {
            }
        }

        internal static void smethod_0(Class46 class46_0)
        {
            foreach (string str in class46_0.method_7().DeleteFiles)
            {
                string item = class46_0.method_7().DisplayName + ":" + str;
                if (!list_0.Contains(item))
                {
                    list_0.Add(item);
                }
            }
            smethod_2();
        }

        internal static void smethod_1(Class46 class46_0)
        {
            foreach (string str in class46_0.method_7().DeleteFiles)
            {
                string item = class46_0.method_7().DisplayName + ":" + str;
                if (list_0.Contains(item))
                {
                    list_0.Remove(item);
                }
            }
            smethod_2();
        }

        internal static void smethod_2()
        {
            if (!Directory.Exists(GetTempDir))
            {
                Directory.CreateDirectory(GetTempDir);
            }
            if ((list_0.Count == 0) && File.Exists(GetDeleteExtensionsFileName))
            {
                File.Delete(GetDeleteExtensionsFileName);
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(GetDeleteExtensionsFileName))
                {
                    foreach (string str in list_0)
                    {
                        writer.WriteLine(str);
                    }
                }
            }
        }

        public void UpdateExtensions()
        {
            if (this.updateExtensionsStartHandler_0 != null)
            {
                this.updateExtensionsStartHandler_0(this);
            }
            if (File.Exists(GetDeleteExtensionsFileName) || (Directory.Exists(GetTempDir) && (Directory.GetDirectories(GetTempDir).Length > 0)))
            {
                RemotingUpdateAction updateAction;
                string str6;
                Remoting remoting = null;
                string str = null;
                string str2 = null;
                bool flag = false;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    flag = true;
                    remoting = new Remoting(AppType.Client);
                    str = remoting.StartServer();
                    Thread.Sleep(0x5dc);
                    str2 = remoting.Initialize();
                    updateAction = remoting.UpdateAction;
                }
                else
                {
                    updateAction = new RemotingUpdateAction();
                }
                this.updateExtensionsForm_0 = new UpdateExtensionsForm(this);
                this.updateExtensionsForm_0.Show();
                Application.DoEvents();
                if (File.Exists(GetDeleteExtensionsFileName))
                {
                    using (StreamReader reader = new StreamReader(GetDeleteExtensionsFileName))
                    {
                        string str9;
                        while ((str9 = reader.ReadLine()) != null)
                        {
                            string[] strArray2 = str9.Split(new char[] { ':' });
                            string path = Path.Combine(GetWLRootDir, strArray2[1]);
                            try
                            {
                                if (!File.Exists(path))
                                {
                                    throw new FileNotFoundException("File not found");
                                }
                                if (str != null)
                                {
                                    throw new Exception("WealthLab.Extensions.Agent starting - " + str);
                                }
                                if (str2 != null)
                                {
                                    throw new Exception("Client initialization - " + str2);
                                }
                                str6 = updateAction.DeleteFile(path);
                                if (str6 != null)
                                {
                                    throw new Exception(str6);
                                }
                            }
                            catch (Exception exception2)
                            {
                                if (this.updateExtensionActionEventHandler_0 != null)
                                {
                                    this.updateExtensionActionEventHandler_0(this, new EventArgs6(Enum6.const_0, Enum7.const_1, strArray2[1], strArray2[0], exception2.Message));
                                }
                                continue;
                            }
                            if (this.updateExtensionActionEventHandler_0 != null)
                            {
                                this.updateExtensionActionEventHandler_0(this, new EventArgs6(Enum6.const_0, Enum7.const_0, strArray2[1], strArray2[0]));
                            }
                        }
                    }
                    File.Delete(GetDeleteExtensionsFileName);
                }
                if (Directory.Exists(GetTempDir))
                {
                    string[] directories = Directory.GetDirectories(GetTempDir);
                    if ((directories.Length > 0) && ((this.updateExtensionsForm_0 == null) || this.updateExtensionsForm_0.IsDisposed))
                    {
                        this.updateExtensionsForm_0 = new UpdateExtensionsForm(this);
                        this.updateExtensionsForm_0.Show();
                        this.updateExtensionsForm_0.Activate();
                    }
                    foreach (string str3 in directories)
                    {
                        foreach (string str4 in Directory.GetFiles(str3))
                        {
                            string[] strArray6 = str3.Split(new char[] { Path.DirectorySeparatorChar });
                            string str7 = strArray6[strArray6.Length - 1];
                            string destFileName = Path.Combine(GetWLRootDir, Path.GetFileName(str4.Remove(str4.LastIndexOf('.'))));
                            try
                            {
                                if (str != null)
                                {
                                    throw new Exception("WealthLab.Extensions.Agent starting - " + str);
                                }
                                if (str2 != null)
                                {
                                    throw new Exception("Client initialization - " + str2);
                                }
                                str6 = updateAction.MoveFile(str4, destFileName);
                                if (str6 != null)
                                {
                                    throw new Exception(str6);
                                }
                            }
                            catch (Exception exception)
                            {
                                if (this.updateExtensionActionEventHandler_0 != null)
                                {
                                    this.updateExtensionActionEventHandler_0(this, new EventArgs6(Enum6.const_2, Enum7.const_1, Path.GetFileName(destFileName), str7, exception.Message));
                                }
                                continue;
                            }
                            if (this.updateExtensionActionEventHandler_0 != null)
                            {
                                this.updateExtensionActionEventHandler_0(this, new EventArgs6(Enum6.const_2, Enum7.const_0, Path.GetFileName(destFileName), str7));
                            }
                        }
                        Directory.Delete(str3, true);
                    }
                }
                if (flag)
                {
                    if (str == null)
                    {
                        updateAction.CloseApplication();
                    }
                    if (remoting != null)
                    {
                        remoting.Unregister();
                    }
                }
                if (this.updateExtensionsCompletedHandler_0 != null)
                {
                    this.updateExtensionsCompletedHandler_0(this);
                }
                while (this.updateExtensionsForm_0 != null)
                {
                    if (this.updateExtensionsForm_0.IsDisposed)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
            }
        }

        internal static string GetDeleteExtensionsFileName
        {
            get
            {
                return Path.Combine(GetTempDir, "DeleteFiles.txt");
            }
        }

        internal static string GetTempDir
        {
            get
            {
                return Path.Combine(Application.UserAppDataPath, "ExtensionManagerTemp");
            }
        }

        internal static string GetWLRootDir
        {
            get
            {
                return Application.StartupPath;
            }
        }

        internal static Enum8 HostApp
        {
            get
            {
                return enum8_0;
            }
        }

        internal static Version HostVersion
        {
            get
            {
                return version_0;
            }
        }

        private delegate void Delegate6();

        private delegate void Delegate7();

        public delegate void UpdateExtensionActionEventHandler(object sender, EventArgs6 e);

        public delegate void UpdateExtensionsCompletedHandler(object sender);

        public delegate void UpdateExtensionsStartHandler(object sender);
    }
}

