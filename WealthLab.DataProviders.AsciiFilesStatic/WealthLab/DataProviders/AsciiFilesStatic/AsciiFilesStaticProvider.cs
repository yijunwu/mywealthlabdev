namespace WealthLab.DataProviders.AsciiFilesStatic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.DataProviders.AsciiFilesStatic.Properties;
    using WealthLab.DataProviders.Helper;
    using WealthLab.DataProviders.MarketManagerService;

    public class AsciiFilesStaticProvider : StaticDataProvider
    {
        private AsciiFilesWizardFieldOptions asciiFilesWizardFieldOptions_0;
        private AsciiFilesWizardFolderPage asciiFilesWizardFolderPage_0;
        public static string ProviderName = "ASCII";
        public static string RootDataPath = "";
        private TextFilesWizardPageData textFilesWizardPageData_0;
        private TextFilesWizardPageScale textFilesWizardPageScale_0;

        public override DataSource CreateDataSource()
        {
            DataSource source = new DataSource(this) {
                DSString = this.asciiFilesWizardFieldOptions_0.asciiFilesDataSet_0.SerializeToString(),
                Scale = (BarScale) this.textFilesWizardPageScale_0.method_1(),
                BarInterval = this.textFilesWizardPageScale_0.method_0()
            };
            Class5.smethod_2(string.Format("CreateDataSource: DSString {0}, BarScale {1}, BarInterval {2}", source.DSString, source.Scale, source.BarInterval));
            return source;
        }

        public override MarketInfo GetMarketInfo(string symbol)
        {
            return MarketManager.GetMarketInfo(symbol, "Eastern Standard Time", ProviderName);
        }

        public override void Initialize(IDataHost dataHost)
        {
            base.Initialize(dataHost);
            BarDataStore store = new BarDataStore(dataHost, this);
            RootDataPath = store.RootPath;
        }

        public override void PopulateSymbols(DataSource dataSource_0, List<string> symbols)
        {
            AsciiFilesDataSet set = (AsciiFilesDataSet) DataSetSettings.DeserializeFromString(dataSource_0.DSString);
            symbols.AddRange(AsciiFilesDataSet.GetFilesFromDir(set.Folder, set.Extension, false, false));
        }

        public override Bars RequestData(DataSource dataSource_0, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            Class5.smethod_8(new object[] { dataSource_0.DSString, symbol, startDate, endDate, maxBars, includePartialBar });
            Bars bars = new Bars(symbol, dataSource_0.Scale, dataSource_0.BarInterval);
            try
            {
                AsciiFilesDataSet set = (AsciiFilesDataSet) DataSetSettings.DeserializeFromString(dataSource_0.DSString);
                string fileName = set.GetFileName(symbol);
                if (!File.Exists(fileName))
                {
                    Class5.smethod_4(TraceEventType.Warning, "RequestData. File not found " + fileName);
                    return bars;
                }
                if (Class6.smethod_6().EnableCache)
                {
                    Bars bars4 = Class6.smethod_7(fileName, dataSource_0.Name);
                    if (bars4 != null)
                    {
                        bars = bars4;
                        bars.SecurityName = "#Cache#" + bars.SecurityName;
                    }
                }
                if (bars.Count == 0)
                {
                    set.Parse(fileName, bars);
                }
                if (Class6.smethod_6().EnableCache && (bars.Count > 0))
                {
                    Class6.smethod_10(bars, fileName, dataSource_0.Name);
                }
            }
            catch (Exception exception)
            {
                Class5.smethod_4(TraceEventType.Error, "RequestData. " + exception.Message);
                MessageBox.Show(string.Format("{0}\r\n\r\n{1}", exception.Message, exception.StackTrace), "ASCII Files DataSet Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return MarketManager.ConvertBars(bars, startDate, endDate, maxBars, ProviderName);
        }

        public override bool SupportsDynamicUpdate(BarScale scale)
        {
            return false;
        }

        public override UserControl WizardFirstPage()
        {
            if (this.asciiFilesWizardFolderPage_0 == null)
            {
                this.asciiFilesWizardFolderPage_0 = new AsciiFilesWizardFolderPage();
                this.asciiFilesWizardFieldOptions_0 = new AsciiFilesWizardFieldOptions();
                this.textFilesWizardPageScale_0 = new TextFilesWizardPageScale();
                this.textFilesWizardPageData_0 = new TextFilesWizardPageData();
            }
            this.asciiFilesWizardFolderPage_0.method_3();
            this.textFilesWizardPageScale_0.method_2();
            this.asciiFilesWizardFieldOptions_0.method_2();
            this.textFilesWizardPageData_0.method_0();
            return this.asciiFilesWizardFolderPage_0;
        }

        public override UserControl WizardNextPage(UserControl currentPage)
        {
            if (currentPage == this.asciiFilesWizardFolderPage_0)
            {
                if (this.asciiFilesWizardFolderPage_0.method_0().Trim() == "")
                {
                    throw new WizardValidationException("Folder is not selected.");
                }
                if ((this.asciiFilesWizardFolderPage_0.method_1().Trim() == "") || (this.asciiFilesWizardFolderPage_0.method_1() == "*.*"))
                {
                    throw new WizardValidationException("File Extension is not selected.");
                }
                if (this.asciiFilesWizardFolderPage_0.method_2() == 0)
                {
                    throw new WizardValidationException(string.Format("Files with extension \"{0}\" are not present in the folder.", this.asciiFilesWizardFolderPage_0.method_1()));
                }
                return this.textFilesWizardPageScale_0;
            }
            if (currentPage == this.textFilesWizardPageScale_0)
            {
                this.asciiFilesWizardFieldOptions_0.asciiFilesDataSet_0.Folder = this.asciiFilesWizardFolderPage_0.method_0();
                this.asciiFilesWizardFieldOptions_0.asciiFilesDataSet_0.Extension = this.asciiFilesWizardFolderPage_0.method_1();
                return this.asciiFilesWizardFieldOptions_0;
            }
            if (currentPage == this.asciiFilesWizardFieldOptions_0)
            {
                this.asciiFilesWizardFieldOptions_0.method_5(this.asciiFilesWizardFieldOptions_0.asciiFilesDataSet_0);
                this.textFilesWizardPageData_0 = new TextFilesWizardPageData();
                this.textFilesWizardPageData_0.asciiFilesDataSet_0 = this.asciiFilesWizardFieldOptions_0.asciiFilesDataSet_0;
                return this.textFilesWizardPageData_0;
            }
            if ((currentPage == this.textFilesWizardPageData_0) && this.textFilesWizardPageData_0.asciiFilesDataSet_0.ParseError)
            {
                throw new WizardValidationException("Parsing failed, return to previous page.");
            }
            return null;
        }

        public override UserControl WizardPreviousPage(UserControl currentPage)
        {
            if (currentPage == this.textFilesWizardPageScale_0)
            {
                return this.asciiFilesWizardFolderPage_0;
            }
            if (currentPage == this.asciiFilesWizardFieldOptions_0)
            {
                return this.textFilesWizardPageScale_0;
            }
            if (currentPage == this.textFilesWizardPageData_0)
            {
                return this.asciiFilesWizardFieldOptions_0;
            }
            return null;
        }

        public override string Description
        {
            get
            {
                return "Provides access to historical data stored in ASCII files. Each security should reside in its own file.";
            }
        }

        public override IList<DataBehaviorUserControl> ExtendedBehaviors
        {
            get
            {
                AsciiCacheControl control = new AsciiCacheControl();
                return new List<DataBehaviorUserControl> { control };
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "ASCII Files";
            }
        }

        public override Bitmap Glyph
        {
            get
            {
                return Resources.TextFiles.ToBitmap();
            }
        }

        public override string SuggestedDataSourceName
        {
            get
            {
                string[] strArray = this.asciiFilesWizardFolderPage_0.method_0().Split(new char[] { Path.DirectorySeparatorChar });
                if (strArray.Length > 0)
                {
                    return strArray[strArray.Length - 1];
                }
                return "";
            }
        }

        public override string URL
        {
            get
            {
                return "http://wealth-lab.com";
            }
        }
    }
}

