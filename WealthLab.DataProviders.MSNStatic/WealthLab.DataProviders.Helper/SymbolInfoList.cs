using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
namespace WealthLab.DataProviders.Helper
{
	public class SymbolInfoList
	{
		public List<SymbolInfo> Items = new List<SymbolInfo>();
		public bool Add(string name, BarScale scale, int interval, DateTime startDate)
		{
			int num = this.method_0(name, scale, interval);
			if (num == -1)
			{
				this.Items.Add(new SymbolInfo(name, scale, interval, startDate));
				return true;
			}
			if (Delegate178.smethod_1(startDate, this.Items[num].StartDate))
			{
				this.Items[num].StartDate = startDate;
				return true;
			}
			return false;
		}
		public bool Add(SymbolInfo symbolInfo)
		{
			return this.Add(symbolInfo.Name, symbolInfo.Scale, symbolInfo.Interval, symbolInfo.StartDate);
		}
		public DateTime Search(string name, BarScale scale, int interval)
		{
			int num = this.method_0(name, scale, interval);
			if (num != -1)
			{
				return this.Items[num].StartDate;
			}
			return DateTime.MaxValue;
		}
		public void Synchronize(BarDataStore dataStore)
		{
			for (int i = this.Items.Count - 1; i >= 0; i--)
			{
				if (!Delegate161.smethod_0(this.Items[i].Name) && !Delegate205.smethod_0(dataStore, this.Items[i].Name, this.Items[i].Scale, this.Items[i].Interval))
				{
					this.Items.RemoveAt(i);
				}
			}
		}
		private int method_0(string string_0, BarScale barScale_0, int int_0)
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (Delegate51.smethod_0(this.Items[i].Name, string_0) && this.Items[i].Scale == barScale_0 && this.Items[i].Interval == int_0)
				{
					return i;
				}
			}
			return -1;
		}
		public void Serialize(string fileName)
		{
			XmlSerializer object_ = Delegate45.smethod_0(Delegate253.smethod_0(typeof(SymbolInfoList).TypeHandle));
			TextWriter textWriter = Delegate46.smethod_0(fileName);
			Delegate292.smethod_0(object_, textWriter, this);
			Delegate293.smethod_0(textWriter);
		}
		public static SymbolInfoList Deserialize(string fileName)
		{
			if (Delegate161.smethod_1(fileName))
			{
				XmlSerializer object_ = Delegate45.smethod_0(Delegate253.smethod_0(typeof(SymbolInfoList).TypeHandle));
				TextReader textReader = Delegate47.smethod_0(fileName);
				SymbolInfoList result = (SymbolInfoList)Delegate294.smethod_0(object_, textReader);
				Delegate295.smethod_0(textReader);
				return result;
			}
			return new SymbolInfoList();
		}
	}
}
