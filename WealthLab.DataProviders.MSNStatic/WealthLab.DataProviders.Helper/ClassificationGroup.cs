using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
namespace WealthLab.DataProviders.Helper
{
	[XmlRoot(ElementName = "CG")]
	public class ClassificationGroup
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name;
		[XmlAttribute(AttributeName = "id")]
		public string ID;
		[XmlAttribute(AttributeName = "type")]
		public string Type;
		[XmlIgnore]
		public string URL;
		[XmlAttribute(AttributeName = "func")]
		public int Func;
		[XmlAttribute(AttributeName = "update")]
		public DateTime Update;
		[XmlAttribute(AttributeName = "count")]
		public string SymbolsCount;
		[XmlElement(ElementName = "CG")]
		public List<ClassificationGroup> Groups = new List<ClassificationGroup>();
		public string Symbols;
		public ClassificationGroup()
		{
		}
		public ClassificationGroup(string name, string id, string type)
		{
			this.Name = name;
			this.ID = id;
			this.Type = type;
		}
		public ClassificationGroup(string name, string id, string type, string url, int func) : this(name, id, type)
		{
			this.URL = url;
			this.Func = func;
		}
		public void Serealize(string fileName)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ClassificationGroup));
			using (TextWriter textWriter = new StreamWriter(fileName))
			{
				xmlSerializer.Serialize(textWriter, this);
			}
		}
		public static ClassificationGroup Deserealize(string fileName)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ClassificationGroup));
			ClassificationGroup result = null;
			using (TextReader textReader = new StreamReader(fileName))
			{
				result = (ClassificationGroup)xmlSerializer.Deserialize(textReader);
			}
			return result;
		}
	}
}
