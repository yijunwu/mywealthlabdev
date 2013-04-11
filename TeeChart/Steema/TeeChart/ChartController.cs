namespace Steema.TeeChart
{
    using Steema.TeeChart.Drawing;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Editors.Export;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [ToolboxBitmap(typeof(ChartController), "Images.ChartController.bmp")]
    public class ChartController : ToolStrip, ITeeEventListener
    {
        private ToolStripButton bCopy = new ToolStripButton();
        private ToolStripButton bDepth = new ToolStripButton();
        private ToolStripButton bEdit = new ToolStripButton();
        private ToolStripButton bExport = new ToolStripButton();
        private ToolStripButton bLabel = new ToolStripButton();
        private ToolStripButton bMove = new ToolStripButton();
        private ToolStripButton bNormal = new ToolStripButton();
        private ToolStripButton bPrint = new ToolStripButton();
        private ToolStripButton bRotate = new ToolStripButton();
        private ToolStripSeparator bSeparator = new ToolStripSeparator();
        private ControllerButtonSize buttonSize;
        private ToolStripButton bView3D = new ToolStripButton();
        private ToolStripButton bZoom = new ToolStripButton();
        private TChart chart;
        private IContainer components;
        private bool dragging;
        private int draggingIndex;
        private Steema.TeeChart.Editor editor;
        private string label;
        private bool labelValues;
        private MouseButtons mouseButtons;
        private int oldX;
        private int oldY;
        private TChart panel;
        private ToolStripItemCollection theItems;

        public event SetLabelEventHandler SetLabel;

        public ChartController()
        {
            if (!base.DesignMode)
            {
                DesignTimeOptions.InitLanguage(false);
            }
            this.AddToolBarButtons(this.CreateStandardItems());
            this.InitializeComponent();
            this.labelValues = true;
            base.ItemClicked += new ToolStripItemClickedEventHandler(this.ChartController_ItemClicked);
            this.SetLabel = (SetLabelEventHandler) Delegate.Combine(this.SetLabel, new SetLabelEventHandler(this.ChartController_SetLabel));
            this.editor = null;
        }

        private void AddToolBarButtons(ToolStripItemCollection tItems)
        {
            this.theItems = tItems;
            this.Items.AddRange(tItems);
        }

        private bool ButtonNormalUp()
        {
            if (this.bNormal == null)
            {
                return !this.bNormal.Checked;
            }
            return true;
        }

        private int CalcAngleChange(int aAngle, int aChange)
        {
            int num;
            if (aChange > 0)
            {
                return Math.Min(360, aAngle + aChange);
            }
            if (this.Panel.Graphics3D.SupportsFullRotation)
            {
                num = 0;
            }
            else
            {
                num = 270;
            }
            return Math.Max(num, aAngle + aChange);
        }

        private int CalcDistPercent(int aPercent, int aWidth, int aHeight, int X, int Y)
        {
            int num = Utils.Round(Math.Sqrt(Utils.Sqr((1.0 * this.oldX) - X) + Utils.Sqr((1.0 * this.oldY) - Y)));
            int num2 = Utils.Round(Math.Sqrt(Utils.Sqr(1.0 * aWidth) + Utils.Sqr(1.0 * aHeight)));
            return Utils.Round((double) (((1.0 * aPercent) * num) / ((double) num2)));
        }

        private void ChartController_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem is ToolStripButton)
            {
                this.DoItemChecked(e.ClickedItem as ToolStripButton);
            }
        }

        private void ChartController_SetLabel(object sender, SetLabelEventArgs e)
        {
            this.bLabel.Text = e.StatusMsg;
        }

        private int CorrectAngle(int aAngle)
        {
            int num = aAngle;
            if (num > 360)
            {
                return (num - 360);
            }
            if (num < 0)
            {
                num = 360 + num;
            }
            return num;
        }

        private ToolStripItemCollection CreateStandardItems()
        {
            string str = "16x16";
            ImageList list = new ImageList();
            if (this.buttonSize == ControllerButtonSize.x24)
            {
                str = "24x24";
                base.ImageScalingSize = new Size(new Point(0x18, 0x18));
                list.ImageSize = new Size(new Point(0x18, 0x18));
            }
            else
            {
                str = "16x16";
                base.ImageScalingSize = new Size(new Point(0x10, 0x10));
                list.ImageSize = new Size(new Point(0x10, 0x10));
            }
            list.TransparentColor = Color.Lime;
            list.Images.Add("Copy", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.Copy" + str + ".bmp"));
            list.Images.Add("Depth", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.Depth" + str + ".bmp"));
            list.Images.Add("Edit", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.Edit" + str + ".bmp"));
            list.Images.Add("Export", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.Export" + str + ".bmp"));
            list.Images.Add("Move", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.Move" + str + ".bmp"));
            list.Images.Add("Normal", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.Normal" + str + ".bmp"));
            list.Images.Add("Print", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.Print" + str + ".bmp"));
            list.Images.Add("Rotate", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.Rotate" + str + ".bmp"));
            list.Images.Add("View3D", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.View3D" + str + ".bmp"));
            list.Images.Add("Zoom", Utils.GetBitmapResource("Steema.TeeChart.Images.CommanderIcons.Zoom" + str + ".bmp"));
            base.ImageList = list;
            this.bNormal.ImageKey = "Normal";
            this.bNormal.Checked = true;
            this.bNormal.ToolTipText = Texts.CommanMsgNormal;
            this.bNormal.CheckOnClick = true;
            this.bNormal.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bNormal.Tag = "bNormal";
            this.bSeparator.Tag = "bSeparator";
            this.bRotate.ImageKey = "Rotate";
            this.bRotate.CheckOnClick = true;
            this.bRotate.ToolTipText = Texts.CommanMsgRotate;
            this.bRotate.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bRotate.Tag = "bRotate";
            this.bMove.ImageKey = "Move";
            this.bMove.CheckOnClick = true;
            this.bMove.ToolTipText = Texts.CommanMsgMove;
            this.bMove.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bMove.Tag = "bMove";
            this.bZoom.ImageKey = "Zoom";
            this.bZoom.CheckOnClick = true;
            this.bZoom.ToolTipText = Texts.CommanMsgZoom;
            this.bZoom.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bZoom.Tag = "bZoom";
            this.bDepth.ImageKey = "Depth";
            this.bDepth.CheckOnClick = true;
            this.bDepth.ToolTipText = Texts.CommanMsgDepth;
            this.bDepth.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bDepth.Tag = "bDepth";
            this.bView3D.ImageKey = "View3D";
            this.bView3D.CheckOnClick = false;
            this.bView3D.ToolTipText = Texts.CommanMsg3D;
            this.bView3D.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bView3D.Tag = "bView3D";
            this.bEdit.ImageKey = "Edit";
            this.bEdit.CheckOnClick = true;
            this.bEdit.ToolTipText = Texts.CommanMsgEdit;
            this.bEdit.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bEdit.Tag = "bEdit";
            this.bPrint.ImageKey = "Print";
            this.bPrint.CheckOnClick = true;
            this.bPrint.ToolTipText = Texts.CommanMsgPrint;
            this.bPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bPrint.Tag = "bPrint";
            this.bCopy.ImageKey = "Copy";
            this.bCopy.CheckOnClick = true;
            this.bCopy.ToolTipText = Texts.CommanMsgCopy;
            this.bCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bCopy.Tag = "bCopy";
            this.bExport.ImageKey = "Export";
            this.bExport.CheckOnClick = true;
            this.bExport.ToolTipText = Texts.CommanMsgSave;
            this.bExport.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.bExport.Tag = "bExport";
            this.bLabel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.bLabel.Tag = "bLabel";
            ///WYJ fix, original: return new ToolStripItem[] { this.bNormal, this.bSeparator, this.bRotate, this.bMove, this.bZoom, this.bDepth, this.bView3D, this.bSeparator, this.bEdit, this.bPrint, this.bCopy, this.bExport, this.bLabel };
            ToolStripItem[] ret = new ToolStripItem[] { this.bNormal, this.bSeparator, this.bRotate, this.bMove, this.bZoom, this.bDepth, this.bView3D, this.bSeparator, this.bEdit, this.bPrint, this.bCopy, this.bExport, this.bLabel };
            return new ToolStripItemCollection(null, ret);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoDepth(int X, int Y)
        {
            this.Set3D();
            int num = this.CalcDistPercent(200, Utils.Round((float) this.Panel.Width), Utils.Round((float) this.Panel.Height), X, Y);
            if (num >= 1)
            {
                this.Panel.Aspect.Chart3DPercent = Math.Min(100, num);
            }
        }

        private void DoItemChecked(ToolStripButton button)
        {
            if (button.CheckOnClick)
            {
                foreach (ToolStripItem item in this.Items)
                {
                    if ((((this.Items.IndexOf(item) != this.Items.IndexOf(button)) && button.Pressed) && ((item is ToolStripButton) && (item != this.bView3D))) && (item != this.bNormal))
                    {
                        (item as ToolStripButton).Checked = false;
                    }
                }
            }
            if (this.Panel != null)
            {
                if (button == this.bEdit)
                {
                    if (this.editor != null)
                    {
                        this.editor.ShowModal();
                    }
                    else
                    {
                        this.Panel.ShowEditor();
                    }
                }
                else if (button == this.bView3D)
                {
                    this.Panel.Aspect.View3D = !this.bView3D.Checked;
                }
                else if (button == this.bCopy)
                {
                    this.Panel.Export.Image.Bitmap.CopyToClipboard();
                }
                else if (button == this.bPrint)
                {
                    this.Panel.Printer.Preview();
                }
                else if (button == this.bExport)
                {
                    ExportEditor.ShowModal(this.Panel.Chart);
                }
                else if (button == this.bMove)
                {
                    this.bNormal.Checked = false;
                }
                else if (button == this.bRotate)
                {
                    this.bNormal.Checked = false;
                }
                else if (button == this.bZoom)
                {
                    this.bNormal.Checked = false;
                }
                else if (button == this.bDepth)
                {
                    this.bNormal.Checked = false;
                }
            }
        }

        private void DoMouseDown(int X, int Y)
        {
            Pie pie = Rotate.FirstSeriesPie(this.Panel.Chart);
            if ((pie != null) || this.ButtonNormalUp())
            {
                this.dragging = true;
                this.oldX = X;
                this.oldY = Y;
                if (pie != null)
                {
                    this.draggingIndex = pie.Clicked(X, Y);
                }
                else
                {
                    this.draggingIndex = -1;
                }
                if (this.Panel != null)
                {
                    this.Panel.Chart.CancelMouse = (this.draggingIndex != -1) || this.ButtonNormalUp();
                }
            }
        }

        private void DoMouseMove(int X, int Y, Keys Shift)
        {
            if (this.dragging)
            {
                this.dragging = false;
                if (this.IsButtonDown(this.bNormal))
                {
                    this.DoNormal(X, Y);
                }
                else if (this.mouseButtons == MouseButtons.Right)
                {
                    this.DoMove(X, Y);
                }
                else if (this.IsButtonDown(this.bRotate))
                {
                    if (Shift == Keys.Shift)
                    {
                        this.DoZoom(X, Y);
                    }
                    else
                    {
                        this.DoRotate(X, Y);
                    }
                }
                else if (this.IsButtonDown(this.bMove))
                {
                    if (Shift == Keys.Shift)
                    {
                        this.DoZoom(X, Y);
                    }
                    else
                    {
                        this.DoMove(X, Y);
                    }
                }
                else if (this.IsButtonDown(this.bZoom))
                {
                    this.DoZoom(X, Y);
                }
                else if (this.IsButtonDown(this.bDepth))
                {
                    this.DoDepth(X, Y);
                }
                if (this.labelValues)
                {
                    this.ShowValues();
                }
                this.dragging = true;
            }
        }

        private void DoMouseUp()
        {
            this.dragging = false;
            this.draggingIndex = -1;
        }

        private void DoMove(int X, int Y)
        {
            this.Set3D();
            this.Panel.Aspect.HorizOffset += X - this.oldX;
            this.Panel.Aspect.VertOffset += Y - this.oldY;
            this.oldX = X;
            this.oldY = Y;
        }

        private void DoNormal(int X, int Y)
        {
            if (this.draggingIndex != -1)
            {
                Pie pie = Rotate.FirstSeriesPie(this.Panel.Chart);
                if (pie != null)
                {
                    int num = Math.Min(100, this.CalcDistPercent(100, pie.CircleWidth, pie.CircleHeight, X, Y));
                    pie.ExplodedSlice[this.draggingIndex] = num;
                    pie.Invalidate();
                }
            }
        }

        private bool DoPanelMouse()
        {
            return (((this.IsButtonUp(this.bRotate) && this.IsButtonUp(this.bDepth)) && this.IsButtonUp(this.bMove)) && this.IsButtonUp(this.bZoom));
        }

        private void DoRotate(int X, int Y)
        {
            this.Set3D();
            this.Panel.Aspect.Orthogonal = false;
            int aChange = Utils.Round((double) ((90.0 * (X - this.oldX)) / ((double) this.Panel.Width)));
            int num2 = Utils.Round((double) ((90.0 * (this.oldY - Y)) / ((double) this.Panel.Height)));
            Pie pie = Rotate.FirstSeriesPie(this.Panel.Chart);
            if (this.Panel.Graphics3D.SupportsFullRotation)
            {
                this.Panel.Aspect.Rotation = this.CorrectAngle(this.Panel.Aspect.Rotation + aChange);
                this.Panel.Aspect.Elevation = this.CorrectAngle(this.Panel.Aspect.Elevation + num2);
            }
            else
            {
                if (pie != null)
                {
                    this.Panel.Aspect.Rotation = 360;
                    if (!this.Panel.Graphics3D.SupportsFullRotation)
                    {
                        this.Panel.Aspect.Perspective = 0;
                    }
                    if (aChange != 0)
                    {
                        pie.RotationAngle = this.CorrectAngle(pie.RotationAngle + aChange);
                    }
                }
                else
                {
                    this.Panel.Aspect.Rotation = this.CalcAngleChange(this.Panel.Aspect.Rotation, aChange);
                }
                this.Panel.Aspect.Elevation = this.CalcAngleChange(this.Panel.Aspect.Elevation, num2);
            }
            this.oldX = X;
            this.oldY = Y;
        }

        private void DoZoom(int X, int Y)
        {
            this.Set3D();
            int num = Utils.Round((double) ((10.0 * (this.oldY - Y)) / Math.Sqrt(Utils.Sqr((double) this.Panel.Width) + Utils.Sqr((double) this.Panel.Height))));
            int num2 = Utils.Round((double) (((double) (num * this.Panel.Aspect.Zoom)) / 100.0));
            if (num > 0)
            {
                this.Panel.Aspect.Zoom += Math.Max(1, num2);
            }
            else
            {
                this.Panel.Aspect.Zoom += Math.Min(-1, num2);
            }
            if (this.Panel.Aspect.Zoom < 5)
            {
                this.Panel.Aspect.Zoom = 5;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.SuspendLayout();
            base.ResumeLayout(false);
        }

        private bool IsButtonDown(ToolStripButton aButton)
        {
            return ((aButton != null) && aButton.Checked);
        }

        private bool IsButtonUp(ToolStripButton aButton)
        {
            if (aButton != null)
            {
                return !aButton.Checked;
            }
            return true;
        }

        protected internal void OnSetLabel(object sender, string statusText)
        {
            if (this.SetLabel != null)
            {
                SetLabelEventArgs e = new SetLabelEventArgs(statusText);
                this.SetLabel(sender, e);
            }
        }

        private void Set3D()
        {
            this.Panel.Aspect.View3D = true;
            if (this.bView3D != null)
            {
                this.bView3D.Checked = true;
            }
        }

        private void SetLabelCaption(string aCaption)
        {
            string statusText = aCaption;
            if (this.labelValues)
            {
                this.label = statusText;
                this.OnSetLabel(this, statusText);
            }
        }

        private void SetPanel(TChart Value)
        {
            if (this.panel != null)
            {
                this.panel.Chart.RemoveListener(this);
            }
            this.panel = Value;
            if ((this.panel != null) && (this.panel.Chart.Listeners != null))
            {
                this.panel.Chart.Listeners.Add(this);
            }
            this.ShowHideControls(this.panel != null);
        }

        private void ShowHideControls(bool value)
        {
            this.bRotate.Enabled = value;
            this.bMove.Enabled = value;
            this.bZoom.Enabled = value;
            this.bNormal.Enabled = value;
            this.bCopy.Enabled = value;
            this.bExport.Enabled = value;
            this.bPrint.Enabled = value;
            this.bView3D.Enabled = value;
            if (value)
            {
                this.bView3D.Checked = this.panel.Aspect.View3D;
            }
        }

        private void ShowValues()
        {
            string aCaption = "";
            if (this.IsButtonDown(this.bRotate))
            {
                aCaption = string.Format(Texts.CommanMsgRotating, this.Panel.Aspect.Rotation, this.Panel.Aspect.Elevation);
            }
            else if (this.IsButtonDown(this.bMove))
            {
                aCaption = string.Format(Texts.CommanMsgMoving, this.Panel.Aspect.HorizOffset, this.Panel.Aspect.VertOffset);
            }
            else if (this.IsButtonDown(this.bZoom))
            {
                aCaption = string.Format(Texts.CommanMsgZooming, this.Panel.Aspect.Zoom);
            }
            else if (this.IsButtonDown(this.bDepth))
            {
                aCaption = string.Format(Texts.CommanMsgDepthing, this.Panel.Aspect.Chart3DPercent);
            }
            else if (this.IsButtonDown(this.bNormal) && (this.draggingIndex != -1))
            {
                Pie pie = Rotate.FirstSeriesPie(this.Panel.Chart);
                if (pie == null)
                {
                    aCaption = "";
                }
                else
                {
                    aCaption = string.Format(Texts.CommanMsgPieExploding, this.draggingIndex, pie.ExplodedSlice[this.draggingIndex]);
                }
            }
            else if (this.label != null)
            {
                aCaption = "";
            }
            this.SetLabelCaption(aCaption);
        }

        public void TeeEvent(Steema.TeeChart.TeeEvent e)
        {
            if (!(e is TeeMouseEvent))
            {
                if (e is View3DEvent)
                {
                    this.ShowHideControls(this.Panel != null);
                }
            }
            else
            {
                TeeMouseEvent event2 = (TeeMouseEvent) e;
                switch (event2.Event)
                {
                    case MouseEventKinds.Down:
                        this.mouseButtons = event2.Button;
                        this.DoMouseDown(event2.mArgs.X, event2.mArgs.Y);
                        break;

                    case MouseEventKinds.Move:
                        this.DoMouseMove(event2.mArgs.X, event2.mArgs.Y, event2.ModifierKeys);
                        break;

                    case MouseEventKinds.Up:
                        this.DoMouseUp();
                        break;
                }
                event2.sender.CancelMouse = !this.DoPanelMouse();
            }
        }

        public ControllerButtonSize ButtonSize
        {
            get
            {
                return this.buttonSize;
            }
            set
            {
                if (value != this.buttonSize)
                {
                    this.buttonSize = value;
                    this.CreateStandardItems();
                }
            }
        }

        [DefaultValue((string) null)]
        public TChart Chart
        {
            get
            {
                return this.chart;
            }
            set
            {
                this.chart = value;
                this.Panel = this.chart;
            }
        }

        [DefaultValue((string) null)]
        public Steema.TeeChart.Editor Editor
        {
            get
            {
                return this.editor;
            }
            set
            {
                if (value != this.editor)
                {
                    this.editor = value;
                }
            }
        }

        [Browsable(false)]
        public ToolStripItemCollection Items   ///WYJ fix, original public ToolStripItem[] Items
        {
            get
            {
                return this.theItems;
            }
            set
            {
                this.theItems = value;
                this.Items.Clear();
                this.Items.AddRange(this.theItems);
            }
        }

        [Description("Accesses the LabelText for the ChartController ToolStrip."), DefaultValue((string) null)]
        public string LabelText
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }

        [Description("Enables/disables the display of the ChartController's advisory status messages."), DefaultValue(false)]
        public bool LabelValues
        {
            get
            {
                return this.labelValues;
            }
            set
            {
                this.labelValues = value;
            }
        }

        private TChart Panel
        {
            get
            {
                return this.panel;
            }
            set
            {
                this.SetPanel(value);
            }
        }

        public class SetLabelEventArgs : EventArgs
        {
            private string statusMsg;

            public SetLabelEventArgs(string statusText)
            {
                this.statusMsg = statusText;
            }

            public string StatusMsg
            {
                get
                {
                    return this.statusMsg;
                }
            }
        }

        public delegate void SetLabelEventHandler(object sender, ChartController.SetLabelEventArgs e);
    }
}

