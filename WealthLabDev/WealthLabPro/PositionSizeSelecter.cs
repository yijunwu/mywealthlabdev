namespace WealthLabPro
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxBitmap(typeof(PositionSizeSelecter), "PositionSizeSelecter")]
    public class PositionSizeSelecter : UserControl
    {
        [CompilerGenerated]
        private bool bool_0;
        private Button btnSelect;
        private IContainer icontainer_0;
        private ImageList imageList_0;
        private Label lblCaption;
        private WealthLab.PositionSize positionSize_0 = new WealthLab.PositionSize();

        public event EventHandler<EventArgs> PositionSizeChanged;

        public PositionSizeSelecter()
        {
            this.InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            PositionSizeSelecterForm form = new PositionSizeSelecterForm(MainModule.Instance.PosSizers);
            if (this.CombinationStrategyChildMode)
            {
                form.CombinationStrategyChildMode = true;
            }
            Point point = base.PointToScreen(base.Location);
            form.Top = ((point.Y - base.Top) + base.Height) - 2;
            form.Left = point.X - base.Left;
            form.PositionSize = this.PositionSize;
            form.TopMost = true;
            if (form.Bottom > MainModule.Instance.FirstMainForm.Bottom)
            {
                form.Top -= form.Bottom - MainModule.Instance.FirstMainForm.Bottom;
            }
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.PositionSize = form.PositionSize;
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PositionSizeSelecter));
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
            this.btnSelect.TabIndex = 3;
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("imgList.ImageStream");
            this.imageList_0.TransparentColor = Color.Fuchsia;
            this.imageList_0.Images.SetKeyName(0, "Ellipsis.bmp");
            this.lblCaption.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.lblCaption.AutoEllipsis = true;
            this.lblCaption.Location = new Point(3, 4);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new Size(0x99, 13);
            this.lblCaption.TabIndex = 4;
            this.lblCaption.Text = "$5000";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Honeydew;
            base.Controls.Add(this.lblCaption);
            base.Controls.Add(this.btnSelect);
            base.Name = "PositionSizeSelecter";
            base.Size = new Size(0xb2, 20);
            base.ResumeLayout(false);
        }

        public void UpdateText()
        {
            if (this.positionSize_0.RawProfitMode)
            {
                this.BackColor = Color.Linen;
            }
            else
            {
                this.BackColor = Color.Honeydew;
            }
            this.lblCaption.Text = this.positionSize_0.Text;
        }

        public bool CombinationStrategyChildMode
        {
            [CompilerGenerated]
            get
            {
                return this.bool_0;
            }
            [CompilerGenerated]
            set
            {
                this.bool_0 = value;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WealthLab.PositionSize PositionSize
        {
            get
            {
                return this.positionSize_0;
            }
            set
            {
                this.positionSize_0 = value;
                this.UpdateText();
            }
        }
    }
}

