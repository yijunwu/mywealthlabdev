namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml.Serialization;

    public class XmlSerializationWriterExtensionManager : XmlSerializationWriter
    {
        protected override void InitCallbacks()
        {
        }

        public void Write1_GetMoreExtensionsUrl(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("GetMoreExtensionsUrl", "http://tempuri.org/", null, false);
            base.WriteEndElement();
        }

        public void Write10_GetDownloadUrlsInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }

        public void Write11_RecordExtensionDownloads(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("RecordExtensionDownloads", "http://tempuri.org/", null, false);
            if (length > 0)
            {
                base.WriteElementString("extensionName", "http://tempuri.org/", (string) p[0]);
            }
            if (length > 1)
            {
                base.WriteElementString("extensionVersion", "http://tempuri.org/", (string) p[1]);
            }
            if (length > 2)
            {
                base.WriteElementString("username", "http://tempuri.org/", (string) p[2]);
            }
            if (length > 3)
            {
                base.WriteElementStringRaw("date", "http://tempuri.org/", XmlSerializationWriter.FromDateTime((DateTime) p[3]));
            }
            if (length > 4)
            {
                base.WriteElementString("ip", "http://tempuri.org/", (string) p[4]);
            }
            base.WriteEndElement();
        }

        public void Write12_Item(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }

        public void Write13_GetExtensionKey(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("GetExtensionKey", "http://tempuri.org/", null, false);
            if (length > 0)
            {
                base.WriteElementString("ExtensionName", "http://tempuri.org/", (string) p[0]);
            }
            if (length > 1)
            {
                base.WriteElementString("ExtensionVersion", "http://tempuri.org/", (string) p[1]);
            }
            base.WriteEndElement();
        }

        public void Write14_GetExtensionKeyInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }

        public void Write15_GetExtensionIV(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("GetExtensionIV", "http://tempuri.org/", null, false);
            if (length > 0)
            {
                base.WriteElementString("ExtensionName", "http://tempuri.org/", (string) p[0]);
            }
            if (length > 1)
            {
                base.WriteElementString("ExtensionVersion", "http://tempuri.org/", (string) p[1]);
            }
            base.WriteEndElement();
        }

        public void Write16_GetExtensionIVInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }

        public void Write2_GetMoreExtensionsUrlInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }

        public void Write3_GetChangeLogUrl(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("GetChangeLogUrl", "http://tempuri.org/", null, false);
            if (length > 0)
            {
                base.WriteElementString("ExtensionName", "http://tempuri.org/", (string) p[0]);
            }
            base.WriteEndElement();
        }

        public void Write4_GetChangeLogUrlInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }

        public void Write5_GetExtensionVersions(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("GetExtensionVersions", "http://tempuri.org/", null, false);
            if (length > 0)
            {
                base.WriteElementString("ExtensionName", "http://tempuri.org/", (string) p[0]);
            }
            base.WriteEndElement();
        }

        public void Write6_GetExtensionVersionsInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }

        public void Write7_GetExtensionInfoAttributes(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("GetExtensionInfoAttributes", "http://tempuri.org/", null, false);
            base.WriteEndElement();
        }

        public void Write8_Item(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
        }

        public void Write9_GetDownloadUrls(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("GetDownloadUrls", "http://tempuri.org/", null, false);
            if (length > 0)
            {
                base.WriteElementString("ExtensionName", "http://tempuri.org/", (string) p[0]);
            }
            if (length > 1)
            {
                base.WriteElementString("ExtensionVersion", "http://tempuri.org/", (string) p[1]);
            }
            base.WriteEndElement();
        }
    }
}

