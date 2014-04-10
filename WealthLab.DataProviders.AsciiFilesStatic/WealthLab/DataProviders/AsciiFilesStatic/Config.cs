namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class Config
    {
        private bool enableCache;
        private List<AsciiCache> asciiCacheList = new List<AsciiCache>();
        private static string configFilePath = string.Empty;
        private static string cachePath = string.Empty;

        static Config()
        {
            smethod_0();
        }

        public static Config Desereailize()
        {
            Config config;
            if (!File.Exists(configFilePath))
            {
                return new Config();
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            object obj2 = null;
            try
            {
                using (FileStream stream = new FileStream(configFilePath, FileMode.Open))
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
            if (!Directory.Exists(Path.GetDirectoryName(configFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(configFilePath));
            }
            using (FileStream stream = new FileStream(configFilePath, FileMode.Create))
            {
                serializer.Serialize((Stream) stream, this);
            }
        }

        private static void smethod_0()
        {
            configFilePath = cachePath = Path.Combine(Application.UserAppDataPath, "Data");
            if (!Directory.Exists(configFilePath))
            {
                Directory.CreateDirectory(configFilePath);
            }
            configFilePath = Path.Combine(configFilePath, "AsciiConfig.xml");
            cachePath = Path.Combine(cachePath, "AsciiCache");
        }

        public List<AsciiCache> AsciiCacheList
        {
            get
            {
                return this.asciiCacheList;
            }
            set
            {
                this.asciiCacheList = value;
            }
        }

        public static string CachePath
        {
            get
            {
                return cachePath;
            }
        }

        public bool EnableCache
        {
            get
            {
                return this.enableCache;
            }
            set
            {
                this.enableCache = value;
            }
        }
    }
}

