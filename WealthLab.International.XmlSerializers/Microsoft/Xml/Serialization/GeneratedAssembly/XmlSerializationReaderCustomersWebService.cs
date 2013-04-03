namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlSerializationReaderCustomersWebService : XmlSerializationReader
    {
        private string id1_ActivateKeyResponse;
        private string id2_httpwwwwealthlabcom;
        private string id3_error;
        private string id4_paramStr;
        private string id5_AuthEducationalResponse;
        private string id6_TResponse;
        private string id7_AuthTrialResponse;
        private string id8_ActivateTrialResponse;

        protected override void InitCallbacks()
        {
        }

        protected override void InitIDs()
        {
            this.id2_httpwwwwealthlabcom = base.Reader.NameTable.Add("http://www.wealth-lab.com/");
            this.id8_ActivateTrialResponse = base.Reader.NameTable.Add("ActivateTrialResponse");
            this.id4_paramStr = base.Reader.NameTable.Add("paramStr");
            this.id1_ActivateKeyResponse = base.Reader.NameTable.Add("ActivateKeyResponse");
            this.id5_AuthEducationalResponse = base.Reader.NameTable.Add("AuthEducationalResponse");
            this.id6_TResponse = base.Reader.NameTable.Add("TResponse");
            this.id7_AuthTrialResponse = base.Reader.NameTable.Add("AuthTrialResponse");
            this.id3_error = base.Reader.NameTable.Add("error");
        }

        public object[] Read10_TResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[0];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id6_TResponse, this.id2_httpwwwwealthlabcom))
                {
                    if (base.Reader.IsEmptyElement)
                    {
                        base.Reader.Skip();
                        base.Reader.MoveToContent();
                        continue;
                    }
                    base.Reader.ReadStartElement();
                    base.Reader.MoveToContent();
                    int num3 = 0;
                    int num4 = base.ReaderCount;
                    while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                    {
                        if (base.Reader.NodeType == XmlNodeType.Element)
                        {
                            base.UnknownNode(o, "");
                        }
                        else
                        {
                            base.UnknownNode(o, "");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://www.wealth-lab.com/:TResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read11_TResponseOutHeaders()
        {
            base.Reader.MoveToContent();
            object[] o = new object[0];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read12_AuthTrialResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[2];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id7_AuthTrialResponse, this.id2_httpwwwwealthlabcom))
                {
                    bool[] flagArray = new bool[2];
                    if (base.Reader.IsEmptyElement)
                    {
                        base.Reader.Skip();
                        base.Reader.MoveToContent();
                        continue;
                    }
                    base.Reader.ReadStartElement();
                    base.Reader.MoveToContent();
                    int num3 = 0;
                    int num4 = base.ReaderCount;
                    while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                    {
                        if (base.Reader.NodeType == XmlNodeType.Element)
                        {
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id3_error)) && (base.Reader.NamespaceURI == this.id2_httpwwwwealthlabcom))
                            {
                                o[0] = base.Reader.ReadElementString();
                                flagArray[0] = true;
                            }
                            else if ((!flagArray[1] && (base.Reader.LocalName == this.id4_paramStr)) && (base.Reader.NamespaceURI == this.id2_httpwwwwealthlabcom))
                            {
                                o[1] = base.Reader.ReadElementString();
                                flagArray[1] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://www.wealth-lab.com/:error, http://www.wealth-lab.com/:paramStr");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://www.wealth-lab.com/:error, http://www.wealth-lab.com/:paramStr");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://www.wealth-lab.com/:AuthTrialResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read13_AuthTrialResponseOutHeaders()
        {
            base.Reader.MoveToContent();
            object[] o = new object[0];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read14_ActivateTrialResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[2];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id8_ActivateTrialResponse, this.id2_httpwwwwealthlabcom))
                {
                    bool[] flagArray = new bool[2];
                    if (base.Reader.IsEmptyElement)
                    {
                        base.Reader.Skip();
                        base.Reader.MoveToContent();
                        continue;
                    }
                    base.Reader.ReadStartElement();
                    base.Reader.MoveToContent();
                    int num3 = 0;
                    int num4 = base.ReaderCount;
                    while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                    {
                        if (base.Reader.NodeType == XmlNodeType.Element)
                        {
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id3_error)) && (base.Reader.NamespaceURI == this.id2_httpwwwwealthlabcom))
                            {
                                o[0] = base.Reader.ReadElementString();
                                flagArray[0] = true;
                            }
                            else if ((!flagArray[1] && (base.Reader.LocalName == this.id4_paramStr)) && (base.Reader.NamespaceURI == this.id2_httpwwwwealthlabcom))
                            {
                                o[1] = base.Reader.ReadElementString();
                                flagArray[1] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://www.wealth-lab.com/:error, http://www.wealth-lab.com/:paramStr");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://www.wealth-lab.com/:error, http://www.wealth-lab.com/:paramStr");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://www.wealth-lab.com/:ActivateTrialResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read15_Item()
        {
            base.Reader.MoveToContent();
            object[] o = new object[0];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read6_ActivateKeyResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[2];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id1_ActivateKeyResponse, this.id2_httpwwwwealthlabcom))
                {
                    bool[] flagArray = new bool[2];
                    if (base.Reader.IsEmptyElement)
                    {
                        base.Reader.Skip();
                        base.Reader.MoveToContent();
                        continue;
                    }
                    base.Reader.ReadStartElement();
                    base.Reader.MoveToContent();
                    int num3 = 0;
                    int num4 = base.ReaderCount;
                    while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                    {
                        if (base.Reader.NodeType == XmlNodeType.Element)
                        {
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id3_error)) && (base.Reader.NamespaceURI == this.id2_httpwwwwealthlabcom))
                            {
                                o[0] = base.Reader.ReadElementString();
                                flagArray[0] = true;
                            }
                            else if ((!flagArray[1] && (base.Reader.LocalName == this.id4_paramStr)) && (base.Reader.NamespaceURI == this.id2_httpwwwwealthlabcom))
                            {
                                o[1] = base.Reader.ReadElementString();
                                flagArray[1] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://www.wealth-lab.com/:error, http://www.wealth-lab.com/:paramStr");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://www.wealth-lab.com/:error, http://www.wealth-lab.com/:paramStr");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://www.wealth-lab.com/:ActivateKeyResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read7_ActivateKeyResponseOutHeaders()
        {
            base.Reader.MoveToContent();
            object[] o = new object[0];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read8_AuthEducationalResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[2];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id5_AuthEducationalResponse, this.id2_httpwwwwealthlabcom))
                {
                    bool[] flagArray = new bool[2];
                    if (base.Reader.IsEmptyElement)
                    {
                        base.Reader.Skip();
                        base.Reader.MoveToContent();
                        continue;
                    }
                    base.Reader.ReadStartElement();
                    base.Reader.MoveToContent();
                    int num3 = 0;
                    int num4 = base.ReaderCount;
                    while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                    {
                        if (base.Reader.NodeType == XmlNodeType.Element)
                        {
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id3_error)) && (base.Reader.NamespaceURI == this.id2_httpwwwwealthlabcom))
                            {
                                o[0] = base.Reader.ReadElementString();
                                flagArray[0] = true;
                            }
                            else if ((!flagArray[1] && (base.Reader.LocalName == this.id4_paramStr)) && (base.Reader.NamespaceURI == this.id2_httpwwwwealthlabcom))
                            {
                                o[1] = base.Reader.ReadElementString();
                                flagArray[1] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://www.wealth-lab.com/:error, http://www.wealth-lab.com/:paramStr");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://www.wealth-lab.com/:error, http://www.wealth-lab.com/:paramStr");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://www.wealth-lab.com/:AuthEducationalResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read9_Item()
        {
            base.Reader.MoveToContent();
            object[] o = new object[0];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.NodeType == XmlNodeType.Element)
                {
                    base.UnknownNode(o, "");
                }
                else
                {
                    base.UnknownNode(o, "");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }
    }
}

