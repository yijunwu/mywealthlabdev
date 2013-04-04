namespace MS123.Web.Strategies
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization.Formatters.Soap;

    [Serializable]
    public class StrategyHandler
    {
        public StrategyHandler(string strategyId, string folder, string strategyXml, DateTime enteredDate, DateTime approvedDate)
        {
            this.StrategyId = strategyId;
            this.Folder = folder;
            this.StrategyXML = strategyXml;
            this.EnteredDate = enteredDate;
            this.ApprovedDate = approvedDate;
        }

        public static StrategyHandler DeserializeFromString(string soapString)
        {
            SoapFormatter formatter = new SoapFormatter();
            MemoryStream stream = new MemoryStream();
            object obj2 = null;
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(soapString);
                writer.Flush();
                stream.Seek(0L, SeekOrigin.Begin);
                obj2 = formatter.Deserialize(stream);
            }
            return (StrategyHandler) obj2;
        }

        public string SerializeToString()
        {
            SoapFormatter formatter = new SoapFormatter();
            MemoryStream serializationStream = new MemoryStream();
            formatter.Serialize(serializationStream, this);
            StreamReader reader = new StreamReader(serializationStream);
            serializationStream.Seek(0L, SeekOrigin.Begin);
            string str = reader.ReadToEnd();
            reader.Close();
            return str;
        }

        public DateTime ApprovedDate { get; private set; }

        public DateTime EnteredDate { get; private set; }

        public string Folder { get; private set; }

        public string StrategyId { get; private set; }

        public string StrategyXML { get; private set; }
    }
}

