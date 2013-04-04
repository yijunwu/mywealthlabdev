namespace QWhale.Common
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(ColorBox), "Images.ColorBox.bmp")]
    public class ColorBox : ComboBox
    {
        private const int colorWidth = 0x1c;
        private Container components;
        private Brush fontBrush;

        public ColorBox()
        {
            this.InternalCreate();
            this.InitializeComponent();
        }

        public ColorBox(IContainer container)
        {
            container.Add(this);
            this.InternalCreate();
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private Color GetColor(int index)
        {
            if ((index > 0) && (index < this.Items.Count))
            {
                return Color.FromName(this.Items[index].ToString());
            }
            return Color.Empty;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private void InternalCreate()
        {
            this.fontBrush = new SolidBrush(this.ForeColor);
            base.DropDownStyle = ComboBoxStyle.DropDown;
            base.DrawMode = DrawMode.OwnerDrawFixed;
            this.UpdateColors();
            this.SelectedIndex = 0;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if ((e.Index >= 0) && (e.Index < this.Items.Count))
            {
                e.DrawBackground();
                bool flag = (e.State & DrawItemState.Disabled) != DrawItemState.None;
                bool flag2 = ((e.State & DrawItemState.Focus) != DrawItemState.None) && ((e.State & DrawItemState.NoFocusRect) == DrawItemState.None);
                bool flag3 = (e.State & DrawItemState.Selected) != DrawItemState.None;
                if (flag2)
                {
                    e.DrawFocusRectangle();
                }
                using (Brush brush = new SolidBrush(this.GetColor(e.Index)))
                {
                    string s = (e.Index == 0) ? StringConsts.EmptyColor : this.GetColor(e.Index).Name;
                    Rectangle rect = new Rectangle(e.Bounds.Left, e.Bounds.Top, Math.Min(e.Bounds.Width, 0x1c), e.Bounds.Height);
                    rect.Inflate(-2, -2);
                    Rectangle layoutRectangle = new Rectangle(rect.Right + 1, e.Bounds.Top, (e.Bounds.Width - rect.Right) - 1, e.Bounds.Height);
                    if (e.State == DrawItemState.HotLight)
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                    }
                    e.Graphics.FillRectangle(brush, rect);
                    e.Graphics.DrawRectangle(Pens.Black, rect);
                    Brush fontBrush = this.fontBrush;
                    if (flag)
                    {
                        fontBrush = Brushes.Gray;
                    }
                    else if (flag3)
                    {
                        fontBrush = SystemBrushes.HighlightText;
                    }
                    e.Graphics.DrawString(s, this.Font, fontBrush, layoutRectangle);
                }
            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.fontBrush = new SolidBrush(this.ForeColor);
        }

        private void UpdateColors()
        {
            this.Items.Add(StringConsts.EmptyColor);
            PropertyInfo[] properties = typeof(Color).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].PropertyType == typeof(Color))
                {
                    this.Items.Add(properties[i].Name);
                }
            }
            properties = typeof(SystemColors).GetProperties();
            for (int j = 0; j < properties.Length; j++)
            {
                if (properties[j].PropertyType == typeof(Color))
                {
                    this.Items.Add(properties[j].Name);
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ComboBox.ObjectCollection Items
        {
            get
            {
                return base.Items;
            }
        }

        [Description("Gets or sets curently selected color in the \"ColorBox\"."), Category("Appearence")]
        public Color SelectedColor
        {
            get
            {
                if (base.SelectedItem == null)
                {
                    return Color.Empty;
                }
                return this.GetColor(this.SelectedIndex);
            }
            set
            {
                if (value == Color.Empty)
                {
                    this.SelectedIndex = 0;
                }
                else
                {
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        if (string.Compare(value.Name, this.Items[i].ToString(), true) == 0)
                        {
                            this.SelectedIndex = i;
                            return;
                        }
                    }
                }
            }
        }

        [Description("Represents text associated with this control.")]
        public string Text
        {
            get
            {
                return base.Text;
            }
        }
    }
}

