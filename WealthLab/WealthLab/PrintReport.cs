namespace WealthLab
{
    using System;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class PrintReport
    {
        private Bitmap bitmap_0;
        private bool bool_0;
        private bool bool_1;
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private bool bool_5;
        private bool bool_6;
        private bool bool_7;
        private bool bool_8;
        private bool bool_9;
        private DataObject dataObject_0;
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
        private Font font_1;
        private Font font_2;
        private Font font_3;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private ListView listView_0;
        private PrintPreview printPreview_0;
        private string string_0;
        private string string_1;
        private string string_2;
        private string string_3;
        private string string_4;
        private string string_5;
        private string string_6;
        private string string_7;
        private static string string_8 = "Backtesting provides a hypothetical calculation of how a security or portfolio of securities, subject to a trading strategy, would have performed over a historical time period. You should not assume that backtesting of a trading strategy will provide any indication of how your portfolio of securities, or a new portfolio of securities, might perform over time. You should choose your own trading strategies based on your particular objectives and risk tolerances. Be sure to review your decisions periodically to make sure they are still consistent with your goals. Past performance is no guarantee of future results.";

        public PrintReport()
        {
            this.string_1 = "Wealth-Lab Pro\x00ae";
            this.string_7 = "";
            this.method_0();
        }

        public PrintReport(bool bUseDefaultDisclosure)
        {
            this.string_1 = "Wealth-Lab Pro\x00ae";
            this.string_7 = "";
            this.method_0();
            this.UseDefaultDisclosure();
        }

        public PrintReport(DataObject doPrint)
        {
            this.string_1 = "Wealth-Lab Pro\x00ae";
            this.string_7 = "";
            this.method_0();
            this.dataObject_0 = doPrint;
        }

        public PrintReport(DataObject doPrint, bool bUseDefaultDisclosure)
        {
            this.string_1 = "Wealth-Lab Pro\x00ae";
            this.string_7 = "";
            this.method_0();
            this.dataObject_0 = doPrint;
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
                this.int_1 = this.printPreview_0.FromPage;
                this.int_2 = this.printPreview_0.ToPage;
            }
            this.int_0 = 1;
            if (this.dataObject_0.GetDataPresent(fmtBaseTitle.Name))
            {
                this.BasePrintTitle = this.dataObject_0.GetData(fmtBaseTitle.Name).ToString();
            }
            if (this.dataObject_0.GetDataPresent(fmtTitle.Name))
            {
                this.printTitle = this.dataObject_0.GetData(fmtTitle.Name).ToString();
            }
            if (this.dataObject_0.GetDataPresent(fmtStrategy.Name))
            {
                this.printStrategy = this.dataObject_0.GetData(fmtStrategy.Name).ToString();
            }
            if (this.dataObject_0.GetDataPresent(fmtSymbol.Name))
            {
                this.printSymbol = this.dataObject_0.GetData(fmtSymbol.Name).ToString();
            }
            if (this.dataObject_0.GetDataPresent(fmtDetails.Name))
            {
                this.printDetails = this.dataObject_0.GetData(fmtDetails.Name).ToString();
            }
            if (this.dataObject_0.GetDataPresent(fmtGraphic.Name))
            {
                this.printGraphic = this.dataObject_0.GetData(fmtGraphic.Name) as Bitmap;
            }
            if (this.dataObject_0.GetDataPresent(fmtListView.Name))
            {
                this.printListView = this.dataObject_0.GetData(fmtListView.Name) as ListView;
                this.int_4 = 0;
            }
            if (this.dataObject_0.GetDataPresent(fmtDisclosure.Name))
            {
                this.rptDisclosure = this.dataObject_0.GetData(fmtDisclosure.Name).ToString();
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
            this.int_3 = ((int) e.Graphics.MeasureString("Test", this.font_1).Height) + 1;
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
            this.font_1 = new Font("Arial", 10f);
            this.font_2 = new Font("Arial", 8f);
            this.font_3 = new Font("Arial", 6f);
            this.int_0 = 0;
            this.int_2 = 0;
            this.bool_0 = false;
            this.dataObject_0 = new DataObject();
            this.string_0 = this.string_1;
            this.bool_1 = true;
            this.bool_2 = true;
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
            if (this.bool_2)
            {
                this.printPreview_0 = new PrintPreview(prtdoc);
                this.printPreview_0.ShowPrintDialog = this.bool_1;
                this.printPreview_0.ShowDialog();
                prtdoc.Dispose();
            }
            else
            {
                PrintDialog dialog = new PrintDialog {
                    Document = prtdoc
                };
                if (this.bool_1)
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
            printPageEventArgs_0.Graphics.DrawString(s, this.font_2, Brushes.Black, destRect, format);
            int num = ((int) printPageEventArgs_0.Graphics.MeasureString(s, this.font_1).Height) + 1;
            if (this.bool_3)
            {
                printPageEventArgs_0.Graphics.DrawString("Strategy: " + this.printStrategy, this.font_1, Brushes.Black, destRect);
                this.bool_3 = false;
                destRect.Y += num;
                destRect.Height -= num;
            }
            if (this.bool_4)
            {
                printPageEventArgs_0.Graphics.DrawString("Dataset/Symbol: " + this.printSymbol, this.font_1, Brushes.Black, destRect);
                destRect.Y += num;
                destRect.Height -= num;
                this.bool_4 = false;
            }
            if (this.bool_5)
            {
                SizeF ef2 = printPageEventArgs_0.Graphics.MeasureString(this.printDetails, this.font_2, destRect.Width);
                destRect.Y += num;
                destRect.Height -= num;
                printPageEventArgs_0.Graphics.DrawString(this.printDetails, this.font_2, Brushes.Black, destRect);
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
            while (this.int_4 < this.printListView.Items.Count)
            {
                int num5 = this.int_3;
                layoutRectangle.X = destRect.X;
                for (int i = 0; i < this.printListView.Items[this.int_4].SubItems.Count; i++)
                {
                    if (this.printListView.Columns.Count > i)
                    {
                        ColumnHeader header = this.printListView.Columns[i];
                        if (header.Width > 3)
                        {
                            layoutRectangle.Width = header.Width * num;
                            ListViewItem.ListViewSubItem item = this.printListView.Items[this.int_4].SubItems[i];
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
                    this.int_4++;
                    break;
                }
                this.int_4++;
            }
            if (this.int_4 >= this.printListView.Items.Count)
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
            this.bool_8 = this.printText.Length > 0;
        }

        public void PrintTitle(PrintPageEventArgs printPageEventArgs_0, ref Rectangle destRect)
        {
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Center
            };
            printPageEventArgs_0.Graphics.DrawString(this.string_0 + " " + this.printTitle, this.font_0, Brushes.Black, destRect, format);
            SizeF ef = printPageEventArgs_0.Graphics.MeasureString(this.string_0 + " " + this.printTitle, this.font_0);
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
                return this.string_0;
            }
            set
            {
                this.string_0 = value;
            }
        }

        public static string DefaultDisclosure
        {
            get
            {
                return string_8;
            }
        }

        public int endPageCount
        {
            get
            {
                return this.int_2;
            }
            set
            {
                this.int_2 = value;
            }
        }

        public bool fPrintListView
        {
            get
            {
                return this.bool_7;
            }
            set
            {
                this.bool_7 = value;
            }
        }

        public bool fPrintText
        {
            get
            {
                return this.bool_8;
            }
            set
            {
                this.bool_8 = value;
            }
        }

        public int intPageCounter
        {
            get
            {
                return this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        public int lvRow
        {
            get
            {
                return this.int_4;
            }
            set
            {
                this.int_4 = value;
            }
        }

        public string printDetails
        {
            get
            {
                return this.string_5;
            }
            set
            {
                this.string_5 = value;
                this.bool_5 = true;
            }
        }

        public Font printFontBody
        {
            get
            {
                return this.font_1;
            }
            set
            {
                this.font_1 = value;
            }
        }

        public Font printFontDetail
        {
            get
            {
                return this.font_2;
            }
            set
            {
                this.font_2 = value;
            }
        }

        public Font printFontFooter
        {
            get
            {
                return this.font_3;
            }
            set
            {
                this.font_3 = value;
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
                this.int_4 = 0;
            }
        }

        public DataObject printObject
        {
            get
            {
                return this.dataObject_0;
            }
            set
            {
                this.dataObject_0 = value;
            }
        }

        public bool printSomePages
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public string printStrategy
        {
            get
            {
                return this.string_3;
            }
            set
            {
                this.string_3 = value;
                this.bool_3 = true;
            }
        }

        public string printSymbol
        {
            get
            {
                return this.string_4;
            }
            set
            {
                this.string_4 = value;
                this.bool_4 = true;
            }
        }

        public string printText
        {
            get
            {
                return this.string_6;
            }
            set
            {
                this.string_6 = value;
                this.fPrintText = true;
            }
        }

        public string printTitle
        {
            get
            {
                return this.string_2;
            }
            set
            {
                this.string_2 = value;
            }
        }

        public string rptDisclosure
        {
            get
            {
                return this.string_7;
            }
            set
            {
                this.string_7 = value;
                this.bool_9 = true;
            }
        }

        public bool ShowPrintDialog
        {
            get
            {
                return this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public bool ShowPrintPreview
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public int startPageCount
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }
    }
}

