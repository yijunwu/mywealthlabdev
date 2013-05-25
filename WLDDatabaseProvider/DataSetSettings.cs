namespace WLDDatabaseProvider
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Soap;

    [Serializable]
    public abstract class DataSetSettings
    {
        protected DataSetSettings()
        {
        }

        public static object DeserializeFromFile(string fileName)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                return formatter.Deserialize(stream);
            }
        }

        public static object DeserializeFromString(string soapString)
        {
            soapString = soapString.Replace('{', '<');
            soapString = soapString.Replace('}', '>');
            SoapFormatter formatter = new SoapFormatter();
            object obj2 = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(soapString);
                    writer.Flush();
                    stream.Seek(0L, SeekOrigin.Begin);
                    obj2 = formatter.Deserialize(stream);
                }
            }
            return obj2;
        }

        public void SerializeToFile(string fileName)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(stream, this);
            }
        }

        public string SerializeToString()
        {
            SoapFormatter formatter = new SoapFormatter();
            string str = string.Empty;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                using (StreamReader reader = new StreamReader(stream))
                {
                    stream.Seek(0L, SeekOrigin.Begin);
                    str = reader.ReadToEnd();
                }
            }
            return str.Replace('<', '{').Replace('>', '}');
        }
    }
}

