namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Collections;
    using System.Xml.Serialization;
    using WealthLab.International.CustomersWebService;

    public class XmlSerializerContract : XmlSerializerImplementation
    {
        private Hashtable readMethods;
        private Hashtable typedSerializers;
        private Hashtable writeMethods;

        public override bool CanSerialize(Type type)
        {
            return (type == typeof(WealthLab.International.CustomersWebService.CustomersWebService));
        }

        public override XmlSerializer GetSerializer(Type type)
        {
            return null;
        }

        public override XmlSerializationReader Reader
        {
            get
            {
                return new XmlSerializationReaderCustomersWebService();
            }
        }

        public override Hashtable ReadMethods
        {
            get
            {
                if (this.readMethods == null)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateKey(System.String ByRef):Response"] = "Read6_ActivateKeyResponse";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateKey(System.String ByRef):OutHeaders"] = "Read7_ActivateKeyResponseOutHeaders";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthEducational(System.String ByRef):Response"] = "Read8_AuthEducationalResponse";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthEducational(System.String ByRef):OutHeaders"] = "Read9_Item";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:Void T(WealthLab.International.CustomersWebService.AuthEducationalParams):Response"] = "Read10_TResponse";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:Void T(WealthLab.International.CustomersWebService.AuthEducationalParams):OutHeaders"] = "Read11_TResponseOutHeaders";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthTrial(System.String ByRef):Response"] = "Read12_AuthTrialResponse";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthTrial(System.String ByRef):OutHeaders"] = "Read13_AuthTrialResponseOutHeaders";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateTrial(System.String ByRef):Response"] = "Read14_ActivateTrialResponse";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateTrial(System.String ByRef):OutHeaders"] = "Read15_Item";
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
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthTrial(System.String ByRef):Response", new ArrayOfObjectSerializer13());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthEducational(System.String ByRef):OutHeaders", new ArrayOfObjectSerializer7());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateKey(System.String ByRef)", new ArrayOfObjectSerializer());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateKey(System.String ByRef):InHeaders", new ArrayOfObjectSerializer2());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:Void T(WealthLab.International.CustomersWebService.AuthEducationalParams)", new ArrayOfObjectSerializer8());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthTrial(System.String ByRef)", new ArrayOfObjectSerializer12());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthTrial(System.String ByRef):OutHeaders", new ArrayOfObjectSerializer15());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthTrial(System.String ByRef):InHeaders", new ArrayOfObjectSerializer14());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:Void T(WealthLab.International.CustomersWebService.AuthEducationalParams):InHeaders", new ArrayOfObjectSerializer10());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateTrial(System.String ByRef):Response", new ArrayOfObjectSerializer17());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateKey(System.String ByRef):OutHeaders", new ArrayOfObjectSerializer3());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthEducational(System.String ByRef)", new ArrayOfObjectSerializer4());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateKey(System.String ByRef):Response", new ArrayOfObjectSerializer1());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:Void T(WealthLab.International.CustomersWebService.AuthEducationalParams):OutHeaders", new ArrayOfObjectSerializer11());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateTrial(System.String ByRef):InHeaders", new ArrayOfObjectSerializer18());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthEducational(System.String ByRef):Response", new ArrayOfObjectSerializer5());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthEducational(System.String ByRef):InHeaders", new ArrayOfObjectSerializer6());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateTrial(System.String ByRef)", new ArrayOfObjectSerializer16());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:Void T(WealthLab.International.CustomersWebService.AuthEducationalParams):Response", new ArrayOfObjectSerializer9());
                    hashtable.Add("WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateTrial(System.String ByRef):OutHeaders", new ArrayOfObjectSerializer19());
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
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateKey(System.String ByRef)"] = "Write6_ActivateKey";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateKey(System.String ByRef):InHeaders"] = "Write7_ActivateKeyInHeaders";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthEducational(System.String ByRef)"] = "Write8_AuthEducational";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthEducational(System.String ByRef):InHeaders"] = "Write9_AuthEducationalInHeaders";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:Void T(WealthLab.International.CustomersWebService.AuthEducationalParams)"] = "Write10_T";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:Void T(WealthLab.International.CustomersWebService.AuthEducationalParams):InHeaders"] = "Write11_TInHeaders";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthTrial(System.String ByRef)"] = "Write12_AuthTrial";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String AuthTrial(System.String ByRef):InHeaders"] = "Write13_AuthTrialInHeaders";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateTrial(System.String ByRef)"] = "Write14_ActivateTrial";
                    hashtable["WealthLab.International.CustomersWebService.CustomersWebService:System.String ActivateTrial(System.String ByRef):InHeaders"] = "Write15_ActivateTrialInHeaders";
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
                return new XmlSerializationWriterCustomersWebService();
            }
        }
    }
}

