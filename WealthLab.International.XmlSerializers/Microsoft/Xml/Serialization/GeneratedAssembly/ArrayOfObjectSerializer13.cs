namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public sealed class ArrayOfObjectSerializer13 : XmlSerializer1
    {
        public override bool CanDeserialize(XmlReader xmlReader)
        {
            return xmlReader.IsStartElement("AuthTrialResponse", "http://www.wealth-lab.com/");
        }

        protected override object Deserialize(XmlSerializationReader reader)
        {
            return ((XmlSerializationReaderCustomersWebService) reader).Read12_AuthTrialResponse();
        }
    }
}

