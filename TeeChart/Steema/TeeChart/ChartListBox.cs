namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Styles;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(ChartListBox), "Images.ChartListBox.bmp")]
    public class ChartListBox : ListBox, ITeeEventListener
    {
        private bool allowAdd;
        private bool allowDelete;
        private bool askDelete;
        internal Steema.TeeChart.Chart chart;
        private CheckBoxesStyle checkStyle;
        private bool ComingFromDoubleClick;
        private Container components;
        private const int defaultItemHeight = 0x18;
        private Rectangle dragBoxFromMouseDown;
        private TextBox edit;
        private bool enableChangeColor;
        private bool enableChangeType;
        private bool enableDragSeries;
        private Steema.TeeChart.Styles.SeriesGroup group;
        private int indexOfItemUnderMouseToDrag;
        private int indexOfItemUnderMouseToDrop;
        private ListBox.ObjectCollection otherItems;
        private Section[] Sections;
        private TChart tChart;

        public event ChangeActiveEventHandler ChangeActive;

        public event ChangeColorEventHandler ChangeColor;

        public event ChangeOrderEventHandler ChangeOrder;

        public event EditSeriesEventHandler EditSeries;

        public event OtherItemsChangeEventHandler OtherItemsChange;

        public event Steema.TeeChart.RefreshEventHandler RefreshEvent;

        public event RemovedSeriesEventHandler RemovedSeries;

        public ChartListBox()
        {
            this.Sections = new Section[4];
            this.InitializeComponent();
            this.SetProperties();
        }

        public ChartListBox(IContainer container) : this()
        {
            container.Add(this);
        }

        private Steema.TeeChart.Styles.Series AddSeriesGallery()
        {
            Steema.TeeChart.Styles.Series s = null;
            if (this.Chart != null)
            {
                s = ChartGallery.CreateNew(this.chart, null);
            }
            if (s != null)
            {
                if (this.MultiSelect)
                {
                    base.ClearSelected();
                }
                s.FillSampleValues();
                this.SelectSeries(s);
                this.OnRefresh(EventArgs.Empty);
            }
            return s;
        }

        private bool AnySelected()
        {
            if (!this.MultiSelect)
            {
                return (this.SelectedIndex != -1);
            }
            return (base.SelectedIndices.Count > 0);
        }

        public void ChangeTypeSeries()
        {
            if (this.AnySelected() && (this.chart != null))
            {
                SeriesCollection seriess = new SeriesCollection(this.chart);
                if (this.MultiSelect)
                {
                    for (int k = 0; k < this.Items.Count; k++)
                    {
                        if (this.IsSelected(k))
                        {
                            seriess.Add(this.Series(k));
                        }
                    }
                }
                else
                {
                    seriess.Add(this.SelectedSeries);
                }
                bool flag = true;
                System.Type newType = null;
                for (int i = 0; i < seriess.Count; i++)
                {
                    Steema.TeeChart.Styles.Series s = seriess[i];
                    if (flag)
                    {
                        newType = ChartGallery.ChangeSeriesType(this.chart, ref s);
                        flag = false;
                    }
                    else
                    {
                        Steema.TeeChart.Styles.Series.ChangeType(ref s, newType);
                    }
                    seriess[i] = s;
                }
                for (int j = 0; j < seriess.Count; j++)
                {
                    this.SelectSeries(seriess[j]);
                }
                this.FillSeries(this.SelectedSeries);
                this.OnRefresh(EventArgs.Empty);
            }
        }

        public void ClearItems()
        {
            this.Items.Clear();
            if (this.OtherItems != null)
            {
                this.OtherItems.Clear();
            }
        }

        public Steema.TeeChart.Styles.Series CloneSeries()
        {
            if (this.SelectedIndex != -1)
            {
                Steema.TeeChart.Styles.Series series = this.SelectedSeries.Clone() as Steema.TeeChart.Styles.Series;
                this.RefreshDesigner();
                return series;
            }
            return null;
        }

        public bool DeleteSeries()
        {
            bool flag = false;
            if (!this.AnySelected())
            {
                return flag;
            }
            if (this.AskDelete)
            {
                string selectedSeries;
                if (!this.MultiSelect || (this.SelCount == 1))
                {
                    selectedSeries = this.SelectedSeries.ToString();
                }
                else
                {
                    selectedSeries = Texts.SelectedSeries;
                }
                if (Utils.YesNoDelete(selectedSeries))
                {
                    this.DoDelete();
                    flag = true;
                }
                return flag;
            }
            this.DoDelete();
            return true;
        }

        private void DoDelete()
        {
            Steema.TeeChart.Styles.Series series;
            int aIndex = -1;
            if (this.MultiSelect)
            {
                int index = 0;
                while (index < this.Items.Count)
                {
                    if (base.GetSelected(index))
                    {
                        if (aIndex == -1)
                        {
                            aIndex = index;
                        }
                        series = this[index];
                        this.chart.Series.Remove(series);
                        this.chart.RemoveFromContainer(series);
                        series.Dispose();
                    }
                    else
                    {
                        index++;
                    }
                }
            }
            else if (this.SelectedIndex != -1)
            {
                aIndex = this.SelectedIndex;
                series = this[this.SelectedIndex];
                this.chart.Series.Remove(series);
                this.chart.RemoveFromContainer(series);
                series.Dispose();
            }
            if (aIndex >= this.Items.Count)
            {
                aIndex = this.Items.Count - 1;
            }
            if (aIndex > -1)
            {
                this.SelectSeries(aIndex);
            }
            if ((this.SelectedIndex == -1) && (this.Items.Count > 0))
            {
                this.SelectSeries(0);
            }
            this.OnRefresh(EventArgs.Empty);
            this.RefreshDesigner();
        }

        public void DoRefresh()
        {
        }

        private void EditorKey(object sender, KeyEventArgs e)
        {
            Keys keyCode = e.KeyCode;
            if (keyCode <= Keys.Escape)
            {
                if ((keyCode != Keys.Enter) && (keyCode != Keys.Escape))
                {
                    return;
                }
            }
            else
            {
                switch (keyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.F2:
                        goto Label_0032;

                    case Keys.Right:
                        return;
                }
                return;
            }
        Label_0032:
            if ((this.edit != null) && this.edit.Visible)
            {
                if (e.KeyCode != Keys.Escape)
                {
                    this.SelectedSeries.Title = this.edit.Text;
                }
                this.edit.Hide();
                base.Focus();
                base.Invalidate();
            }
        }

        private void EditorPress(object sender, KeyPressEventArgs e)
        {
        }

        private void ExchangeOtherItems(int tmp1, int tmp2)
        {
            Steema.TeeChart.Styles.Series item = this.Series(tmp1);
            Steema.TeeChart.Styles.Series series2 = this.Series(tmp2);
            if (this.OtherItems != null)
            {
                this.OtherItems.Insert(tmp1, series2);
                this.OtherItems.RemoveAt(tmp1 + 1);
                this.OtherItems.Insert(tmp2, item);
                this.OtherItems.RemoveAt(tmp2 + 1);
            }
        }

        public void FillSeries(Steema.TeeChart.Styles.Series oldSeries)
        {
            this.ClearItems();
            base.ClearSelected();
            base.BeginUpdate();
            if (this.chart != null)
            {
                if (this.group != null)
                {
                    this.chart.Series.FillItems(this.Items, this.GetSeriesGroup());
                }
                else
                {
                    this.chart.Series.FillItems(this.Items);
                }
            }
            base.EndUpdate();
            if (this.OtherItems != null)
            {
                this.OtherItems.Clear();
                foreach (object obj2 in this.Items)
                {
                    this.OtherItems.Add(obj2);
                }
            }
            int aIndex = -1;
            if (oldSeries != null)
            {
                aIndex = this.chart.Series.IndexOf(oldSeries);
            }
            if ((aIndex == -1) && (this.Items.Count > 0))
            {
                aIndex = 0;
            }
            if ((aIndex != -1) && (this.Items.Count > aIndex))
            {
                this.SelectSeries(aIndex);
            }
            this.OnOtherItemsChange(EventArgs.Empty);
            this.OnRefresh(EventArgs.Empty);
            base.Invalidate();
        }

        private SeriesCollection GetSeriesGroup()
        {
            if (this.group != null)
            {
                return this.group.Series;
            }
            return this.chart.series;
        }

        public void HideEditor()
        {
            if ((this.edit != null) && this.edit.Visible)
            {
                this.edit.Hide();
                base.Focus();
                base.Invalidate();
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private bool IsSelected(int tmp)
        {
            if (!this.MultiSelect)
            {
                return (this.SelectedIndex == tmp);
            }
            return base.SelectedIndices.Contains(tmp);
        }

        protected virtual void OnChangeActive(NotifySeriesEventArgs e)
        {
            if (this.ChangeActive != null)
            {
                this.ChangeActive(this, e);
            }
        }

        protected virtual void OnChangeColor(NotifySeriesEventArgs e)
        {
            if (this.ChangeColor != null)
            {
                this.ChangeColor(this, e);
            }
        }

        protected virtual void OnChangeOrder(ChangeOrderEventArgs e)
        {
            if (this.ChangeOrder != null)
            {
                this.ChangeOrder(this, e);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.OnRefresh(EventArgs.Empty);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            Point point;
            this.ComingFromDoubleClick = true;
            int tmp = this.SeriesAtMousePos(out point);
            if ((tmp != -1) && this.IsSelected(tmp))
            {
                Steema.TeeChart.Styles.Series s = this.Series(tmp);
                if (this.PointInSection(point, 0) && this.EnableChangeType)
                {
                    this.ChangeTypeSeries();
                    return;
                }
                if (this.PointInSection(point, 2) && this.EnableChangeColor)
                {
                    if (s.UseSeriesColor)
                    {
                        ChartBrush b = s.bBrush.Clone() as ChartBrush;
                        if (BrushEditor.Edit(b, false))
                        {
                            s.Color = b.Color;
                            s.ColorEach = false;
                            s.Invalidate();
                            base.Invalidate();
                            NotifySeriesEventArgs args = new NotifySeriesEventArgs(s);
                            this.OnChangeColor(args);
                        }
                    }
                    return;
                }
                if (this.PointInSection(point, 3) && (this.EditSeries != null))
                {
                    this.EditSeries(this, s);
                    return;
                }
            }
            base.OnDoubleClick(e);
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            if ((this.indexOfItemUnderMouseToDrag >= 0) && (this.indexOfItemUnderMouseToDrop >= 0))
            {
                this.SwapSeries(this.indexOfItemUnderMouseToDrag, this.indexOfItemUnderMouseToDrop);
            }
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            if ((drgevent.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
            {
                drgevent.Effect = DragDropEffects.Move;
            }
            else
            {
                drgevent.Effect = DragDropEffects.None;
            }
            this.indexOfItemUnderMouseToDrop = base.IndexFromPoint(base.PointToClient(new Point(drgevent.X, drgevent.Y)));
            base.OnDragOver(drgevent);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            SolidBrush brush = new SolidBrush(this.BackColor);
            Rectangle rect = new Rectangle(e.Bounds.Location, e.Bounds.Size) {
                Width = this.SectionLeft(3)
            };
            e.Graphics.FillRectangle(brush, rect);
            brush.Color = e.BackColor;
            rect = new Rectangle(e.Bounds.Location, e.Bounds.Size) {
                X = this.SectionLeft(4)
            };
            e.Graphics.FillRectangle(brush, rect);
            Steema.TeeChart.Styles.Series series = this.Series(e.Index);
            if (series != null)
            {
                int num;
                if (this.ShowSeriesIcon)
                {
                    Image bitmapEditor = series.GetBitmapEditor();
                    if (bitmapEditor != null)
                    {
                        e.Graphics.DrawImage(bitmapEditor, this.SectionLeft(0), e.Bounds.Top);
                        bitmapEditor.Dispose();
                    }
                }
                if (this.ShowSeriesColor && series.UseSeriesColor)
                {
                    num = this.SectionLeft(2) - 2;
                    rect = new Rectangle(num, e.Bounds.Top, this.Sections[2].Width, e.Bounds.Height);
                    rect.Inflate(-4, -4);
                    series.PaintLegend(e.Graphics, rect);
                }
                if (this.ShowActiveCheck)
                {
                    num = this.SectionLeft(1);
                    Rectangle rectangle2 = new Rectangle(num + 2, e.Bounds.Top + 5, 10, 12);
                    Utils.DrawCheckBox(rectangle2.X, rectangle2.Y, e.Graphics, series.Active, e.BackColor, this.CheckStyle == CheckBoxesStyle.Check);
                }
                if (this.ShowSeriesTitle)
                {
                    Point point = new Point(this.SectionLeft(3) + 1, e.Bounds.Top + ((this.ItemHeight - this.Font.Height) / 2));
                    string s = this.Items[e.Index].ToString().Replace('\n', ' ');
                    e.Graphics.DrawString(s, this.Font, new SolidBrush(e.ForeColor), (PointF) point);
                }
                else
                {
                    e.Graphics.DrawString("", this.Font, brush, (PointF) new Point(0, 0));
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Insert:
                    if (this.AllowAddSeries)
                    {
                        this.AddSeriesGallery();
                    }
                    break;

                case Keys.Delete:
                    if (this.AllowDeleteSeries)
                    {
                        this.DeleteSeries();
                    }
                    break;

                case Keys.F2:
                    if (this.SelectedSeries != null)
                    {
                        this.ShowEditor();
                    }
                    break;
            }
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.HideEditor();
            base.OnMouseDown(e);
            if (base.SelectedIndices.Count > 0)
            {
                Point point;
                int index = this.SeriesAtMousePos(out point);
                if ((index != -1) && this.PointInSection(point, 1))
                {
                    if (this.CheckStyle == CheckBoxesStyle.Check)
                    {
                        this.Series(index).Active = !this.Series(index).Active;
                    }
                    else
                    {
                        for (int i = 0; i < this.Items.Count; i++)
                        {
                            this.Series(i).Active = i == index;
                        }
                    }
                    if (this.Series(index).Chart == null)
                    {
                        base.Invalidate();
                    }
                    NotifySeriesEventArgs args = new NotifySeriesEventArgs(this.Series(index));
                    this.OnChangeActive(args);
                }
                else if ((this.EnableDragSeries && this.PointInSection(point, 3)) && !this.ComingFromDoubleClick)
                {
                    this.indexOfItemUnderMouseToDrag = base.IndexFromPoint(e.X, e.Y);
                    if (this.indexOfItemUnderMouseToDrag != -1)
                    {
                        Size dragSize = SystemInformation.DragSize;
                        this.dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                    }
                    else
                    {
                        this.dragBoxFromMouseDown = Rectangle.Empty;
                    }
                }
                this.ComingFromDoubleClick = false;
                this.OnOtherItemsChange(EventArgs.Empty);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point point;
            base.OnMouseMove(e);
            if ((this.SeriesAtMousePos(out point) != -1) && (this.PointInSection(point, 0) || this.PointInSection(point, 2)))
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
            if (((this.EnableDragSeries && this.PointInSection(point, 3)) && (!this.ComingFromDoubleClick && (e.Button == MouseButtons.Left))) && ((this.dragBoxFromMouseDown != Rectangle.Empty) && !this.dragBoxFromMouseDown.Contains(e.X, e.Y)))
            {
                base.DoDragDrop(this.Items[this.indexOfItemUnderMouseToDrag], DragDropEffects.Move | DragDropEffects.Copy);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.dragBoxFromMouseDown = Rectangle.Empty;
        }

        protected virtual void OnOtherItemsChange(EventArgs e)
        {
            if (this.OtherItemsChange != null)
            {
                this.OtherItemsChange(this, e);
            }
        }

        protected virtual void OnRefresh(EventArgs e)
        {
            if (this.RefreshEvent != null)
            {
                this.RefreshEvent(this, e);
            }
        }

        protected virtual void OnRemovedSeries(EventArgs e)
        {
            if (this.RemovedSeries != null)
            {
                this.RemovedSeries(this, e);
            }
        }

        private bool PointInSection(Point p, int ASection)
        {
            if (!this.Sections[ASection].Visible)
            {
                return false;
            }
            int num = this.SectionLeft(ASection);
            return ((p.X > num) && (p.X < (num + this.Sections[ASection].Width)));
        }

        private void RefreshDesigner()
        {
        }

        private int SectionLeft(int ASection)
        {
            int num = 0;
            for (int i = 0; i < ASection; i++)
            {
                if (this.Sections[i].Visible)
                {
                    num += this.Sections[i].Width;
                }
            }
            return num;
        }

        public void SelectAll()
        {
            if (this.MultiSelect)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    base.SetSelected(i, true);
                }
                this.RefreshDesigner();
            }
        }

        private void SelectSeries(Steema.TeeChart.Styles.Series s)
        {
            this.SelectSeries(this.Items.IndexOf(s));
        }

        private void SelectSeries(int AIndex)
        {
            if (this.MultiSelect)
            {
                base.SetSelected(AIndex, true);
            }
            else
            {
                this.SelectedIndex = AIndex;
            }
            this.OnRefresh(EventArgs.Empty);
        }

        public Steema.TeeChart.Styles.Series Series(int index)
        {
            if ((index != -1) && (index < this.Items.Count))
            {
                return (Steema.TeeChart.Styles.Series) this.Items[index];
            }
            return null;
        }

        private int SeriesAtMousePos(out Point p)
        {
            p = base.PointToClient(Control.MousePosition);
            return base.IndexFromPoint(p);
        }

        internal void SetChart(Steema.TeeChart.Chart c)
        {
            if (this.chart != null)
            {
                this.chart.RemoveListener(this);
            }
            this.chart = c;
            if (this.chart == null)
            {
                this.ClearItems();
            }
            else
            {
                if (this.chart.Listeners != null)
                {
                    this.chart.Listeners.Add(this);
                }
                this.FillSeries(null);
            }
        }

        private void SetCheckStyle(CheckBoxesStyle Value)
        {
            if (this.checkStyle != Value)
            {
                this.checkStyle = Value;
                if (this.checkStyle == CheckBoxesStyle.Radio)
                {
                    int num = 0;
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        if (this.Series(i).Active)
                        {
                            num++;
                            if (num > 1)
                            {
                                this.Series(i).Active = false;
                            }
                        }
                    }
                }
                base.Invalidate();
            }
        }

        private void SetProperties()
        {
            this.ComingFromDoubleClick = false;
            this.ItemHeight = 0x18;
            this.MultiSelect = true;
            this.AllowDrop = true;
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            base.Sorted = false;
            this.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Sections[0].Width = 0x1a;
            this.Sections[0].Visible = true;
            this.Sections[1].Width = 0x10;
            this.Sections[1].Visible = true;
            this.Sections[2].Width = 0x1a;
            this.Sections[2].Visible = true;
            this.Sections[3].Width = 0xd8;
            this.Sections[3].Visible = true;
            this.allowAdd = true;
            this.allowDelete = true;
            this.askDelete = true;
            this.checkStyle = CheckBoxesStyle.Check;
            this.enableChangeColor = true;
            this.enableDragSeries = true;
            this.enableChangeType = true;
        }

        public void ShowEditor()
        {
            if ((this.SelectedSeries != null) && this.ShowSeriesTitle)
            {
                if (this.edit == null)
                {
                    this.edit = new TextBox();
                    base.Controls.Add(this.edit);
                }
                this.edit.Multiline = true;
                this.edit.Text = this.SelectedSeries.ToString();
                this.edit.Width = this.Sections[3].Width;
                this.edit.Left = this.SectionLeft(3) - 2;
                Rectangle itemRectangle = base.GetItemRectangle(this.SelectedIndex);
                this.edit.Top = itemRectangle.Top;
                this.edit.Height = itemRectangle.Height;
                this.edit.KeyDown += new KeyEventHandler(this.EditorKey);
                this.edit.KeyPress += new KeyPressEventHandler(this.EditorPress);
                this.edit.Show();
                this.edit.Focus();
            }
        }

        private void SwapSeries(int tmp1, int tmp2)
        {
            Steema.TeeChart.Styles.Series s = this.Series(tmp1);
            Steema.TeeChart.Styles.Series series2 = this.Series(tmp2);
            this.ExchangeOtherItems(tmp1, tmp2);
            if (this.Chart != null)
            {
                this.ClearItems();
                this.Chart.Series.Exchange(tmp1, tmp2);
                base.ClearSelected();
                this.SelectSeries(s);
                ChangeOrderEventArgs e = new ChangeOrderEventArgs(s, series2);
                this.OnChangeOrder(e);
                Form form = base.FindForm();
                if (form != null)
                {
                    form.ActiveControl = this;
                }
                this.OnRefresh(EventArgs.Empty);
            }
            else if (this.Items.Count > 0)
            {
                this.Items.Insert(tmp1, series2);
                this.Items.RemoveAt(tmp1 + 1);
                this.Items.Insert(tmp2, s);
                this.Items.RemoveAt(tmp2 + 1);
                ChangeOrderEventArgs args2 = new ChangeOrderEventArgs(s, series2);
                this.OnChangeOrder(args2);
                Form form2 = base.FindForm();
                if (form2 != null)
                {
                    form2.ActiveControl = this;
                }
                this.OnRefresh(EventArgs.Empty);
            }
        }

        public void TeeEvent(Steema.TeeChart.TeeEvent e)
        {
            if (e is SeriesEvent)
            {
                Steema.TeeChart.Styles.Series series = (e as SeriesEvent).Series;
                switch ((e as SeriesEvent).Event)
                {
                    case SeriesEventStyle.Add:
                    case SeriesEventStyle.Swap:
                        this.FillSeries(this.SelectedSeries);
                        break;

                    case SeriesEventStyle.Remove:
                    {
                        int index = this.Items.IndexOf(series);
                        if (index == -1)
                        {
                            break;
                        }
                        this.Items.RemoveAt(index);
                        if (this.OtherItems != null)
                        {
                            this.OtherItems.RemoveAt(index);
                        }
                        if (this.RemovedSeries == null)
                        {
                            break;
                        }
                        this.OnRemovedSeries(EventArgs.Empty);
                        return;
                    }
                    case SeriesEventStyle.RemoveAll:
                        this.FillSeries(null);
                        return;

                    case SeriesEventStyle.ChangeTitle:
                    case SeriesEventStyle.ChangeColor:
                    case SeriesEventStyle.ChangeActive:
                        base.Invalidate();
                        return;

                    default:
                        return;
                }
            }
        }

        [DefaultValue(true), Description("Setting AllowAddSeries enables/disables the addition of Series.")]
        public bool AllowAddSeries
        {
            get
            {
                return this.allowAdd;
            }
            set
            {
                this.allowAdd = value;
            }
        }

        [Description("Enable/disable deletion of Series."), DefaultValue(true)]
        public bool AllowDeleteSeries
        {
            get
            {
                return this.allowDelete;
            }
            set
            {
                this.allowDelete = value;
            }
        }

        [DefaultValue(true), Description("Enable prompt 'yes/no' on deleting Series.")]
        public bool AskDelete
        {
            get
            {
                return this.askDelete;
            }
            set
            {
                this.askDelete = value;
            }
        }

        [DefaultValue((string) null)]
        public TChart Chart
        {
            get
            {
                return this.tChart;
            }
            set
            {
                this.tChart = value;
                if (this.tChart != null)
                {
                    this.SetChart(this.tChart.Chart);
                }
                else
                {
                    this.SetChart(null);
                }
            }
        }

        [Description("The ListBox uses this property to display checkboxes or radiobuttons at each list item."), DefaultValue(0)]
        public CheckBoxesStyle CheckStyle
        {
            get
            {
                return this.checkStyle;
            }
            set
            {
                this.SetCheckStyle(value);
            }
        }

        [Browsable(false), DefaultValue(1)]
        public System.Windows.Forms.DrawMode DrawMode
        {
            get
            {
                return this.DrawMode;
            }
            set
            {
                this.DrawMode = value;
            }
        }

        [Description("Enables/disables the Series color to be changed within the ChartListBox."), DefaultValue(true)]
        public bool EnableChangeColor
        {
            get
            {
                return this.enableChangeColor;
            }
            set
            {
                this.enableChangeColor = value;
            }
        }

        [DefaultValue(true), Description("Enables/disables the Series type to be changed within the ChartListBox.")]
        public bool EnableChangeType
        {
            get
            {
                return this.enableChangeType;
            }
            set
            {
                this.enableChangeType = value;
            }
        }

        [Description("Enables/disables Series to be dragged within the ChartListBox."), DefaultValue(true)]
        public bool EnableDragSeries
        {
            get
            {
                return this.enableDragSeries;
            }
            set
            {
                this.enableDragSeries = value;
            }
        }

        public Steema.TeeChart.Styles.Series this[int index]
        {
            get
            {
                return (Steema.TeeChart.Styles.Series) this.Items[index];
            }
            set
            {
                this.Items[index] = value;
            }
        }

        [DefaultValue(0x18)]
        public int ItemHeight
        {
            get
            {
                return this.ItemHeight;
            }
            set
            {
                this.ItemHeight = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ListBox.ObjectCollection Items
        {
            get
            {
                return base.Items;
            }
        }

        [DefaultValue(true)]
        public bool MultiSelect
        {
            get
            {
                if (this.SelectionMode != System.Windows.Forms.SelectionMode.MultiSimple)
                {
                    return (this.SelectionMode == System.Windows.Forms.SelectionMode.MultiExtended);
                }
                return true;
            }
            set
            {
                if (value)
                {
                    this.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
                }
                else
                {
                    this.SelectionMode = System.Windows.Forms.SelectionMode.One;
                }
            }
        }

        public ListBox.ObjectCollection OtherItems
        {
            get
            {
                return this.otherItems;
            }
            set
            {
                this.otherItems = value;
            }
        }

        private int SelCount
        {
            get
            {
                return base.SelectedIndices.Count;
            }
        }

        [Browsable(false), Description("Shows the SeriesList index value of the selected Series."), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Styles.Series SelectedSeries
        {
            get
            {
                if (this.SelectedIndex != -1)
                {
                    return this.Series(this.SelectedIndex);
                }
                return null;
            }
            set
            {
                base.ClearSelected();
                int index = this.Items.IndexOf(value);
                if (index != -1)
                {
                    base.SetSelected(index, true);
                }
                this.OnRefresh(EventArgs.Empty);
            }
        }

        [DefaultValue(3)]
        public System.Windows.Forms.SelectionMode SelectionMode
        {
            get
            {
                return this.SelectionMode;
            }
            set
            {
                this.SelectionMode = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Steema.TeeChart.Styles.SeriesGroup SeriesGroup
        {
            get
            {
                return this.group;
            }
            set
            {
                if (this.group != value)
                {
                    this.group = value;
                    this.FillSeries(this.SelectedSeries);
                }
            }
        }

        [Description("Displays/Hides the 'Active Series' CheckBox in the Series List."), DefaultValue(true)]
        public bool ShowActiveCheck
        {
            get
            {
                return this.Sections[1].Visible;
            }
            set
            {
                this.Sections[1].Visible = value;
                base.Invalidate();
            }
        }

        [Description("Displays/Hides the 'Series Colour' box in the Series List."), DefaultValue(true)]
        public bool ShowSeriesColor
        {
            get
            {
                return this.Sections[2].Visible;
            }
            set
            {
                this.Sections[2].Visible = value;
                base.Invalidate();
            }
        }

        [DefaultValue(true), Description("Displays/Hides the 'Series type icon' in the Series List.")]
        public bool ShowSeriesIcon
        {
            get
            {
                return this.Sections[0].Visible;
            }
            set
            {
                this.Sections[0].Visible = value;
                base.Invalidate();
            }
        }

        [DefaultValue(true), Description("Displays/Hides the 'Series Title' in the Series List.")]
        public bool ShowSeriesTitle
        {
            get
            {
                return this.Sections[3].Visible;
            }
            set
            {
                this.Sections[3].Visible = value;
                base.Invalidate();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Section
        {
            public int Width;
            public bool Visible;
        }
    }
}

