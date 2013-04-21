using System;
namespace WealthLab.DataProviders.Helper
{
	public class SymbolInfo
	{
		public string Name;
		public BarScale Scale;
		public int Interval;
		public DateTime StartDate;
		public SymbolInfo()
		{
		}
		public SymbolInfo(string name, BarScale scale, int interval, DateTime startDate)
		{
			this.Name = name;
			this.Scale = scale;
			this.Interval = interval;
			this.StartDate = startDate;
		}
	}
}
