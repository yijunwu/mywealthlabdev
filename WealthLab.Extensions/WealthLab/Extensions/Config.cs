namespace WealthLab.Extensions
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class Config
    {
        private bool? nullable_0 = null;
        private static string string_0 = "ExtensionManagerConfig.xml";

        public static Config Deserialize()
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
            using (FileStream stream = new FileStream(smethod_0(), FileMode.Create))
            {
                serializer.Serialize((Stream) stream, this);
            }
        }

        private static string smethod_0()
        {
            return Path.Combine(Path.Combine(Application.UserAppDataPath, "Data"), string_0);
        }

        public bool? CheckUpdates
        {
            get
            {
                return this.nullable_0;
            }
            set
            {
                this.nullable_0 = value;
            }
        }
    }
}

