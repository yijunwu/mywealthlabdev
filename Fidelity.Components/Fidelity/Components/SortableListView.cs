namespace Fidelity.Components
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(SortableListView), "SortableListView")]
    public class SortableListView : ListView, IComparer
    {
        private IContainer icontainer_0;
        private int int_0 = -1;
        private SortOrder sortOrder_0 = SortOrder.Ascending;
        private SortOrder sortOrder_1 = SortOrder.Ascending;

        public SortableListView()
        {
            base.View = View.Details;
            this.method_0();
            base.ListViewItemSorter = null;
        }

        public void BeginUpdate()
        {
            this.int_0 = -1;
            base.BeginUpdate();
        }

        public int Compare(object object_0, object object_1)
        {
            try
            {
                if (this.int_0 != -1)
                {
                    int num4 = (this.sortOrder_0 == SortOrder.Ascending) ? 1 : -1;
                    ListViewItem item = (ListViewItem) object_0;
                    ListViewItem item2 = (ListViewItem) object_1;
                    if ((this.int_0 <= item.SubItems.Count) && (this.int_0 <= item2.SubItems.Count))
                    {
                        string text;
                        string str2;
                        string str3;
                        if (this.int_0 == 0)
                        {
                            text = item.Text;
                            str2 = item2.Text;
                        }
                        else
                        {
                            text = item.SubItems[this.int_0].Text;
                            str2 = item2.SubItems[this.int_0].Text;
                        }
                        ColumnHeader header = base.Columns[this.int_0];
                        if (header.Tag == null)
                        {
                            str3 = "";
                        }
                        else
                        {
                            str3 = header.Tag.ToString();
                        }
                        string str4 = str3;
                        if (str4 != null)
                        {
                            if (str4 == "N")
                            {
                                try
                                {
                                    double num5 = double.Parse(text, NumberStyles.Number);
                                    double num6 = double.Parse(str2, NumberStyles.Number);
                                    return (num5.CompareTo(num6) * num4);
                                }
                                catch
                                {
                                    return (text.CompareTo(str2) * num4);
                                }
                            }
                            if (str4 == "C")
                            {
                                try
                                {
                                    double num7 = double.Parse(text, NumberStyles.Currency);
                                    double num8 = double.Parse(str2, NumberStyles.Currency);
                                    return (num7.CompareTo(num8) * num4);
                                }
                                catch
                                {
                                    return (text.CompareTo(str2) * num4);
                                }
                            }
                            if (str4 == "D")
                            {
                                try
                                {
                                    DateTime time = DateTime.Parse(text);
                                    DateTime time2 = DateTime.Parse(str2);
                                    return (time.CompareTo(time2) * num4);
                                }
                                catch
                                {
                                    return (text.CompareTo(str2) * num4);
                                }
                            }
                            if (str4 == "DT")
                            {
                                try
                                {
                                    string[] strArray = text.Split(new char[] { ' ' });
                                    if (strArray.Length > 2)
                                    {
                                        strArray[1] = strArray[1] + " " + strArray[2];
                                    }
                                    string[] strArray2 = str2.Split(new char[] { ' ' });
                                    if (strArray2.Length > 2)
                                    {
                                        strArray2[1] = strArray2[1] + " " + strArray2[2];
                                    }
                                    DateTime time3 = DateTime.Parse(strArray[1]);
                                    TimeSpan span = (TimeSpan) (time3 - time3.Date);
                                    DateTime time4 = DateTime.Parse(strArray[0]) + span;
                                    DateTime time5 = DateTime.Parse(strArray2[1]);
                                    TimeSpan span2 = (TimeSpan) (time5 - time5.Date);
                                    DateTime time6 = DateTime.Parse(strArray2[0]) + span2;
                                    return (time4.CompareTo(time6) * num4);
                                }
                                catch
                                {
                                    return (text.CompareTo(str2) * num4);
                                }
                            }
                            if (str4 == "double")
                            {
                                double tag;
                                double num3;
                                if (this.int_0 == 0)
                                {
                                    tag = (double) item.Tag;
                                    num3 = (double) item2.Tag;
                                }
                                else
                                {
                                    tag = (double) item.SubItems[this.int_0].Tag;
                                    num3 = (double) item2.SubItems[this.int_0].Tag;
                                }
                                if (tag == num3)
                                {
                                    return 0;
                                }
                                if (tag == double.MinValue)
                                {
                                    return -num4;
                                }
                                if (num3 == double.MinValue)
                                {
                                    return num4;
                                }
                                return (tag.CompareTo(num3) * num4);
                            }
                        }
                        return (text.CompareTo(str2) * num4);
                    }
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public void DisableSort()
        {
            base.ListViewItemSorter = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        protected override void OnColumnClick(ColumnClickEventArgs columnClickEventArgs_0)
        {
            if (columnClickEventArgs_0.Column == this.int_0)
            {
                if (this.sortOrder_0 == SortOrder.Ascending)
                {
                    this.sortOrder_0 = SortOrder.Descending;
                }
                else
                {
                    this.sortOrder_0 = SortOrder.Ascending;
                }
            }
            else
            {
                this.int_0 = columnClickEventArgs_0.Column;
                this.sortOrder_0 = this.sortOrder_1;
                this.sortOrder_1 = SortOrder.Ascending;
            }
            if (base.ListViewItemSorter == null)
            {
                base.ListViewItemSorter = this;
            }
            else
            {
                base.Sort();
            }
            base.OnColumnClick(columnClickEventArgs_0);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        public void SortByColumn(ColumnHeader columnHeader)
        {
            this.int_0 = -1;
            this.OnColumnClick(new ColumnClickEventArgs(columnHeader.Index));
        }

        public void SortByColumn(ColumnHeader columnHeader, SortOrder _order)
        {
            this.sortOrder_1 = _order;
            this.SortByColumn(columnHeader);
        }
    }
}

