using System;
using WealthLab.Extensions;
using WealthLab.Extensions.Attribute;

internal class Class46 : IComparable
{
    private Enum11 enum11_0;
    private ExtensionControl extensionControl_0;
    private ExtensionInfoAttribute extensionInfoAttribute_0;
    private ExtensionInfoAttribute extensionInfoAttribute_1;
    private string string_0;

    public Class46(ExtensionInfoAttribute extensionInfoAttribute_2, ExtensionInfoAttribute extensionInfoAttribute_3)
    {
        if (extensionInfoAttribute_2 != null)
        {
            this.enum11_0 = Enum11.flag_0;
            if (extensionInfoAttribute_3 != null)
            {
                this.enum11_0 = Enum11.flag_1 | Enum11.flag_0;
            }
        }
        else
        {
            if (extensionInfoAttribute_3 == null)
            {
                throw new ArgumentException("Local and Server information on the Extension are not present.");
            }
            this.enum11_0 = Enum11.flag_1;
        }
        this.extensionInfoAttribute_0 = extensionInfoAttribute_2;
        this.extensionInfoAttribute_1 = extensionInfoAttribute_3;
    }

    public string method_0()
    {
        return this.string_0;
    }

    public void method_1(string string_1)
    {
        this.string_0 = string_1;
    }

    public ExtensionControl method_2()
    {
        return this.extensionControl_0;
    }

    public void method_3(ExtensionControl extensionControl_1)
    {
        this.extensionControl_0 = extensionControl_1;
    }

    public bool method_4()
    {
        if (string.IsNullOrEmpty(this.method_9().PreInstallBatch))
        {
            return false;
        }
        return this.method_9().PreInstallBatch.ToLower().Contains("[fidelityapproved=true]");
    }

    public Enum12 method_5()
    {
        if (this.enum11_0 != Enum11.flag_0)
        {
            if (this.enum11_0 == Enum11.flag_1)
            {
                return Enum12.const_2;
            }
            if (ExtensionManager.HostApp == Enum8.const_1)
            {
                if (new Version(this.extensionInfoAttribute_0.Version) < new Version(this.extensionInfoAttribute_1.Version))
                {
                    return Enum12.const_1;
                }
            }
            else
            {
                if (Class40.smethod_2(this.method_7()) && !Class40.smethod_2(this.method_8()))
                {
                    return Enum12.const_0;
                }
                if (new Version(this.extensionInfoAttribute_0.Version) < new Version(this.extensionInfoAttribute_1.Version))
                {
                    return Enum12.const_1;
                }
            }
        }
        return Enum12.const_0;
    }

    public Enum11 method_6()
    {
        return this.enum11_0;
    }

    public ExtensionInfoAttribute method_7()
    {
        return this.extensionInfoAttribute_0;
    }

    public ExtensionInfoAttribute method_8()
    {
        return this.extensionInfoAttribute_1;
    }

    public ExtensionInfoAttribute method_9()
    {
        if (this.enum11_0 == Enum11.flag_1)
        {
            return this.extensionInfoAttribute_1;
        }
        return this.extensionInfoAttribute_0;
    }

    int IComparable.CompareTo(object object_0)
    {
        Class46 class2 = (Class46) object_0;
        return string.Compare(this.method_9().DisplayName, class2.method_9().DisplayName);
    }
}

