using System;
using WealthLab.DataProviders.Helper;
namespace WealthLab.DataProviders.Msn
{
	[Serializable]
	public class MsnStaticSettings : DataSetSettings
	{
		public int Version = 2;
		private string _symbols = string.Empty;
		private bool _updateGroups;
		private string _groups = string.Empty;
		private DateTime _startDate = new DateTime(2000, 1, 1);
		public string Symbols
		{
			get
			{
				return this._symbols;
			}
			set
			{
				this._symbols = value;
			}
		}
		public bool UpdateGroups
		{
			get
			{
				return this._updateGroups;
			}
			set
			{
				this._updateGroups = value;
			}
		}
		public string Groups
		{
			get
			{
				return this._groups;
			}
			set
			{
				this._groups = value;
			}
		}
		public DateTime StartDate
		{
			get
			{
				return this._startDate;
			}
			set
			{
				this._startDate = value;
			}
		}
	}
}
