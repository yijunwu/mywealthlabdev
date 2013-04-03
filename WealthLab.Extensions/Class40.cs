using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using WealthLab.Cryptography;
using WealthLab.Extensions;
using WealthLab.Extensions.Attribute;

internal class Class40
{
    private Delegate12 delegate12_0;
    private Delegate13 delegate13_0;
    private Delegate14 delegate14_0;
    private Delegate15 delegate15_0;
    private Delegate15 delegate15_1;
    private Delegate15 delegate15_2;
    private Enum10 enum10_0;
    private Enum9 enum9_0;
    public List<Class46> list_0 = new List<Class46>();
    private List<ExtensionInfoAttribute> list_1;
    private List<ExtensionInfoAttribute> list_2;

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_0(Delegate12 delegate12_1)
    {
        this.delegate12_0 = (Delegate12) Delegate.Combine(this.delegate12_0, delegate12_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_1(Delegate12 delegate12_1)
    {
        this.delegate12_0 = (Delegate12) Delegate.Remove(this.delegate12_0, delegate12_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_10(Delegate15 delegate15_3)
    {
        this.delegate15_2 = (Delegate15) Delegate.Combine(this.delegate15_2, delegate15_3);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_11(Delegate15 delegate15_3)
    {
        this.delegate15_2 = (Delegate15) Delegate.Remove(this.delegate15_2, delegate15_3);
    }

    public Enum10 method_12()
    {
        return this.enum10_0;
    }

    public Enum9 method_13()
    {
        return this.enum9_0;
    }

    private void method_14()
    {
        this.enum10_0 = Enum10.const_1;
        this.list_0.Clear();
        this.list_1 = new List<ExtensionInfoAttribute>();
        this.list_2 = new List<ExtensionInfoAttribute>();
        this.list_1 = this.method_23();
        this.list_2 = this.method_24();
        this.method_27();
        this.enum10_0 = Enum10.const_2;
        if (this.delegate14_0 != null)
        {
            this.delegate14_0(this, new EventArgs9(Enum9.flag_1 | Enum9.flag_0));
        }
    }

    public void method_15()
    {
        new Delegate11(this.method_14).BeginInvoke(null, null);
    }

    private Class46 method_16(string string_0, bool bool_0)
    {
        foreach (Class46 class2 in this.list_0)
        {
            if ((class2.method_7() != null) && (class2.method_7().StrongName == string_0))
            {
                return class2;
            }
            if ((class2.method_8() != null) && (class2.method_8().StrongName == string_0))
            {
                return class2;
            }
        }
        return null;
    }

    public void method_17(string string_0)
    {
        if (this.method_12() != Enum10.const_2)
        {
            MessageBox.Show("Error (not searched)");
        }
        else
        {
            Class46 class2 = this.method_21(string_0);
            if (class2 != null)
            {
                Class46 class3 = this.method_16(class2.method_8().StrongName, true);
                if (smethod_1(class2.method_8()))
                {
                    if (class3 != null)
                    {
                        if (!class3.method_2().method_6())
                        {
                            if (class3.method_7() != null)
                            {
                                Version version = new Version(class3.method_7().Version);
                                Version version2 = new Version(class2.method_8().Version);
                                if ((version == version2) && (MessageBox.Show(ExtensionManagerForm.extensionManagerForm_0, "The Extension is already up to date. Are you sure you want to continue?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel))
                                {
                                    return;
                                }
                                if ((version > version2) && (MessageBox.Show(ExtensionManagerForm.extensionManagerForm_0, "The Extension's version is outdated. Are you sure you want to continue?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel))
                                {
                                    return;
                                }
                            }
                            Application.DoEvents();
                            if (!class3.method_2().method_6())
                            {
                                class3.method_2().method_11(class2.method_8());
                                if (this.delegate15_0 != null)
                                {
                                    this.delegate15_0(this, new EventArgs10(class3));
                                }
                            }
                        }
                    }
                    else
                    {
                        class2.method_3(new ExtensionControl());
                        class2.method_2().method_1(class2);
                        class2.method_2().method_2(new EventHandler(this.method_19));
                        class2.method_2().method_4(new EventHandler(this.method_18));
                        this.list_0.Add(class2);
                        this.list_0.Sort();
                        class2.method_2().method_11(class2.method_8());
                        if (this.delegate15_1 != null)
                        {
                            this.delegate15_1(this, new EventArgs10(class2));
                        }
                        if (this.delegate15_0 != null)
                        {
                            this.delegate15_0(this, new EventArgs10(class2));
                        }
                    }
                }
            }
        }
    }

    private void method_18(object sender, EventArgs e)
    {
        this.method_20((sender as ExtensionControl).method_0());
    }

    private void method_19(object sender, EventArgs e)
    {
        this.method_20((sender as ExtensionControl).method_0());
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_2(Delegate13 delegate13_1)
    {
        this.delegate13_0 = (Delegate13) Delegate.Combine(this.delegate13_0, delegate13_1);
    }

    private void method_20(Class46 class46_0)
    {
        this.list_0.Remove(class46_0);
        if (this.delegate15_2 != null)
        {
            this.delegate15_2(this, new EventArgs10(class46_0));
        }
    }

    public Class46 method_21(string string_0)
    {
        string str = null;
        Class46 class3;
        using (StreamReader reader = new StreamReader(string_0, Encoding.UTF8))
        {
            str = reader.ReadToEnd();
        }
        byte[] globalKey = RijndaelCryptography.GetGlobalKey();
        byte[] globalIV = RijndaelCryptography.GetGlobalIV();
        try
        {
            RijndaelCryptography cryptography = new RijndaelCryptography(globalKey, globalIV);
            return new Class46(null, ExtensionInfoAttribute.DeserializeFromString(cryptography.DecryptString(str)));
        }
        catch
        {
            MessageBox.Show("Invalid WLE File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            class3 = null;
        }
        return class3;
    }

    private Class46 method_22(ExtensionInfoAttribute extensionInfoAttribute_0)
    {
        Class46 class3;
        using (List<Class46>.Enumerator enumerator = this.list_0.GetEnumerator())
        {
            Class46 current;
            while (enumerator.MoveNext())
            {
                current = enumerator.Current;
                if (current.method_9().StrongName == extensionInfoAttribute_0.StrongName)
                {
                    goto Label_003A;
                }
            }
            return null;
        Label_003A:
            class3 = current;
        }
        return class3;
    }

    private List<ExtensionInfoAttribute> method_23()
    {
        this.enum9_0 = Enum9.flag_0;
        List<ExtensionInfoAttribute> list = new List<ExtensionInfoAttribute>();
        if (this.delegate12_0 != null)
        {
            this.delegate12_0(this, new EventArgs7(Enum9.flag_0));
        }
        string getWLRootDir = ExtensionManager.GetWLRootDir;
        string[] strArray = Directory.GetFiles(getWLRootDir, "*.exe", SearchOption.AllDirectories);
        string[] strArray2 = Directory.GetFiles(getWLRootDir, "*.dll", SearchOption.AllDirectories);
        string[] array = new string[strArray.Length + strArray2.Length];
        strArray.CopyTo(array, 0);
        strArray2.CopyTo(array, strArray.Length);
        bool flag = false;
        string message = null;
        try
        {
            for (int i = 0; i < array.Length; i++)
            {
                ExtensionInfoAttribute attributeFromFile = ExtensionInfoAttribute.GetAttributeFromFile(array[i], getWLRootDir);
                if (attributeFromFile != null)
                {
                    list.Add(attributeFromFile);
                }
                if (this.delegate13_0 != null)
                {
                    this.delegate13_0(this, new EventArgs8(Enum9.flag_0, (100 * (i + 1)) / array.Length));
                }
            }
        }
        catch (Exception exception)
        {
            flag = true;
            message = exception.Message;
        }
        if (this.delegate14_0 != null)
        {
            if (!flag)
            {
                this.delegate14_0(this, new EventArgs9(Enum9.flag_0));
                return list;
            }
            this.delegate14_0(this, new EventArgs9(Enum9.flag_0, true, message));
        }
        return list;
    }

    private List<ExtensionInfoAttribute> method_24()
    {
        this.enum9_0 = Enum9.flag_1;
        List<ExtensionInfoAttribute> list = new List<ExtensionInfoAttribute>();
        if (this.delegate12_0 != null)
        {
            this.delegate12_0(this, new EventArgs7(Enum9.flag_1));
        }
        try
        {
            foreach (string str in Class41.smethod_0().GetExtensionInfoAttributes())
            {
                list.Add(ExtensionInfoAttribute.DeserializeFromString(str));
            }
        }
        catch (Exception exception)
        {
            list.Clear();
            if (!ExtensionManager.Config.CheckUpdates.Value)
            {
                MessageBox.Show(string.Format("Unable to request Extension data from the server.\r\n\r\nError:\r\n{0}", exception.Message), "Server request error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        Class42.smethod_2("Server extensions count " + list.Count);
        return list;
    }

    private ExtensionInfoAttribute method_25(List<ExtensionInfoAttribute> list_3, string string_0)
    {
        ExtensionInfoAttribute attribute2;
        using (List<ExtensionInfoAttribute>.Enumerator enumerator = list_3.GetEnumerator())
        {
            ExtensionInfoAttribute current;
            while (enumerator.MoveNext())
            {
                current = enumerator.Current;
                if (current.StrongName == string_0)
                {
                    goto Label_002B;
                }
            }
            return null;
        Label_002B:
            attribute2 = current;
        }
        return attribute2;
    }

    public void method_26()
    {
        foreach (Class46 class2 in this.list_0)
        {
            class2.method_3(new ExtensionControl());
            class2.method_2().method_1(class2);
        }
    }

    public void method_27()
    {
        for (int i = 0; i < this.list_1.Count; i++)
        {
            ExtensionInfoAttribute attribute = this.list_1[i];
            ExtensionInfoAttribute attribute2 = this.method_25(this.list_2, attribute.StrongName);
            Class46 item = new Class46(attribute, attribute2);
            this.list_0.Add(item);
            if (attribute2 != null)
            {
                this.list_2.Remove(attribute2);
            }
        }
        if (ExtensionManager.HostApp == Enum8.const_0)
        {
            foreach (ExtensionInfoAttribute attribute3 in this.list_2)
            {
                if (smethod_2(attribute3))
                {
                    this.list_0.Add(new Class46(null, attribute3));
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_3(Delegate13 delegate13_1)
    {
        this.delegate13_0 = (Delegate13) Delegate.Remove(this.delegate13_0, delegate13_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_4(Delegate14 delegate14_1)
    {
        this.delegate14_0 = (Delegate14) Delegate.Combine(this.delegate14_0, delegate14_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_5(Delegate14 delegate14_1)
    {
        this.delegate14_0 = (Delegate14) Delegate.Remove(this.delegate14_0, delegate14_1);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_6(Delegate15 delegate15_3)
    {
        this.delegate15_0 = (Delegate15) Delegate.Combine(this.delegate15_0, delegate15_3);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_7(Delegate15 delegate15_3)
    {
        this.delegate15_0 = (Delegate15) Delegate.Remove(this.delegate15_0, delegate15_3);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_8(Delegate15 delegate15_3)
    {
        this.delegate15_1 = (Delegate15) Delegate.Combine(this.delegate15_1, delegate15_3);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void method_9(Delegate15 delegate15_3)
    {
        this.delegate15_1 = (Delegate15) Delegate.Remove(this.delegate15_1, delegate15_3);
    }

    private static void smethod_0(string string_0)
    {
        MessageBox.Show(string_0, "Extension Manager", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    public static bool smethod_1(ExtensionInfoAttribute extensionInfoAttribute_0)
    {
        Version version;
        if (ExtensionManager.HostApp == Enum8.const_0)
        {
            if (extensionInfoAttribute_0.HostApp == ExtensionHostApp.Developer)
            {
                smethod_0("Extension can be installed in Wealth-Lab Developer only.");
                return false;
            }
            if (!string.IsNullOrEmpty(extensionInfoAttribute_0.MinProVersion))
            {
                version = new Version(extensionInfoAttribute_0.MinProVersion);
                if (ExtensionManager.HostVersion < version)
                {
                    smethod_0(string.Format("Extension requires Wealth-Lab Pro version {0} or later.", version.ToString()));
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(extensionInfoAttribute_0.MaxProVersion))
            {
                version = new Version(extensionInfoAttribute_0.MaxProVersion);
                if (ExtensionManager.HostVersion > version)
                {
                    smethod_0(string.Format("Extension requires Wealth-Lab Pro version {0} or earlier.", version.ToString()));
                    return false;
                }
            }
        }
        if (ExtensionManager.HostApp == Enum8.const_1)
        {
            if (extensionInfoAttribute_0.HostApp == ExtensionHostApp.Pro)
            {
                smethod_0("Extension can be installed in Wealth-Lab Pro only.");
                return false;
            }
            if (!string.IsNullOrEmpty(extensionInfoAttribute_0.MinDeveloperVersion))
            {
                version = new Version(extensionInfoAttribute_0.MinDeveloperVersion);
                if (ExtensionManager.HostVersion < version)
                {
                    smethod_0(string.Format("Extension requires Wealth-Lab Developer version {0} or later.", version.ToString()));
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(extensionInfoAttribute_0.MaxDeveloperVersion))
            {
                version = new Version(extensionInfoAttribute_0.MaxDeveloperVersion);
                if (ExtensionManager.HostVersion > version)
                {
                    smethod_0(string.Format("Extension requires Wealth-Lab Developer version {0} or earlier.", version.ToString()));
                    return false;
                }
            }
        }
        return true;
    }

    public static bool smethod_2(ExtensionInfoAttribute extensionInfoAttribute_0)
    {
        return (!string.IsNullOrEmpty(extensionInfoAttribute_0.PreInstallBatch) && extensionInfoAttribute_0.PreInstallBatch.ToLower().Contains("[fidelityapproved=true]"));
    }

    private delegate void Delegate11();

    public delegate void Delegate12(object sender, EventArgs7 e);

    public delegate void Delegate13(object sender, EventArgs8 e);

    public delegate void Delegate14(object sender, EventArgs9 e);

    public delegate void Delegate15(object sender, EventArgs10 e);
}

