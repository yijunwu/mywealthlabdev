using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WealthLab.International;
using WealthLab.International.CustomersWebService;

internal class Form0 : Form
{
    private bool bool_0;
    private bool bool_1;
    private bool bool_2;
    private readonly ButtonBehaviour buttonBehaviour_0;
    private readonly ButtonBehaviour buttonBehaviour_1;
    private readonly ButtonBehaviour buttonBehaviour_2;
    private Class57 class57_0;
    private Class57 class57_1;
    private Class57 class57_2;
    private Class57 class57_3;
    private Class57 class57_4;
    private Class58 class58_0;
    private static Class59 class59_0;
    private IContainer icontainer_0;
    private KeyMethodParams keyMethodParams_0;
    private Label label_0;
    public static readonly string string_0 = ActivateTrialCompletedEventArgs.smethod_0("氽┿㙁ㅃ㑅♇橉㡋⅍灏≑♓㍕⹗㍙㍛⭝፟䉡ᑣݥཧཀྵ䱫཭ṯᙱ味uᵷࡹᕻ᡽勵ꊁﲇﺋ늑ﮙ늛", 0x12);
    public static readonly string string_1 = ActivateTrialCompletedEventArgs.smethod_0("紽ⰿ⭁❃ⵅ桇桉ੋ❍㹏㭑❓㹕穗穙⡛ㅝ䁟ᅡၣݥᩧṩ䱫㥭ᕯ፱ᡳɵၷ坹ほώꊁ삃ﺇ뢕", 0x12);
    public static readonly string string_2 = ActivateTrialCompletedEventArgs.smethod_0("紽⼿⽁㑃㍅㱇⽉㹋湍㍏㹑㭓㕕㍗穙㡛㝝ٟѡţᑥ᭧䩩੫ᱭὯά味ɵၷό屻ൽ慎궉ﾋ꺍ﮑ煉뢗뺝춟춡횣쎥袧\udea9\uc4ab쾭\udeaf\u92b1튳\ud9b5\ucdb7좹鲻\udabd\ua1bf믁럃", 0x12);
    private readonly string string_3;
    private string string_4;
    private static string string_5;
    private string string_6;
    private static WizardFormMode wizardFormMode_0;
    private WizardPage wizardPage_0;
    private WizardPageActivateKey wizardPageActivateKey_0;
    private WizardPageActivateTrial wizardPageActivateTrial_0;
    private WizardPageAuthInfo wizardPageAuthInfo_0;
    private WizardPageError wizardPageError_0;
    private WizardPageHwidToRegister wizardPageHwidToRegister_0;
    private WizardPageInfo wizardPageInfo_0;
    private WizardPageProgress wizardPageProgress_0;
    private WizardPageSelectActivationMethod wizardPageSelectActivationMethod_0;

    public Form0()
    {
        this.wizardPageError_0 = new WizardPageError();
        this.wizardPageInfo_0 = new WizardPageInfo();
        this.string_3 = ActivateTrialCompletedEventArgs.smethod_0("嘱倳ᬵ男眹焻ጽ㤿㭁㵃㽅", 6);
        this.buttonBehaviour_0 = ButtonBehaviour.Cancel | ButtonBehaviour.Previous;
        this.buttonBehaviour_1 = ButtonBehaviour.Finish;
        this.buttonBehaviour_2 = ButtonBehaviour.Cancel | ButtonBehaviour.Previous | ButtonBehaviour.Next;
        this.method_17();
        base.ControlBox = false;
        this.wizardPageInfo_0.method_6(new Delegate20(this.method_3));
        this.wizardPageError_0.method_6(new Delegate20(this.method_3));
    }

    public Form0(bool bool_3) : this()
    {
        int num = 0x10;
        if (bool_3)
        {
            wizardFormMode_0 = WizardFormMode.FirstStart;
        }
        else
        {
            wizardFormMode_0 = WizardFormMode.FromApplication;
        }
        this.Text = smethod_8() + ActivateTrialCompletedEventArgs.smethod_0("᰻椽⤿㡁╃㑅ⱇ", num);
    }

    private void class57_0_Click(object sender, EventArgs e)
    {
        if (this.wizardPageInfo_0.method_14())
        {
            class59_0.method_9(true);
            if ((this.wizardPageActivateKey_0 != null) && (this.wizardPageHwidToRegister_0 != null))
            {
                class59_0.method_1(this.wizardPageActivateKey_0.method_6());
                class59_0.method_3(this.wizardPageActivateKey_0.method_8());
                class59_0.method_5(this.wizardPageActivateKey_0.method_10());
                class59_0.method_7(this.wizardPageHwidToRegister_0.method_6());
            }
            if (this.wizardPageActivateTrial_0 != null)
            {
                class59_0.method_13(this.wizardPageActivateTrial_0.method_6());
                class59_0.method_15(this.wizardPageActivateTrial_0.method_7());
            }
        }
        else
        {
            bool flag = class59_0.method_10();
            class59_0 = new Class59(string_5);
            class59_0.method_11(flag);
        }
        class59_0.method_18();
        base.DialogResult = DialogResult.OK;
    }

