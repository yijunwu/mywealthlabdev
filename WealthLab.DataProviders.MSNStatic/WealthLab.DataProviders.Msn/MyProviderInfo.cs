using System;
using WealthLab.DataProviders.MarketManagerService;
namespace WealthLab.DataProviders.Msn
{
	public class MyProviderInfo : MarketManagerInfo
	{
		public override string ProviderName()
		{
			return "MSN";
		}
	}
}
