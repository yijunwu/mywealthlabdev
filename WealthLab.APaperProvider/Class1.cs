using System;
using System.Collections.Generic;
using System.Reflection;
using WealthLab;
using WealthLab.APaperProvider;

internal class Class1
{
    private bool bool_0;
    private IDataHost idataHost_0;
    private MethodInfo methodInfo_0;
    private MethodInfo methodInfo_1;
    private object object_0;
    private object object_1;
    private const string string_0 = "i";
    private string string_1;

    public Class1(IDataHost idataHost_1)
    {
        this.idataHost_0 = idataHost_1;
    }

    private void method_0()
    {
        if (this.bool_0 && (this.method_5() != this.string_1))
        {
            this.bool_0 = false;
        }
        if (this.bool_0)
        {
            return;
        }
        List<Type> list = this.method_4();
        this.string_1 = this.method_5();
        Type type = null;
        this.bool_0 = true;
        using (List<Type>.Enumerator enumerator = list.GetEnumerator())
        {
            Type current;
            while (enumerator.MoveNext())
            {
                current = enumerator.Current;
                if (current.Name == "i")
                {
                    goto Label_007A;
                }
            }
            goto Label_008D;
        Label_007A:
            type = current;
        }
    Label_008D:
        if (this.string_1 != null)
        {
            foreach (Type type2 in list)
            {
                if (((this.string_1 == type2.Name) && !this.method_1(type2, type != null)) && (type != null))
                {
                    this.method_1(type, false);
                }
            }
        }
        else if (type != null)
        {
            ErrorMessage.Show("Streaming provider not specified (please open Preferences/Streaming Data to configure). By default, Paper accounts will use Yahoo! data.");
            this.method_1(type, false);
        }
        else
        {
            ErrorMessage.Show("Streaming provider not specified (please open Preferences/Streaming Data to configure)");
        }
    }

    private bool method_1(Type type_0, bool bool_1)
    {
        try
        {
            this.methodInfo_0 = type_0.GetMethod("GetQuote");
            if (this.methodInfo_0 == null)
            {
                throw new Exception("GetQuote method not found.");
            }
            this.object_0 = Activator.CreateInstance(type_0);
            (this.object_0 as StreamingDataProvider).Initialize(this.idataHost_0);
            this.object_1 = (this.object_0 as StreamingDataProvider).GetStaticProvider();
            this.methodInfo_1 = this.object_1.GetType().GetMethod("RequestHistoricalData");
            if (this.methodInfo_1 == null)
            {
                throw new Exception("RequestHistoricalData method not found.");
            }
            return true;
        }
        catch (Exception exception)
        {
            this.object_0 = null;
            string str = "The selected Streaming provider does not support Paper accounts. Please try selecting a different Streaming provider in Preferences/Streaming Data. ";
            if (bool_1)
            {
                str = str + "By default, Paper accounts will use Yahoo! data.";
            }
            ErrorMessage.Show(str + Environment.NewLine + Environment.NewLine + "Provider Type: " + type_0.Name + Environment.NewLine + "Error: " + exception.Message);
        }
        return false;
    }

    public Quote method_2(string string_2)
    {
        this.method_0();
        if (this.object_0 != null)
        {
            return (Quote) this.methodInfo_0.Invoke(this.object_0, new object[] { string_2 });
        }
        return null;
    }

    public Bars method_3(string string_2, DateTime dateTime_0, DateTime dateTime_1)
    {
        this.method_0();
        if (this.object_0 != null)
        {
            return (Bars) this.methodInfo_1.Invoke(this.object_1, new object[] { string_2, dateTime_0, dateTime_1 });
        }
        return new Bars(string_2, BarScale.Daily, 0);
    }

    public List<Type> method_4()
    {
        List<Type> list = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if ((type.BaseType != null) && (type.BaseType.ToString() == "WealthLab.StreamingDataProvider"))
                    {
                        list.Add(type);
                    }
                }
            }
            catch
            {
            }
        }
        return list;
    }

    public string method_5()
    {
        string str = null;
        if (this.idataHost_0.SettingsHost.ContainsKey("StreamingProvider"))
        {
            str = this.idataHost_0.SettingsHost.Get("StreamingProvider", "");
            if (str != null)
            {
                str = str.Trim();
                if (str == string.Empty)
                {
                    str = null;
                }
            }
        }
        return str;
    }
}

