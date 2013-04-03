namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public sealed class ArrayOfObjectSerializer17 : XmlSerializer1
    {
        public override bool CanDeserialize(XmlReader xmlReader)
        {
            return xmlReader.IsStartElement("ActivateTrialResponse", "http://www.wealth-lab.com/");
        }

        protected override object Deserialize(XmlSerializationReader reader)
        {
            return ((XmlSerializationReaderCustomersWebService) reader).Read14_ActivateTrialResponse();
        }
    }
}

