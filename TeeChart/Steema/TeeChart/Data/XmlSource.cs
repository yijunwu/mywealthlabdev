namespace Steema.TeeChart.Data
{
    using Steema.TeeChart;
    using Steema.TeeChart.Styles;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using System.Xml.XPath;

    [Serializable, ToolboxBitmap(typeof(XmlSource), "Images.XmlSource.bmp")]
    public class XmlSource : SeriesSource
    {
        private Steema.TeeChart.Chart iChart;
        private string sDataMember;
        private string sSeriesNode;

        public XmlSource()
        {
            this.sSeriesNode = "";
            this.sDataMember = "";
        }

        public XmlSource(Steema.TeeChart.Chart c) : this()
        {
            this.Chart = c;
        }

        public XmlSource(Series s) : this()
        {
            base.Series = s;
        }

        public void Load(string[] xmlLines)
        {
            XmlDocument d = new XmlDocument();
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            foreach (string str in xmlLines)
            {
                writer.WriteLine(str);
            }
            writer.Flush();
            stream.Position = 0L;
            d.Load(stream);
            this.Load(d);
            base.Series = null;
            this.SeriesNode = "";
        }

        public void Load(string fileName)
        {
            XmlDocument d = new XmlDocument();
            d.Load(fileName);
            this.Load(d);
        }

        public void Load(XmlDocument d)
        {
            if ((this.Chart != null) || (base.Series != null))
            {
                XPathNavigator navigator = d.CreateNavigator();
                XPathNodeIterator iterator = navigator.Select("chart/series");
                if (iterator == null)
                {
                    this.XMLError("No <series> nodes.");
                }
                else
                {
                    if (base.Series == null)
                    {
                        this.Chart.Series.Clear(true);
                    }
                    if (this.SeriesNode.Length == 0)
                    {
                        if (iterator.Count > 0)
                        {
                            while (iterator.MoveNext())
                            {
                                this.LoadSeriesNode(iterator.Current, navigator.NamespaceURI);
                                if (base.Series != null)
                                {
                                    return;
                                }
                            }
                        }
                        else
                        {
                            this.XMLError("Empty <series> node.");
                        }
                    }
                    else
                    {
                        bool flag = false;
                        while (iterator.MoveNext())
                        {
                            if (iterator.Current.GetAttribute("title", navigator.NamespaceURI).ToUpper(CultureInfo.CurrentCulture) == this.SeriesNode.ToUpper())
                            {
                                this.LoadSeriesNode(iterator.Current, navigator.NamespaceURI);
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            this.XMLError("Series " + this.SeriesNode + " not found");
                        }
                    }
                }
            }
        }

        private void LoadSeriesNode(XPathNavigator node, string nameSpaceURI)
        {
            string valueSource;
            Series series = base.Series;
            if (series != null)
            {
                series.Clear();
            }
            else
            {
                Type type = null;
                string attribute = node.GetAttribute("type", nameSpaceURI);
                if (!Utils.IsNullOrEmpty(attribute))
                {
                    valueSource = "STEEMA.TEECHART.STYLES.";
                    valueSource = valueSource + attribute.ToUpper();
                    foreach (Type type2 in Utils.SeriesTypesOf)
                    {
                        if (type2.ToString().ToUpper() == valueSource)
                        {
                            type = type2;
                            break;
                        }
                    }
                    if (type == null)
                    {
                        type = Utils.SeriesTypesOf[5];
                    }
                    series = this.Chart.Series.Add(type);
                    attribute = node.GetAttribute("title", nameSpaceURI);
                    if (!Utils.IsNullOrEmpty(attribute))
                    {
                        series.Title = attribute;
                    }
                    string s = node.GetAttribute("color", nameSpaceURI);
                    if (!Utils.IsNullOrEmpty(s))
                    {
                        s = s.Replace("#", "");
                        if (s.Length == 6)
                        {
                            s = "FF" + s;
                        }
                        series.Color = Utils.HexToColor(s);
                    }
                }
            }
            XPathNodeIterator iterator = node.Select("points/point");
            if (iterator.Count != 0)
            {
                valueSource = series.mandatory.valueSource;
                if (valueSource.Length == 0)
                {
                    valueSource = this.DataMember;
                }
                if (valueSource.Length == 0)
                {
                    valueSource = series.mandatory.Name;
                }
                string localName = series.notMandatory.valueSource;
                if (localName.Length == 0)
                {
                    localName = series.notMandatory.Name;
                }
                while (iterator.MoveNext())
                {
                    string str3 = iterator.Current.GetAttribute("text", nameSpaceURI);
                    Color emptyColor = Utils.EmptyColor;
                    string str7 = iterator.Current.GetAttribute("color", nameSpaceURI);
                    if (!Utils.IsNullOrEmpty(str7))
                    {
                        str7 = str7.Replace("#", "");
                        if (str7.Length == 6)
                        {
                            str7 = "FF" + str7;
                        }
                        emptyColor = Utils.HexToColor(str7);
                    }
                    for (int i = 2; i < series.ValuesLists.Count; i++)
                    {
                        string name = series.ValuesLists[i].valueSource;
                        if (name.Length == 0)
                        {
                            name = series.ValuesLists[i].Name;
                        }
                        str7 = iterator.Current.GetAttribute(name, nameSpaceURI);
                        if (!Utils.IsNullOrEmpty(str7))
                        {
                            series.ValuesLists[i].TempValue = Convert.ToDouble(str7);
                        }
                    }
                    string str8 = iterator.Current.GetAttribute(valueSource, nameSpaceURI);
                    if (Utils.IsNullOrEmpty(str8))
                    {
                        str8 = iterator.Current.GetAttribute("Y", nameSpaceURI);
                    }
                    string str9 = iterator.Current.GetAttribute(localName, nameSpaceURI);
                    if (Utils.IsNullOrEmpty(str8))
                    {
                        if (Utils.IsNullOrEmpty(str9))
                        {
                            if (!Utils.IsNullOrEmpty(str3))
                            {
                                series.Add(str3);
                            }
                        }
                        else if (!Utils.IsNullOrEmpty(str3))
                        {
                            series.Add(Convert.ToDouble(str9), (double) 0.0, str3);
                        }
                        else
                        {
                            series.Add(Convert.ToDouble(str9), (double) 0.0);
                        }
                    }
                    else
                    {
                        if (Utils.IsNullOrEmpty(str9))
                        {
                            if (!Utils.IsNullOrEmpty(str3))
                            {
                                if (emptyColor != Utils.EmptyColor)
                                {
                                    series.Add(Convert.ToDouble(str8), str3, emptyColor);
                                }
                                else
                                {
                                    series.Add(Convert.ToDouble(str8), str3);
                                }
                            }
                            else if (emptyColor != Utils.EmptyColor)
                            {
                                series.Add(Convert.ToDouble(str8), emptyColor);
                            }
                            else
                            {
                                series.Add(Convert.ToDouble(str8));
                            }
                            continue;
                        }
                        if (!Utils.IsNullOrEmpty(str3))
                        {
                            if (emptyColor != Utils.EmptyColor)
                            {
                                series.Add(Convert.ToDouble(str9), Convert.ToDouble(str8), str3, emptyColor);
                            }
                            else
                            {
                                series.Add(Convert.ToDouble(str9), Convert.ToDouble(str8), str3);
                            }
                        }
                        else if (emptyColor != Utils.EmptyColor)
                        {
                            series.Add(Convert.ToDouble(str9), Convert.ToDouble(str8), emptyColor);
                        }
                        else
                        {
                            series.Add(Convert.ToDouble(str9), Convert.ToDouble(str8));
                        }
                    }
                }
            }
        }

        private void XMLError(string error)
        {
            throw new Exception(error);
        }

        public Steema.TeeChart.Chart Chart
        {
            get
            {
                return this.iChart;
            }
            set
            {
                this.iChart = value;
            }
        }

        public string DataMember
        {
            get
            {
                return this.sDataMember;
            }
            set
            {
                this.sDataMember = value;
            }
        }

        public string SeriesNode
        {
            get
            {
                return this.sSeriesNode;
            }
            set
            {
                this.sSeriesNode = value;
            }
        }
    }
}

