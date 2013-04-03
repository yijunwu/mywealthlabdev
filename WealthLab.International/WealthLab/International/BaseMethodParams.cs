namespace WealthLab.International
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using WealthLab.Cryptography;

    public class BaseMethodParams
    {
        private bool bool_0;
        private bool bool_1;
        private DateTime dateTime_0;
        private int int_0;
        private int int_1;
        private int int_2;
        private static readonly string string_0 = CustomersWebService.ActivateTrialCompletedEventArgs.smethod_0("฽฿A潃ⅅ᱇ⵉ㹋㭍㝏扑ὓ絕≗⁙՛൝䭟ॡ⍣卥㭧屩ᙫ䅭፯❱ᵳት⽷⩹䕻ѽꭿ첁첃\udf85\ube87\udb8f\ud791ꦓ", 0x12);
        private static readonly string string_1 = CustomersWebService.ActivateTrialCompletedEventArgs.smethod_0("栽㌿⥁獃⁅❇㥉Ὃⱍ⅏㵑打╕ⱗṙᅛ੝ɟᝡᙣ፥㥧坩八", 0x12);
        private string string_2;
        private string string_3;
        private string string_4;
        private string string_5;
        private string string_6;
        private string string_7;
        private string string_8;
        private string string_9;

        public static object Deserialize(string string_10)
        {
            string_10 = new RijndaelCryptography(Convert.FromBase64String(string_0), Convert.FromBase64String(string_1)).DecryptString(string_10);
            XmlSerializer serializer = new XmlSerializer(typeof(BaseMethodParams), new Type[] { typeof(TrialMethodParams), typeof(KeyMethodParams) });
            using (TextReader reader = new StringReader(string_10))
            {
                return serializer.Deserialize(reader);
            }
        }

        public string Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BaseMethodParams), new Type[] { typeof(TrialMethodParams), typeof(KeyMethodParams) });
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, this);
            }
            RijndaelCryptography cryptography = new RijndaelCryptography(Convert.FromBase64String(string_0), Convert.FromBase64String(string_1));
            return cryptography.EncryptString(sb.ToString());
        }

        public string SetMessageId()
        {
            this.string_5 = Guid.NewGuid().ToString();
            return this.string_5;
        }

        public string A
        {
            get
            {
                return this.string_9;
            }
            set
            {
                this.string_9 = value;
            }
        }

        public string APVersion
        {
            get
            {
                return this.string_6;
            }
            set
            {
                this.string_6 = value;
            }
        }

        public bool AuthenticatedRequest
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

        public int ConnectionId
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

        public DateTime CurrentDate
        {
            get
            {
                return this.dateTime_0;
            }
            set
            {
                this.dateTime_0 = value;
            }
        }

        public string Hwid
        {
            get
            {
                return this.string_3;
            }
            set
            {
                this.string_3 = value;
            }
        }

        public bool IsMaintenance
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

        public string MaintenanceMessage
        {
            get
            {
                return this.string_4;
            }
            set
            {
                this.string_4 = value;
            }
        }

        public string MessageId
        {
            get
            {
                return this.string_5;
            }
            set
            {
                this.string_5 = value;
            }
        }

        public string OSPlatform
        {
            get
            {
                return this.string_8;
            }
            set
            {
                this.string_8 = value;
            }
        }

        public string OSVersion
        {
            get
            {
                return this.string_7;
            }
            set
            {
                this.string_7 = value;
            }
        }

        public int ProductId
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

        public int Result
        {
            get
            {
                return this.int_2;
            }
            set
            {
                this.int_2 = value;
            }
        }

        public string Version
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }
    }
}

