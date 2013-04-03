using System;
using System.Management;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using WealthLab.International;
using WealthLab.International.CustomersWebService;

internal class Class55
{
    private WealthLab.International.CustomersWebService.CustomersWebService customersWebService_0 = new WealthLab.International.CustomersWebService.CustomersWebService();
    private Enum14 enum14_0;
    private KeyMethodParams keyMethodParams_0;
    private string string_0;
    private string string_1 = ActivateTrialCompletedEventArgs.smethod_0("根笻簽ܿ潁絃൅ੇ繉態᥍捏ᙑᥓ筕歗捙ᵛٝ", 14);
    public static readonly string string_2 = ActivateTrialCompletedEventArgs.smethod_0("種弰堲嬴堶丸唺ᴼ娾㍀ㅂ⩄㕆楈捊経晎罐", 3);
    private TrialMethodParams trialMethodParams_0;

    public Class55()
    {
        this.customersWebService_0.ServiceHeaderValue = new ServiceHeader();
        this.customersWebService_0.ServiceHeaderValue.Param = this.string_1;
    }

    public string method_0()
    {
        return this.string_0;
    }

    public TrialMethodParams method_1()
    {
        return this.trialMethodParams_0;
    }

    private void method_10()
    {
        string message = null;
        try
        {
            string str2;
            string str3;
            switch (this.enum14_0)
            {
                case Enum14.const_0:
                    this.trialMethodParams_0.CurrentDate = DateTime.Now;
                    this.trialMethodParams_0.Hwid = Form0.smethod_4();
                    str2 = this.trialMethodParams_0.SetMessageId();
                    str3 = this.trialMethodParams_0.Serialize();
                    message = this.customersWebService_0.ActivateTrial(ref str3);
                    this.trialMethodParams_0 = (TrialMethodParams) BaseMethodParams.Deserialize(str3);
                    this.method_9(this.trialMethodParams_0, str2);
                    break;

                case Enum14.const_1:
                    this.keyMethodParams_0.CurrentDate = DateTime.Now;
                    str2 = this.keyMethodParams_0.SetMessageId();
                    str3 = this.keyMethodParams_0.Serialize();
                    message = this.customersWebService_0.ActivateKey(ref str3);
                    this.keyMethodParams_0 = (KeyMethodParams) BaseMethodParams.Deserialize(str3);
                    this.method_9(this.keyMethodParams_0, str2);
                    break;

                case Enum14.const_2:
                    this.trialMethodParams_0.CurrentDate = DateTime.Now;
                    this.trialMethodParams_0.Hwid = Form0.smethod_4();
                    str2 = this.trialMethodParams_0.SetMessageId();
                    str3 = this.trialMethodParams_0.Serialize();
                    message = this.customersWebService_0.AuthTrial(ref str3);
                    this.trialMethodParams_0 = (TrialMethodParams) BaseMethodParams.Deserialize(str3);
                    this.method_9(this.trialMethodParams_0, str2);
                    break;
            }
            if (message != null)
            {
                throw new DataBaseException(message);
            }
            if ((this.enum14_0 == Enum14.const_0) && (this.trialMethodParams_0.Result == 0))
            {
                throw new DataBaseException(string_2);
            }
        }
        catch (Exception exception)
        {
            this.string_0 = exception.Message;
        }
    }

    public void method_2(TrialMethodParams trialMethodParams_1)
    {
        this.trialMethodParams_0 = trialMethodParams_1;
    }

    public KeyMethodParams method_3()
    {
        return this.keyMethodParams_0;
    }

    public void method_4(KeyMethodParams keyMethodParams_1)
    {
        this.keyMethodParams_0 = keyMethodParams_1;
    }

    private void method_5(Delegate19 delegate19_0)
    {
        this.string_0 = null;
        IAsyncResult result = delegate19_0.BeginInvoke(null, null);
        while (!result.IsCompleted)
        {
            Application.DoEvents();
            Thread.Sleep(5);
        }
    }

    private string method_6()
    {
        ManagementObjectCollection instances = new ManagementClass(ActivateTrialCompletedEventArgs.smethod_0("愵儷吹༻ఽἿു㑃⍅㩇⭉㡋❍㹏㕑ݓ⽕⭗⹙㥛㍝", 10)).GetInstances();
        ManagementObject[] objectCollection = new ManagementObject[instances.Count];
        instances.CopyTo(objectCollection, 0);
        string str = (string) objectCollection[0][ActivateTrialCompletedEventArgs.smethod_0("电夷䨹䠻圽⼿ⱁ", 10)];
        return (((str.Replace(Convert.ToChar(0xae), ' ').Replace(Convert.ToChar(0x2122), ' ').Replace(ActivateTrialCompletedEventArgs.smethod_0("笵儷夹主儽㌿ⵁ≃㉅", 10), "").Replace(ActivateTrialCompletedEventArgs.smethod_0("愵儷吹堻儽㜿ㅁ", 10), "").Trim() + ActivateTrialCompletedEventArgs.smethod_0("ᘵ", 10) + Environment.OSVersion.ServicePack).Replace(ActivateTrialCompletedEventArgs.smethod_0("攵崷䠹䨻圽⌿❁摃ᙅ⥇⥉❋", 10), ActivateTrialCompletedEventArgs.smethod_0("攵样", 10)) + ActivateTrialCompletedEventArgs.smethod_0("ᘵ့", 10) + Environment.OSVersion.Version.ToString(3) + ActivateTrialCompletedEventArgs.smethod_0("ἵᠷ", 10)) + objectCollection[0][ActivateTrialCompletedEventArgs.smethod_0("礵欷瘹崻倽✿㝁╃ⅅⵇ", 10)]);
    }

    private void method_7(BaseMethodParams baseMethodParams_0)
    {
        baseMethodParams_0.Version = WealthLabAuthProvider.Version.ToString();
        baseMethodParams_0.ProductId = WealthLabAuthProvider.ProductId;
        baseMethodParams_0.APVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        try
        {
            baseMethodParams_0.OSVersion = this.method_6();
            baseMethodParams_0.OSPlatform = Environment.OSVersion.Platform.ToString();
        }
        catch
        {
        }
    }

    public void method_8(Enum14 enum14_1)
    {
        this.enum14_0 = enum14_1;
        switch (enum14_1)
        {
            case Enum14.const_0:
            case Enum14.const_2:
                this.method_7(this.trialMethodParams_0);
                break;

            case Enum14.const_1:
                this.method_7(this.keyMethodParams_0);
                break;
        }
        this.method_5(new Delegate19(this.method_10));
    }

    private void method_9(BaseMethodParams baseMethodParams_0, string string_3)
    {
        int num = 9;
        if ((baseMethodParams_0.MessageId == null) && (baseMethodParams_0.MessageId != string_3))
        {
            MessageBox.Show(ActivateTrialCompletedEventArgs.smethod_0("破制䨸䠺尼堾⑀捂ⱄ⍆ⱈ╊㥌♎㝐㩒㙔㙖ⵘ㑚⽜罞ɠౢᝤᕦᱨ᭪ᥬ佮ٰ᭲ၴ᥶奸᩺Ṽ᱾권ﮎ戀떔ﲘ爵펠趢", num), ActivateTrialCompletedEventArgs.smethod_0("瀴䔶䬸吺似", num), MessageBoxButtons.OK);
            Environment.Exit(1);
        }
    }

    private delegate void Delegate19();
}

