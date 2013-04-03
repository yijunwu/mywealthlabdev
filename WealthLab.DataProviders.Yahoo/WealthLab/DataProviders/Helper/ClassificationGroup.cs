namespace WealthLab.DataProviders.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    [XmlRoot(ElementName="CG")]
    public class ClassificationGroup
    {
        [XmlAttribute(AttributeName="func")]
        public int Func;
        [XmlElement(ElementName="CG")]
        public List<ClassificationGroup> Groups;
        [XmlAttribute(AttributeName="id")]
        public string ID;
        [XmlAttribute(AttributeName="name")]
        public string Name;
        public string Symbols;
        [XmlAttribute(AttributeName="count")]
        public string SymbolsCount;
        [XmlAttribute(AttributeName="type")]
        public string Type;
        [XmlAttribute(AttributeName="update")]
        public DateTime Update;
        [XmlIgnore]
        public string URL;

        public ClassificationGroup()
        {
            this.Groups = new List<ClassificationGroup>();
        }

        public ClassificationGroup(string name, string string_0, string type)
        {
            this.Groups = new List<ClassificationGroup>();
            this.Name = name;
            this.ID = string_0;
            this.Type = type;
        }

        public ClassificationGroup(string name, string string_0, string type, string string_1, int func) : this(name, string_0, type)
        {
            this.URL = string_1;
            this.Func = func;
        }

        public static ClassificationGroup Deserealize(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClassificationGroup));
            using (TextReader reader = new StreamReader(fileName))
            {
                return (ClassificationGroup) serializer.Deserialize(reader);
            }
        }

        public void Serealize(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClassificationGroup));
            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}

