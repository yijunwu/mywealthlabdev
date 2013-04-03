namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxBitmap(typeof(BarDataRangeSelecter), "BarDataRangeSelecter")]
    public class BarDataRangeSelecter : UserControl
    {
        private BarDataRange barDataRange_0 = new BarDataRange();
        private bool bool_0;
        private Button btnSelect;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private Label lblCaption;

        private EventHandler<EventArgs> eventHandler_0;

        public event EventHandler<EventArgs> DataRangeChanged
        {
            add
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<EventArgs> eventHandler;
                EventHandler<EventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<EventArgs> eventHandler1 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<EventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public BarDataRangeSelecter()
        {
            this.InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            BarDataRangeSelecterForm form = new BarDataRangeSelecterForm(this.bool_0);
            Point point = base.PointToScreen(base.Location);
            form.Top = ((point.Y - base.Top) + base.Height) - 2;
            form.Left = point.X - base.Left;
            form.Range = this.DataRange.Range;
            form.FixedBarsValue = this.DataRange.FixedBars;
            form.RecentValue = this.DataRange.RecentValue;
            form.StartDate = this.DataRange.StartDate;
            form.EndDate = this.DataRange.EndDate;
            form.TopMost = true;
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.DataRange.Range = form.Range;
                this.DataRange.FixedBars = form.FixedBarsValue;
                this.DataRange.RecentValue = form.RecentValue;
                this.DataRange.StartDate = form.StartDate;
                this.DataRange.EndDate = form.EndDate;
                this.UpdateText();
                if (this.eventHandler_0 != null)
                {
                    this.eventHandler_0(this, EventArgs.Empty);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(BarDataRangeSelecter));
            this.btnSelect = new Button();
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.lblCaption = new Label();
            base.SuspendLayout();
            this.btnSelect.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnSelect.BackgroundImageLayout = ImageLayout.Center;
            this.btnSelect.FlatAppearance.BorderColor = SystemColors.ControlDarkDark;
            this.btnSelect.FlatStyle = FlatStyle.Flat;
            this.btnSelect.Image = (Image) manager.GetObject("btnSelect.Image");
            this.btnSelect.Location = new Point(0xa2, 0);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new Size(0x10, 20);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("imgList.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "Ellipsis.bmp");
            this.lblCaption.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lblCaption.AutoEllipsis = true;
            this.lblCaption.Location = new Point(4, 4);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new Size(0x98, 13);
            this.lblCaption.TabIndex = 2;
            this.lblCaption.Text = "All Data";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.AliceBlue;
            base.Controls.Add(this.lblCaption);
            base.Controls.Add(this.btnSelect);
            base.Name = "BarDataRangeSelecter";
            base.Size = new Size(0xb2, 20);
            base.ResumeLayout(false);
        }

        public void UpdateText()
        {
            this.lblCaption.Text = this.DataRange.Text;
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BarDataRange DataRange
        {
            get
            {
                return this.barDataRange_0;
            }
            set
            {
                this.barDataRange_0 = value;
                this.UpdateText();
            }
        }

        public bool IsStreaming
        {
            get
            {
                return this.bool_0;
            }
            set
            {
                this.barDataRange_0.IsStreaming = value;
                this.bool_0 = value;
            }
        }

       
    }
}

