namespace WLDDatabaseProvider
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    public class DBProviderSettings : DataSetSettings
    {
        private bool dividendAdj;
        private bool splitAdj = true;
        private bool alwaysPartialBar;
        private bool neverPerformOnDemandUpdates;
        private byte[] byte_0 = new byte[] { 
            0x68, 2, 0xcc, 0x37, 0x47, 0x54, 0x23, 9, 0x57, 0x41, 0x34, 0x93, 0x5b, 0x16, 0x29, 100, 
            220, 0xcc, 0x12, 0xd6, 0x4a, 0xfe, 0xb1, 0x9a, 0xd9, 0x40, 0x39, 0x31, 0x63, 0x36, 0x29, 0xc4
         };
        private byte[] byte_1 = new byte[] { 0x3f, 0x30, 0x25, 0x73, 220, 0xb6, 0xa3, 0x36, 0x25, 0x37, 11, 0xbc, 0x3b, 0x4d, 0x7a, 0x40 };
        private int threadCount = 10;
        private int attemptCount = 5;

        private string user = string.Empty;
        private string password = string.Empty;
        private static string settingFolder;
        public int Version = 2;

        private string connStr = string.Empty;

        private string queryStr = string.Empty;
        private string dateCol = "date";
        private string symbolCol = "symbol";

        private string openCol = "open";
        private string highCol = "open";
        private string lowCol = "low";
        private string closeCol = "close";
        private string volumeCol = "volume";

        public string DateCol
        {
            get { return dateCol; }
            set { dateCol = value; }
        }

        public string SymbolCol
        {
            get { return symbolCol; }
            set { symbolCol = value; }
        }

        public string ConnStr
        {
            get { return connStr; }
            set { connStr = value; }
        }

        public string QueryStr
        {
            get { return queryStr; }
            set { queryStr = value; }
        }
        public string OpenCol
        {
            get { return openCol; }
            set { openCol = value; }
        }
        public string HighCol
        {
            get { return highCol; }
            set { highCol = value; }
        }

        public string LowCol
        {
            get { return lowCol; }
            set { lowCol = value; }
        }
        public string CloseCol
        {
            get { return closeCol; }
            set { closeCol = value; }
        }
        public string VolumeCol
        {
            get { return volumeCol; }
            set { volumeCol = value; }
        }

        public static DBProviderSettings Deserialize(string folderName)
        {
            settingFolder = folderName;
            DBProviderSettings settings = new DBProviderSettings();
            if (!File.Exists(getSettingsFilePath(folderName)))
            {
                return settings;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(DBProviderSettings));
            using (TextReader reader = new StreamReader(getSettingsFilePath(folderName)))
            {
                return (DBProviderSettings) serializer.Deserialize(reader);
            }
        }

        public void Serialize()
        {
            if (settingFolder != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DBProviderSettings));
                using (TextWriter writer = new StreamWriter(settingFolder + Path.DirectorySeparatorChar + "YahooClientSettings.xml"))
                {
                    serializer.Serialize(writer, this);
                }
            }
        }

        private static string getSettingsFilePath(string dirPath)
        {
            return (dirPath + Path.DirectorySeparatorChar + "YahooClientSettings.xml");
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("ThreadCount " + this.threadCount);
            builder.AppendLine("AttemptCount " + this.attemptCount);
            builder.AppendLine("DividendAdj " + this.dividendAdj);
            builder.AppendLine("SplitAdj " + this.splitAdj);
            builder.AppendLine("PartialBar " + this.alwaysPartialBar);
            builder.AppendLine("NeverPerformOnDemandUpdates " + this.neverPerformOnDemandUpdates);
            if ((this.user != null) && (this.user.Length > 0))
            {
                builder.AppendLine("Login Yes");
            }
            if ((this.password != null) && (this.password.Length > 0))
            {
                builder.AppendLine("Password Yes");
            }
            return builder.ToString();
        }

        public bool AlwaysPartialBar
        {
            get
            {
                return this.alwaysPartialBar;
            }
            set
            {
                this.alwaysPartialBar = value;
            }
        }

        public int AttemptCount
        {
            get
            {
                return this.attemptCount;
            }
            set
            {
                this.attemptCount = value;
            }
        }

        public bool DividendAdj
        {
            get
            {
                return this.dividendAdj;
            }
            set
            {
                this.dividendAdj = value;
            }
        }

        public string Login
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }

        public bool NeverPerformOnDemandUpdates
        {
            get
            {
                return this.neverPerformOnDemandUpdates;
            }
            set
            {
                this.neverPerformOnDemandUpdates = value;
            }
        }

        [XmlIgnore]
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        public bool SplitAdj
        {
            get
            {
                return this.splitAdj;
            }
            set
            {
                this.splitAdj = value;
            }
        }

        public int ThreadCount
        {
            get
            {
                return this.threadCount;
            }
            set
            {
                this.threadCount = value;
            }
        }

        private DateTime _startDate = new DateTime(0x7d0, 1, 1);
        private string _symbols = string.Empty;
        private bool _updateGroups;
        //public int Version = 2;


        public DateTime StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }

        public string Symbols
        {
            get
            {
                return this._symbols;
            }
            set
            {
                this._symbols = value;
            }
        }

    }
}

