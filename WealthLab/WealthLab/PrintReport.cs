namespace WealthLab
{
    using System;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class PrintReport
    {
        private Bitmap bitmap_0;
        private bool _printSomePages;
        private bool showPrintDialog;
        private bool showPrintPreview;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private bool bool_6;
        private bool _fPrintListView;
        private bool _fPrintText;
        private bool bool_9;
        private DataObject _printObject;
        public static DataFormats.Format fmtBaseTitle = DataFormats.GetFormat("BaseTitle");
        public static DataFormats.Format fmtDetails = DataFormats.GetFormat("Details");
        public static DataFormats.Format fmtDisclosure = DataFormats.GetFormat("Disclosure");
        public static DataFormats.Format fmtGraphic = DataFormats.GetFormat("Graphic");
        public static DataFormats.Format fmtListView = DataFormats.GetFormat("ListView");
        public static DataFormats.Format fmtPrintChart = DataFormats.GetFormat("ChartReport");
        public static DataFormats.Format fmtStrategy = DataFormats.GetFormat("Strategy");
        public static DataFormats.Format fmtSymbol = DataFormats.GetFormat("Symbol");
        public static DataFormats.Format fmtTitle = DataFormats.GetFormat("Title");
        private Font font_0;
        private Font _printFontBody;
        private Font _printFontDetail;
        private Font _printFontFooter;
        private int _intPageCounter;
        private int _startPageCount;
        private int _endPageCount;
        private int int_3;
        private int _lvRow;
        private ListView listView_0;
        private PrintPreview printPreview_0;
        private string basePrintTitle;
        private string string_1;
        private string _printTitle;
        private string _printStrategy;
        private string _printSymbol;
        private string _printDetails;
        private string _printText;
        private string _rptDisclosure;
        private static string defaultDisclosure = "Backtesting provides a hypothetical calculation of how a security or portfolio of securities, subject to a trading strategy, would have performed over a historical time period. You should not assume that backtesting of a trading strategy will provide any indication of how your portfolio of securities, or a new portfolio of securities, might perform over time. You should choose your own trading strategies based on your particular objectives and risk tolerances. Be sure to review your decisions periodically to make sure they are still consistent with your goals. Past performance is no guarantee of future results.";

        public PrintReport()
        {
            this.string_1 = "Wealth-Lab Pro\x00ae";
            this._rptDisclosure = "";
            this.method_0();
        }

        public PrintReport(bool bUseDefaultDisclosure)
        {
            this.string_1 = "Wealth-Lab Pro\x00ae";
            this._rptDisclosure = "";
            this.method_0();
            this.UseDefaultDisclosure();
        }

        public PrintReport(DataObject doPrint)
        {
            this.string_1 = "Wealth-Lab Pro\x00ae";
            this._rptDisclosure = "";
            this.method_0();
            this._printObject = doPrint;
        }

        public PrintReport(DataObject doPrint, bool bUseDefaultDisclosure)
        {
            this.string_1 = "Wealth-Lab Pro\x00ae";
            this._rptDisclosure = "";
            this.method_0();
            this._printObject = doPrint;
            this.UseDefaultDisclosure();
        }

        public static Bitmap CaptureScreen(Control control)
        {
            Point upperLeftSource = control.PointToScreen(new Point(control.Left, control.Top));
            Graphics g = control.CreateGraphics();
            Size blockRegionSize = control.Size;
            Bitmap image = new Bitmap(blockRegionSize.Width, blockRegionSize.Height, g);
            Graphics.FromImage(image).CopyFromScreen(upperLeftSource, new Point(0, 0), blockRegionSize);
            g.Dispose();
            return image;
        }

        public SizeF GetDisclosureRect(PrintPageEventArgs printPageEventArgs_0, ref Rectangle destRect)
        {
            SizeF ef = new SizeF(0f, 0f);
            if (this.bool_9)
            {
                int charactersFitted = 0;
                int linesFilled = 0;
                ef = printPageEventArgs_0.Graphics.MeasureString(this.rptDisclosure, this.printFontFooter, (SizeF) destRect.Size, StringFormat.GenericDefault, out charactersFitted, out linesFilled);
                destRect.Height -= ((int) ef.Height) + this.int_3;
            }
            return ef;
        }

        public void graphicReport_BeginPrint(object sender, PrintEventArgs e)
        {
            if ((this.printPreview_0 != null) && this.printPreview_0.PrintSomePages)
            {
                this._startPageCount = this.printPreview_0.FromPage;
                this._endPageCount = this.printPreview_0.ToPage;
            }
            this._intPageCounter = 1;
            if (this._printObject.GetDataPresent(fmtBaseTitle.Name))
            {
                this.BasePrintTitle = this._printObject.GetData(fmtBaseTitle.Name).ToString();
            }
            if (this._printObject.GetDataPresent(fmtTitle.Name))
            {
                this.printTitle = this._printObject.GetData(fmtTitle.Name).ToString();
            }
            if (this._printObject.GetDataPresent(fmtStrategy.Name))
            {
                this.printStrategy = this._printObject.GetData(fmtStrategy.Name).ToString();
            }
            if (this._printObject.GetDataPresent(fmtSymbol.Name))
            {
                this.printSymbol = this._printObject.GetData(fmtSymbol.Name).ToString();
            }
            if (this._printObject.GetDataPresent(fmtDetails.Name))
            {
                this.printDetails = this._printObject.GetData(fmtDetails.Name).ToString();
            }
            if (this._printObject.GetDataPresent(fmtGraphic.Name))
            {
                this.printGraphic = this._printObject.GetData(fmtGraphic.Name) as Bitmap;
            }
            if (this._printObject.GetDataPresent(fmtListView.Name))
            {
                this.printListView = this._printObject.GetData(fmtListView.Name) as ListView;
                this._lvRow = 0;
            }
            if (this._printObject.GetDataPresent(fmtDisclosure.Name))
            {
                this.rptDisclosure = this._printObject.GetData(fmtDisclosure.Name).ToString();
            }
        }

        public void graphicReport_EndPrint(object sender, PrintEventArgs e)
        {
            if (this.printPreview_0 != null)
            {
                int num;
                this.printPreview_0.FromPage = 1;
                this.intPageCounter = num = this.intPageCounter - 1;
                this.printPreview_0.ToPage = this.endPageCount = num;
            }
        }

        public void graphicReport_PrintPage(object sender, PrintPageEventArgs e)
        {
            bool printPage = true;
            Rectangle destRect = new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, e.MarginBounds.Width, e.MarginBounds.Height);
            this.int_3 = ((int) e.Graphics.MeasureString("Test", this._printFontBody).Height) + 1;
            if (this.printSomePages && (this.intPageCounter < this.startPageCount))
            {
                printPage = false;
            }
            else
            {
                this.PrintTitle(e, ref destRect);
                this.PrintHeader(e, ref destRect);
            }
            SizeF disclosureRect = this.GetDisclosureRect(e, ref destRect);
            if (this.bool_6)
            {
                this.PrintGraphic(e, ref destRect, printPage);
                destRect.Y += this.int_3;
                destRect.Height -= this.int_3;
            }
            if (this.fPrintListView)
            {
                this.PrintListView(e, ref destRect, printPage);
            }
            this.PrintFooter(e, printPage, ref destRect, disclosureRect);
            e.HasMorePages = this.bool_6 | this.fPrintListView;
            if ((++this.intPageCounter > this.endPageCount) && this.printSomePages)
            {
                e.HasMorePages = false;
            }
        }

        private void method_0()
        {
            this.font_0 = new Font("Arial", 14f, FontStyle.Bold);
            this._printFontBody = new Font("Arial", 10f);
            this._printFontDetail = new Font("Arial", 8f);
            this._printFontFooter = new Font("Arial", 6f);
            this._intPageCounter = 0;
            this._endPageCount = 0;
            this._printSomePages = false;
            this._printObject = new DataObject();
            this.basePrintTitle = this.string_1;
            this.showPrintDialog = true;
            this.showPrintPreview = true;
        }

        public void PrintFooter(PrintPageEventArgs printPageEventArgs_0, bool printPage, ref Rectangle destRect, SizeF disclosureSizeF)
        {
            if (printPage)
            {
                if (this.bool_9)
                {
                    destRect.Y += destRect.Height;
                    destRect.Height += ((int) disclosureSizeF.Height) + this.int_3;
                    printPageEventArgs_0.Graphics.DrawString(this.rptDisclosure, this.printFontFooter, Brushes.Black, destRect, StringFormat.GenericDefault);
                }
                destRect.Y = printPageEventArgs_0.MarginBounds.Bottom;
                if (destRect.Height < (2 * this.int_3))
                {
                    destRect.Height = 2 * this.int_3;
                }
                printPageEventArgs_0.Graphics.DrawString("Fidelity Brokerage Services, Member NYSE, SIPC\n900 Salem Street, Smithfield, RI 02917", this.printFontDetail, Brushes.Black, destRect);
                StringFormat format = new StringFormat {
                    Alignment = StringAlignment.Far
                };
                printPageEventArgs_0.Graphics.DrawString("Page " + this.intPageCounter.ToString(), this.printFontBody, Brushes.Black, destRect, format);
            }
        }

        public void PrintGraphic(PrintPageEventArgs printPageEventArgs_0, ref Rectangle destRect, bool printPage)
        {
            float num = ((float) this.printGraphic.Width) / ((float) this.printGraphic.Height);
            float num2 = ((float) destRect.Width) / num;
            if (num2 > destRect.Height)
            {
                destRect.Width = (int) (destRect.Height * num);
            }
            else
            {
                destRect.Height = (int) num2;
            }
            printPageEventArgs_0.Graphics.DrawImage(this.printGraphic, destRect);
            destRect.Y += destRect.Height;
            if (printPageEventArgs_0.MarginBounds.Height > destRect.Y)
            {
                destRect.Height = printPageEventArgs_0.MarginBounds.Height - destRect.Y;
            }
            else
            {
                destRect.Height = 0;
            }
            this.bool_6 = false;
        }

        public void PrintGraphicReport(PageSettings _pageSettings)
        {
            PrintDocument prtdoc = new PrintDocument();
            prtdoc.BeginPrint += new PrintEventHandler(this.graphicReport_BeginPrint);
            prtdoc.PrintPage += new PrintPageEventHandler(this.graphicReport_PrintPage);
            prtdoc.EndPrint += new PrintEventHandler(this.graphicReport_EndPrint);
            prtdoc.DefaultPageSettings = _pageSettings;
            if (this.showPrintPreview)
            {
                this.printPreview_0 = new PrintPreview(prtdoc);
                this.printPreview_0.ShowPrintDialog = this.showPrintDialog;
                this.printPreview_0.ShowDialog();
                prtdoc.Dispose();
            }
            else
            {
                PrintDialog dialog = new PrintDialog {
                    Document = prtdoc
                };
                if (this.showPrintDialog)
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        dialog.Document.Print();
                        dialog.Dispose();
                    }
                }
                else
                {
                    dialog.Document.Print();
                    dialog.Dispose();
                }
            }
        }

        public void PrintHeader(PrintPageEventArgs printPageEventArgs_0, ref Rectangle destRect)
        {
            string s = DateTime.Now.ToString();
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Far
            };
            printPageEventArgs_0.Graphics.DrawString(s, this._printFontDetail, Brushes.Black, destRect, format);
            int num = ((int) printPageEventArgs_0.Graphics.MeasureString(s, this._printFontBody).Height) + 1;
            if (this.bool_3)
            {
                printPageEventArgs_0.Graphics.DrawString("Strategy: " + this.printStrategy, this._printFontBody, Brushes.Black, destRect);
                this.bool_3 = false;
                destRect.Y += num;
                destRect.Height -= num;
            }
            if (this.bool_4)
            {
                printPageEventArgs_0.Graphics.DrawString("Dataset/Symbol: " + this.printSymbol, this._printFontBody, Brushes.Black, destRect);
                destRect.Y += num;
                destRect.Height -= num;
                this.bool_4 = false;
            }
            if (this.bool_5)
            {
                SizeF ef2 = printPageEventArgs_0.Graphics.MeasureString(this.printDetails, this._printFontDetail, destRect.Width);
                destRect.Y += num;
                destRect.Height -= num;
                printPageEventArgs_0.Graphics.DrawString(this.printDetails, this._printFontDetail, Brushes.Black, destRect);
                destRect.Y += ((int) ef2.Height) + 1;
                destRect.Height -= ((int) ef2.Height) + 1;
                this.bool_5 = false;
            }
            destRect.Y += num;
            destRect.Height -= num;
        }

        public void PrintListView(PrintPageEventArgs printPageEventArgs_0, ref Rectangle destRect, bool printPage)
        {
            if (this.int_3 == 0)
            {
                this.int_3 = ((int) printPageEventArgs_0.Graphics.MeasureString("Test", this.printListView.Font).Height) + 1;
            }
            RectangleF layoutRectangle = new RectangleF((float) destRect.X, (float) destRect.Y, 0f, (float) destRect.Height);
            int num4 = 0;
            bool flag = false;
            foreach (ColumnHeader header3 in this.printListView.Columns)
            {
                num4 += header3.Width + 3;
                if (header3.Text.Length > 0)
                {
                    flag = true;
                }
            }
            float num = ((float) destRect.Width) / ((float) num4);
            if (flag)
            {
                int num3 = this.int_3;
                foreach (ColumnHeader header2 in this.printListView.Columns)
                {
                    if (header2.Width > 3)
                    {
                        layoutRectangle.Width = header2.Width * num;
                        if (header2.Text.Length > 0)
                        {
                            StringFormat format2 = new StringFormat();
                            if (header2.TextAlign == HorizontalAlignment.Left)
                            {
                                format2.Alignment = StringAlignment.Near;
                            }
                            else if (header2.TextAlign == HorizontalAlignment.Center)
                            {
                                format2.Alignment = StringAlignment.Center;
                            }
                            else if (header2.TextAlign == HorizontalAlignment.Right)
                            {
                                format2.Alignment = StringAlignment.Far;
                            }
                            int num2 = ((int) printPageEventArgs_0.Graphics.MeasureString(header2.Text, this.printListView.Font, new SizeF(layoutRectangle.Width, layoutRectangle.Height)).Height) + 1;
                            if (num2 > num3)
                            {
                                num3 = num2;
                            }
                            printPageEventArgs_0.Graphics.DrawString(header2.Text, this.printListView.Font, Brushes.Black, layoutRectangle, format2);
                        }
                        layoutRectangle.X += layoutRectangle.Width + 3f;
                    }
                }
                layoutRectangle.Y += num3;
                destRect.Y += num3;
                destRect.Height -= num3;
            }
            while (this._lvRow < this.printListView.Items.Count)
            {
                int num5 = this.int_3;
                layoutRectangle.X = destRect.X;
                for (int i = 0; i < this.printListView.Items[this._lvRow].SubItems.Count; i++)
                {
                    if (this.printListView.Columns.Count > i)
                    {
                        ColumnHeader header = this.printListView.Columns[i];
                        if (header.Width > 3)
                        {
                            layoutRectangle.Width = header.Width * num;
                            ListViewItem.ListViewSubItem item = this.printListView.Items[this._lvRow].SubItems[i];
                            if (item.Text.Length > 0)
                            {
                                StringFormat format = new StringFormat();
                                if (header.TextAlign == HorizontalAlignment.Left)
                                {
                                    format.Alignment = StringAlignment.Near;
                                }
                                else if (header.TextAlign == HorizontalAlignment.Center)
                                {
                                    format.Alignment = StringAlignment.Center;
                                }
                                else if (header.TextAlign == HorizontalAlignment.Right)
                                {
                                    format.Alignment = StringAlignment.Far;
                                }
                                int num7 = ((int) printPageEventArgs_0.Graphics.MeasureString(item.Text, item.Font, new SizeF(layoutRectangle.Width, layoutRectangle.Height)).Height) + 1;
                                if (num7 > num5)
                                {
                                    num5 = num7;
                                }
                                printPageEventArgs_0.Graphics.DrawString(item.Text, item.Font, Brushes.Black, layoutRectangle, format);
                            }
                            layoutRectangle.X += layoutRectangle.Width + 3f;
                        }
                    }
                }
                layoutRectangle.Y += num5;
                destRect.Y += num5;
                destRect.Height -= num5;
                if (destRect.Height < num5)
                {
                    this._lvRow++;
                    break;
                }
                this._lvRow++;
            }
            if (this._lvRow >= this.printListView.Items.Count)
            {
                this.fPrintListView = false;
            }
        }

        public void PrintText(PrintPageEventArgs printPageEventArgs_0, ref Rectangle destRect, bool printPage)
        {
            int charactersFitted = 0;
            int linesFilled = 0;
            printPageEventArgs_0.Graphics.MeasureString(this.printText, this.printFontBody, (SizeF) destRect.Size, StringFormat.GenericDefault, out charactersFitted, out linesFilled);
            if (printPage)
            {
                printPageEventArgs_0.Graphics.DrawString(this.printText, this.printFontBody, Brushes.Black, destRect, StringFormat.GenericDefault);
            }
            destRect.Y += destRect.Height;
            if (printPageEventArgs_0.MarginBounds.Height > destRect.Y)
            {
                destRect.Height = printPageEventArgs_0.MarginBounds.Height - destRect.Y;
            }
            else
            {
                destRect.Height = 0;
            }
            this.printText = this.printText.Substring(charactersFitted);
            this._fPrintText = this.printText.Length > 0;
        }

        public void PrintTitle(PrintPageEventArgs printPageEventArgs_0, ref Rectangle destRect)
        {
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Center
            };
            printPageEventArgs_0.Graphics.DrawString(this.basePrintTitle + " " + this.printTitle, this.font_0, Brushes.Black, destRect, format);
            SizeF ef = printPageEventArgs_0.Graphics.MeasureString(this.basePrintTitle + " " + this.printTitle, this.font_0);
            destRect.Y += ((int) ef.Height) + 1;
            destRect.Height -= ((int) ef.Height) + 1;
        }

        public void UseDefaultDisclosure()
        {
            this.rptDisclosure = DefaultDisclosure;
        }

        public string BasePrintTitle
        {
            get
            {
                return this.basePrintTitle;
            }
            set
            {
                this.basePrintTitle = value;
            }
        }

        public static string DefaultDisclosure
        {
            get
            {
                return defaultDisclosure;
            }
        }

        public int endPageCount
        {
            get
            {
                return this._endPageCount;
            }
            set
            {
                this._endPageCount = value;
            }
        }

        public bool fPrintListView
        {
            get
            {
                return this._fPrintListView;
            }
            set
            {
                this._fPrintListView = value;
            }
        }

        public bool fPrintText
        {
            get
            {
                return this._fPrintText;
            }
            set
            {
                this._fPrintText = value;
            }
        }

        public int intPageCounter
        {
            get
            {
                return this._intPageCounter;
            }
            set
            {
                this._intPageCounter = value;
            }
        }

        public int lvRow
        {
            get
            {
                return this._lvRow;
            }
            set
            {
                this._lvRow = value;
            }
        }

        public string printDetails
        {
            get
            {
                return this._printDetails;
            }
            set
            {
                this._printDetails = value;
                this.bool_5 = true;
            }
        }

        public Font printFontBody
        {
            get
            {
                return this._printFontBody;
            }
            set
            {
                this._printFontBody = value;
            }
        }

        public Font printFontDetail
        {
            get
            {
                return this._printFontDetail;
            }
            set
            {
                this._printFontDetail = value;
            }
        }

        public Font printFontFooter
        {
            get
            {
                return this._printFontFooter;
            }
            set
            {
                this._printFontFooter = value;
            }
        }

        public Font printFontTitle
        {
            get
            {
                return this.font_0;
            }
            set
            {
                this.font_0 = value;
            }
        }

        public Bitmap printGraphic
        {
            get
            {
                return this.bitmap_0;
            }
            set
            {
                this.bitmap_0 = value;
                this.bool_6 = true;
            }
        }

        public ListView printListView
        {
            get
            {
                return this.listView_0;
            }
            set
            {
                this.listView_0 = value;
                this.fPrintListView = true;
                this._lvRow = 0;
            }
        }

        public DataObject printObject
        {
            get
            {
                return this._printObject;
            }
            set
            {
                this._printObject = value;
            }
        }

        public bool printSomePages
        {
            get
            {
                return this._printSomePages;
            }
            set
            {
                this._printSomePages = value;
            }
        }

        public string printStrategy
        {
            get
            {
                return this._printStrategy;
            }
            set
            {
                this._printStrategy = value;
                this.bool_3 = true;
            }
        }

        public string printSymbol
        {
            get
            {
                return this._printSymbol;
            }
            set
            {
                this._printSymbol = value;
                this.bool_4 = true;
            }
        }

        public string printText
        {
            get
            {
                return this._printText;
            }
            set
            {
                this._printText = value;
                this.fPrintText = true;
            }
        }

        public string printTitle
        {
            get
            {
                return this._printTitle;
            }
            set
            {
                this._printTitle = value;
            }
        }

        public string rptDisclosure
        {
            get
            {
                return this._rptDisclosure;
            }
            set
            {
                this._rptDisclosure = value;
                this.bool_9 = true;
            }
        }

        public bool ShowPrintDialog
        {
            get
            {
                return this.showPrintDialog;
            }
            set
            {
                this.showPrintDialog = value;
            }
        }

        public bool ShowPrintPreview
        {
            get
            {
                return this.showPrintPreview;
            }
            set
            {
                this.showPrintPreview = value;
            }
        }

        public int startPageCount
        {
            get
            {
                return this._startPageCount;
            }
            set
            {
                this._startPageCount = value;
            }
        }
    }
}

