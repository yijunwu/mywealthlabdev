namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Collections;
    using System.Xml.Serialization;
    using WealthLab.StrategyProviders.WLWebServices;

    public class XmlSerializerContract : XmlSerializerImplementation
    {
        private Hashtable readMethods;
        private Hashtable typedSerializers;
        private Hashtable writeMethods;

        public override bool CanSerialize(Type type)
        {
            return (type == typeof(StrategyWebservice));
        }

        public override XmlSerializer GetSerializer(Type type)
        {
            return null;
        }

        public override XmlSerializationReader Reader
        {
            get
            {
                return new XmlSerializationReaderStrategyWebservice();
            }
        }

        public override Hashtable ReadMethods
        {
            get
            {
                if (this.readMethods == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable["WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPublicStrategies(Int32):Response"] = "Read1_GetPublicStrategiesResponse";
                    hashtable["WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPublicStrategies(Int32):OutHeaders"] = "Read2_Item";
                    hashtable["WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPrivateStrategies(Int32, System.String, Byte[]):Response"] = "Read3_GetPrivateStrategiesResponse";
                    hashtable["WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPrivateStrategies(Int32, System.String, Byte[]):OutHeaders"] = "Read4_Item";
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
                    hashtable.Add("WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPublicStrategies(Int32):Response", new ArrayOfObjectSerializer1());
                    hashtable.Add("WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPrivateStrategies(Int32, System.String, Byte[]):OutHeaders", new ArrayOfObjectSerializer7());
                    hashtable.Add("WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPublicStrategies(Int32):InHeaders", new ArrayOfObjectSerializer2());
                    hashtable.Add("WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPublicStrategies(Int32)", new ArrayOfObjectSerializer());
                    hashtable.Add("WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPrivateStrategies(Int32, System.String, Byte[]):InHeaders", new ArrayOfObjectSerializer6());
                    hashtable.Add("WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPrivateStrategies(Int32, System.String, Byte[]):Response", new ArrayOfObjectSerializer5());
                    hashtable.Add("WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPrivateStrategies(Int32, System.String, Byte[])", new ArrayOfObjectSerializer4());
                    hashtable.Add("WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPublicStrategies(Int32):OutHeaders", new ArrayOfObjectSerializer3());
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
                    hashtable["WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPublicStrategies(Int32)"] = "Write1_GetPublicStrategies";
                    hashtable["WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPublicStrategies(Int32):InHeaders"] = "Write2_GetPublicStrategiesInHeaders";
                    hashtable["WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPrivateStrategies(Int32, System.String, Byte[])"] = "Write3_GetPrivateStrategies";
                    hashtable["WealthLab.StrategyProviders.WLWebServices.StrategyWebservice:System.String[] GetPrivateStrategies(Int32, System.String, Byte[]):InHeaders"] = "Write4_GetPrivateStrategiesInHeaders";
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
                return new XmlSerializationWriterStrategyWebservice();
            }
        }
    }
}

