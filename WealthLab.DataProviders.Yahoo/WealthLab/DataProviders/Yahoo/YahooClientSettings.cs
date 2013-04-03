namespace WealthLab.DataProviders.Yahoo
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using WealthLab.Cryptography;
    using WealthLab.DataProviders.Helper;

    public class YahooClientSettings
    {
        private AdjustedModeWhenDataRange adjustedModeWhenDataRange_0;
        private bool bool_0;
        private bool bool_1 = true;
        private bool bool_2;
        private bool bool_3;
        private byte[] byte_0 = new byte[] { 
            0x68, 2, 0xcc, 0x37, 0x47, 0x54, 0x23, 9, 0x57, 0x41, 0x34, 0x93, 0x5b, 0x16, 0x29, 100, 
            220, 0xcc, 0x12, 0xd6, 0x4a, 0xfe, 0xb1, 0x9a, 0xd9, 0x40, 0x39, 0x31, 0x63, 0x36, 0x29, 0xc4
         };
        private byte[] byte_1 = new byte[] { 0x3f, 0x30, 0x25, 0x73, 220, 0xb6, 0xa3, 0x36, 0x25, 0x37, 11, 0xbc, 0x3b, 0x4d, 0x7a, 0x40 };
        private int int_0 = 10;
        private int int_1 = 5;
        private string string_0 = string.Empty;
        private string string_1 = string.Empty;
        private static string string_2;
        public int Version = 2;

        public static YahooClientSettings Deserialize(string folderName)
        {
            string_2 = folderName;
            YahooClientSettings settings = new YahooClientSettings();
            if (!File.Exists(smethod_0(folderName)))
            {
                return settings;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(YahooClientSettings));
            using (TextReader reader = new StreamReader(smethod_0(folderName)))
            {
                return (YahooClientSettings) serializer.Deserialize(reader);
            }
        }

        public void Serialize()
        {
            if (string_2 != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(YahooClientSettings));
                using (TextWriter writer = new StreamWriter(string_2 + Path.DirectorySeparatorChar + "YahooClientSettings.xml"))
                {
                    serializer.Serialize(writer, this);
                }
            }
        }

        private static string smethod_0(string string_3)
        {
            return (string_3 + Path.DirectorySeparatorChar + "YahooClientSettings.xml");
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("ThreadCount " + this.int_0);
            builder.AppendLine("AttemptCount " + this.int_1);
            builder.AppendLine("DividendAdj " + this.bool_0);
            builder.AppendLine("SplitAdj " + this.bool_1);
            builder.AppendLine("PartialBar " + this.bool_2);
            builder.AppendLine("NeverPerformOnDemandUpdates " + this.bool_3);
            if ((this.string_0 != null) && (this.string_0.Length > 0))
            {
                builder.AppendLine("Login Yes");
            }
            if ((this.string_1 != null) && (this.string_1.Length > 0))
            {
                builder.AppendLine("Password Yes");
            }
            return builder.ToString();
        }

        public AdjustedModeWhenDataRange AdjModeWhenDataRange
        {
            get
            {
                return this.adjustedModeWhenDataRange_0;
            }
            set
            {
                this.adjustedModeWhenDataRange_0 = value;
            }
        }

        public bool AlwaysPartialBar
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public int AttemptCount
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }

        public bool DividendAdj
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

        public string EncryptPassword
        {
            get
            {
                RijndaelCryptography cryptography = new RijndaelCryptography(this.byte_0, this.byte_1);
                return cryptography.EncryptString(this.string_1);
            }
            set
            {
                this.string_1 = new RijndaelCryptography(this.byte_0, this.byte_1).DecryptString(value);
            }
        }

        public string Login
        {
            get
            {
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public bool NeverPerformOnDemandUpdates
        {
            get
            {
                return this.bool_3;
            }
            set
            {
                this.bool_3 = value;
            }
        }

        [XmlIgnore]
        public string Password
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

        public bool SplitAdj
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

        public int ThreadCount
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }
    }
}

