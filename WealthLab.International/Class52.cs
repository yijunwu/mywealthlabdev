using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WealthLab.Cryptography;
using WealthLab.International.CustomersWebService;

internal class Class52
{
    private DateTime dateTime_0 = DateTime.MinValue;
    private DateTime dateTime_1 = DateTime.MinValue;
    private readonly string string_0 = ActivateTrialCompletedEventArgs.smethod_0("瀰堲䄴弶༸縺砼ാ@⁂ńെ罈恊㕌ᕎ敐ὒ╔᭖མㅚݜ⡞ၠൢ⽤⡦ŨṪ⽬孮♰ቲၴ፶౸⍺᥼㡾떄몆", 5);
    private readonly string string_1 = ActivateTrialCompletedEventArgs.smethod_0("0愲怴夶欸砺䴼圾㙀㭂ⵄ͆絈⩊㱌⁎ᥐ᱒ᅔ᱖ᵘⱚ恜扞", 5);
    private string string_2;

    public string method_0()
    {
        return this.string_2;
    }

    public void method_1(string string_3)
    {
        this.string_2 = string_3;
    }

    private string method_10()
    {
        int num = 2;
        string localUserAppDataPath = Application.LocalUserAppDataPath;
        if (!Directory.Exists(localUserAppDataPath))
        {
            Directory.CreateDirectory(localUserAppDataPath);
        }
        return (localUserAppDataPath + Path.DirectorySeparatorChar + ActivateTrialCompletedEventArgs.smethod_0("夭尯嘱ĳᠵ尷嬹䠻", num));
    }

    public bool method_11()
    {
        return File.Exists(this.method_10());
    }

    public void method_12(int int_0)
    {
        if (this.method_14(int_0))
        {
            using (StreamWriter writer = new StreamWriter(this.method_10()))
            {
                writer.Write(this.method_7());
            }
        }
    }

    public void method_13(int int_0)
    {
        if (this.method_14(int_0) && this.method_11())
        {
            try
            {
                string str = null;
                using (StreamReader reader = new StreamReader(this.method_10()))
                {
                    str = reader.ReadToEnd();
                }
                if (str != null)
                {
                    this.method_9(str);
                }
            }
            catch
            {
            }
        }
    }

    private bool method_14(int int_0)
    {
        return (int_0 == 0x5316656);
    }

    public DateTime method_2()
    {
        return this.dateTime_0;
    }

    public void method_3(DateTime dateTime_2)
    {
        this.dateTime_0 = dateTime_2;
    }

    public DateTime method_4()
    {
        return this.dateTime_1;
    }

    public void method_5(DateTime dateTime_2)
    {
        this.dateTime_1 = dateTime_2;
    }

    private string method_6()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(this.string_2);
        builder.Append(ActivateTrialCompletedEventArgs.smethod_0("ؼ", 0x11));
        builder.Append(Convert.ToString(this.dateTime_0.ToBinary()));
        builder.Append(ActivateTrialCompletedEventArgs.smethod_0("ؼ", 0x11));
        builder.Append(Convert.ToString(this.dateTime_1.ToBinary()));
        builder.Append(ActivateTrialCompletedEventArgs.smethod_0("ؼ", 0x11));
        return builder.ToString();
    }

    private string method_7()
    {
        RijndaelCryptography cryptography = new RijndaelCryptography(Convert.FromBase64String(this.string_0), Convert.FromBase64String(this.string_1));
        return cryptography.EncryptString(this.method_6());
    }

    private void method_8(string string_3)
    {
        string[] strArray = string_3.Split(new char[] { ';' });
        this.method_1(strArray[0]);
        this.method_3(DateTime.FromBinary(Convert.ToInt64(strArray[1])));
        this.method_5(DateTime.FromBinary(Convert.ToInt64(strArray[2])));
    }

    private void method_9(string string_3)
    {
        RijndaelCryptography cryptography = new RijndaelCryptography(Convert.FromBase64String(this.string_0), Convert.FromBase64String(this.string_1));
        this.method_8(cryptography.DecryptString(string_3));
    }
}

