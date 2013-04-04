namespace MS123.Web.Strategies
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XmlSerializer
    {
        public static object FromXml(string Xml, Type ObjType)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(ObjType);
            StringReader input = new StringReader(Xml);
            XmlTextReader xmlReader = new XmlTextReader(input);
            object obj2 = serializer.Deserialize(xmlReader);
            xmlReader.Close();
            input.Close();
            return obj2;
        }

        private static XmlSerializerNamespaces GetNamespaces()
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            return namespaces;
        }

        public static string ToXml(object Obj, Type ObjType)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(ObjType);
            MemoryStream w = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(w, Encoding.UTF8) {
                Namespaces = true
            };
            serializer.Serialize(xmlWriter, Obj, GetNamespaces());
            xmlWriter.Close();
            w.Close();
            string str = Encoding.UTF8.GetString(w.GetBuffer());
            str = str.Substring(str.IndexOf(Convert.ToChar(60)));
            return str.Substring(0, str.LastIndexOf(Convert.ToChar(0x3e)) + 1);
        }
    }
}