    private void class57_1_Click(object sender, EventArgs e)
    {
        int num = 11;
        if (this.wizardPage_0 is WizardPageSelectActivationMethod)
        {
            if (this.wizardPageSelectActivationMethod_0.method_6() == Enum13.const_0)
            {
                if (this.wizardPageActivateTrial_0 == null)
                {
                    this.wizardPageActivateTrial_0 = new WizardPageActivateTrial();
                }
                this.method_9(this.wizardPageActivateTrial_0);
            }
            if (this.wizardPageSelectActivationMethod_0.method_6() == Enum13.const_1)
            {
                if (this.wizardPageActivateKey_0 == null)
                {
                    this.wizardPageActivateKey_0 = new WizardPageActivateKey();
                }
                this.method_9(this.wizardPageActivateKey_0);
            }
        }
        else if (this.wizardPage_0 is WizardPageAuthInfo)
        {
            class59_0.method_11(this.wizardPageAuthInfo_0.method_6());
            if (this.wizardPageSelectActivationMethod_0 == null)
            {
                this.wizardPageSelectActivationMethod_0 = new WizardPageSelectActivationMethod();
            }
            this.method_9(this.wizardPageSelectActivationMethod_0);
        }
        else
        {
            Class55 class2;
            if (this.wizardPage_0 is WizardPageHwidToRegister)
            {
                if (this.wizardPage_0.vmethod_0())
                {
                    if (this.wizardPageProgress_0 == null)
                    {
                        this.wizardPageProgress_0 = new WizardPageProgress();
                    }
                    this.wizardPageProgress_0.method_7(ActivateTrialCompletedEventArgs.smethod_0("愶尸䤺吼夾㡀⩂⭄⁆楈㉊≌㩎⍐獒㭔㙖㑘㹚絜㹞འݢ䕤౦౨ቪ", num));
                    this.wizardPageProgress_0.method_1(smethod_1());
                    this.method_9(this.wizardPageProgress_0);
                    Application.DoEvents();
                    class2 = new Class55();
                    class2.method_4(new KeyMethodParams());
                    class2.method_3().FirstName = this.wizardPageActivateKey_0.method_6();
                    class2.method_3().LastName = this.wizardPageActivateKey_0.method_8();
                    class2.method_3().ActivationKey = this.wizardPageActivateKey_0.method_10();
                    class2.method_3().Hwid = string_5;
                    class2.method_3().A = this.string_6;
                    class2.method_3().ComputerName = this.wizardPageHwidToRegister_0.method_6();
                    if (wizardFormMode_0 == WizardFormMode.FromApplication)
                    {
                        class2.method_3().AuthenticatedRequest = true;
                    }
                    class2.method_8(Enum14.const_1);
                    this.keyMethodParams_0 = class2.method_3();
                    this.method_13(class2);
                }
            }
            else if ((this.wizardPage_0 is WizardPageInfo) && (this.wizardPage_0 as WizardPageInfo).method_8())
            {
                if (this.wizardPageProgress_0 == null)
                {
                    this.wizardPageProgress_0 = new WizardPageProgress();
                }
                this.wizardPageProgress_0.method_7(string.Format(ActivateTrialCompletedEventArgs.smethod_0("收尸䴺刼吾⑀捂ᕄц楈ൊ⑌ⅎ㙐㙒❔❖⭘㉚㍜⭞䅠䅢Ṥ坦ᑨ䥪", num), this.keyMethodParams_0.HwidRegistered));
                this.wizardPageProgress_0.method_1(smethod_2());
                this.method_9(this.wizardPageProgress_0);
                Application.DoEvents();
                class2 = new Class55();
                class2.method_4(this.keyMethodParams_0);
                class2.method_3().Result = 0;
                class2.method_3().CompromisedOldHwid = true;
                class2.method_8(Enum14.const_1);
                this.method_13(class2);
            }
            else if (this.wizardPage_0 is WizardPageActivateKey)
            {
                if (this.wizardPage_0.vmethod_0())
                {
                    if (this.wizardPageHwidToRegister_0 == null)
                    {
                        this.wizardPageHwidToRegister_0 = new WizardPageHwidToRegister();
                    }
                    this.method_9(this.wizardPageHwidToRegister_0);
                    Application.DoEvents();
                }
            }
            else if ((this.wizardPage_0 is WizardPageActivateTrial) && this.wizardPage_0.vmethod_0())
            {
                if (this.wizardPageProgress_0 == null)
                {
                    this.wizardPageProgress_0 = new WizardPageProgress();
                }
                this.wizardPageProgress_0.method_7(ActivateTrialCompletedEventArgs.smethod_0("愶尸䤺吼夾㡀⩂⭄⁆楈㉊≌㩎⍐獒㭔㙖㑘㹚", num));
                this.wizardPageProgress_0.method_1(smethod_2());
                this.method_9(this.wizardPageProgress_0);
                Application.DoEvents();
                class2 = new Class55();
                class2.method_2(new TrialMethodParams());
                class2.method_1().UserName = this.wizardPageActivateTrial_0.method_6();
                class2.method_1().Password = this.wizardPageActivateTrial_0.method_7();
                if (wizardFormMode_0 == WizardFormMode.FirstStart)
                {
                    class2.method_8(Enum14.const_0);
                }
                else
                {
                    class2.method_8(Enum14.const_2);
                }
                this.method_12(class2);
            }
        }
    }

    private void class57_2_Click(object sender, EventArgs e)
    {
        if (this.wizardPage_0 is WizardPageSelectActivationMethod)
        {
            this.method_9(this.wizardPageAuthInfo_0);
        }
        if (!(this.wizardPage_0 is WizardPageActivateTrial) && !(this.wizardPage_0 is WizardPageActivateKey))
        {
            if ((this.wizardPage_0 is WizardPageError) || (this.wizardPage_0 is WizardPageInfo))
            {
                if (this.wizardPageSelectActivationMethod_0 != null)
                {
                    if (this.wizardPageSelectActivationMethod_0.method_6() == Enum13.const_0)
                    {
                        this.method_9(this.wizardPageActivateTrial_0);
                    }
                    else
                    {
                        this.method_9(this.wizardPageActivateKey_0);
                    }
                }
                else
                {
                    this.method_9(this.wizardPageActivateKey_0);
                }
            }
            if (this.wizardPage_0 is WizardPageHwidToRegister)
            {
                this.method_9(this.wizardPageActivateKey_0);
            }
        }
        else
        {
            this.method_9(this.wizardPageSelectActivationMethod_0);
        }
    }

