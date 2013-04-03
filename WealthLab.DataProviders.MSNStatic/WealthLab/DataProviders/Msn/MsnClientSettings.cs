namespace WealthLab.DataProviders.Msn
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class MsnClientSettings
    {
        private int _attemptCount = 5;
        private bool _dividendAdj;
        private int _threadCount = 10;
        public int Version = 2;

        public static MsnClientSettings Deserialize(string folderName)
        {
            MsnClientSettings settings = new MsnClientSettings();
            if (!File.Exists(GetFileName(folderName)))
            {
                return settings;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(MsnClientSettings));
            using (TextReader reader = new StreamReader(GetFileName(folderName)))
            {
                return (MsnClientSettings) serializer.Deserialize(reader);
            }
        }

        private static string GetFileName(string folderName)
        {
            return (folderName + Path.DirectorySeparatorChar + "MsnClientSettings.xml");
        }

        public void Serialize(string folderName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MsnClientSettings));
            using (TextWriter writer = new StreamWriter(folderName + Path.DirectorySeparatorChar + "MsnClientSettings.xml"))
            {
                serializer.Serialize(writer, this);
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("ThreadCount " + this._threadCount);
            builder.AppendLine("AttemptCount " + this._attemptCount);
            builder.AppendLine("DividendAdj " + this._dividendAdj);
            return builder.ToString();
        }

        public int AttemptCount
        {
            get
            {
                return this._attemptCount;
            }
            set
            {
                this._attemptCount = value;
            }
        }

        public bool DividendAdj
        {
            get
            {
                return this._dividendAdj;
            }
            set
            {
                this._dividendAdj = value;
            }
        }

        public int ThreadCount
        {
            get
            {
                return this._threadCount;
            }
            set
            {
                this._threadCount = value;
            }
        }
    }
}

