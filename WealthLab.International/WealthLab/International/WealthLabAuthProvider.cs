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
                FindKeySeedForWealth();
                return string.Format(ActivateTrialCompletedEventArgs.smethod_0("Wealth-Lab Developer 6.4", 0x13), version_0.Major, version_0.Minor);
            }
        }

        public static void FindKeySeedForWealth()
        {
            string ciphertext = "栾⑀≂⥄㍆ⅈ晊Ō⹎㍐獒ᅔ㉖⽘㹚ㅜぞᅠ٢ᝤ䝦ቨ孪ၬ䅮ੰ䉲ࡴ";
            string crib = "Wealth"; // 已知明文中包含的子串

            Console.WriteLine($"Ciphertext: {ciphertext}");
            Console.WriteLine($"Searching for keySeed where plaintext contains: \"{crib}\"");
            Console.WriteLine("--------------------------------------------------");

            // 定义 keySeed 的搜索范围
            int minKeySeed = 0;
            int maxKeySeed = 20000; // 初始尝试范围，可以根据需要调整
            bool found = false;

            for (int currentSeedAttempt = minKeySeed; currentSeedAttempt <= maxKeySeed; currentSeedAttempt++)
            {
                string plaintext = ActivateTrialCompletedEventArgs.smethod_0(ciphertext, currentSeedAttempt);
                if (plaintext.Contains(crib))
                {
                    Console.WriteLine($"SUCCESS! Found matching keySeed: {currentSeedAttempt}");
                    Console.WriteLine($"Plaintext: {plaintext}");
                    Console.WriteLine("--------------------------------------------------");
                    found = true;
                    // 如果你只期望一个结果，可以在这里 break;
                    // break; 
                }

                // 可以每隔N次尝试打印一次进度，以防搜索时间过长
                if (currentSeedAttempt % 1000 == 0 && currentSeedAttempt != minKeySeed)
                {
                    Console.WriteLine($"... still searching, tried up to keySeed: {currentSeedAttempt}");
                }
            }

            // 如果在正数范围内没有找到，可以尝试负数范围
            if (!found)
            {
                Console.WriteLine($"Crib not found in positive range {minKeySeed} to {maxKeySeed}. Trying negative range...");
                minKeySeed = -maxKeySeed; // 例如 -20000
                maxKeySeed = -1;
                for (int currentSeedAttempt = maxKeySeed; currentSeedAttempt >= minKeySeed; currentSeedAttempt--)
                {
                    string plaintext = ActivateTrialCompletedEventArgs.smethod_0(ciphertext, currentSeedAttempt);
                    if (plaintext.Contains(crib))
                    {
                        Console.WriteLine($"SUCCESS! Found matching keySeed: {currentSeedAttempt}");
                        Console.WriteLine($"Plaintext: {plaintext}");
                        Console.WriteLine("--------------------------------------------------");
                        found = true;
                        // break;
                    }
                    if (currentSeedAttempt % 1000 == 0)
                    {
                        Console.WriteLine($"... still searching (negative), tried down to keySeed: {currentSeedAttempt}");
                    }
                }
            }


            if (!found)
            {
                Console.WriteLine($"Failed to find a keySeed that decrypts to a plaintext containing \"{crib}\" within the tested ranges.");
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

