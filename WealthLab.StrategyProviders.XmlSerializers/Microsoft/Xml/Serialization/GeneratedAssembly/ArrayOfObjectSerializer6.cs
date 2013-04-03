namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public sealed class ArrayOfObjectSerializer6 : XmlSerializer1
    {
        public override bool CanDeserialize(XmlReader xmlReader)
        {
            return xmlReader.IsStartElement("GetPrivateStrategiesInHeaders", "http://www.wealth-lab.com/WebServices/");
        }

        protected override void Serialize(object objectToSerialize, XmlSerializationWriter writer)
        {
            ((XmlSerializationWriterStrategyWebservice) writer).Write4_GetPrivateStrategiesInHeaders((object[]) objectToSerialize);
        }
    }
}

