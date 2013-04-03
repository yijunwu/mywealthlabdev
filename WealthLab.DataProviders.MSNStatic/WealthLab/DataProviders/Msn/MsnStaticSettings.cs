namespace WealthLab.DataProviders.Msn
{
    using System;
    using WealthLab.DataProviders.Helper;

    [Serializable]
    public class MsnStaticSettings : DataSetSettings
    {
        private string _groups = string.Empty;
        private DateTime _startDate = new DateTime(0x7d0, 1, 1);
        private string _symbols = string.Empty;
        private bool _updateGroups;
        public int Version = 2;

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
    }
}

