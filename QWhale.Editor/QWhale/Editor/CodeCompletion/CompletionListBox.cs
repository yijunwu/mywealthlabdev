namespace QWhale.Editor.CodeCompletion
{
    using QWhale.Common;
    using QWhale.Editor;
    using QWhale.Syntax.CodeCompletion;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false), DesignTimeVisible(false)]
    public class CompletionListBox : ListBox
    {
        private ICodeCompletionColumns columns = new CodeCompletionColumns();
        private IList<int> columnWidths = new List<int>();
        private string filter = string.Empty;
        private bool filtered;
        private ImageList images;
        private int itemWidth;
        private Keys[] navKeys = new Keys[] { Keys.Up, Keys.Down, Keys.PageUp, Keys.Next };
        private IPainter painter = new GdiPainter();
        private int priority = -1;
        private ICodeCompletionProvider provider;
        private HtmlSyntaxPaint syntaxPaint;
        private EventHandler updateSize;

        public CompletionListBox()
        {
            this.painter.Font = this.Font;
            this.syntaxPaint = new HtmlSyntaxPaint(this.painter, null);
            this.DrawMode = DrawMode.OwnerDrawVariable;
            base.BorderStyle = BorderStyle.None;
            base.HorizontalScrollbar = true;
            base.Sorted = true;
        }

        public ICodeCompletionColumn AddColumn()
        {
            ICodeCompletionColumn item = new CodeCompletionColumn();
            this.columns.Add(item);
            return item;
        }

        public void ClearColumns()
        {
            this.columns.Clear();
        }

        private void DrawColumn(Rectangle rect, string text, Color foreColor, FontStyle style)
        {
            if (text != null)
            {
                this.painter.TextColor = foreColor;
                this.painter.Opaque = false;
                this.painter.FontStyle = style;
                short[] colorData = null;
                this.syntaxPaint.PrepareData(0, style, ref text, ref colorData);
                this.syntaxPaint.DrawLine(0, text, colorData, rect.Location, rect);
            }
        }

        public ICodeCompletionColumn GetColumn(int index)
        {
            return this.columns[index];
        }

        public int GetIndex()
        {
            return this.GetIndex(this.SelectedIndex);
        }

        public int GetIndex(int index)
        {
            if (((this.Filtered || (this.Priority != -1)) || base.Sorted) && ((index >= 0) && (index < base.Items.Count)))
            {
                return ((ListboxItem) base.Items[index]).Index;
            }
            return index;
        }

        public ICodeCompletionColumn InsertColumn(int index)
        {
            ICodeCompletionColumn item = new CodeCompletionColumn();
            this.columns.Insert(index, item);
            return item;
        }

        protected virtual bool IsFiltered(string s, int priority)
        {
            return (this.IsFilteredString(s) && this.IsFilteredPriority(priority));
        }

        protected virtual bool IsFilteredPriority(int priority)
        {
            if (this.priority != -1)
            {
                return (priority == this.priority);
            }
            return true;
        }

        protected virtual bool IsFilteredString(string s)
        {
            if ((this.Filtered && (this.filter != null)) && !(this.filter == string.Empty))
            {
                return s.ToLower().StartsWith(this.filter.ToLower());
            }
            return true;
        }

        private int MeasureWidth(string text, FontStyle style)
        {
            if (text != null)
            {
                this.painter.FontStyle = style;
                short[] colorData = null;
                this.syntaxPaint.PrepareData(0, style, ref text, ref colorData);
                return this.syntaxPaint.MeasureLine(text, colorData, 0, text.Length);
            }
            return 0;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            int index = this.GetIndex(e.Index);
            if (((this.provider == null) || (index < 0)) || (index >= this.provider.Count))
            {
                base.OnDrawItem(e);
            }
            else
            {
                bool flag = (e.State & DrawItemState.Disabled) != DrawItemState.None;
                bool flag2 = ((e.State & DrawItemState.Focus) != DrawItemState.None) && ((e.State & DrawItemState.NoFocusRect) == DrawItemState.None);
                bool flag3 = (e.State & DrawItemState.Selected) != DrawItemState.None;
                Size size = (this.images != null) ? new Size(this.images.ImageSize.Width + EditConsts.DefaultColumnSeparator, this.images.ImageSize.Height) : new Size(0, 0);
                if (this.images != null)
                {
                    int imageIndex = ((CodeCompletionProvider) this.provider).GetImageIndex(index);
                    if ((imageIndex >= 0) && (imageIndex < this.images.Images.Count))
                    {
                        this.images.Draw(e.Graphics, e.Bounds.Left, e.Bounds.Top + ((e.Bounds.Height - size.Height) / 2), imageIndex);
                    }
                    e.Graphics.TranslateTransform((float) size.Width, 0f);
                }
                e.DrawBackground();
                if (flag2)
                {
                    e.DrawFocusRectangle();
                }
                int x = (e.Bounds.Left + size.Width) + EditConsts.DefaultColumnSeparator;
                this.painter.BeginPaint(e.Graphics);
                try
                {
                    for (int i = 0; i < this.provider.ColumnCount; i++)
                    {
                        ICodeCompletionColumn column = (i < this.columns.Count) ? this.GetColumn(i) : null;
                        if (((column == null) || column.Visible) && this.provider.ColumnVisible(i))
                        {
                            Color foreColor = (column != null) ? column.ForeColor : this.ForeColor;
                            int width = this.columnWidths[i];
                            Rectangle rect = new Rectangle(x, e.Bounds.Top, width, e.Bounds.Height);
                            if (flag)
                            {
                                foreColor = EditConsts.DefaultDisabledForeColor;
                            }
                            else if (flag3)
                            {
                                foreColor = EditConsts.DefaultHighlightForeColor;
                            }
                            this.DrawColumn(rect, this.provider.GetColumnText(index, i), foreColor, (column != null) ? column.FontStyle : FontStyle.Regular);
                            x += width;
                        }
                    }
                }
                finally
                {
                    this.painter.EndPaint();
                }
            }
        }

        protected virtual void OnFilterChanged()
        {
            if (this.Filtered)
            {
                this.ResetContent(0);
            }
        }

        protected virtual void OnFilteredChanged()
        {
            this.ResetContent(0);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.painter.Clear();
            this.painter.Font = this.Font;
            this.UpdateControlSize();
        }

        protected virtual void OnImagesChanged()
        {
            this.UpdateControlSize();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((Array.IndexOf<Keys>(EditConsts.NavKeys, e.KeyCode) >= 0) && (Array.IndexOf<Keys>(this.navKeys, e.KeyCode) < 0))
            {
                e.Handled = e.KeyCode != Keys.Tab;
            }
            base.OnKeyDown(e);
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);
            if (this.images != null)
            {
                e.ItemHeight = Math.Max(e.ItemHeight, this.images.ImageSize.Height);
            }
        }

        protected virtual void OnPriorityChanged()
        {
            this.ResetContent(0);
        }

        protected virtual void ProviderChanged()
        {
            base.BeginUpdate();
            try
            {
                base.Items.Clear();
                this.columnWidths.Clear();
                this.itemWidth = 0;
                this.syntaxPaint.Provider = this.provider;
                if (this.provider != null)
                {
                    for (int i = 0; i < this.provider.Count; i++)
                    {
                        string name = this.provider.GetName(i);
                        if (this.IsFiltered(name, this.provider.GetPriority(i)))
                        {
                            base.Items.Add(new ListboxItem(name, i));
                        }
                    }
                    for (int j = 0; j < this.provider.ColumnCount; j++)
                    {
                        int defaultColumnSeparator = EditConsts.DefaultColumnSeparator;
                        if (this.provider.ColumnVisible(j))
                        {
                            ICodeCompletionColumn column = (j < this.columns.Count) ? this.GetColumn(j) : null;
                            for (int k = 0; k < this.provider.Count; k++)
                            {
                                if (this.IsFiltered(this.provider.GetName(k), this.provider.GetPriority(k)))
                                {
                                    defaultColumnSeparator = Math.Max(defaultColumnSeparator, this.MeasureWidth(this.provider.GetColumnText(k, j), (column != null) ? column.FontStyle : FontStyle.Regular));
                                }
                            }
                            defaultColumnSeparator += EditConsts.DefaultColumnSeparator;
                        }
                        this.itemWidth += defaultColumnSeparator;
                        this.columnWidths.Add(defaultColumnSeparator);
                    }
                }
                this.UpdateControlSize();
            }
            finally
            {
                base.EndUpdate();
            }
        }

        public void RemoveColumnAt(int index)
        {
            this.columns.RemoveAt(index);
        }

        public void ResetContent()
        {
            this.ProviderChanged();
        }

        public void ResetContent(int index)
        {
            base.BeginUpdate();
            try
            {
                this.ResetContent();
                if ((index >= 0) && (index < base.Items.Count))
                {
                    this.SelectedIndex = index;
                }
            }
            finally
            {
                base.EndUpdate();
            }
        }

        protected void UpdateControlSize()
        {
            int num = this.painter.FontHeight + EditConsts.DefaultRowSeparator;
            if (this.images != null)
            {
                num = Math.Max(num, this.images.ImageSize.Height);
            }
            this.ItemHeight = num;
            base.HorizontalExtent = ((this.provider != null) && (this.provider.ColumnCount != 0)) ? this.itemWidth : ((this.images != null) ? (this.images.ImageSize.Width + EditConsts.DefaultColumnSeparator) : 0);
            if (this.UpdateSize != null)
            {
                this.UpdateSize(this, EventArgs.Empty);
            }
        }

        public int ColumnCount
        {
            get
            {
                return this.columns.Count;
            }
        }

        public ICodeCompletionColumns Columns
        {
            get
            {
                return this.columns;
            }
        }

        public string Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                if (this.filter != value)
                {
                    this.filter = value;
                    this.OnFilterChanged();
                }
            }
        }

        public bool Filtered
        {
            get
            {
                return this.filtered;
            }
            set
            {
                if (this.filtered != value)
                {
                    this.filtered = value;
                    this.OnFilteredChanged();
                }
            }
        }

        public ImageList Images
        {
            get
            {
                return this.images;
            }
            set
            {
                if (this.images != value)
                {
                    this.images = value;
                    this.OnImagesChanged();
                }
            }
        }

        public int ItemWidth
        {
            get
            {
                return this.itemWidth;
            }
        }

        public Keys[] NavKeys
        {
            get
            {
                return this.navKeys;
            }
        }

        public int Priority
        {
            get
            {
                return this.priority;
            }
            set
            {
                if (this.priority != value)
                {
                    this.priority = value;
                    this.OnPriorityChanged();
                }
            }
        }

        public ICodeCompletionProvider Provider
        {
            get
            {
                return this.provider;
            }
            set
            {
                if (this.provider != value)
                {
                    this.provider = value;
                    this.ProviderChanged();
                }
            }
        }

        public EventHandler UpdateSize
        {
            get
            {
                return this.updateSize;
            }
            set
            {
                this.updateSize = value;
            }
        }

        internal class ListboxItem
        {
            public int Index;
            public string String;

            public ListboxItem(string s, int index)
            {
                this.String = s;
                this.Index = index;
            }

            public override string ToString()
            {
                return this.String;
            }
        }
    }
}

