using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WealthLab.Cryptography;
using WealthLab.International.CustomersWebService;

internal class Class51
{
    private List<string> list_0 = new List<string>();
    private List<string> list_1 = new List<string>();

    public Class51()
    {
        this.list_0.Add(ActivateTrialCompletedEventArgs.smethod_0("昰嘲吴嬶䴸区焼帾⍀݂⁄ㅆ杈⹊㕌⩎", 5));
        this.list_0.Add(ActivateTrialCompletedEventArgs.smethod_0("昰嘲吴嬶䴸区焼帾⍀浂⅄⭆╈", 5));
        this.list_1.Add(ActivateTrialCompletedEventArgs.smethod_0("到ز眴䈶簸区䴼ܾ♀求ⵄᕆH㥊ࡌ⑎⡐ᙒ≔ᵖ⅘㱚恜扞", 5));
        this.list_1.Add(ActivateTrialCompletedEventArgs.smethod_0("欰弲吴稶眸娺猼栾畀ⱂل⡆繈ቊŌ㽎⭐ᑒṔⵖ㍘ਗ਼恜扞", 5));
        this.list_1.Add(ActivateTrialCompletedEventArgs.smethod_0("԰嘲ఴึ儸଺瀼䘾♀݂籄ెൈॊ⍌╎繐籒㝔╖ቘⱚ恜扞", 5));
    }

    public void method_0()
    {
        new Thread(new ThreadStart(this.method_1)) { IsBackground = true }.Start();
    }

    private void method_1()
    {
        do
        {
            Thread.Sleep((int) (0x7530 + ((int) (new Random().NextDouble() * 30000.0))));
        }
        while (!this.method_3());
        foreach (string str in this.list_0)
        {
            string item = this.method_2(str);
            if ((item != null) && !this.list_1.Contains(item))
            {
                Application.Exit();
            }
        }
    }

    private string method_2(string string_0)
    {
        string str = null;
        try
        {
            using (FileStream stream = new FileStream(Application.StartupPath + Path.DirectorySeparatorChar + string_0, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int) stream.Length);
                str = Convert.ToBase64String(RSACryptography.GetMD5Hash(buffer));
            }
        }
        catch
        {
        }
        return str;
    }

    private bool method_3()
    {
        int num = 0x13;
        if (Application.OpenForms != null)
        {
            bool flag;
            using (IEnumerator enumerator = Application.OpenForms.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Form current = (Form) enumerator.Current;
                    if (current.GetType().Name == ActivateTrialCompletedEventArgs.smethod_0("爾⁀⩂⭄ņ♈㥊⁌", num))
                    {
                        goto Label_004E;
                    }
                }
                goto Label_0068;
            Label_004E:
                flag = true;
            }
            return flag;
        }
    Label_0068:
        return false;
    }
}

