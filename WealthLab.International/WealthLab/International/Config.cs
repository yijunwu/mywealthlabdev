namespace WealthLab.International
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using WealthLab.International.CustomersWebService;

    public class Config
    {
        private bool bool_0;
        private bool bool_1;
        private static readonly string string_0 = ActivateTrialCompletedEventArgs.smethod_0("眽⸿㙁⅃㑅♇⭉㡋❍㽏㱑㕓㩕᭗㕙㉛㡝य़ա䩣ṥէ٩", 0x12);
        private string string_1;

        public static Config Desereailize()
        {
            if (!File.Exists(smethod_0()))
            {
                return new Config();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            object obj2 = null;
            using (FileStream stream = new FileStream(smethod_0(), FileMode.Open))
            {
                obj2 = serializer.Deserialize(stream);
            }
            return (Config) obj2;
        }

        public void Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            if (!Directory.Exists(Path.GetDirectoryName(smethod_0())))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(smethod_0()));
            }
            using (FileStream stream = new FileStream(smethod_0(), FileMode.Create))
            {
                serializer.Serialize((Stream) stream, this);
            }
        }

        private static string smethod_0()
        {
            return Path.Combine(Path.Combine(Application.UserAppDataPath, ActivateTrialCompletedEventArgs.smethod_0("爵夷丹崻", 10)), string_0);
        }

        public string Data
        {
            get
            {
                return this.string_1;
            }
            set
            {
                this.string_1 = value;
            }
        }

        public bool NotShowAuthInfo
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public bool RememberData
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }
    }
}

