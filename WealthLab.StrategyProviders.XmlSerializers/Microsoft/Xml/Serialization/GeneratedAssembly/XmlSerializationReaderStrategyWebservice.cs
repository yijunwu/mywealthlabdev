namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlSerializationReaderStrategyWebservice : XmlSerializationReader
    {
        private string id1_GetPublicStrategiesResponse;
        private string id2_Item;
        private string id3_GetPublicStrategiesResult;
        private string id4_string;
        private string id5_GetPrivateStrategiesResponse;
        private string id6_GetPrivateStrategiesResult;

        protected override void InitCallbacks()
        {
        }

        protected override void InitIDs()
        {
            this.id3_GetPublicStrategiesResult = base.Reader.NameTable.Add("GetPublicStrategiesResult");
            this.id2_Item = base.Reader.NameTable.Add("http://www.wealth-lab.com/WebServices/");
            this.id4_string = base.Reader.NameTable.Add("string");
            this.id1_GetPublicStrategiesResponse = base.Reader.NameTable.Add("GetPublicStrategiesResponse");
            this.id6_GetPrivateStrategiesResult = base.Reader.NameTable.Add("GetPrivateStrategiesResult");
            this.id5_GetPrivateStrategiesResponse = base.Reader.NameTable.Add("GetPrivateStrategiesResponse");
        }

        public object[] Read1_GetPublicStrategiesResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[1];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id1_GetPublicStrategiesResponse, this.id2_Item))
                {
                    bool[] flagArray = new bool[1];
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
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id3_GetPublicStrategiesResult)) && (base.Reader.NamespaceURI == this.id2_Item))
                            {
                                if (!base.ReadNull())
                                {
                                    string[] a = null;
                                    int index = 0;
                                    if (base.Reader.IsEmptyElement)
                                    {
                                        base.Reader.Skip();
                                    }
                                    else
                                    {
                                        base.Reader.ReadStartElement();
                                        base.Reader.MoveToContent();
                                        int num6 = 0;
                                        int num7 = base.ReaderCount;
                                        while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                                        {
                                            if (base.Reader.NodeType == XmlNodeType.Element)
                                            {
                                                if ((base.Reader.LocalName == this.id4_string) && (base.Reader.NamespaceURI == this.id2_Item))
                                                {
                                                    if (base.ReadNull())
                                                    {
                                                        a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                                        a[index++] = null;
                                                    }
                                                    else
                                                    {
                                                        a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                                        a[index++] = base.Reader.ReadElementString();
                                                    }
                                                }
                                                else
                                                {
                                                    base.UnknownNode(null, "http://www.wealth-lab.com/WebServices/:string");
                                                }
                                            }
                                            else
                                            {
                                                base.UnknownNode(null, "http://www.wealth-lab.com/WebServices/:string");
                                            }
                                            base.Reader.MoveToContent();
                                            base.CheckReaderCount(ref num6, ref num7);
                                        }
                                        base.ReadEndElement();
                                    }
                                    o[0] = (string[]) base.ShrinkArray(a, index, typeof(string), false);
                                }
                                flagArray[0] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://www.wealth-lab.com/WebServices/:GetPublicStrategiesResult");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://www.wealth-lab.com/WebServices/:GetPublicStrategiesResult");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://www.wealth-lab.com/WebServices/:GetPublicStrategiesResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read2_Item()
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

        public object[] Read3_GetPrivateStrategiesResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[1];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id5_GetPrivateStrategiesResponse, this.id2_Item))
                {
                    bool[] flagArray = new bool[1];
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
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id6_GetPrivateStrategiesResult)) && (base.Reader.NamespaceURI == this.id2_Item))
                            {
                                if (!base.ReadNull())
                                {
                                    string[] a = null;
                                    int index = 0;
                                    if (base.Reader.IsEmptyElement)
                                    {
                                        base.Reader.Skip();
                                    }
                                    else
                                    {
                                        base.Reader.ReadStartElement();
                                        base.Reader.MoveToContent();
                                        int num6 = 0;
                                        int num7 = base.ReaderCount;
                                        while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
                                        {
                                            if (base.Reader.NodeType == XmlNodeType.Element)
                                            {
                                                if ((base.Reader.LocalName == this.id4_string) && (base.Reader.NamespaceURI == this.id2_Item))
                                                {
                                                    if (base.ReadNull())
                                                    {
                                                        a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                                        a[index++] = null;
                                                    }
                                                    else
                                                    {
                                                        a = (string[]) base.EnsureArrayIndex(a, index, typeof(string));
                                                        a[index++] = base.Reader.ReadElementString();
                                                    }
                                                }
                                                else
                                                {
                                                    base.UnknownNode(null, "http://www.wealth-lab.com/WebServices/:string");
                                                }
                                            }
                                            else
                                            {
                                                base.UnknownNode(null, "http://www.wealth-lab.com/WebServices/:string");
                                            }
                                            base.Reader.MoveToContent();
                                            base.CheckReaderCount(ref num6, ref num7);
                                        }
                                        base.ReadEndElement();
                                    }
                                    o[0] = (string[]) base.ShrinkArray(a, index, typeof(string), false);
                                }
                                flagArray[0] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://www.wealth-lab.com/WebServices/:GetPrivateStrategiesResult");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://www.wealth-lab.com/WebServices/:GetPrivateStrategiesResult");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://www.wealth-lab.com/WebServices/:GetPrivateStrategiesResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read4_Item()
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

