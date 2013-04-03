namespace Microsoft.Xml.Serialization.GeneratedAssembly
{
    using System;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlSerializationReaderExtensionManager : XmlSerializationReader
    {
        private string id1_GetMoreExtensionsUrlResponse;
        private string id10_Item;
        private string id11_GetDownloadUrlsResponse;
        private string id12_GetDownloadUrlsResult;
        private string id13_Item;
        private string id14_GetExtensionKeyResponse;
        private string id15_GetExtensionKeyResult;
        private string id16_GetExtensionIVResponse;
        private string id17_GetExtensionIVResult;
        private string id2_httptempuriorg;
        private string id3_GetMoreExtensionsUrlResult;
        private string id4_GetChangeLogUrlResponse;
        private string id5_GetChangeLogUrlResult;
        private string id6_GetExtensionVersionsResponse;
        private string id7_GetExtensionVersionsResult;
        private string id8_string;
        private string id9_Item;

        protected override void InitCallbacks()
        {
        }

        protected override void InitIDs()
        {
            this.id3_GetMoreExtensionsUrlResult = base.Reader.NameTable.Add("GetMoreExtensionsUrlResult");
            this.id14_GetExtensionKeyResponse = base.Reader.NameTable.Add("GetExtensionKeyResponse");
            this.id6_GetExtensionVersionsResponse = base.Reader.NameTable.Add("GetExtensionVersionsResponse");
            this.id17_GetExtensionIVResult = base.Reader.NameTable.Add("GetExtensionIVResult");
            this.id16_GetExtensionIVResponse = base.Reader.NameTable.Add("GetExtensionIVResponse");
            this.id9_Item = base.Reader.NameTable.Add("GetExtensionInfoAttributesResponse");
            this.id1_GetMoreExtensionsUrlResponse = base.Reader.NameTable.Add("GetMoreExtensionsUrlResponse");
            this.id2_httptempuriorg = base.Reader.NameTable.Add("http://tempuri.org/");
            this.id10_Item = base.Reader.NameTable.Add("GetExtensionInfoAttributesResult");
            this.id5_GetChangeLogUrlResult = base.Reader.NameTable.Add("GetChangeLogUrlResult");
            this.id11_GetDownloadUrlsResponse = base.Reader.NameTable.Add("GetDownloadUrlsResponse");
            this.id4_GetChangeLogUrlResponse = base.Reader.NameTable.Add("GetChangeLogUrlResponse");
            this.id13_Item = base.Reader.NameTable.Add("RecordExtensionDownloadsResponse");
            this.id7_GetExtensionVersionsResult = base.Reader.NameTable.Add("GetExtensionVersionsResult");
            this.id15_GetExtensionKeyResult = base.Reader.NameTable.Add("GetExtensionKeyResult");
            this.id8_string = base.Reader.NameTable.Add("string");
            this.id12_GetDownloadUrlsResult = base.Reader.NameTable.Add("GetDownloadUrlsResult");
        }

        public object[] Read1_GetMoreExtensionsUrlResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[1];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id1_GetMoreExtensionsUrlResponse, this.id2_httptempuriorg))
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
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id3_GetMoreExtensionsUrlResult)) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
                            {
                                o[0] = base.Reader.ReadElementString();
                                flagArray[0] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://tempuri.org/:GetMoreExtensionsUrlResult");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://tempuri.org/:GetMoreExtensionsUrlResult");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://tempuri.org/:GetMoreExtensionsUrlResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read10_Item()
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

        public object[] Read11_Item()
        {
            base.Reader.MoveToContent();
            object[] o = new object[0];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id13_Item, this.id2_httptempuriorg))
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
                    base.UnknownNode(null, "http://tempuri.org/:RecordExtensionDownloadsResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read12_Item()
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

        public object[] Read13_GetExtensionKeyResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[1];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id14_GetExtensionKeyResponse, this.id2_httptempuriorg))
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
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id15_GetExtensionKeyResult)) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
                            {
                                o[0] = base.ToByteArrayBase64(false);
                                flagArray[0] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://tempuri.org/:GetExtensionKeyResult");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://tempuri.org/:GetExtensionKeyResult");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://tempuri.org/:GetExtensionKeyResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read14_Item()
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

        public object[] Read15_GetExtensionIVResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[1];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id16_GetExtensionIVResponse, this.id2_httptempuriorg))
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
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id17_GetExtensionIVResult)) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
                            {
                                o[0] = base.ToByteArrayBase64(false);
                                flagArray[0] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://tempuri.org/:GetExtensionIVResult");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://tempuri.org/:GetExtensionIVResult");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://tempuri.org/:GetExtensionIVResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read16_Item()
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

        public object[] Read3_GetChangeLogUrlResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[1];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id4_GetChangeLogUrlResponse, this.id2_httptempuriorg))
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
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id5_GetChangeLogUrlResult)) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
                            {
                                o[0] = base.Reader.ReadElementString();
                                flagArray[0] = true;
                            }
                            else
                            {
                                base.UnknownNode(o, "http://tempuri.org/:GetChangeLogUrlResult");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://tempuri.org/:GetChangeLogUrlResult");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://tempuri.org/:GetChangeLogUrlResponse");
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

        public object[] Read5_GetExtensionVersionsResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[1];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id6_GetExtensionVersionsResponse, this.id2_httptempuriorg))
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
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id7_GetExtensionVersionsResult)) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
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
                                                if ((base.Reader.LocalName == this.id8_string) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
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
                                                    base.UnknownNode(null, "http://tempuri.org/:string");
                                                }
                                            }
                                            else
                                            {
                                                base.UnknownNode(null, "http://tempuri.org/:string");
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
                                base.UnknownNode(o, "http://tempuri.org/:GetExtensionVersionsResult");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://tempuri.org/:GetExtensionVersionsResult");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://tempuri.org/:GetExtensionVersionsResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read6_Item()
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

        public object[] Read7_Item()
        {
            base.Reader.MoveToContent();
            object[] o = new object[1];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id9_Item, this.id2_httptempuriorg))
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
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id10_Item)) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
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
                                                if ((base.Reader.LocalName == this.id8_string) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
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
                                                    base.UnknownNode(null, "http://tempuri.org/:string");
                                                }
                                            }
                                            else
                                            {
                                                base.UnknownNode(null, "http://tempuri.org/:string");
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
                                base.UnknownNode(o, "http://tempuri.org/:GetExtensionInfoAttributesResult");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://tempuri.org/:GetExtensionInfoAttributesResult");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://tempuri.org/:GetExtensionInfoAttributesResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }

        public object[] Read8_Item()
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

        public object[] Read9_GetDownloadUrlsResponse()
        {
            base.Reader.MoveToContent();
            object[] o = new object[1];
            base.Reader.MoveToContent();
            int whileIterations = 0;
            int readerCount = base.ReaderCount;
            while ((base.Reader.NodeType != XmlNodeType.EndElement) && (base.Reader.NodeType != XmlNodeType.None))
            {
                if (base.Reader.IsStartElement(this.id11_GetDownloadUrlsResponse, this.id2_httptempuriorg))
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
                            if ((!flagArray[0] && (base.Reader.LocalName == this.id12_GetDownloadUrlsResult)) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
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
                                                if ((base.Reader.LocalName == this.id8_string) && (base.Reader.NamespaceURI == this.id2_httptempuriorg))
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
                                                    base.UnknownNode(null, "http://tempuri.org/:string");
                                                }
                                            }
                                            else
                                            {
                                                base.UnknownNode(null, "http://tempuri.org/:string");
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
                                base.UnknownNode(o, "http://tempuri.org/:GetDownloadUrlsResult");
                            }
                        }
                        else
                        {
                            base.UnknownNode(o, "http://tempuri.org/:GetDownloadUrlsResult");
                        }
                        base.Reader.MoveToContent();
                        base.CheckReaderCount(ref num3, ref num4);
                    }
                    base.ReadEndElement();
                }
                else
                {
                    base.UnknownNode(null, "http://tempuri.org/:GetDownloadUrlsResponse");
                }
                base.Reader.MoveToContent();
                base.CheckReaderCount(ref whileIterations, ref readerCount);
            }
            return o;
        }
    }
}

