using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using WealthLab.International;
using WealthLab.International.CustomersWebService;

internal class Class53
{
    private Enum16 enum16_0;
    private readonly int int_0 = 1;
    private int int_1;
    private int int_2;
    private string[] string_0 = new string[] { ActivateTrialCompletedEventArgs.smethod_0("搲尴夶ਸऺ戼笾⡀あ⹄͆㭈≊㭌⩎๐Ṓ㩔㍖㱘㝚", 7), ActivateTrialCompletedEventArgs.smethod_0("搲尴夶ਸऺ戼笾⡀あ⹄͆㭈≊㭌⩎๐R㱔ざ㝘㩚⥜⩞፠٢", 7), ActivateTrialCompletedEventArgs.smethod_0("搲尴夶ਸऺ戼紾⁀あ⁄Ն♈⩊㽌⭎๐Ṓ㑔㥖ⱘ㵚㱜㱞ᕠᙢᝤɦ᭨", 7), ActivateTrialCompletedEventArgs.smethod_0("搲尴夶ਸऺ戼紾⁀あ⁄Ն♈⩊㽌⭎๐͒❔㡖㵘⹚㹜⭞", 7), ActivateTrialCompletedEventArgs.smethod_0("搲尴夶ਸऺ戼紾⁀あ⁄Ն♈⩊㽌⭎๐Rご╖じ㩚ㅜᅞᑠ๢ݤɦ᭨", 7), ActivateTrialCompletedEventArgs.smethod_0("搲尴夶ਸऺ戼紾ࡀూᙄᡆш⩊⍌㩎㝐㉒㙔⍖ⱘ⥚㡜ⵞ", 7), ActivateTrialCompletedEventArgs.smethod_0("搲尴夶ਸऺ戼紾ࡀూᙄᡆᭈ⹊⅌⩎ぐ⁒ごፖ㡘⽚㡜", 7), ActivateTrialCompletedEventArgs.smethod_0("搲尴夶ਸऺ戼紾ࡀూᙄᡆὈ⹊㽌㱎㡐㱒㭔", 7), ActivateTrialCompletedEventArgs.smethod_0("搲尴夶ਸऺ戼紾ࡀూᙄᡆᩈ⹊㽌♎ぐ㽒᭔≖㑘㥚㡜ⵞ", 7) };

    public Enum16 method_0()
    {
        return this.enum16_0;
    }

    public int method_1()
    {
        return this.int_1;
    }

    public void method_2(int int_3)
    {
        this.int_1 = int_3;
    }

    public int method_3()
    {
        return this.int_2;
    }

    private string method_4(Enum16 enum16_1)
    {
        int index = Convert.ToInt32(enum16_1.ToString().Remove(0, 1));
        string[] strArray = this.string_0[index].ToString().Split(new char[] { '_' });
        string path = strArray[0] + ActivateTrialCompletedEventArgs.smethod_0("怾", 0x13) + strArray[1];
        string str3 = strArray[2];
        this.int_1++;
        try
        {
            ManagementObjectCollection instances = new ManagementClass(path).GetInstances();
            ManagementObject[] objectCollection = new ManagementObject[instances.Count];
            instances.CopyTo(objectCollection, 0);
            string str4 = objectCollection[0][str3].ToString().Trim();
            if (str4 != string.Empty)
            {
                this.int_2++;
                this.enum16_0 |= enum16_1;
            }
            return str4;
        }
        catch
        {
            return string.Empty;
        }
    }

    public string method_5()
    {
        this.int_1 = 0;
        this.int_2 = 0;
        StringBuilder builder = new StringBuilder();
        string[] strArray = this.method_4(Enum16.flag_1).Split(new char[] { ' ' });
        if (strArray.Length > 0)
        {
            builder.AppendLine(strArray[0]);
        }
        else
        {
            builder.AppendLine();
        }
        builder.AppendLine(this.method_4(Enum16.flag_2));
        builder.AppendLine(this.method_4(Enum16.flag_3));
        builder.AppendLine(this.method_4(Enum16.flag_4));
        builder.AppendLine(this.method_4(Enum16.flag_5));
        builder.AppendLine(this.method_4(Enum16.flag_6));
        builder.AppendLine(this.method_4(Enum16.flag_7));
        builder.AppendLine(this.method_4(Enum16.flag_8));
        builder.AppendLine(this.method_4(Enum16.flag_9));
        return builder.ToString();
    }

    public byte[] method_6()
    {
        byte[] bytes = Encoding.UTF8.GetBytes(this.method_5());
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        return provider.ComputeHash(bytes);
    }

    public string method_7()
    {
        return Class56.smethod_3(Class56.smethod_2(this.method_6()) + Class56.smethod_0(this.int_0) + Class56.smethod_0((int) this.enum16_0), 5);
    }
}

