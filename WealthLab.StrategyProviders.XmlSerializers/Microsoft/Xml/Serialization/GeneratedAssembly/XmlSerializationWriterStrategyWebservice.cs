namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlSerializationWriterStrategyWebservice : XmlSerializationWriter
    {
        protected override void InitCallbacks()
        {
        }

        public void Write1_GetPublicStrategies(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("GetPublicStrategies", "http://www.wealth-lab.com/WebServices/", null, false);
            if (length > 0)
            {
                base.WriteElementStringRaw("numberOfDaysBack", "http://www.wealth-lab.com/WebServices/", XmlConvert.ToString((int) p[0]));
            }
            base.WriteEndElement();
        }

        public void Write2_GetPublicStrategiesInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }

        public void Write3_GetPrivateStrategies(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("GetPrivateStrategies", "http://www.wealth-lab.com/WebServices/", null, false);
            if (length > 0)
            {
                base.WriteElementStringRaw("numberOfDaysBack", "http://www.wealth-lab.com/WebServices/", XmlConvert.ToString((int) p[0]));
            }
            if (length > 1)
            {
                base.WriteElementString("username", "http://www.wealth-lab.com/WebServices/", (string) p[1]);
            }
            if (length > 2)
            {
                base.WriteElementStringRaw("password", "http://www.wealth-lab.com/WebServices/", XmlSerializationWriter.FromByteArrayBase64((byte[]) p[2]));
            }
            base.WriteEndElement();
        }

        public void Write4_GetPrivateStrategiesInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }
    }
}

