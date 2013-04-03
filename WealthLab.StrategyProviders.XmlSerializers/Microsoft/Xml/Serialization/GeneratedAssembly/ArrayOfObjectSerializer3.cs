namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public sealed class ArrayOfObjectSerializer3 : XmlSerializer1
    {
        public override bool CanDeserialize(XmlReader xmlReader)
        {
            return xmlReader.IsStartElement("GetPublicStrategiesResponseOutHeaders", "http://www.wealth-lab.com/WebServices/");
        }

        protected override object Deserialize(XmlSerializationReader reader)
        {
            return ((XmlSerializationReaderStrategyWebservice) reader).Read2_Item();
        }
    }
}

