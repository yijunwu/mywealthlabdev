namespace WealthLab.DataProviders.Helper
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Soap;

    [Serializable]
    internal abstract class DataSetSettings
    {
        protected DataSetSettings()
        {
        }

        public string method_0()
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

        public void method_1(string string_0)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (FileStream stream = new FileStream(string_0, FileMode.Create))
            {
                formatter.Serialize(stream, this);
            }
        }

        public static object smethod_0(string string_0)
        {
            SoapFormatter formatter = new SoapFormatter();
            using (FileStream stream = new FileStream(string_0, FileMode.Open))
            {
                return formatter.Deserialize(stream);
            }
        }

        public static object smethod_1(string string_0)
        {
            string_0 = string_0.Replace('{', '<');
            string_0 = string_0.Replace('}', '>');
            SoapFormatter formatter = new SoapFormatter();
            object obj2 = null;
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(string_0);
                    writer.Flush();
                    stream.Seek(0L, SeekOrigin.Begin);
                    obj2 = formatter.Deserialize(stream);
                }
            }
            return obj2;
        }
    }
}

