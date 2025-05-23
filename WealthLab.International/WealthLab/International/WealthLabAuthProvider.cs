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
        // Class52 class52_0 = new Class52();
        private ExtensionManager extensionManager_0;
        private static System.Version version_0 = Assembly.GetEntryAssembly().GetName().Version;

        public WealthLabAuthProvider()
        {
            //this.method_0();
        }

        public override bool Authenticate(ref int daysBeforeNextAuthRequired, ref string string_0)
        {
            if (true) { return true; }
           
        }

        public override void Initialize(IDataHost dataHost, IAuthenticationHost authHost)
        {
            base.Initialize(dataHost, authHost);
            authHost.AddMenuItem(ActivateTrialCompletedEventArgs.smethod_0("紷䈹䠻嬽⸿ㅁⵃ⥅♇橉ŋ⽍㹏㍑㍓㍕⩗", 12), ActivateTrialCompletedEventArgs.smethod_0("ḷ渹医儽ⰿㅁ", 12), ActivateTrialCompletedEventArgs.smethod_0("欷䌹儻尽⼿⹁摃恅Ň⑉⩋⅍灏ὑ㕓㡕㥗㵙㥛ⱝ", 12), new ClickMenuItem(this.method_6), Resources.package);
            this.extensionManager_0.CheckForUpdates(authHost);
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
                return string.Format(ActivateTrialCompletedEventArgs.smethod_0("Wealth-Lab Developer 6.4", 0x13), version_0.Major, version_0.Minor);
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
                return ActivateTrialCompletedEventArgs.smethod_0("Authenticate", 0x11);
            }
        }

        public virtual bool LoginButtonVisible
        {
            get
            {
                return false;
            }
        }

        public override string LoginPhrase
        {
            get
            {
                return ActivateTrialCompletedEventArgs.smethod_0("Authenticate", 13);
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
                return DateTime.Now.AddDays(10000);
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
                return string.Format(ActivateTrialCompletedEventArgs.smethod_0("Wealth-Lab Developer 6", 9), version_0.Major);
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

