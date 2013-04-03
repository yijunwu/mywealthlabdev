using System;
using System.Text;
using WealthLab.Cryptography;
using WealthLab.International;
using WealthLab.International.CustomersWebService;

internal class Class59
{
    private bool bool_0;
    private bool bool_1;
    private byte[] byte_0 = new byte[0x20];
    private byte[] byte_1 = new byte[0x10];
    private string string_0 = string.Empty;
    private string string_1 = string.Empty;
    private string string_2 = string.Empty;
    private string string_3 = string.Empty;
    private string string_4 = string.Empty;
    private string string_5 = string.Empty;
    private string string_6 = string.Empty;
    private readonly string string_7 = ActivateTrialCompletedEventArgs.smethod_0("漭䔯䘱尳爵夷丹崻", 2);

    public Class59(string string_8)
    {
        this.string_5 = string_8;
        this.method_16();
    }

    public string method_0()
    {
        return this.string_2;
    }

    public void method_1(string string_8)
    {
        this.string_2 = string_8;
    }

    public bool method_10()
    {
        return this.bool_1;
    }

    public void method_11(bool bool_2)
    {
        this.bool_1 = bool_2;
    }

    public string method_12()
    {
        return this.string_0;
    }

    public void method_13(string string_8)
    {
        this.string_0 = string_8;
    }

    public string method_14()
    {
        return this.string_1;
    }

    public void method_15(string string_8)
    {
        this.string_1 = string_8;
    }

    private void method_16()
    {
        string s = this.string_5.Replace(ActivateTrialCompletedEventArgs.smethod_0("ጽ", 0x12), "").Remove(0x10, 4);
        this.byte_1 = Encoding.ASCII.GetBytes(s);
        this.byte_0 = Encoding.ASCII.GetBytes(s + s);
    }

    public void method_17()
    {
        int num = 8;
        Config config = Config.Desereailize();
        this.bool_1 = config.NotShowAuthInfo;
        if (config.RememberData)
        {
            this.bool_0 = true;
            try
            {
                string str = this.method_20(config.Data);
                if (str.StartsWith(this.string_7))
                {
                    string[] strArray2 = str.Split(new string[] { ActivateTrialCompletedEventArgs.smethod_0("㤳㰵", num) }, StringSplitOptions.None);
                    this.string_2 = strArray2[1];
                    this.string_3 = strArray2[2];
                    this.string_4 = strArray2[3];
                    this.string_6 = strArray2[4];
                    if (strArray2.Length > 5)
                    {
                        this.string_0 = strArray2[5];
                        this.string_1 = strArray2[6];
                    }
                }
            }
            catch
            {
                this.bool_0 = false;
            }
        }
    }

    public void method_18()
    {
        Config config = new Config {
            RememberData = this.bool_0,
            NotShowAuthInfo = this.bool_1
        };
        if (this.bool_0)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(this.string_7);
            builder.AppendLine(this.string_2);
            builder.AppendLine(this.string_3);
            builder.AppendLine(this.string_4);
            builder.AppendLine(this.string_6);
            builder.AppendLine(this.string_0);
            builder.AppendLine(this.string_1);
            config.Data = this.method_19(builder.ToString());
        }
        config.Serialize();
    }

    private string method_19(string string_8)
    {
        RijndaelCryptography cryptography = new RijndaelCryptography(this.byte_0, this.byte_1);
        return cryptography.EncryptString(string_8);
    }

    public string method_2()
    {
        return this.string_3;
    }

    private string method_20(string string_8)
    {
        RijndaelCryptography cryptography = new RijndaelCryptography(this.byte_0, this.byte_1);
        return cryptography.DecryptString(string_8);
    }

    public void method_3(string string_8)
    {
        this.string_3 = string_8;
    }

    public string method_4()
    {
        return this.string_4;
    }

    public void method_5(string string_8)
    {
        this.string_4 = string_8;
    }

    public string method_6()
    {
        return this.string_6;
    }

    public void method_7(string string_8)
    {
        this.string_6 = string_8;
    }

    public bool method_8()
    {
        return this.bool_0;
    }

    public void method_9(bool bool_2)
    {
        this.bool_0 = bool_2;
    }
}

