namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Serialization;
    using WealthLab.International.CustomersWebService;

    public class XmlSerializationWriterCustomersWebService : XmlSerializationWriter
    {
        protected override void InitCallbacks()
        {
        }

        public void Write10_T(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("T", "http://www.wealth-lab.com/", null, false);
            if (length > 0)
            {
                this.Write5_AuthEducationalParams("authParams", "http://www.wealth-lab.com/", (AuthEducationalParams) p[0], false, false);
            }
            base.WriteEndElement();
        }

        public void Write11_TInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
        }

        public void Write12_AuthTrial(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("AuthTrial", "http://www.wealth-lab.com/", null, false);
            if (length > 0)
            {
                base.WriteElementString("paramStr", "http://www.wealth-lab.com/", (string) p[0]);
            }
            base.WriteEndElement();
        }

        public void Write13_AuthTrialInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            if (p.Length > 0)
            {
                this.Write3_ServiceHeader("ServiceHeader", "http://www.wealth-lab.com/", (ServiceHeader) p[0], false, false);
            }
        }

        public void Write14_ActivateTrial(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("ActivateTrial", "http://www.wealth-lab.com/", null, false);
            if (length > 0)
            {
                base.WriteElementString("paramStr", "http://www.wealth-lab.com/", (string) p[0]);
            }
            base.WriteEndElement();
        }

        public void Write15_ActivateTrialInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            if (p.Length > 0)
            {
                this.Write3_ServiceHeader("ServiceHeader", "http://www.wealth-lab.com/", (ServiceHeader) p[0], false, false);
            }
        }

        private void Write3_ServiceHeader(string n, string ns, ServiceHeader o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(ServiceHeader)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("ServiceHeader", "http://www.wealth-lab.com/");
                }
                if (o.EncodedMustUnderstand != "0")
                {
                    base.WriteAttribute("mustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/", o.EncodedMustUnderstand);
                }
                if (o.EncodedMustUnderstand12 != "0")
                {
                    base.WriteAttribute("mustUnderstand", "http://www.w3.org/2003/05/soap-envelope", o.EncodedMustUnderstand12);
                }
                if ((o.Actor != null) && (o.Actor.Length != 0))
                {
                    base.WriteAttribute("actor", "http://schemas.xmlsoap.org/soap/envelope/", o.Actor);
                }
                if ((o.Role != null) && (o.Role.Length != 0))
                {
                    base.WriteAttribute("role", "http://www.w3.org/2003/05/soap-envelope", o.Role);
                }
                if (o.EncodedRelay != "0")
                {
                    base.WriteAttribute("relay", "http://www.w3.org/2003/05/soap-envelope", o.EncodedRelay);
                }
                XmlAttribute[] anyAttr = o.AnyAttr;
                if (anyAttr != null)
                {
                    for (int i = 0; i < anyAttr.Length; i++)
                    {
                        XmlAttribute node = anyAttr[i];
                        base.WriteXmlAttribute(node, o);
                    }
                }
                base.WriteElementString("Param", "http://www.wealth-lab.com/", o.Param);
                base.WriteEndElement(o);
            }
        }

        private string Write4_AuthEducationalResult(AuthEducationalResult v)
        {
            switch (v)
            {
                case AuthEducationalResult.None:
                    return "None";

                case AuthEducationalResult.Successfully:
                    return "Successfully";

                case AuthEducationalResult.UniversityNotFound:
                    return "UniversityNotFound";

                case AuthEducationalResult.KeyNotFound:
                    return "KeyNotFound";

                case AuthEducationalResult.KeyExpired:
                    return "KeyExpired";

                case AuthEducationalResult.ProductNotEducational:
                    return "ProductNotEducational";

                case AuthEducationalResult.ComputerClockDiffers:
                    return "ComputerClockDiffers";

                case AuthEducationalResult.KeyAlreadyActivated:
                    return "KeyAlreadyActivated";
            }
            long num = (long) v;
            throw base.CreateInvalidEnumValueException(num.ToString(CultureInfo.InvariantCulture), "WealthLab.International.CustomersWebService.AuthEducationalResult");
        }

        private void Write5_AuthEducationalParams(string n, string ns, AuthEducationalParams o, bool isNullable, bool needType)
        {
            if (o == null)
            {
                if (isNullable)
                {
                    base.WriteNullTagLiteral(n, ns);
                }
            }
            else
            {
                if (!needType && (o.GetType() != typeof(AuthEducationalParams)))
                {
                    throw base.CreateUnknownTypeException(o);
                }
                base.WriteStartElement(n, ns, o, false, null);
                if (needType)
                {
                    base.WriteXsiType("AuthEducationalParams", "http://www.wealth-lab.com/");
                }
                base.WriteElementStringRaw("ProductId", "http://www.wealth-lab.com/", XmlConvert.ToString(o.ProductId));
                base.WriteElementString("University", "http://www.wealth-lab.com/", o.University);
                base.WriteElementString("ActivationKey", "http://www.wealth-lab.com/", o.ActivationKey);
                base.WriteElementString("Hwid", "http://www.wealth-lab.com/", o.Hwid);
                base.WriteElementString("Result", "http://www.wealth-lab.com/", this.Write4_AuthEducationalResult(o.Result));
                base.WriteElementString("MessageId", "http://www.wealth-lab.com/", o.MessageId);
                base.WriteElementString("Version", "http://www.wealth-lab.com/", o.Version);
                base.WriteElementStringRaw("ComputerDate", "http://www.wealth-lab.com/", XmlSerializationWriter.FromDateTime(o.ComputerDate));
                base.WriteElementStringRaw("LifetimeDate", "http://www.wealth-lab.com/", XmlSerializationWriter.FromDateTime(o.LifetimeDate));
                base.WriteElementString("ComputerName", "http://www.wealth-lab.com/", o.ComputerName);
                base.WriteEndElement(o);
            }
        }

        public void Write6_ActivateKey(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("ActivateKey", "http://www.wealth-lab.com/", null, false);
            if (length > 0)
            {
                base.WriteElementString("paramStr", "http://www.wealth-lab.com/", (string) p[0]);
            }
            base.WriteEndElement();
        }

        public void Write7_ActivateKeyInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            if (p.Length > 0)
            {
                this.Write3_ServiceHeader("ServiceHeader", "http://www.wealth-lab.com/", (ServiceHeader) p[0], false, false);
            }
        }

        public void Write8_AuthEducational(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
            int length = p.Length;
            base.WriteStartElement("AuthEducational", "http://www.wealth-lab.com/", null, false);
            if (length > 0)
            {
                base.WriteElementString("paramStr", "http://www.wealth-lab.com/", (string) p[0]);
            }
            base.WriteEndElement();
        }

        public void Write9_AuthEducationalInHeaders(object[] p)
        {
            base.WriteStartDocument();
            base.TopLevelElement();
        }
    }
}

