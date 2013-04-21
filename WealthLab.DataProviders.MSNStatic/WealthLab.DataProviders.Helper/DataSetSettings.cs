using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
namespace WealthLab.DataProviders.Helper
{
	[Serializable]
	public abstract class DataSetSettings
	{
		public string SerializeToString()
		{
			SoapFormatter soapFormatter = new SoapFormatter();
			string text = string.Empty;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				soapFormatter.Serialize(memoryStream, this);
				using (StreamReader streamReader = new StreamReader(memoryStream))
				{
					memoryStream.Seek(0L, SeekOrigin.Begin);
					text = streamReader.ReadToEnd();
				}
			}
			text = text.Replace('<', '{');
			text = text.Replace('>', '}');
			return text;
		}
		public void SerializeToFile(string fileName)
		{
			SoapFormatter soapFormatter = new SoapFormatter();
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
			{
				soapFormatter.Serialize(fileStream, this);
			}
		}
		public static object DeserializeFromFile(string fileName)
		{
			SoapFormatter soapFormatter = new SoapFormatter();
			object result = null;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
			{
				result = soapFormatter.Deserialize(fileStream);
			}
			return result;
		}
		public static object DeserializeFromString(string soapString)
		{
			soapString = soapString.Replace('{', '<');
			soapString = soapString.Replace('}', '>');
			SoapFormatter soapFormatter = new SoapFormatter();
			object result = null;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (StreamWriter streamWriter = new StreamWriter(memoryStream))
				{
					streamWriter.Write(soapString);
					streamWriter.Flush();
					memoryStream.Seek(0L, SeekOrigin.Begin);
					result = soapFormatter.Deserialize(memoryStream);
				}
			}
			return result;
		}
	}
}
