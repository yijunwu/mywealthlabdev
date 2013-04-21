using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
namespace WealthLab.DataProviders.Msn
{
	public class MsnClientSettings
	{
		public int Version = 2;
		private int _threadCount = 10;
		private bool _dividendAdj;
		private int _attemptCount = 5;
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
		private static string GetFileName(string folderName)
		{
			return folderName + Path.DirectorySeparatorChar + "MsnClientSettings.xml";
		}
		public static MsnClientSettings Deserialize(string folderName)
		{
			MsnClientSettings result = new MsnClientSettings();
			if (File.Exists(MsnClientSettings.GetFileName(folderName)))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(MsnClientSettings));
				using (TextReader textReader = new StreamReader(MsnClientSettings.GetFileName(folderName)))
				{
					result = (MsnClientSettings)xmlSerializer.Deserialize(textReader);
				}
			}
			return result;
		}
		public void Serialize(string folderName)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(MsnClientSettings));
			using (TextWriter textWriter = new StreamWriter(folderName + Path.DirectorySeparatorChar + "MsnClientSettings.xml"))
			{
				xmlSerializer.Serialize(textWriter, this);
			}
		}
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ThreadCount " + this._threadCount);
			stringBuilder.AppendLine("AttemptCount " + this._attemptCount);
			stringBuilder.AppendLine("DividendAdj " + this._dividendAdj);
			return stringBuilder.ToString();
		}
	}
}
