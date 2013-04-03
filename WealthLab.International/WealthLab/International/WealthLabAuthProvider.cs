namespace WealthLab.International
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.Extensions;
    using WealthLab.International.CustomersWebService;
    using WealthLab.International.Properties;
    using System.Reflection;

    public class WealthLabAuthProvider : AuthenticationProvider
    {
        private Class52 class52_0 = new Class52();
        private ExtensionManager extensionManager_0;
        private static System.Version version_0 = Assembly.GetEntryAssembly().GetName().Version;

        public WealthLabAuthProvider()
        {
            this.method_0();
        }

        public override bool Authenticate(ref int daysBeforeNextAuthRequired, ref string string_0)
        {
            int num = 0x13;
            base.AllowStreaming = true;
            Form0 form = new Form0(false);
            form.method_16();
            form.method_7();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.method_5())
                {
                    base.LoggedIn = true;
                    if (form.method_0())
                    {
                        daysBeforeNextAuthRequired = 30;
                        this.method_1(30);
                    }
                    else
                    {
                        this.class52_0.method_13(0x5316656);
                        TimeSpan span = this.class52_0.method_4().Subtract(DateTime.Now);
                        daysBeforeNextAuthRequired = span.Days + 1;
                    }
                    return true;
                }
            }
            else if (form.method_2() == null)
            {
                string_0 = ActivateTrialCompletedEventArgs.smethod_0("簾⁀ⵂ♄≆╈⹊⥌潎㍐⩒畔≖⩘㹚⽜", num);
            }
            else
            {
                string_0 = form.method_2();
            }
            return false;
        }

        public override void Initialize(IDataHost dataHost, IAuthenticationHost authHost)
        {
            base.Initialize(dataHost, authHost);
            authHost.AddMenuItem(ActivateTrialCompletedEventArgs.smethod_0("紷䈹䠻嬽⸿ㅁⵃ⥅♇橉ŋ⽍㹏㍑㍓㍕⩗", 12), ActivateTrialCompletedEventArgs.smethod_0("ḷ渹医儽ⰿㅁ", 12), ActivateTrialCompletedEventArgs.smethod_0("欷䌹儻尽⼿⹁摃恅Ň⑉⩋⅍灏ὑ㕓㡕㥗㵙㥛ⱝ", 12), new ClickMenuItem(this.method_6), Resources.package);
            this.extensionManager_0.CheckForUpdates(authHost);
        }

        private void method_0()
        {
            int num = 0x11;
            new Class51().method_0();
            if (!this.class52_0.method_11())
            {
                if (this.method_5())
                {
                    DateTime time3 = DateTime.Now.AddDays((double) Convert.ToInt32(ActivateTrialCompletedEventArgs.smethod_0("ြ฾", num)));
                    if (this.method_4() != DateTime.MinValue)
                    {
                        if (DateTime.Now.AddDays((double) Convert.ToInt32(ActivateTrialCompletedEventArgs.smethod_0("฼ਾ", num))) > this.method_4())
                        {
                            time3 = this.method_4();
                        }
                    }
                    else if (DateTime.Now > this.method_3())
                    {
                        time3 = this.method_3().AddDays((double) Convert.ToInt32(ActivateTrialCompletedEventArgs.smethod_0("฼༾", num)));
                    }
                    this.method_2(time3);
                }
                else
                {
                    Form0 form = new Form0(true);
                    form.method_16();
                    form.method_7();
                    form.ShowDialog();
                    if (!form.method_5())
                    {
                        Environment.Exit(1);
                    }
                    this.method_1(Convert.ToInt32(ActivateTrialCompletedEventArgs.smethod_0("฼༾", num)));
                }
            }
        }

        private void method_1(int int_0)
        {
            this.class52_0.method_1(version_0.ToString(4));
            this.class52_0.method_3(DateTime.Now);
            this.class52_0.method_5(DateTime.Now.AddDays((double) int_0));
            this.class52_0.method_12(0x5316656);
        }

        private void method_2(DateTime dateTime_0)
        {
            this.class52_0.method_1(version_0.ToString(4));
            this.class52_0.method_3(DateTime.Now);
            this.class52_0.method_5(dateTime_0);
            this.class52_0.method_12(0x5316656);
        }

        private DateTime method_3()
        {
            DateTime minValue = DateTime.MinValue;
            string path = Path.Combine(Application.LocalUserAppDataPath, ActivateTrialCompletedEventArgs.smethod_0("崸䴺似ᄾ㕀㭂ㅄ", 13));
            if (File.Exists(path))
            {
                minValue = File.GetCreationTime(path);
            }
            return minValue;
        }

        private DateTime method_4()
        {
            int num = 0x11;
            DateTime minValue = DateTime.MinValue;
            string path = Path.Combine(Path.Combine(Application.UserAppDataPath, ActivateTrialCompletedEventArgs.smethod_0("礼帾㕀≂", 0x11)), ActivateTrialCompletedEventArgs.smethod_0("樼娾⁀⽂ㅄ⽆Ո⩊⽌౎㹐㵒㍔㹖㹘畚⥜❞ᕠ", 0x11));
            if (File.Exists(path))
            {
                string str2 = string.Empty;
                using (StreamReader reader = new StreamReader(path))
                {
                    str2 = reader.ReadToEnd();
                }
                if (str2.Contains(ActivateTrialCompletedEventArgs.smethod_0("猼紾ፀ繂", num)))
                {
                    int index = str2.IndexOf(ActivateTrialCompletedEventArgs.smethod_0("猼紾ፀ繂", num));
                    int num3 = str2.IndexOf(ActivateTrialCompletedEventArgs.smethod_0("〼", num), index);
                    minValue = DateTime.FromBinary(Convert.ToInt64(str2.Substring(index, num3 - index).Replace(ActivateTrialCompletedEventArgs.smethod_0("猼紾ፀ繂", num), "")));
                }
            }
            return minValue;
        }

        private bool method_5()
        {
            if (this.class52_0.method_11() || (!(this.method_3() != DateTime.MinValue) && !(this.method_4() != DateTime.MinValue)))
            {
                return false;
            }
            return true;
        }

        private void method_6(object sender, EventArgs e)
        {
            this.extensionManager_0.ClickHandler();
        }

        public override void PreInitialize()
        {
            base.PreInitialize();
            this.extensionManager_0 = new ExtensionManager();
            this.extensionManager_0.UpdateExtensions();
        }

        public override string ApplicationName
        {
            get
            {
                return FullProductName;
            }
        }

        public override string ApplicationVersion
        {
            get
            {
                return version_0.ToString(2);
            }
        }

        public static string FullProductName
        {
            get
            {
                return string.Format(ActivateTrialCompletedEventArgs.smethod_0("栾⑀≂⥄㍆ⅈ晊Ō⹎㍐獒ᅔ㉖⽘㹚ㅜぞᅠ٢ᝤ䝦ቨ孪ၬ䅮ੰ䉲ࡴ", 0x13), version_0.Major, version_0.Minor);
            }
        }

        public override DateTime GetCurrentDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.Bitmap_0;
            }
        }

        public override int GracePeriod
        {
            get
            {
                return 30;
            }
        }

        public override string LoggedInPhrase
        {
            get
            {
                return ActivateTrialCompletedEventArgs.smethod_0("簼䨾㕀⭂⁄⥆㵈≊⹌⹎═㙒ㅔ", 0x11);
            }
        }

        public override string LoginPhrase
        {
            get
            {
                return ActivateTrialCompletedEventArgs.smethod_0("砸为䤼圾⑀ⵂㅄ⹆⩈⩊㥌⩎", 13);
            }
        }

        public override string Name
        {
            get
            {
                return ActivateTrialCompletedEventArgs.smethod_0("洫䴭䐯嬱䈳圵䰷匹医倽怿ु⅃㽅", 0);
            }
        }

        public override DateTime NextAuthRequired
        {
            get
            {
                int num = 0x10;
                DateTime time2 = DateTime.Now.AddDays((double) Convert.ToInt32(ActivateTrialCompletedEventArgs.smethod_0("ᄻ༽", 0x10)));
                base.AllowStreaming = true;
                if (this.class52_0.method_11())
                {
                    this.class52_0.method_13(0x5316656);
                    if ((this.class52_0.method_0() == null) || !this.class52_0.method_0().Contains(ActivateTrialCompletedEventArgs.smethod_0("ሻ", num)))
                    {
                        return time2;
                    }
                    if ((DateTime.Now > this.class52_0.method_2().AddDays((double) Convert.ToInt32(ActivateTrialCompletedEventArgs.smethod_0("༻ଽ", num)))) || (DateTime.Now < this.class52_0.method_4().AddDays((double) Convert.ToInt32(ActivateTrialCompletedEventArgs.smethod_0("ᄻഽ甿", num)))))
                    {
                        return time2;
                    }
                    if (this.class52_0.method_4() <= this.class52_0.method_2())
                    {
                        return time2;
                    }
                    if ((this.class52_0.method_2() == DateTime.MinValue) || (this.class52_0.method_4() == DateTime.MinValue))
                    {
                        return time2;
                    }
                    if (!(this.class52_0.method_2() == DateTime.MaxValue) && !(this.class52_0.method_4() == DateTime.MaxValue))
                    {
                        return this.class52_0.method_4().Date;
                    }
                }
                return time2;
            }
        }

        public static int ProductId
        {
            get
            {
                return 8;
            }
        }

        public static string ProductName
        {
            get
            {
                return string.Format(ActivateTrialCompletedEventArgs.smethod_0("戴制堸场䤼圾汀ག⑄╆楈ཊ⡌㥎㑐㽒㩔❖㱘⥚絜⑞兠Ṣ", 9), version_0.Major);
            }
        }

        public override string ThirdPartySiteWarning
        {
            get
            {
                return "";
            }
        }

        public static System.Version Version
        {
            get
            {
                return version_0;
            }
        }

        public override string WhatsNewLink
        {
            get
            {
                return string.Format(ActivateTrialCompletedEventArgs.smethod_0("堯䘱䀳䘵ȷᔹጻ䤽ⰿ癁橃ㅅⵇ⭉⁋㩍㡏网㡓㝕㩗瑙㽛ㅝൟ䵡ݣťŧ䝩๫ݭṯ嵱⍳፵᥷ᙹࡻᙽ챿ꢅ첇욉삋ꆍ晴鍊龟솣풥\udba7\uc3a9쎫삭趯즱蒳쮵", 4), version_0.ToString(3));
            }
        }
    }
}

