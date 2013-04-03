namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class Config
    {
        private bool bool_0;
        private List<AsciiCache> list_0 = new List<AsciiCache>();
        private static string string_0 = string.Empty;
        private static string string_1 = string.Empty;

        static Config()
        {
            smethod_0();
        }

        public static Config Desereailize()
        {
            Config config;
            if (!File.Exists(string_0))
            {
                return new Config();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            object obj2 = null;
            try
            {
                using (FileStream stream = new FileStream(string_0, FileMode.Open))
                {
                    obj2 = serializer.Deserialize(stream);
                }
                return (Config) obj2;
            }
            catch
            {
                config = new Config();
            }
            return config;
        }

        public void Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            if (!Directory.Exists(Path.GetDirectoryName(string_0)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(string_0));
            }
            using (FileStream stream = new FileStream(string_0, FileMode.Create))
            {
                serializer.Serialize((Stream) stream, this);
            }
        }

        private static void smethod_0()
        {
            string_0 = string_1 = Path.Combine(Application.UserAppDataPath, "Data");
            if (!Directory.Exists(string_0))
            {
                Directory.CreateDirectory(string_0);
            }
            string_0 = Path.Combine(string_0, "AsciiConfig.xml");
            string_1 = Path.Combine(string_1, "AsciiCache");
        }

        public List<AsciiCache> AsciiCacheList
        {
            get
            {
                return this.list_0;
            }
            set
            {
                this.list_0 = value;
            }
        }

        public static string CachePath
        {
            get
            {
                return string_1;
            }
        }

        public bool EnableCache
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

