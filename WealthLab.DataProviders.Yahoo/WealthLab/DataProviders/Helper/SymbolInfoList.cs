namespace WealthLab.DataProviders.Helper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using WealthLab;

    public class SymbolInfoList
    {
        public List<WealthLab.DataProviders.Helper.SymbolInfo> Items = new List<WealthLab.DataProviders.Helper.SymbolInfo>();

        public bool Add(WealthLab.DataProviders.Helper.SymbolInfo symbolInfo)
        {
            return this.Add(symbolInfo.Name, symbolInfo.Scale, symbolInfo.Interval, symbolInfo.StartDate);
        }

        public bool Add(string name, BarScale scale, int interval, DateTime startDate)
        {
            if (!string.IsNullOrEmpty(name))
            {
                int index = this.indexOf(name, scale, interval);
                if (index == -1)
                {
                    this.Items.Add(new WealthLab.DataProviders.Helper.SymbolInfo(name, scale, interval, startDate));
                    return true;
                }
                if (startDate < this.Items[index].StartDate)
                {
                    this.Items[index].StartDate = startDate;
                    return true;
                }
            }
            return false;
        }

        public static SymbolInfoList Deserialize(string fileName)
        {
            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SymbolInfoList));
                TextReader textReader = new StreamReader(fileName);
                SymbolInfoList list = (SymbolInfoList) serializer.Deserialize(textReader);
                textReader.Close();
                return list;
            }
            return new SymbolInfoList();
        }

        private int indexOf(string string_0, BarScale barScale, int interval)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (((this.Items[i].Name == string_0) && (this.Items[i].Scale == barScale)) && (this.Items[i].Interval == interval))
                {
                    return i;
                }
            }
            return -1;
        }

        public DateTime Search(string name, BarScale scale, int interval)
        {
            int num = this.indexOf(name, scale, interval);
            if (num != -1)
            {
                return this.Items[num].StartDate;
            }
            return DateTime.MaxValue;
        }

        public void Serialize(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SymbolInfoList));
            TextWriter textWriter = new StreamWriter(fileName);
            serializer.Serialize(textWriter, this);
            textWriter.Close();
        }

        public void Synchronize(BarDataStore dataStore)
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                if (!string.IsNullOrEmpty(this.Items[i].Name) && !dataStore.ContainsSymbol(this.Items[i].Name, this.Items[i].Scale, this.Items[i].Interval))
                {
                    this.Items.RemoveAt(i);
                }
            }
        }
    }
}

