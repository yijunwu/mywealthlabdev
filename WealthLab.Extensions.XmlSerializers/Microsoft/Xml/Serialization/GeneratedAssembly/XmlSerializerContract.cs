namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Collections;
    using System.Xml.Serialization;
    using WealthLab.Extensions.Server;

    public class XmlSerializerContract : XmlSerializerImplementation
    {
        private Hashtable readMethods;
        private Hashtable typedSerializers;
        private Hashtable writeMethods;

        public override bool CanSerialize(Type type)
        {
            return (type == typeof(ExtensionManager));
        }

        public override XmlSerializer GetSerializer(Type type)
        {
            return null;
        }

        public override XmlSerializationReader Reader
        {
            get
            {
                return new XmlSerializationReaderExtensionManager();
            }
        }

        public override Hashtable ReadMethods
        {
            get
            {
                if (this.readMethods == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String GetMoreExtensionsUrl():Response"] = "Read1_GetMoreExtensionsUrlResponse";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String GetMoreExtensionsUrl():OutHeaders"] = "Read2_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String GetChangeLogUrl(System.String):Response"] = "Read3_GetChangeLogUrlResponse";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String GetChangeLogUrl(System.String):OutHeaders"] = "Read4_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionVersions(System.String):Response"] = "Read5_GetExtensionVersionsResponse";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionVersions(System.String):OutHeaders"] = "Read6_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionInfoAttributes():Response"] = "Read7_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionInfoAttributes():OutHeaders"] = "Read8_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetDownloadUrls(System.String, System.String):Response"] = "Read9_GetDownloadUrlsResponse";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetDownloadUrls(System.String, System.String):OutHeaders"] = "Read10_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Void RecordExtensionDownloads(System.String, System.String, System.String, System.DateTime, System.String):Response"] = "Read11_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Void RecordExtensionDownloads(System.String, System.String, System.String, System.DateTime, System.String):OutHeaders"] = "Read12_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionKey(System.String, System.String):Response"] = "Read13_GetExtensionKeyResponse";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionKey(System.String, System.String):OutHeaders"] = "Read14_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionIV(System.String, System.String):Response"] = "Read15_GetExtensionIVResponse";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionIV(System.String, System.String):OutHeaders"] = "Read16_Item";
                    if (this.readMethods == null)
                    {
                        this.readMethods = hashtable;
                    }
                }
                return this.readMethods;
            }
        }

        public override Hashtable TypedSerializers
        {
            get
            {
                if (this.typedSerializers == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionVersions(System.String):InHeaders", new ArrayOfObjectSerializer10());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String GetMoreExtensionsUrl():InHeaders", new ArrayOfObjectSerializer2());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String GetMoreExtensionsUrl():OutHeaders", new ArrayOfObjectSerializer3());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionKey(System.String, System.String):InHeaders", new ArrayOfObjectSerializer26());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionVersions(System.String)", new ArrayOfObjectSerializer8());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionIV(System.String, System.String):InHeaders", new ArrayOfObjectSerializer30());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Void RecordExtensionDownloads(System.String, System.String, System.String, System.DateTime, System.String)", new ArrayOfObjectSerializer20());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionInfoAttributes():OutHeaders", new ArrayOfObjectSerializer15());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String GetChangeLogUrl(System.String)", new ArrayOfObjectSerializer4());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String GetChangeLogUrl(System.String):OutHeaders", new ArrayOfObjectSerializer7());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Void RecordExtensionDownloads(System.String, System.String, System.String, System.DateTime, System.String):InHeaders", new ArrayOfObjectSerializer22());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Void RecordExtensionDownloads(System.String, System.String, System.String, System.DateTime, System.String):OutHeaders", new ArrayOfObjectSerializer23());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetDownloadUrls(System.String, System.String):OutHeaders", new ArrayOfObjectSerializer19());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String GetMoreExtensionsUrl():Response", new ArrayOfObjectSerializer1());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetDownloadUrls(System.String, System.String)", new ArrayOfObjectSerializer16());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionIV(System.String, System.String):OutHeaders", new ArrayOfObjectSerializer31());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetDownloadUrls(System.String, System.String):InHeaders", new ArrayOfObjectSerializer18());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionInfoAttributes():InHeaders", new ArrayOfObjectSerializer14());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionIV(System.String, System.String)", new ArrayOfObjectSerializer28());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Void RecordExtensionDownloads(System.String, System.String, System.String, System.DateTime, System.String):Response", new ArrayOfObjectSerializer21());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String GetChangeLogUrl(System.String):InHeaders", new ArrayOfObjectSerializer6());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionVersions(System.String):OutHeaders", new ArrayOfObjectSerializer11());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionIV(System.String, System.String):Response", new ArrayOfObjectSerializer29());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetDownloadUrls(System.String, System.String):Response", new ArrayOfObjectSerializer17());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionInfoAttributes()", new ArrayOfObjectSerializer12());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String GetChangeLogUrl(System.String):Response", new ArrayOfObjectSerializer5());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionVersions(System.String):Response", new ArrayOfObjectSerializer9());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionKey(System.String, System.String):Response", new ArrayOfObjectSerializer25());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionInfoAttributes():Response", new ArrayOfObjectSerializer13());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:System.String GetMoreExtensionsUrl()", new ArrayOfObjectSerializer());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionKey(System.String, System.String):OutHeaders", new ArrayOfObjectSerializer27());
                    hashtable.Add("WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionKey(System.String, System.String)", new ArrayOfObjectSerializer24());
                    if (this.typedSerializers == null)
                    {
                        this.typedSerializers = hashtable;
                    }
                }
                return this.typedSerializers;
            }
        }

        public override Hashtable WriteMethods
        {
            get
            {
                if (this.writeMethods == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String GetMoreExtensionsUrl()"] = "Write1_GetMoreExtensionsUrl";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String GetMoreExtensionsUrl():InHeaders"] = "Write2_GetMoreExtensionsUrlInHeaders";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String GetChangeLogUrl(System.String)"] = "Write3_GetChangeLogUrl";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String GetChangeLogUrl(System.String):InHeaders"] = "Write4_GetChangeLogUrlInHeaders";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionVersions(System.String)"] = "Write5_GetExtensionVersions";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionVersions(System.String):InHeaders"] = "Write6_GetExtensionVersionsInHeaders";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionInfoAttributes()"] = "Write7_GetExtensionInfoAttributes";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetExtensionInfoAttributes():InHeaders"] = "Write8_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetDownloadUrls(System.String, System.String)"] = "Write9_GetDownloadUrls";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:System.String[] GetDownloadUrls(System.String, System.String):InHeaders"] = "Write10_GetDownloadUrlsInHeaders";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Void RecordExtensionDownloads(System.String, System.String, System.String, System.DateTime, System.String)"] = "Write11_RecordExtensionDownloads";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Void RecordExtensionDownloads(System.String, System.String, System.String, System.DateTime, System.String):InHeaders"] = "Write12_Item";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionKey(System.String, System.String)"] = "Write13_GetExtensionKey";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionKey(System.String, System.String):InHeaders"] = "Write14_GetExtensionKeyInHeaders";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionIV(System.String, System.String)"] = "Write15_GetExtensionIV";
                    hashtable["WealthLab.Extensions.Server.ExtensionManager:Byte[] GetExtensionIV(System.String, System.String):InHeaders"] = "Write16_GetExtensionIVInHeaders";
                    if (this.writeMethods == null)
                    {
                        this.writeMethods = hashtable;
                    }
                }
                return this.writeMethods;
            }
        }

        public override XmlSerializationWriter Writer
        {
            get
            {
                return new XmlSerializationWriterExtensionManager();
            }
        }
    }
}

