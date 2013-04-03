namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public sealed class ArrayOfObjectSerializer1 : XmlSerializer1
    {
        public override bool CanDeserialize(XmlReader xmlReader)
        {
            return xmlReader.IsStartElement("GetPublicStrategiesResponse", "http://www.wealth-lab.com/WebServices/");
        }

        protected override object Deserialize(XmlSerializationReader reader)
        {
            return ((XmlSerializationReaderStrategyWebservice) reader).Read1_GetPublicStrategiesResponse();
        }
    }
}

