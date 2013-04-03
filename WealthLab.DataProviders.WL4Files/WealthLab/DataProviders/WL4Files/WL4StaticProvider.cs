namespace WealthLab.DataProviders.WL4Files
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.DataProviders.Helper;
    using WealthLab.DataProviders.WL4Files.Properties;

    public class WL4StaticProvider : StaticDataProvider
    {
        private PageApp pageApp_0;
        private PageDataFolder pageDataFolder_0;
        private PageDataSources pageDataSources_0;
        private PageScale pageScale_0;
        private PageSelectMode pageSelectMode_0;
        private string string_0 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public WL4StaticProvider()
        {
            Class14.smethod_8(new object[0]);
        }

        public override DataSource CreateDataSource()
        {
            DataSource source = new DataSource(this);
            WL4DataSetSettings settings = new WL4DataSetSettings {
                SelectMode = this.pageSelectMode_0.method_2(),
                App = this.pageApp_0.method_0()
            };
            switch (this.pageSelectMode_0.method_2())
            {
                case SelectMode.DataSource:
                    settings.DataBasePath = this.pageDataSources_0.method_3();
                    settings.WL4DataSource = this.pageDataSources_0.method_2();
                    break;

                case SelectMode.Folder:
                    settings.DataFolderPath = this.pageDataFolder_0.method_2();
                    settings.AllSymbolsFromFolder = this.pageDataFolder_0.method_4();
                    settings.Symbols = new Class12(this.pageDataFolder_0.method_3()).ToString();
                    break;
            }
            source.BarInterval = this.pageScale_0.method_0();
            source.Scale = this.pageScale_0.method_3();
            source.DSString = settings.method_0();
            Class14.smethod_7(new object[] { source.Scale, source.BarInterval, source.DSString });
            return source;
        }

        private void method_0(string string_1)
        {
            string str = null;
            string str2 = null;
            foreach (string str3 in string_1.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (str3.StartsWith("Interval"))
                {
                    str = str3.Split(new char[] { '=' })[1];
                }
                if (str3.StartsWith("Scale"))
                {
                    str2 = str3.Split(new char[] { '=' })[1];
                }
            }
            if ((str != null) && (str2 != null))
            {
                int num2 = Convert.ToInt32(str2);
                int num3 = Convert.ToInt32(str);
                if (num2 >= 3)
                {
                    this.pageScale_0.method_1(num3);
                }
                else
                {
                    this.pageScale_0.method_1(1);
                }
                switch (num2)
                {
                    case 0:
                        this.pageScale_0.method_4(BarScale.Daily);
                        return;

                    case 1:
                        this.pageScale_0.method_4(BarScale.Weekly);
                        return;

                    case 2:
                        this.pageScale_0.method_4(BarScale.Monthly);
                        return;

                    case 3:
                        this.pageScale_0.method_4(BarScale.Minute);
                        return;

                    case 4:
                        this.pageScale_0.method_4(BarScale.Second);
                        return;

                    case 5:
                        this.pageScale_0.method_4(BarScale.Tick);
                        break;

                    default:
                        return;
                }
            }
        }

        private void method_1()
        {
            try
            {
                string str;
                string[] strArray2;
                switch (this.pageApp_0.method_0())
                {
                    case App.Dev:
                    {
                        if (this.pageSelectMode_0.method_2() != SelectMode.DataSource)
                        {
                            break;
                        }
                        string details = this.pageDataSources_0.method_2().Details;
                        if (details.StartsWith("UseAdjCloseYahoo") || details.StartsWith("Version"))
                        {
                            this.pageScale_0.method_4(BarScale.Daily);
                            this.pageScale_0.method_1(1);
                        }
                        if (details.StartsWith("Provider"))
                        {
                            this.method_0(details);
                        }
                        return;
                    }
                    case App.Pro:
                        switch (this.pageSelectMode_0.method_2())
                        {
                            case SelectMode.DataSource:
                                goto Label_00C3;

                            case SelectMode.Folder:
                                goto Label_018D;
                        }
                        return;

                    default:
                        return;
                }
                this.pageScale_0.method_2();
                return;
            Label_00C3:
                str = this.pageDataSources_0.method_2().Location;
                if (str.Contains("Daily"))
                {
                    this.pageScale_0.method_4(BarScale.Daily);
                    this.pageScale_0.method_1(1);
                }
                if (str.Contains("Minute"))
                {
                    this.pageScale_0.method_4(BarScale.Minute);
                    strArray2 = str.Split(new char[] { ' ' });
                    this.pageScale_0.method_1(Convert.ToInt32(strArray2[0].Trim(new char[] { '/' })));
                }
                if (this.pageDataSources_0.method_2().Details.StartsWith("Provider"))
                {
                    this.method_0(this.pageDataSources_0.method_2().Details);
                }
                return;
            Label_018D:;
                string[] strArray = this.pageDataFolder_0.method_2().Split(new char[] { Path.DirectorySeparatorChar });
                if (strArray.Length > 2)
                {
                    str = strArray[strArray.Length - 1];
                    if (strArray[strArray.Length - 1].Length == 1)
                    {
                        str = strArray[strArray.Length - 2];
                    }
                    else if (str.Contains("Daily"))
                    {
                        this.pageScale_0.method_4(BarScale.Daily);
                        this.pageScale_0.method_1(1);
                    }
                    if (str.Contains("Minute"))
                    {
                        this.pageScale_0.method_4(BarScale.Minute);
                        strArray2 = str.Split(new char[] { ' ' });
                        this.pageScale_0.method_1(Convert.ToInt32(strArray2[0].Trim(new char[] { '/' })));
                    }
                }
            }
            catch
            {
                this.pageScale_0.method_2();
            }
        }

        public override void PopulateSymbols(DataSource dataSource_0, List<string> symbols)
        {
            WL4DataSetSettings settings = (WL4DataSetSettings) DataSetSettings.smethod_1(dataSource_0.DSString);
            switch (settings.SelectMode)
            {
                case SelectMode.DataSource:
                    switch (settings.App)
                    {
                        case App.Dev:
                            symbols.AddRange(Class13.smethod_10(settings.WL4DataSource.Location));
                            return;

                        case App.Pro:
                            if (settings.WL4DataSource.Type == "F")
                            {
                                symbols.AddRange(Class13.smethod_8(settings.DataBasePath, settings.WL4DataSource.ID));
                                return;
                            }
                            symbols.AddRange(Class13.smethod_10(settings.WL4DataSource.Location));
                            return;
                    }
                    return;

                case SelectMode.Folder:
                    if (!settings.AllSymbolsFromFolder)
                    {
                        Class12 class2 = new Class12(settings.Symbols);
                        symbols.AddRange(Class13.smethod_11(settings.DataFolderPath, class2.list_0));
                        return;
                    }
                    symbols.AddRange(Class13.smethod_10(settings.DataFolderPath));
                    return;
            }
        }

        public override Bars RequestData(DataSource dataSource_0, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            Class14.smethod_8(new object[] { symbol, maxBars });
            WL4DataSetSettings settings = (WL4DataSetSettings) DataSetSettings.smethod_1(dataSource_0.DSString);
            Bars bars = new Bars(symbol, dataSource_0.Scale, dataSource_0.BarInterval);
            string str = string.Empty;
            if (symbol.Length < 1)
            {
                return bars;
            }
            switch (settings.SelectMode)
            {
                case SelectMode.DataSource:
                    switch (settings.App)
                    {
                        case App.Dev:
                            str = Class13.smethod_4(settings.WL4DataSource.Location, symbol);
                            goto Label_014C;

                        case App.Pro:
                            if (settings.WL4DataSource.Type == "F")
                            {
                                string str2 = Class13.smethod_6(App.Pro) + Path.AltDirectorySeparatorChar + settings.WL4DataSource.Location.Trim(new char[] { '/' });
                                string str3 = symbol.ToUpper()[0].ToString();
                                if (this.string_0.Contains(str3))
                                {
                                    str2 = Path.Combine(str2, str3);
                                }
                                str = Class13.smethod_4(str2, symbol);
                            }
                            else
                            {
                                str = Class13.smethod_4(settings.WL4DataSource.Location, symbol);
                            }
                            goto Label_014C;
                    }
                    break;

                case SelectMode.Folder:
                    str = Class13.smethod_4(settings.DataFolderPath, symbol);
                    break;
            }
        Label_014C:;
            Class14.smethod_7(new object[] { str });
            if (str != string.Empty)
            {
                if (((settings.SelectMode == SelectMode.DataSource) && (settings.WL4DataSource.Type == "F")) && (settings.App == App.Pro))
                {
                    Class13.smethod_0(str, App.Pro, ref bars, startDate, endDate, maxBars);
                    return bars;
                }
                App dev = App.Dev;
                if (settings.App == App.Pro)
                {
                    dev = Class13.smethod_2(str);
                }
                Class13.smethod_0(str, dev, ref bars, startDate, endDate, maxBars);
            }
            return bars;
        }

        public override bool SupportsDynamicUpdate(BarScale scale)
        {
            return false;
        }

        public override UserControl WizardFirstPage()
        {
            if (this.pageApp_0 == null)
            {
                this.pageApp_0 = new PageApp();
                this.pageSelectMode_0 = new PageSelectMode();
                this.pageDataSources_0 = new PageDataSources();
                this.pageDataFolder_0 = new PageDataFolder();
                this.pageScale_0 = new PageScale();
            }
            this.pageApp_0.method_1();
            this.pageSelectMode_0.method_4();
            this.pageDataSources_0.method_4();
            this.pageDataSources_0.method_1(App.Unknow);
            this.pageDataFolder_0.method_5();
            this.pageDataFolder_0.method_1(App.Unknow);
            this.pageScale_0.method_2();
            return this.pageApp_0;
        }

        public override UserControl WizardNextPage(UserControl currentPage)
        {
            if (currentPage == this.pageApp_0)
            {
                this.pageSelectMode_0.method_1(this.pageApp_0.method_0());
                return this.pageSelectMode_0;
            }
            if (currentPage == this.pageSelectMode_0)
            {
                if (this.pageSelectMode_0.method_2() == SelectMode.DataSource)
                {
                    this.pageDataSources_0.method_1(this.pageApp_0.method_0());
                    return this.pageDataSources_0;
                }
                this.pageDataFolder_0.method_1(this.pageApp_0.method_0());
                return this.pageDataFolder_0;
            }
            if (currentPage == this.pageDataSources_0)
            {
                if ((this.pageDataSources_0.method_3() == null) || (this.pageDataSources_0.method_3() == string.Empty))
                {
                    throw new WizardValidationException("Path to the Database is not specified.");
                }
                if (this.pageDataSources_0.method_2() == null)
                {
                    throw new WizardValidationException("DataSource is not selected.");
                }
                this.method_1();
                return this.pageScale_0;
            }
            if (currentPage != this.pageDataFolder_0)
            {
                return null;
            }
            if ((this.pageDataFolder_0.method_2() == null) || (this.pageDataFolder_0.method_2() == string.Empty))
            {
                throw new WizardValidationException("Folder is not specified.");
            }
            if (this.pageDataFolder_0.method_3().Count == 0)
            {
                throw new WizardValidationException("Please select some symbols.");
            }
            this.method_1();
            return this.pageScale_0;
        }

        public override UserControl WizardPreviousPage(UserControl currentPage)
        {
            if (currentPage == this.pageSelectMode_0)
            {
                return this.pageApp_0;
            }
            if ((currentPage == this.pageDataSources_0) || (currentPage == this.pageDataFolder_0))
            {
                return this.pageSelectMode_0;
            }
            if (currentPage == this.pageScale_0)
            {
                if (this.pageSelectMode_0.method_2() == SelectMode.DataSource)
                {
                    return this.pageDataSources_0;
                }
                if (this.pageSelectMode_0.method_2() == SelectMode.Folder)
                {
                    return this.pageDataFolder_0;
                }
            }
            return null;
        }

        public override string Description
        {
            get
            {
                return "Provides access to historical data stored in Wealth-Lab Pro/Developer V4 format files.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "WL4 Files";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.WLD;
            }
        }

        public override string SuggestedDataSourceName
        {
            get
            {
                switch (this.pageSelectMode_0.method_2())
                {
                    case SelectMode.DataSource:
                        return this.pageDataSources_0.method_2().Name;

                    case SelectMode.Folder:
                    {
                        string[] strArray = this.pageDataFolder_0.method_2().Split(new char[] { Path.DirectorySeparatorChar });
                        return strArray[strArray.Length - 1].Trim(new char[] { Path.DirectorySeparatorChar });
                    }
                }
                return string.Empty;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www.wealth-lab.com";
            }
        }
    }
}