    private void class57_3_Click(object sender, EventArgs e)
    {
        int num = 7;
        if ((wizardFormMode_0 != WizardFormMode.FirstStart) || (MessageBox.Show(ActivateTrialCompletedEventArgs.smethod_0("爲嘴䌶倸䴺尼䬾⡀ⱂ⭄杆⁈㡊浌ⅎ㑐げご⑖⩘㩚⽜♞䅠բ੤ᕦ䥨㱪࡬๮ᵰݲᵴ婶㕸᩺ὼ彾얀ﶌ뎒릘쪠趢ꢤ궦쒪趬횮\udeb0\uc6b2閴얶\udcb8\udaba톼펾룀닄ꛆꟈ뿊믎뻐뛔뛖럘룚룜돞쇠苢蛤鏦胨鷪賬鯮飰鳲鯴죶", num), ActivateTrialCompletedEventArgs.smethod_0("瀲娴夶弸刺似刾⁀㝂ⱄ⡆❈", num), MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.Cancel))
        {
            if (wizardFormMode_0 == WizardFormMode.FromApplication)
            {
                class59_0.method_18();
            }
            base.DialogResult = DialogResult.Cancel;
        }
    }

    private void class57_4_Click(object sender, EventArgs e)
    {
        int num = 9;
        try
        {
            Process.Start(ActivateTrialCompletedEventArgs.smethod_0("崴䌶䴸䬺ܼှ湀㑂㉄う筈敊㩌⩎ぐ㽒⅔㽖瑘㝚㱜㵞你b੤੦䙨㱪Ⅼ婮♰ᩲṴṶ噸ⱺㅼ㭾뒀슂ﶊﮎﲒﮔ릖", num));
        }
        catch
        {
        }
    }

    public bool method_0()
    {
        return this.bool_1;
    }

    public void method_1(bool bool_3)
    {
        this.bool_2 = bool_3;
    }

    private void method_10()
    {
        if (this.class57_0.Enabled)
        {
            base.AcceptButton = this.class57_0;
            this.class57_0.Focus();
        }
        if (this.class57_1.Enabled)
        {
            base.AcceptButton = this.class57_1;
            this.class57_1.Focus();
        }
        if (this.class57_3.Enabled)
        {
            base.CancelButton = this.class57_3;
        }
        if (this.class57_2.Enabled && !this.class57_1.Enabled)
        {
            base.AcceptButton = this.class57_2;
            this.class57_2.Focus();
        }
    }

    private void method_11(ButtonBehaviour buttonBehaviour_3)
    {
        this.class57_3.Enabled = (buttonBehaviour_3 & ButtonBehaviour.Cancel) != 0;
        this.class57_2.Enabled = (buttonBehaviour_3 & ButtonBehaviour.Previous) != 0;
        this.class57_1.Enabled = (buttonBehaviour_3 & ButtonBehaviour.Next) != 0;
        this.class57_0.Enabled = (buttonBehaviour_3 & ButtonBehaviour.Finish) != 0;
    }

    private void method_12(Class55 class55_0)
    {
        int num = 13;
        this.string_4 = null;
        if (class55_0.method_0() != null)
        {
            this.method_15(class55_0.method_0(), smethod_2());
            this.string_4 = class55_0.method_0();
        }
        else
        {
            if (this.wizardPageInfo_0 == null)
            {
                this.wizardPageInfo_0 = new WizardPageInfo();
            }
            switch (class55_0.method_1().Result)
            {
                case 1:
                    this.string_4 = ActivateTrialCompletedEventArgs.smethod_0("瀸唺帼倾㍀ㅂ⁄⑆㵈歊㡌㱎㑐⅒㭔㙖㑘㹚絜ぞ፠䍢ᕤ٦ᩨᡪᩬnͰᝲ孴究獸", num);
                    this.method_14(this.string_4 + string_0, smethod_2(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 2:
                    this.string_4 = string.Format(ActivateTrialCompletedEventArgs.smethod_0("洸䤺吼帾ⵀ捂⍄⡆㭈歊㡌㱎㑐⅒畔畖≘歚⁜絞䅠ᑢѤᑦ䥨੪Ŭᵮᑰቲᅴ๶奸᩺Ṽ୾권ﾐ뎒Ꚗ떚", num), this.wizardPageActivateTrial_0.method_6(), class55_0.method_1().ActivationTrialDate.ToString(this.string_3));
                    this.method_14(this.string_4, smethod_2(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 3:
                    if (wizardFormMode_0 != WizardFormMode.FirstStart)
                    {
                        this.method_14(string.Format(ActivateTrialCompletedEventArgs.smethod_0("砸为䤼圾⑀ⵂㅄ⹆⩈⩊㥌♎㹐㵒畔⁖㡘⡚絜ⱞᑠb٤ɦᩨᡪ୬ᩮᵰ嵲硴絶⁸ᑺࡼ彾ꦈﾐ뎒煮뾞햠쮢삤螦\udda8\ud9aa쒬캮\uddb0\u93b2펴\ud8b6\ucbb8鮺욼达변ꇄꛆ냈뻌￐", num), class55_0.method_1().DaysFromActivation + 1), smethod_2(), Enum15.const_0, this.buttonBehaviour_1);
                        break;
                    }
                    this.method_14(string.Format(ActivateTrialCompletedEventArgs.smethod_0("紸帺尼䴾慀㡂畄㩆效歊㥌❎ぐ㵒㹔⑖祘㵚㉜ⵞ䅠ၢᅤ٦᭨ὪѬŮᙰ卲౴ᡶ౸ॺ嵼୾ꞈ蚊螌횎ﺐ랖ﺞ춠莢햤슦\udba8\uc2aa슬쮮醰\udab2\uc6b4鞶\ud8b8\ud8ba즼횾럀ꛂ닆ꟈ뿊꓌ꏎ꣒꫖훚ퟜ", num) + string_1, this.wizardPageActivateTrial_0.method_6(), DateTime.Now.AddMonths(1).ToString(this.string_3)), smethod_2(), Enum15.const_0, this.buttonBehaviour_1);
                    break;

                case 4:
                    this.string_4 = string_2;
                    this.method_14(smethod_3() + this.string_4, smethod_2(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 5:
                    this.string_4 = string.Format(ActivateTrialCompletedEventArgs.smethod_0("洸䤺吼帾ⵀ捂⍄⡆㭈歊㡌㱎㑐⅒畔畖≘歚⁜絞䅠ᑢѤᑦ䥨ժɬ᭮兰ቲᙴͶၸൺᱼ୾ꮄ", num), this.wizardPageActivateTrial_0.method_6());
                    this.method_14(smethod_3() + this.string_4, smethod_2(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 6:
                    this.string_4 = ActivateTrialCompletedEventArgs.smethod_0("洸䤺吼帾ⵀ捂⁄㽆㥈≊㽌⩎㕐絒", num);
                    this.method_14(string.Format(smethod_3() + this.string_4 + ActivateTrialCompletedEventArgs.smethod_0("ᤸ䀺഼䈾慀❂⑄㹆㩈歊╌⹎≐獒╔㙖⩘⡚㡜㭞䅠ၢ౤०੨๪䵬๮ተݲᱴŶᡸེᑼၾ궂", num), class55_0.method_1().DaysFromActivation), smethod_2(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 7:
                    this.string_4 = ActivateTrialCompletedEventArgs.smethod_0("洸䤺吼帾ⵀ捂㉄♆㩈歊ⱌ⍎⍐㙒㑔㍖⁘筚㱜㱞ᕠ੢፤٦ᵨ๪६佮Ṱᵲ啴Ͷᅸቺ๼彾ﲈﾊﶎ뾐", num);
                    this.method_14(smethod_3() + this.string_4, smethod_2(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                default:
                    return;
            }
            this.bool_0 = true;
        }
    }

    private void method_13(Class55 class55_0)
    {
        int num = 4;
        this.string_4 = null;
        if (class55_0.method_0() != null)
        {
            this.method_15(class55_0.method_0(), smethod_1());
            this.string_4 = class55_0.method_0();
        }
        else
        {
            string str;
            if (this.wizardPageInfo_0 == null)
            {
                this.wizardPageInfo_0 = new WizardPageInfo();
            }
            switch (class55_0.method_3().Result)
            {
                case 1:
                    this.string_4 = ActivateTrialCompletedEventArgs.smethod_0("支䄱儳䐵ᠷ吹医䨽怿⑁⭃㍅♇⹉手䍍婏", num);
                    this.method_14(this.string_4 + string_0, smethod_1(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 2:
                    this.string_4 = ActivateTrialCompletedEventArgs.smethod_0("礯就圳夵䨷䠹夻崽㐿扁Ճ╅㱇⍉㩋⽍⑏㭑㭓㡕硗ᅙ㥛❝也潡湣", num);
                    this.method_14(this.string_4 + string_0, smethod_1(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 3:
                    this.string_4 = string.Format(ActivateTrialCompletedEventArgs.smethod_0("缯䀱倳匵䨷ᨹ娻儽㈿扁ぃ⹅ⵇ橉ോⵍ⑏㭑≓㝕ⱗ㍙㍛そ䁟ॡţὥ䡧ɩ൫ᵭ偯űs᝵౷ཹཻ幽ꉿ女뒃ﮅꪇ", num), class55_0.method_3().OrderStatusDescription);
                    this.method_14(smethod_3() + this.string_4, smethod_1(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 4:
                    this.string_4 = ActivateTrialCompletedEventArgs.smethod_0("怯焱ᐳ瀵儷吹嬻嬽㈿㉁㙃⽅♇㹉汋⅍㙏牑⁓㹕ㅗ⥙籛㵝ཟཡᑣ፥ᱧཀྵṫ乭ᡯ፱ݳ噵᩷ό᥻ၽꁿ낏ﮓ뢗ﮝ肟즡솣\udfa5\u88a7쾩슫\udaad\ud5af삱톳튵隷랹뚻", num);
                    this.method_14(smethod_3() + this.string_4, smethod_1(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 5:
                    this.string_4 = ActivateTrialCompletedEventArgs.smethod_0("簯圱䜳䔵ᠷ丹吻弽⸿扁牃癅桇⹉ⵋ㝍⍏牑㱓㝕⭗穙ⱛ㽝፟ᅡţɥ䡧ᥩիm፯᝱味ɵၷ፹ཻ幽ﶃꚅﾇﾋ꺍ﲑﾙ뺝쾟첡蒣장욧얩\ud8ab\uc6ad햯삱钳﮷钹놻뒽", num);
                    this.method_14(smethod_3() + this.string_4 + string.Format(ActivateTrialCompletedEventArgs.smethod_0("怯焱ᐳ瀵儷吹嬻嬽㈿㉁㙃⽅♇㹉汋汍⭏扑⥓瑕硗㕙㉛繝͟ൡॣᙥᵧṩ५ᱭ偯偱ཱི䝵շ塹屻ॽꒃﶍﲗ몙겝\udd9f\u82a1삣장톧芩\udfab\u87ad邯펱펳\ud9b5\u96b7骹튽ꖿꏁ럃ꏅ꧉ꏋꃍ꓏돑럓ꋕ觙꧛껝郟跡難鋥웧", num), class55_0.method_3().HwidRegistered, class55_0.method_3().ComputerNameRegistered, class55_0.method_3().DaysFromLastRegistrationHwid), smethod_1(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 6:
                    this.string_4 = ActivateTrialCompletedEventArgs.smethod_0("椯崱䄳䐵ᠷ儹夻䜽怿㕁╃㕅桇⭉⁋㱍㕏㍑こ⽕硗㭙㽛⩝य़ᑡգብ൧๩䱫ŭṯ剱ᕳ噵᭷ᕹᅻ๽ꢇﶉ揄늑ﾕﺗﲙ鍊얟첡킣蚥貫\ud9af\udcb1펳펵쪷쪹캻ힽ꺿뛁쯅싇", num);
                    this.method_14(this.string_4 + string.Format(ActivateTrialCompletedEventArgs.smethod_0("怯焱ᐳ瀵儷吹嬻嬽㈿㉁㙃⽅♇㹉汋汍⭏扑⥓瑕硗㕙㉛繝͟ൡॣᙥᵧṩ५ᱭ偯偱ཱི䝵շ塹屻ॽꒃﶍﲗ몙겝\udd9f\u82a1삣장톧芩\udfab\u87ad邯펱펳\ud9b5\u96b7骹", num), class55_0.method_3().HwidRegistered, class55_0.method_3().ComputerNameRegistered, class55_0.method_3().DaysFromLastRegistrationHwid) + ActivateTrialCompletedEventArgs.smethod_0("椯崱䄳ᘵ嬷嬹刻ḽ㈿❁㉃⥅⍇⽉汋⅍㱏㙑瑓ٕ᭗穙ᩛ㝝๟աţᑥᡧᡩիmѯ剱ᕳᡵᱷ婹ᵻᵽﲇ겋揄뒓ﶕﶗ벛쾟킡蒣장袧캩얫좭횯ힱ욳펵횷캹鲻\ud8bd\ua9bf곁ꏃꏅ뫇뫉뻋ꟍ뻏ꛑ䀘苕럗龎뿛뇝軟雡跣裥鷧迩샫컭胯胱釳藵请\udaf9\udefb냽旿稁瀃␅☇", num), smethod_1(), Enum15.const_2, this.buttonBehaviour_2);
                    this.wizardPageInfo_0.method_9(true);
                    return;

                case 7:
                    str = ActivateTrialCompletedEventArgs.smethod_0("琯圱唳䐵ᠷ", num) + this.wizardPageActivateKey_0.method_6();
                    if (wizardFormMode_0 != WizardFormMode.FirstStart)
                    {
                        str = str + ActivateTrialCompletedEventArgs.smethod_0("ᰯሱ唳䌵䰷刹夻倽㐿⭁❃❅㱇⍉⍋⁍灏║㕓╕硗⥙⥛㵝͟ݡᝣᕥ๧Ὡk䁭絯硱", num);
                        break;
                    }
                    str = str + ActivateTrialCompletedEventArgs.smethod_0("ᰯሱ䴳夵䴷䠹᰻崽⼿㉁㵃晅⁇⭉㽋湍㉏㝑ㅓ㡕硗⥙⥛㵝͟ݡᝣᕥ๧Ὡkɭ९剱ᕳᕵ౷፹੻ώꢅ薇肉", num);
                    break;

                case 8:
                    this.string_4 = string_2;
                    this.method_14(smethod_3() + this.string_4, smethod_1(), Enum15.const_1, this.buttonBehaviour_0);
                    return;

                case 9:
                    this.string_4 = string.Format(ActivateTrialCompletedEventArgs.smethod_0("累匱崳堵䰷弹刻弽⸿⅁⅃晅ⵇ㉉㱋❍≏㝑こ癕㝗㑙籛╝偟ὡ䩣步执", num), class55_0.method_3().ExpirationDate.Value.ToString(this.string_3));
                    this.method_14(smethod_3() + this.string_4 + ActivateTrialCompletedEventArgs.smethod_0("猯帱崳唵匷ᨹ䠻嘽┿扁⡃⽅♇ⅉ汋ⱍ㕏㹑㭓⅕硗⹙㍛繝቟ݡ੣ͥὧ䩩Ⅻ཭᥯ᱱs፵ᙷ᭹ቻᵽꊁﾇꒉ", num), smethod_1(), Enum15.const_1, this.buttonBehaviour_0);
                    this.wizardPageInfo_0.method_10(true);
                    this.wizardPageInfo_0.method_12(string.Format(class55_0.method_3().MaintenanceUrl, class55_0.method_3().ActivationKey));
                    return;

                default:
                    return;
            }
            if (class55_0.method_3().ExpirationDate.HasValue)
            {
                DateTime local1 = class55_0.method_3().ExpirationDate.Value;
                str = str + string.Format(ActivateTrialCompletedEventArgs.smethod_0("累匱崳堵䰷弹刻弽⸿⅁⅃晅㡇⽉㹋❍㽏㙑瑓㍕⁗⩙㕛ⱝ՟ᅡ䑣॥٧䩩ᝫ幭൯山祳籵", num), class55_0.method_3().ExpirationDate.Value.ToString(this.string_3));
                if (class55_0.method_3().DaysToExpiration < 40)
                {
                    str = str + ActivateTrialCompletedEventArgs.smethod_0("猯帱崳唵匷ᨹ医倽怿⹁ⵃ⡅⍇橉⹋⭍㱏㵑⍓癕ⱗ㕙籛ⱝ՟ౡţᅥ䡧❩൫ݭṯٱᅳᡵ᥷ᑹύ᭽ꁿꚇ", num);
                }
                if (this.wizardPageInfo_0 == null)
                {
                    this.wizardPageInfo_0 = new WizardPageInfo();
                }
            }
            this.method_14(str, smethod_1(), Enum15.const_0, this.buttonBehaviour_1);
            if (class55_0.method_3().ExpirationDate.HasValue && (class55_0.method_3().DaysToExpiration < 40))
            {
                this.wizardPageInfo_0.method_10(true);
                this.wizardPageInfo_0.method_12(string.Format(class55_0.method_3().MaintenanceUrl, class55_0.method_3().ActivationKey));
            }
            this.bool_0 = true;
            this.bool_1 = true;
        }
    }

    private void method_14(string string_7, string string_8, Enum15 enum15_0, ButtonBehaviour buttonBehaviour_3)
    {
        if (this.wizardPageInfo_0 == null)
        {
            this.wizardPageInfo_0 = new WizardPageInfo();
        }
        this.wizardPageInfo_0.method_1(string_8);
        this.wizardPageInfo_0.method_13(string_7);
        this.wizardPageInfo_0.method_15(enum15_0);
        this.method_9(this.wizardPageInfo_0);
        this.method_11(buttonBehaviour_3);
        this.method_10();
    }

    private void method_15(string string_7, string string_8)
    {
        if (this.wizardPageError_0 == null)
        {
            this.wizardPageError_0 = new WizardPageError();
        }
        this.wizardPageError_0.method_8(string_7);
        this.wizardPageError_0.method_1(string_8);
        this.method_9(this.wizardPageError_0);
    }

    public void method_16()
    {
        int num = 13;
        try
        {
            string_5 = new Class53().method_7();
            this.string_6 = new Class53().method_5();
        }
        catch (Exception exception)
        {
            MessageBox.Show(ActivateTrialCompletedEventArgs.smethod_0("永唺尼崾ⵀ♂敄㍆♈歊⥌⩎═㙒❔㩖じ㕚㡜罞ㅠ⁢䕤Ⅶhժ੬੮ͰͲݴṶ᝸ེ卼牾讀", num) + exception.Message, ActivateTrialCompletedEventArgs.smethod_0("簸䤺似倾㍀", num), MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Environment.Exit(1);
        }
        class59_0 = new Class59(string_5);
        class59_0.method_17();
    }

    private void method_17()
    {
        this.class57_3 = new Class57();
        this.class57_2 = new Class57();
        this.class57_1 = new Class57();
        this.class57_0 = new Class57();
        this.class58_0 = new Class58();
        this.label_0 = new Label();
        this.class57_4 = new Class57();
        this.class58_0.SuspendLayout();
        base.SuspendLayout();
        this.class57_3.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.class57_3.method_1(ButtonBehaviour.Cancel);
        this.class57_3.Location = new Point(0xa4, 0xc6);
        this.class57_3.Name = ActivateTrialCompletedEventArgs.smethod_0("崾㕀ⵂل♆❈⡊⡌⍎", 0x13);
        this.class57_3.Size = new Size(0x4b, 0x17);
        this.class57_3.TabIndex = 3;
        this.class57_3.Text = ActivateTrialCompletedEventArgs.smethod_0("簾⁀ⵂ♄≆╈", 0x13);
        this.class57_3.UseVisualStyleBackColor = true;
        this.class57_3.Click += new EventHandler(this.class57_3_Click);
        this.class57_2.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.class57_2.method_1(ButtonBehaviour.Previous);
        this.class57_2.Location = new Point(0xf5, 0xc6);
        this.class57_2.Name = ActivateTrialCompletedEventArgs.smethod_0("崾㕀ⵂᕄ㕆ⱈ㵊⑌⁎⑐⁒", 0x13);
        this.class57_2.Size = new Size(0x4b, 0x17);
        this.class57_2.TabIndex = 2;
        this.class57_2.Text = ActivateTrialCompletedEventArgs.smethod_0("̾汀捂ᕄ㕆ⱈ㵊⑌⁎⑐⁒", 0x13);
        this.class57_2.UseVisualStyleBackColor = true;
        this.class57_2.Click += new EventHandler(this.class57_2_Click);
        this.class57_1.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.class57_1.method_1(ButtonBehaviour.Next);
        this.class57_1.Location = new Point(0x146, 0xc6);
        this.class57_1.Name = ActivateTrialCompletedEventArgs.smethod_0("崾㕀ⵂୄ≆ㅈ㽊", 0x13);
        this.class57_1.Size = new Size(0x4b, 0x17);
        this.class57_1.TabIndex = 1;
        this.class57_1.Text = ActivateTrialCompletedEventArgs.smethod_0("焾⑀㭂ㅄ杆摈畊", 0x13);
        this.class57_1.UseVisualStyleBackColor = true;
        this.class57_1.Click += new EventHandler(this.class57_1_Click);
        this.class57_0.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.class57_0.method_1(ButtonBehaviour.Finish);
        this.class57_0.Location = new Point(0x197, 0xc6);
        this.class57_0.Name = ActivateTrialCompletedEventArgs.smethod_0("崾㕀ⵂ̈́⹆❈≊㹌❎", 0x13);
        this.class57_0.Size = new Size(0x4b, 0x17);
        this.class57_0.TabIndex = 0;
        this.class57_0.Text = ActivateTrialCompletedEventArgs.smethod_0("社⡀ⵂⱄ㑆ⅈ", 0x13);
        this.class57_0.UseVisualStyleBackColor = true;
        this.class57_0.Click += new EventHandler(this.class57_0_Click);
        this.class58_0.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.class58_0.BackColor = SystemColors.Window;
        this.class58_0.method_6(true);
        this.class58_0.method_4(Color.FromArgb(0x7f, 0x9d, 0xb9));
        this.class58_0.method_10(false);
        this.class58_0.method_8(false);
        this.class58_0.method_12(false);
        this.class58_0.method_2(2f);
        this.class58_0.Controls.Add(this.label_0);
        this.class58_0.Location = new Point(0, 0);
        this.class58_0.Name = ActivateTrialCompletedEventArgs.smethod_0("娾㥀㝂ᕄ♆❈⹊⅌繎", 0x13);
        this.class58_0.Size = new Size(0x1ee, 0x24);
        this.class58_0.TabIndex = 0;
        this.label_0.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
        this.label_0.Font = new Font(ActivateTrialCompletedEventArgs.smethod_0("爾⡀⁂㝄⡆㩈⑊⭌㭎煐R㑔㥖⩘筚๜㩞፠੢ͤ", 0x13), 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0xcc);
        this.label_0.Location = new Point(0, 2);
        this.label_0.Name = ActivateTrialCompletedEventArgs.smethod_0("匾⍀⽂ᕄ♆⹈⹊͌⹎㱐㙒", 0x13);
        this.label_0.Size = new Size(0x1f0, 0x20);
        this.label_0.TabIndex = 0;
        this.label_0.Text = ActivateTrialCompletedEventArgs.smethod_0("匾⍀⽂ᕄ♆⹈⹊͌⹎㱐㙒", 0x13);
        this.label_0.TextAlign = ContentAlignment.MiddleCenter;
        this.class57_4.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
        this.class57_4.method_1(ButtonBehaviour.Cancel);
        this.class57_4.Location = new Point(12, 0xc6);
        this.class57_4.Name = ActivateTrialCompletedEventArgs.smethod_0("崾㕀ⵂൄ≆╈㭊", 0x13);
        this.class57_4.Size = new Size(0x4b, 0x17);
        this.class57_4.TabIndex = 4;
        this.class57_4.Text = ActivateTrialCompletedEventArgs.smethod_0("眾⑀⽂㕄", 0x13);
        this.class57_4.UseVisualStyleBackColor = true;
        this.class57_4.Click += new EventHandler(this.class57_4_Click);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.ClientSize = new Size(0x1ee, 0xe9);
        base.Controls.Add(this.class57_4);
        base.Controls.Add(this.class57_3);
        base.Controls.Add(this.class57_2);
        base.Controls.Add(this.class57_1);
        base.Controls.Add(this.class57_0);
        base.Controls.Add(this.class58_0);
        base.FormBorderStyle = FormBorderStyle.FixedDialog;
        base.KeyPreview = true;
        base.MaximizeBox = false;
        base.MinimizeBox = false;
        base.Name = ActivateTrialCompletedEventArgs.smethod_0("栾⡀㥂⑄㕆ⵈൊ≌㵎㱐", 0x13);
        base.StartPosition = FormStartPosition.CenterScreen;
        this.Text = ActivateTrialCompletedEventArgs.smethod_0("績≀㝂ⱄㅆ⡈㽊⑌⁎㽐獒ɔ㹖⍘㩚⽜㭞", 0x13);
        this.class58_0.ResumeLayout(false);
        base.ResumeLayout(false);
    }

    public string method_2()
    {
        return this.string_4;
    }

    private void method_3(object sender, EventArgs14 e)
    {
        int num = 9;
        StringBuilder builder = new StringBuilder();
        builder.Append(this.method_4());
        builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("琴䈶䴸区ᴼ爾⹀❂⁄絆楈お経㉎", 9), wizardFormMode_0.ToString()));
        if (this.label_0.Text == smethod_1())
        {
            builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("猴帶䬸䠺䤼Ἶཀ≂⡄≆獈歊㙌罎ⱐ", num), this.wizardPageActivateKey_0.method_6()));
            builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("礴嘶䨸伺ᴼ焾⁀⹂⁄絆楈お経㉎", num), this.wizardPageActivateKey_0.method_8()));
            builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("琴吶䴸刺䬼帾㕀⩂⩄⥆楈J⡌㙎歐獒⹔杖⑘", num), this.wizardPageActivateKey_0.method_10()));
            builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("攴琶ᤸ紺吼儾♀♂㝄㝆㭈≊⍌㭎歐獒⹔杖⑘", num), string_5));
            builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("攴琶ᤸ町尼刾⑀祂敄㱆祈㙊", num), this.wizardPageHwidToRegister_0.method_6()));
            builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("瀴䔶䬸吺似Ծ慀㡂畄㩆", num), e.string_0));
        }
        if (this.label_0.Text == smethod_2())
        {
            builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("怴䐶尸䤺ᴼ焾⁀⹂⁄絆楈お経㉎", num), this.wizardPageActivateTrial_0.method_6()));
            builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("攴嘶䨸䠺䨼倾㍀❂罄杆㉈筊が", num), this.wizardPageActivateTrial_0.method_7()));
            builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("瀴䔶䬸吺似Ծ慀㡂畄㩆", num), e.string_0));
        }
        try
        {
            Clipboard.SetText(builder.ToString());
        }
        catch
        {
        }
    }

    private string method_4()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("猻洽稿扁㽃癅㕇", 0x10), Environment.OSVersion));
        builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("椻樽̿硁摃㵅硇㝉", 0x10), DateTime.UtcNow.ToString(ActivateTrialCompletedEventArgs.smethod_0("医", 0x10))));
        builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("缻儽ⴿ⽁╃⡅ⱇ橉K❍㹏㝑湓癕⍗橙⅛", 0x10), Environment.CommandLine));
        builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("氻䰽⼿♁ㅃ╅㱇橉ɋ⽍㵏㝑湓癕⍗橙⅛", 0x10), WealthLabAuthProvider.ProductName));
        builder.AppendLine(string.Format(ActivateTrialCompletedEventArgs.smethod_0("樻嬽㈿ㅁⵃ⥅♇灉汋㕍恏⽑", 0x10), WealthLabAuthProvider.Version));
        return builder.ToString();
    }

    public bool method_5()
    {
        return this.bool_0;
    }

    public bool method_6()
    {
        bool flag;
        using (IEnumerator enumerator = Application.OpenForms.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                Form current = (Form) enumerator.Current;
                if (current.Text.StartsWith(WealthLabAuthProvider.FullProductName))
                {
                    goto Label_0036;
                }
            }
            return false;
        Label_0036:
            flag = true;
        }
        return flag;
    }

    public void method_7()
    {
        if (wizardFormMode_0 == WizardFormMode.FirstStart)
        {
            this.wizardPageSelectActivationMethod_0 = new WizardPageSelectActivationMethod();
            this.method_9(this.wizardPageSelectActivationMethod_0);
        }
        else if (this.method_6())
        {
            if (class59_0.method_10())
            {
                if (this.wizardPageSelectActivationMethod_0 == null)
                {
                    this.wizardPageSelectActivationMethod_0 = new WizardPageSelectActivationMethod();
                }
                this.method_9(this.wizardPageSelectActivationMethod_0);
            }
            else
            {
                this.wizardPageAuthInfo_0 = new WizardPageAuthInfo();
                this.method_9(this.wizardPageAuthInfo_0);
            }
        }
        else
        {
            this.bool_2 = true;
            this.wizardPageActivateKey_0 = new WizardPageActivateKey();
            this.method_9(this.wizardPageActivateKey_0);
        }
    }

    private void method_8(WizardPage wizardPage_1)
    {
        wizardPage_1.Parent = null;
    }

    private void method_9(WizardPage wizardPage_1)
    {
        wizardPage_1.Parent = this;
        wizardPage_1.Location = new Point(0, 0x2d);
        wizardPage_1.Visible = true;
        Interface0 interface2 = wizardPage_1 as Interface0;
        this.label_0.Text = interface2.imethod_0();
        this.method_11(interface2.imethod_1());
        if (this.wizardPage_0 != null)
        {
            this.method_8(this.wizardPage_0);
        }
        this.wizardPage_0 = wizardPage_1;
        if ((this.wizardPage_0 is WizardPageSelectActivationMethod) && (wizardFormMode_0 == WizardFormMode.FromApplication))
        {
            this.class57_2.Enabled = true;
        }
        if (((this.wizardPage_0 is WizardPageSelectActivationMethod) && (wizardFormMode_0 == WizardFormMode.FromApplication)) && class59_0.method_10())
        {
            this.class57_2.Enabled = false;
        }
        if ((this.wizardPage_0 is WizardPageActivateKey) && this.bool_2)
        {
            this.class57_2.Enabled = false;
        }
        this.method_10();
        Application.DoEvents();
        this.wizardPage_0.vmethod_1();
    }

    public static Class59 smethod_0()
    {
        return class59_0;
    }

    public static string smethod_1()
    {
        int num = 9;
        if (wizardFormMode_0 == WizardFormMode.FirstStart)
        {
            return ActivateTrialCompletedEventArgs.smethod_0("琴吶䴸刺䬼帾㕀♂敄㍆ⅈ⹊浌⹎⅐⍒㥔㹖㩘㩚⥜㙞๠ൢ䕤ၦhὪլ佮ၰ卲㹴ቶx", num);
        }
        return ActivateTrialCompletedEventArgs.smethod_0("琴䈶䴸区堼儾㕀⩂♄♆㵈⹊浌㡎㡐❒㵔睖㡘筚ᙜ㩞ᡠ", num);
    }

    public static string smethod_2()
    {
        int num = 5;
        if (wizardFormMode_0 == WizardFormMode.FirstStart)
        {
            return ActivateTrialCompletedEventArgs.smethod_0("挰嘲刴帶䨸伺堼䴾慀╂⩄㕆楈⩊浌籎慐獒ㅔ㙖⁘筚⥜ⵞࡠɢ।", num);
        }
        return ActivateTrialCompletedEventArgs.smethod_0("瀰䘲䄴弶尸唺䤼嘾≀≂ㅄ≆楈㱊⑌㭎㥐獒⅔╖じ㩚ㅜ罞ɠᅢdͦ౨ժᥬٮၰὲٴ", num);
    }

    public static string smethod_3()
    {
        return (smethod_8() + ActivateTrialCompletedEventArgs.smethod_0("ᘵ嬷嬹刻ḽ⸿ⵁぃ晅⩇⽉汋㹍㕏⁑㉓㥕⩗㝙㥛㩝也潡湣", 10));
    }

    public static string smethod_4()
    {
        return string_5;
    }

    public static void smethod_5(string string_7)
    {
        string_5 = string_7;
    }

    public static WizardFormMode smethod_6()
    {
        return wizardFormMode_0;
    }

    public static void smethod_7(WizardFormMode wizardFormMode_1)
    {
        wizardFormMode_0 = wizardFormMode_1;
    }

    public static string smethod_8()
    {
        int num = 4;
        if (wizardFormMode_0 == WizardFormMode.FirstStart)
        {
            return ActivateTrialCompletedEventArgs.smethod_0("焯儱䀳張丷嬹䠻圽⼿ⱁ", num);
        }
        return ActivateTrialCompletedEventArgs.smethod_0("焯䜱䀳帵崷吹䠻圽⌿⍁ぃ⽅❇⑉", num);
    }

    void Form.Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }
}

