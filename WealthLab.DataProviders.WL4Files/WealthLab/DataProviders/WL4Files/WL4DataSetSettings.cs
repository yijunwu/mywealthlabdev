namespace WealthLab.DataProviders.WL4Files
{
    using System;
    using WealthLab.DataProviders.Helper;

    [Serializable]
    internal class WL4DataSetSettings : DataSetSettings
    {
        private bool _allSymbolFromFolder;
        private WealthLab.DataProviders.WL4Files.App _app;
        private string _dataBasePath;
        private string _dataFolderPath;
        private WealthLab.DataProviders.WL4Files.SelectMode _selectMode;
        private string _symbols;
        private WealthLab.DataProviders.WL4Files.WL4DataSource _wlDataSource;

        public bool AllSymbolsFromFolder
        {
            get
            {
                return this._allSymbolFromFolder;
            }
            set
            {
                this._allSymbolFromFolder = value;
            }
        }

        internal WealthLab.DataProviders.WL4Files.App App
        {
            get
            {
                return this._app;
            }
            set
            {
                this._app = value;
            }
        }

        public string DataBasePath
        {
            get
            {
                return this._dataBasePath;
            }
            set
            {
                this._dataBasePath = value;
            }
        }

        public string DataFolderPath
        {
            get
            {
                return this._dataFolderPath;
            }
            set
            {
                this._dataFolderPath = value;
            }
        }

        internal WealthLab.DataProviders.WL4Files.SelectMode SelectMode
        {
            get
            {
                return this._selectMode;
            }
            set
            {
                this._selectMode = value;
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

        internal WealthLab.DataProviders.WL4Files.WL4DataSource WL4DataSource
        {
            get
            {
                return this._wlDataSource;
            }
            set
            {
                this._wlDataSource = value;
            }
        }
    }
}

