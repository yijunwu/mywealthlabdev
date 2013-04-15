namespace WealthLabPro
{
    using Fidelity.Components;
    using log4net;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;

    public class OrdersAlertsForm : Form, IWorkspace
    {
        private ToolStripButton btnAutoRemove;
        private ToolStripButton btnCancelAll;
        private ToolStripButton btnCancelReplace;
        private ToolStripButton btnCancelSelected;
        private ToolStripButton btnEdit;
        private ToolStripButton btnHelp;
        private ToolStripButton btnPlace;
        private ToolStripButton btnPreferences;
        private ToolStripButton btnRemove;
        private ToolStripButton btnRemoveCompleted;
        private ToolStripButton btnUpdate;
        private ToolStripComboBox cmbAccount;
        private ToolStripComboBox cmbAutoTrading;
        private ColumnHeader columnHeader_0;
        private ColumnHeader columnHeader_1;
        private ColumnHeader columnHeader_10;
        private ColumnHeader columnHeader_11;
        private ColumnHeader columnHeader_12;
        private ColumnHeader columnHeader_13;
        private ColumnHeader columnHeader_14;
        private ColumnHeader columnHeader_15;
        private ColumnHeader columnHeader_16;
        private ColumnHeader columnHeader_17;
        private ColumnHeader columnHeader_18;
        private ColumnHeader columnHeader_2;
        private ColumnHeader columnHeader_3;
        private ColumnHeader columnHeader_4;
        private ColumnHeader columnHeader_5;
        private ColumnHeader columnHeader_6;
        private ColumnHeader columnHeader_7;
        private ColumnHeader columnHeader_8;
        private ColumnHeader columnHeader_9;
        private Font font_0;
        private Font font_1;
        private IContainer icontainer_0;
        private static readonly ILog ilog_0 = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ImageList imageList_0;
        private ImageList imageList_1;
        public static OrdersAlertsForm Instance = null;
        private ToolStripLabel lblAccount;
        private ToolStripLabel lblAutoTrading;
        private Label lblFullText;
        private ToolStripLabel lblMessages;
        private ToolStripLabel lblOrders;
        private ToolStripStatusLabel lblStatusBar;
        private List<Order> list_0 = new List<Order>();
        private List<Order> list_1 = new List<Order>();
        private SortableListView lvMessages;
        private SortableListView lvOrders;
        private ToolStripMenuItem mniCancel;
        private ToolStripMenuItem mniCancelReplace;
        private ToolStripMenuItem mniCopy;
        private ToolStripMenuItem mniEdit;
        private ToolStripMenuItem mniPlace;
        private ToolStripMenuItem mniPlaceLimit;
        private ToolStripMenuItem mniPrint;
        private ToolStripMenuItem mniRemoveCompleted;
        private ToolStripMenuItem mniRemoveSelected;
        private Panel pnlMessage;
        private ContextMenuStrip popupOrders;
        private ToolStripSeparator sepAccounts;
        private ToolStripSeparator sepAuto;
        private ToolStripSeparator sepCancel;
        private ToolStripSeparator sepCopy;
        private ToolStripSeparator sepHelp;
        private ToolStripSeparator sepPlace;
        private ToolStripSeparator sepRemove;
        private SplitContainer split;
        private StatusStrip status;
        private ToolStripStatusLabel statusActive;
        private ToolStripStatusLabel statusFilled;
        private ToolStripStatusLabel statusOrders;
        private static string string_0 = "All Live Accounts";
        private ToolStrip toolbar;
        private ToolStrip toolbarManipulate;
        private ToolStrip toolbarMessages;
        private ToolStripSeparator toolStripSeparator2;
        public WebBrowser txtMessageBrowser;

        public OrdersAlertsForm()
        {
            this.InitializeComponent();
            this.font_0 = this.Font;
            this.font_1 = new Font(this.font_0, FontStyle.Bold);
        }

        public ListViewItem AddOrder(Order order)
        {
            ListViewItem item = this.lvOrders.Items.Add(order.Account);
            item.Tag = order;
            order.Tag = item;
            item.UseItemStyleForSubItems = false;
            this.method_7(order, item);
            return item;
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "This action will Cancel any Active Orders and turn Auto-Trading off - Continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (this.cmbAutoTrading.SelectedIndex != 0)
                {
                    this.cmbAutoTrading.SelectedIndex = 0;
                }
                MainModule.Instance.TradeManager.CancelAll();
            }
        }

        private void btnCancelReplace_Click(object sender, EventArgs e)
        {
            if (this.lvOrders.SelectedItems.Count == 1)
            {
                ListViewItem item = this.lvOrders.SelectedItems[0];
                Order tag = (Order) item.Tag;
                Order order = new Order(tag);
                EditOrderForm form = new EditOrderForm(order) {
                    Text = "Cancel/Replace Order",
                    CancelReplaceMode = true
                };
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    MainModule.Instance.TradeManager.CancelReplaceOrder(tag, order);
                }
            }
        }

        private void btnCancelSelected_Click(object sender, EventArgs e)
        {
            this.list_0.Clear();
            foreach (Order order in this.SelectedOrders)
            {
                if (this.method_6(order) && order.IsActiveAtBackEnd)
                {
                    this.list_0.Add(order);
                }
            }
            MainModule.Instance.TradeManager.CancelOrders(this.list_0);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.lvOrders.SelectedItems.Count == 1)
            {
                Order tag = (Order) this.lvOrders.SelectedItems[0].Tag;
                EditOrderForm form = new EditOrderForm(tag);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    this.method_7(tag, this.lvOrders.SelectedItems[0]);
                    if (tag.Status == OrderStatus.Error)
                    {
                        MainModule.Instance.TradeManager.OrderStatusUpdate(tag.OrderID, OrderStatus.Staged, DateTime.Now, 0.0, 0.0, 0, "");
                    }
                    this.lvOrders_SelectedIndexChanged(this, e);
                }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MainModule.Instance.ContextSensitiveHelp("orders.htm");
        }

        private void btnPlace_Click(object sender, EventArgs e)
        {
            this.list_0.Clear();
            foreach (Order order in this.SelectedOrders)
            {
                if ((order.Status == OrderStatus.Staged) && this.method_6(order))
                {
                    if (order.Account.StartsWith("Paper"))
                    {
                        order.AlertDate = MainModule.Instance.AuthProvider.GetCurrentDateTime;
                    }
                    this.list_0.Add(order);
                }
            }
            if (this.list_0.Count > 0)
            {
                MainModule.Instance.TradeManager.PlaceOrder(this.list_0);
            }
        }

        private void btnPreferences_Click(object sender, EventArgs e)
        {
            new PreferencesForm().ShowDialog("Trading");
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.list_0.Clear();
            foreach (Order order in this.SelectedOrders)
            {
                if (!order.IsActive)
                {
                    this.list_0.Add(order);
                }
            }
            if (this.list_0.Count > 0)
            {
                MainModule.Instance.TradeManager.RemoveOrders(this.list_0);
                this.method_8();
            }
        }

        private void btnRemoveCompleted_Click(object sender, EventArgs e)
        {
            MainModule.Instance.TradeManager.RemoveCompleted(this.SelectedAccountNumber);
            this.method_8();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.list_0.Clear();
            foreach (ListViewItem item in this.lvOrders.Items)
            {
                Order tag = (Order) item.Tag;
                if ((tag.IsActiveAtBackEnd || (tag.Status == OrderStatus.CancelPending)) || (tag.Status == OrderStatus.Unknown))
                {
                    this.list_0.Add(tag);
                }
            }
            if (this.list_0.Count > 0)
            {
                MainModule.Instance.BrokerProvider.RequestOrderStatusUpdatesForOrders(this.list_0);
            }
        }

        private void cmbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.method_8();
            this.btnCancelAll.Enabled = this.IsLoggedIn || this.PaperAccountSelected;
        }

        private void cmbAutoTrading_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnAutoRemove.Visible = this.cmbAutoTrading.SelectedIndex != 0;
            MainModule.Instance.AutoTradingEnabled = (AutoTradingMode) this.cmbAutoTrading.SelectedIndex;
        }

        public void CopyToClipboard()
        {
            MainModule.Instance.CopyListViewToClipboard(this.lvOrders);
        }

        public void DisplayStatusBarMessage(string message)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate61(this.method_5), new object[] { message });
            }
            else
            {
                this.method_5(message);
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

        public ListViewItem FindOrderItem(Order order)
        {
            foreach (ListViewItem item in this.lvOrders.Items)
            {
                if (item.Tag != order)
                {
                    continue;
                }
                ListViewItem listViewItem = item;
                return listViewItem;
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.icontainer_0 = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(OrdersAlertsForm));
            this.imageList_0 = new ImageList(this.icontainer_0);
            this.status = new StatusStrip();
            this.statusOrders = new ToolStripStatusLabel();
            this.statusActive = new ToolStripStatusLabel();
            this.statusFilled = new ToolStripStatusLabel();
            this.lblStatusBar = new ToolStripStatusLabel();
            this.split = new SplitContainer();
            this.lvOrders = new SortableListView();
            this.columnHeader_0 = new ColumnHeader();
            this.columnHeader_1 = new ColumnHeader();
            this.columnHeader_2 = new ColumnHeader();
            this.columnHeader_3 = new ColumnHeader();
            this.columnHeader_4 = new ColumnHeader();
            this.columnHeader_5 = new ColumnHeader();
            this.columnHeader_6 = new ColumnHeader();
            this.columnHeader_7 = new ColumnHeader();
            this.columnHeader_8 = new ColumnHeader();
            this.columnHeader_9 = new ColumnHeader();
            this.columnHeader_16 = new ColumnHeader();
            this.columnHeader_10 = new ColumnHeader();
            this.columnHeader_11 = new ColumnHeader();
            this.columnHeader_12 = new ColumnHeader();
            this.columnHeader_13 = new ColumnHeader();
            this.columnHeader_17 = new ColumnHeader();
            this.popupOrders = new ContextMenuStrip(this.icontainer_0);
            this.mniEdit = new ToolStripMenuItem();
            this.mniPlace = new ToolStripMenuItem();
            this.mniPlaceLimit = new ToolStripMenuItem();
            this.sepPlace = new ToolStripSeparator();
            this.mniCancel = new ToolStripMenuItem();
            this.mniCancelReplace = new ToolStripMenuItem();
            this.sepCancel = new ToolStripSeparator();
            this.mniRemoveSelected = new ToolStripMenuItem();
            this.mniRemoveCompleted = new ToolStripMenuItem();
            this.sepCopy = new ToolStripSeparator();
            this.mniCopy = new ToolStripMenuItem();
            this.mniPrint = new ToolStripMenuItem();
            this.imageList_1 = new ImageList(this.icontainer_0);
            this.toolbarManipulate = new ToolStrip();
            this.btnEdit = new ToolStripButton();
            this.btnPlace = new ToolStripButton();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.btnCancelSelected = new ToolStripButton();
            this.btnCancelReplace = new ToolStripButton();
            this.sepRemove = new ToolStripSeparator();
            this.btnRemove = new ToolStripButton();
            this.btnRemoveCompleted = new ToolStripButton();
            this.btnAutoRemove = new ToolStripButton();
            this.toolbar = new ToolStrip();
            this.lblOrders = new ToolStripLabel();
            this.lblAccount = new ToolStripLabel();
            this.cmbAccount = new ToolStripComboBox();
            this.sepAccounts = new ToolStripSeparator();
            this.btnCancelAll = new ToolStripButton();
            this.sepAuto = new ToolStripSeparator();
            this.lblAutoTrading = new ToolStripLabel();
            this.cmbAutoTrading = new ToolStripComboBox();
            this.sepHelp = new ToolStripSeparator();
            this.btnUpdate = new ToolStripButton();
            this.btnPreferences = new ToolStripButton();
            this.btnHelp = new ToolStripButton();
            this.lvMessages = new SortableListView();
            this.columnHeader_14 = new ColumnHeader();
            this.columnHeader_15 = new ColumnHeader();
            this.pnlMessage = new Panel();
            this.txtMessageBrowser = new WebBrowser();
            this.lblFullText = new Label();
            this.toolbarMessages = new ToolStrip();
            this.lblMessages = new ToolStripLabel();
            this.columnHeader_18 = new ColumnHeader();
            this.status.SuspendLayout();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.popupOrders.SuspendLayout();
            this.toolbarManipulate.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.pnlMessage.SuspendLayout();
            this.toolbarMessages.SuspendLayout();
            base.SuspendLayout();
            this.imageList_0.ImageStream = (ImageListStreamer) manager.GetObject("imgOrders.ImageStream");
            this.imageList_0.TransparentColor = Color.Silver;
            this.imageList_0.Images.SetKeyName(0, "buy.bmp");
            this.imageList_0.Images.SetKeyName(1, "sell.bmp");
            this.imageList_0.Images.SetKeyName(2, "short.bmp");
            this.imageList_0.Images.SetKeyName(3, "cover.bmp");
            this.status.Items.AddRange(new ToolStripItem[] { this.statusOrders, this.statusActive, this.statusFilled, this.lblStatusBar });
            this.status.Location = new Point(0, 0x178);
            this.status.Name = "status";
            this.status.Size = new Size(0x3dd, 0x18);
            this.status.TabIndex = 6;
            this.status.Text = "statusStrip1";
            this.statusOrders.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusOrders.Name = "statusOrders";
            this.statusOrders.Size = new Size(0x37, 0x13);
            this.statusOrders.Text = "0 Orders";
            this.statusActive.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusActive.Name = "statusActive";
            this.statusActive.Size = new Size(0x35, 0x13);
            this.statusActive.Text = "0 Active";
            this.statusFilled.BorderSides = ToolStripStatusLabelBorderSides.Right;
            this.statusFilled.Name = "statusFilled";
            this.statusFilled.Size = new Size(0x30, 0x13);
            this.statusFilled.Text = "0 Filled";
            this.lblStatusBar.Name = "lblStatusBar";
            this.lblStatusBar.Size = new Size(0, 0x13);
            this.split.BackColor = SystemColors.ControlDark;
            this.split.Dock = DockStyle.Fill;
            this.split.Location = new Point(0, 0);
            this.split.Name = "split";
            this.split.Orientation = Orientation.Horizontal;
            this.split.Panel1.BackColor = SystemColors.Control;
            this.split.Panel1.Controls.Add(this.lvOrders);
            this.split.Panel1.Controls.Add(this.toolbarManipulate);
            this.split.Panel1.Controls.Add(this.toolbar);
            this.split.Panel2.BackColor = SystemColors.Control;
            this.split.Panel2.Controls.Add(this.lvMessages);
            this.split.Panel2.Controls.Add(this.pnlMessage);
            this.split.Panel2.Controls.Add(this.toolbarMessages);
            this.split.Size = new Size(0x3dd, 0x178);
            this.split.SplitterDistance = 0xfc;
            this.split.TabIndex = 7;
            this.lvOrders.Columns.AddRange(new ColumnHeader[] { 
                this.columnHeader_0, this.columnHeader_1, this.columnHeader_2, this.columnHeader_3, this.columnHeader_4, this.columnHeader_5, this.columnHeader_6, this.columnHeader_7, this.columnHeader_8, this.columnHeader_9, this.columnHeader_16, this.columnHeader_10, this.columnHeader_11, this.columnHeader_12, this.columnHeader_13, this.columnHeader_17, 
                this.columnHeader_18
             });
            this.lvOrders.ContextMenuStrip = this.popupOrders;
            this.lvOrders.Dock = DockStyle.Fill;
            this.lvOrders.FullRowSelect = true;
            this.lvOrders.HideSelection = false;
            this.lvOrders.Location = new Point(0, 50);
            this.lvOrders.Name = "lvOrders";
            this.lvOrders.Size = new Size(0x3dd, 0xca);
            this.lvOrders.SmallImageList = this.imageList_0;
            this.lvOrders.StateImageList = this.imageList_1;
            this.lvOrders.TabIndex = 0x10;
            this.lvOrders.UseCompatibleStateImageBehavior = false;
            this.lvOrders.View = View.Details;
            this.lvOrders.SelectedIndexChanged += new EventHandler(this.lvOrders_SelectedIndexChanged);
            this.lvOrders.DoubleClick += new EventHandler(this.lvOrders_DoubleClick);
            this.lvOrders.KeyDown += new KeyEventHandler(this.lvOrders_KeyDown);
            this.columnHeader_0.Text = "Account";
            this.columnHeader_0.Width = 80;
            this.columnHeader_1.Text = "Date/Time";
            this.columnHeader_1.Width = 120;
            this.columnHeader_2.Text = "Status";
            this.columnHeader_2.Width = 80;
            this.columnHeader_3.Text = "Symbol";
            this.columnHeader_4.Text = "Action";
            this.columnHeader_5.Text = "Type";
            this.columnHeader_6.Text = "Quantity";
            this.columnHeader_6.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_7.Text = "Order Price";
            this.columnHeader_7.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_7.Width = 0x42;
            this.columnHeader_8.Text = "Filled";
            this.columnHeader_8.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_9.Text = "Fill Price";
            this.columnHeader_9.TextAlign = HorizontalAlignment.Right;
            this.columnHeader_16.Text = "Route";
            this.columnHeader_10.Text = "Strategy";
            this.columnHeader_11.Text = "Signal";
            this.columnHeader_12.Text = "Scale";
            this.columnHeader_13.Text = "Message";
            this.columnHeader_17.Text = "TIF";
            this.popupOrders.Items.AddRange(new ToolStripItem[] { this.mniEdit, this.mniPlace, this.mniPlaceLimit, this.sepPlace, this.mniCancel, this.mniCancelReplace, this.sepCancel, this.mniRemoveSelected, this.mniRemoveCompleted, this.sepCopy, this.mniCopy, this.mniPrint });
            this.popupOrders.Name = "popupOrders";
            this.popupOrders.Size = new Size(0xed, 220);
            this.mniEdit.Image = (Image) manager.GetObject("mniEdit.Image");
            this.mniEdit.ImageTransparentColor = Color.Fuchsia;
            this.mniEdit.Name = "mniEdit";
            this.mniEdit.Size = new Size(0xec, 0x16);
            this.mniEdit.Text = "Edit Selected Order";
            this.mniEdit.Click += new EventHandler(this.btnEdit_Click);
            this.mniPlace.Image = (Image) manager.GetObject("mniPlace.Image");
            this.mniPlace.ImageTransparentColor = Color.Fuchsia;
            this.mniPlace.Name = "mniPlace";
            this.mniPlace.Size = new Size(0xec, 0x16);
            this.mniPlace.Text = "Place Selected Order(s)";
            this.mniPlace.Click += new EventHandler(this.btnPlace_Click);
            this.mniPlaceLimit.Name = "mniPlaceLimit";
            this.mniPlaceLimit.Size = new Size(0xec, 0x16);
            this.mniPlaceLimit.Text = "Place Limit on Open";
            this.mniPlaceLimit.Visible = false;
            this.mniPlaceLimit.Click += new EventHandler(this.mniPlaceLimit_Click);
            this.sepPlace.Name = "sepPlace";
            this.sepPlace.Size = new Size(0xe9, 6);
            this.mniCancel.Image = (Image) manager.GetObject("mniCancel.Image");
            this.mniCancel.ImageTransparentColor = Color.Fuchsia;
            this.mniCancel.Name = "mniCancel";
            this.mniCancel.Size = new Size(0xec, 0x16);
            this.mniCancel.Text = "Cancel Selected Order(s)";
            this.mniCancel.Click += new EventHandler(this.btnCancelSelected_Click);
            this.mniCancelReplace.Image = (Image) manager.GetObject("mniCancelReplace.Image");
            this.mniCancelReplace.ImageTransparentColor = Color.Fuchsia;
            this.mniCancelReplace.Name = "mniCancelReplace";
            this.mniCancelReplace.Size = new Size(0xec, 0x16);
            this.mniCancelReplace.Text = "Cancel/Replace Selected Order";
            this.mniCancelReplace.Click += new EventHandler(this.btnCancelReplace_Click);
            this.sepCancel.Name = "sepCancel";
            this.sepCancel.Size = new Size(0xe9, 6);
            this.mniRemoveSelected.Image = (Image) manager.GetObject("mniRemoveSelected.Image");
            this.mniRemoveSelected.ImageTransparentColor = Color.Fuchsia;
            this.mniRemoveSelected.Name = "mniRemoveSelected";
            this.mniRemoveSelected.Size = new Size(0xec, 0x16);
            this.mniRemoveSelected.Text = "Remove Selected Order(s)";
            this.mniRemoveSelected.Click += new EventHandler(this.btnRemove_Click);
            this.mniRemoveCompleted.Image = (Image) manager.GetObject("mniRemoveCompleted.Image");
            this.mniRemoveCompleted.ImageTransparentColor = Color.Fuchsia;
            this.mniRemoveCompleted.Name = "mniRemoveCompleted";
            this.mniRemoveCompleted.Size = new Size(0xec, 0x16);
            this.mniRemoveCompleted.Text = "Remove all Completed Orders";
            this.mniRemoveCompleted.Click += new EventHandler(this.btnRemoveCompleted_Click);
            this.sepCopy.Name = "sepCopy";
            this.sepCopy.Size = new Size(0xe9, 6);
            this.mniCopy.Image = (Image) manager.GetObject("mniCopy.Image");
            this.mniCopy.ImageTransparentColor = Color.Fuchsia;
            this.mniCopy.Name = "mniCopy";
            this.mniCopy.Size = new Size(0xec, 0x16);
            this.mniCopy.Text = "Copy to Clipboard";
            this.mniCopy.Click += new EventHandler(this.mniCopy_Click);
            this.mniPrint.Image = (Image) manager.GetObject("mniPrint.Image");
            this.mniPrint.Name = "mniPrint";
            this.mniPrint.Size = new Size(0xec, 0x16);
            this.mniPrint.Text = "Print";
            this.mniPrint.Click += new EventHandler(this.mniPrint_Click);
            this.imageList_1.ColorDepth = ColorDepth.Depth8Bit;
            this.imageList_1.ImageSize = new Size(14, 14);
            this.imageList_1.TransparentColor = Color.Fuchsia;
            this.toolbarManipulate.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarManipulate.Items.AddRange(new ToolStripItem[] { this.btnEdit, this.btnPlace, this.toolStripSeparator2, this.btnCancelSelected, this.btnCancelReplace, this.sepRemove, this.btnRemove, this.btnRemoveCompleted, this.btnAutoRemove });
            this.toolbarManipulate.Location = new Point(0, 0x19);
            this.toolbarManipulate.Name = "toolbarManipulate";
            this.toolbarManipulate.Size = new Size(0x3dd, 0x19);
            this.toolbarManipulate.TabIndex = 15;
            this.toolbarManipulate.Text = "toolStrip1";
            this.btnEdit.Enabled = false;
            this.btnEdit.Image = (Image) manager.GetObject("btnEdit.Image");
            this.btnEdit.ImageTransparentColor = Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(0x5e, 0x16);
            this.btnEdit.Text = "Edit Selected";
            this.btnEdit.ToolTipText = "Edit the selected Order";
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            this.btnPlace.Enabled = false;
            this.btnPlace.Image = (Image) manager.GetObject("btnPlace.Image");
            this.btnPlace.ImageTransparentColor = Color.Magenta;
            this.btnPlace.Name = "btnPlace";
            this.btnPlace.Size = new Size(0x66, 0x16);
            this.btnPlace.Text = "Place Selected";
            this.btnPlace.ToolTipText = "Place the selected Orders";
            this.btnPlace.Click += new EventHandler(this.btnPlace_Click);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new Size(6, 0x19);
            this.btnCancelSelected.Enabled = false;
            this.btnCancelSelected.Image = (Image) manager.GetObject("btnCancelSelected.Image");
            this.btnCancelSelected.ImageTransparentColor = Color.Magenta;
            this.btnCancelSelected.Name = "btnCancelSelected";
            this.btnCancelSelected.Size = new Size(110, 0x16);
            this.btnCancelSelected.Text = "Cancel Selected";
            this.btnCancelSelected.ToolTipText = "Cancel the selected Orders";
            this.btnCancelSelected.Click += new EventHandler(this.btnCancelSelected_Click);
            this.btnCancelReplace.Enabled = false;
            this.btnCancelReplace.Image = (Image) manager.GetObject("btnCancelReplace.Image");
            this.btnCancelReplace.ImageTransparentColor = Color.Magenta;
            this.btnCancelReplace.Name = "btnCancelReplace";
            this.btnCancelReplace.Size = new Size(0x9c, 0x16);
            this.btnCancelReplace.Text = "Cancel/Replace Selected";
            this.btnCancelReplace.ToolTipText = "Cancel and Replace the selected Order";
            this.btnCancelReplace.Click += new EventHandler(this.btnCancelReplace_Click);
            this.sepRemove.Name = "sepRemove";
            this.sepRemove.Size = new Size(6, 0x19);
            this.btnRemove.Enabled = false;
            this.btnRemove.Image = (Image) manager.GetObject("btnRemove.Image");
            this.btnRemove.ImageTransparentColor = Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new Size(0x75, 0x16);
            this.btnRemove.Text = "Remove Selected";
            this.btnRemove.ToolTipText = "Remove the selected Orders";
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            this.btnRemoveCompleted.Image = (Image) manager.GetObject("btnRemoveCompleted.Image");
            this.btnRemoveCompleted.ImageTransparentColor = Color.Magenta;
            this.btnRemoveCompleted.Name = "btnRemoveCompleted";
            this.btnRemoveCompleted.Size = new Size(0x84, 0x16);
            this.btnRemoveCompleted.Text = "Remove Completed";
            this.btnRemoveCompleted.ToolTipText = "Remove all Completed Orders";
            this.btnRemoveCompleted.Click += new EventHandler(this.btnRemoveCompleted_Click);
            this.btnAutoRemove.CheckOnClick = true;
            this.btnAutoRemove.Image = (Image) manager.GetObject("btnAutoRemove.Image");
            this.btnAutoRemove.ImageTransparentColor = Color.Magenta;
            this.btnAutoRemove.Name = "btnAutoRemove";
            this.btnAutoRemove.Size = new Size(0x65, 0x16);
            this.btnAutoRemove.Text = "Auto-Remove";
            this.btnAutoRemove.ToolTipText = "Automatically Remove Orders Canceled through Auto-Trading";
            this.btnAutoRemove.Visible = false;
            this.toolbar.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new ToolStripItem[] { this.lblOrders, this.lblAccount, this.cmbAccount, this.sepAccounts, this.btnCancelAll, this.sepAuto, this.lblAutoTrading, this.cmbAutoTrading, this.sepHelp, this.btnUpdate, this.btnPreferences, this.btnHelp });
            this.toolbar.Location = new Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new Size(0x3dd, 0x19);
            this.toolbar.TabIndex = 14;
            this.toolbar.Text = "toolStrip1";
            this.lblOrders.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblOrders.Name = "lblOrders";
            this.lblOrders.Size = new Size(0x2d, 0x16);
            this.lblOrders.Text = "Orders";
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new Size(0x37, 0x16);
            this.lblAccount.Text = "Account:";
            this.cmbAccount.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new Size(0x79, 0x19);
            this.cmbAccount.ToolTipText = "Show Orders from selected Account";
            this.cmbAccount.SelectedIndexChanged += new EventHandler(this.cmbAccount_SelectedIndexChanged);
            this.sepAccounts.Name = "sepAccounts";
            this.sepAccounts.Size = new Size(6, 0x19);
            this.btnCancelAll.Image = (Image) manager.GetObject("btnCancelAll.Image");
            this.btnCancelAll.ImageTransparentColor = Color.Fuchsia;
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new Size(80, 0x16);
            this.btnCancelAll.Text = "Cancel All";
            this.btnCancelAll.ToolTipText = "Cancel all Active Orders and disable Auto-Trading";
            this.btnCancelAll.Click += new EventHandler(this.btnCancelAll_Click);
            this.sepAuto.Name = "sepAuto";
            this.sepAuto.Size = new Size(6, 0x19);
            this.lblAutoTrading.Name = "lblAutoTrading";
            this.lblAutoTrading.Size = new Size(0x52, 0x16);
            this.lblAutoTrading.Text = "Auto-Trading:";
            this.cmbAutoTrading.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbAutoTrading.Items.AddRange(new object[] { "Off", "Paper Accounts" });
            this.cmbAutoTrading.Name = "cmbAutoTrading";
            this.cmbAutoTrading.Size = new Size(0x62, 0x19);
            this.cmbAutoTrading.SelectedIndexChanged += new EventHandler(this.cmbAutoTrading_SelectedIndexChanged);
            this.sepHelp.Name = "sepHelp";
            this.sepHelp.Size = new Size(6, 0x19);
            this.btnUpdate.Image = (Image) manager.GetObject("btnUpdate.Image");
            this.btnUpdate.ImageTransparentColor = Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new Size(0x41, 0x16);
            this.btnUpdate.Text = "Update";
            this.btnUpdate.ToolTipText = "Update all active orders";
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            this.btnPreferences.Image = (Image) manager.GetObject("btnPreferences.Image");
            this.btnPreferences.ImageTransparentColor = Color.Magenta;
            this.btnPreferences.Name = "btnPreferences";
            this.btnPreferences.Size = new Size(0x84, 0x16);
            this.btnPreferences.Text = "Trading Preferences";
            this.btnPreferences.ToolTipText = "Set Trading Preferences, including default Account to Trade";
            this.btnPreferences.Click += new EventHandler(this.btnPreferences_Click);
            this.btnHelp.Image = (Image) manager.GetObject("btnHelp.Image");
            this.btnHelp.ImageTransparentColor = Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x34, 0x16);
            this.btnHelp.Text = "Help";
            this.btnHelp.ToolTipText = "Help on Orders";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.lvMessages.Columns.AddRange(new ColumnHeader[] { this.columnHeader_14, this.columnHeader_15 });
            this.lvMessages.Dock = DockStyle.Fill;
            this.lvMessages.FullRowSelect = true;
            this.lvMessages.HideSelection = false;
            this.lvMessages.Location = new Point(0, 0x19);
            this.lvMessages.MultiSelect = false;
            this.lvMessages.Name = "lvMessages";
            this.lvMessages.Size = new Size(0x2dc, 0x5f);
            this.lvMessages.TabIndex = 5;
            this.lvMessages.UseCompatibleStateImageBehavior = false;
            this.lvMessages.View = View.Details;
            this.lvMessages.SelectedIndexChanged += new EventHandler(this.lvMessages_SelectedIndexChanged);
            this.columnHeader_14.Text = "Date/Time";
            this.columnHeader_14.Width = 120;
            this.columnHeader_15.Text = "Message";
            this.columnHeader_15.Width = 0x1aa;
            this.pnlMessage.Controls.Add(this.txtMessageBrowser);
            this.pnlMessage.Controls.Add(this.lblFullText);
            this.pnlMessage.Dock = DockStyle.Right;
            this.pnlMessage.Location = new Point(0x2dc, 0x19);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new Size(0x101, 0x5f);
            this.pnlMessage.TabIndex = 4;
            this.txtMessageBrowser.AllowWebBrowserDrop = false;
            this.txtMessageBrowser.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.txtMessageBrowser.IsWebBrowserContextMenuEnabled = false;
            this.txtMessageBrowser.Location = new Point(7, 0x16);
            this.txtMessageBrowser.MinimumSize = new Size(20, 20);
            this.txtMessageBrowser.Name = "txtMessageBrowser";
            this.txtMessageBrowser.ScriptErrorsSuppressed = true;
            this.txtMessageBrowser.Size = new Size(0xf7, 0x49);
            this.txtMessageBrowser.TabIndex = 1;
            this.txtMessageBrowser.Url = new Uri("about:blank", UriKind.Absolute);
            this.lblFullText.AutoSize = true;
            this.lblFullText.Location = new Point(3, 6);
            this.lblFullText.Name = "lblFullText";
            this.lblFullText.Size = new Size(0x5d, 13);
            this.lblFullText.TabIndex = 2;
            this.lblFullText.Text = "Message Full Text";
            this.toolbarMessages.GripStyle = ToolStripGripStyle.Hidden;
            this.toolbarMessages.Items.AddRange(new ToolStripItem[] { this.lblMessages });
            this.toolbarMessages.Location = new Point(0, 0);
            this.toolbarMessages.Name = "toolbarMessages";
            this.toolbarMessages.Size = new Size(0x3dd, 0x19);
            this.toolbarMessages.TabIndex = 3;
            this.toolbarMessages.Text = "toolStrip1";
            this.lblMessages.Font = new Font("Tahoma", 8.25f, FontStyle.Bold);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new Size(0x62, 0x16);
            this.lblMessages.Text = "Order Messages";
            this.columnHeader_18.Text = "Trade Type";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x3dd, 400);
            base.Controls.Add(this.split);
            base.Controls.Add(this.status);
            base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.Name = "OrdersAlertsForm";
            base.StartPosition = FormStartPosition.WindowsDefaultBounds;
            this.Text = "Orders";
            base.Load += new EventHandler(this.OrdersAlertsForm_Load);
            base.FormClosed += new FormClosedEventHandler(this.OrdersAlertsForm_FormClosed);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel1.PerformLayout();
            this.split.Panel2.ResumeLayout(false);
            this.split.Panel2.PerformLayout();
            this.split.ResumeLayout(false);
            this.popupOrders.ResumeLayout(false);
            this.toolbarManipulate.ResumeLayout(false);
            this.toolbarManipulate.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.pnlMessage.ResumeLayout(false);
            this.pnlMessage.PerformLayout();
            this.toolbarMessages.ResumeLayout(false);
            this.toolbarMessages.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        public void LoadWorkspaceItems(IList<string> items, int version)
        {
        }

        private void lvMessages_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.lvMessages.SelectedItems.Count != 0)
                {
                    OrderMessage tag = (OrderMessage) this.lvMessages.SelectedItems[0].Tag;
                    if (tag != null)
                    {
                        string str = "<style type= 'text/css'> body { font-family: 'Microsoft Sans Serif'; font-size:11px; color:#000000; margin:2px; letter-spacing:0px; } </style>";
                        this.txtMessageBrowser.DocumentText = str + tag.Message;
                    }
                }
            }
            catch (Exception exception)
            {
                ilog_0.Error("Exception in order manager messages: ", exception);
            }
        }

        private void lvOrders_DoubleClick(object sender, EventArgs e)
        {
            if (this.btnEdit.Enabled)
            {
                this.btnEdit_Click(sender, e);
            }
        }

        private void lvOrders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.btnRemove.PerformClick();
            }
        }

        private void lvOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool flag = MainModule.Instance.AuthProvider.LoggedIn && (MainModule.Instance.BrokerProvider != null);
                int count = this.SelectedOrders.Count;
                if (this.SelectedOrder == null)
                {
                    this.btnEdit.Enabled = false;
                }
                else
                {
                    this.btnEdit.Enabled = ((count != 1) || (this.SelectedOrder.Status != OrderStatus.Staged)) ? (this.SelectedOrder.Status == OrderStatus.Error) : true;
                }
                bool flag2 = false;
                using (List<Order>.Enumerator enumerator = this.list_1.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Order current = enumerator.Current;
                        if (!current.IsActive)
                        {
                            ///goto  Label_00A2;  ///WYJ fix, simplify the flow
                            flag2 = true;
                            break;
                        }
                    }
                }
                this.btnRemove.Enabled = flag2;
                bool flag3 = false;
                using (List<Order>.Enumerator enumerator2 = this.list_1.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        Order order2 = enumerator2.Current;
                        if (order2.IsActiveAtBackEnd && this.method_6(order2))
                        {
                            ///goto  Label_00FA;  ///WYJ fix, simplify the flow
                            flag3 = true;
                            break;
                        }
                    }
                }
                this.btnCancelSelected.Enabled = flag3;
                bool flag4 = ((MainModule.Instance.BrokerProvider != null) && (this.SelectedOrder != null)) && MainModule.Instance.BrokerProvider.AllowCancelReplace(this.SelectedOrder, null);
                this.btnCancelReplace.Enabled = ((flag && (count == 1)) && this.SelectedOrder.IsActiveAtBackEnd) && flag4;
                bool flag5 = false;
                using (List<Order>.Enumerator enumerator3 = this.list_1.GetEnumerator())
                {
                    while (enumerator3.MoveNext())
                    {
                        Order order3 = enumerator3.Current;
                        if ((order3.Status == OrderStatus.Staged) && this.method_6(order3))
                        {
                            ///goto  Label_01A7;  ///WYJ fix, simplify the flow
                            flag5 = true;
                            break;
                        }
                    }
                }
                this.btnPlace.Enabled = flag5;
                this.lvMessages.Items.Clear();
                if (count == 1)
                {
                    foreach (OrderMessage message in this.SelectedOrder.Messages)
                    {
                        ListViewItem item = this.lvMessages.Items.Add(message.DateTime.ToString());
                        item.Tag = message;
                        item.SubItems.Add(message.Message);
                    }
                }
                this.mniEdit.Enabled = this.btnEdit.Enabled;
                this.mniPlace.Enabled = this.btnPlace.Enabled;
                this.mniCancel.Enabled = this.btnCancelSelected.Enabled;
                this.mniCancelReplace.Enabled = this.btnCancelReplace.Enabled;
                this.mniRemoveSelected.Enabled = this.btnRemove.Enabled;
                this.mniRemoveCompleted.Enabled = this.btnRemoveCompleted.Enabled;
                this.method_10();
            }
            catch (Exception exception)
            {
                ilog_0.Error("Exception in orders manager list: ", exception);
            }
        }

        private void method_0(Order order_0)
        {
            IEnumerator enumerator = this.lvOrders.Items.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ListViewItem current = (ListViewItem)enumerator.Current;
                        if (current.Tag == order_0)
                        {
                            if (!this.btnAutoRemove.Checked || order_0.Status != OrderStatus.Canceled || !order_0.FromAutoTrading)
                            {
                                this.method_7(order_0, current);
                            }
                            else
                            {
                                this.list_0.Clear();
                                this.list_0.Add(order_0);
                                MainModule.Instance.TradeManager.RemoveOrders(this.list_0);
                                this.method_8();
                            }
                            if (!current.Selected)
                            {
                                break;
                            }
                            this.method_10();
                            this.lvOrders_SelectedIndexChanged(this.lvOrders, EventArgs.Empty);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private void method_1(Order order_0)
        {
            if ((this.SelectedAccountNumber == "") || (order_0.Account == this.SelectedAccountNumber))
            {
                this.AddOrder(order_0);
            }
        }

        private void method_10()
        {
            this.lvMessages.Items.Clear();
            this.txtMessageBrowser.DocumentText = "";
            if (this.lvOrders.SelectedItems.Count == 1)
            {
                Order tag = (Order) this.lvOrders.SelectedItems[0].Tag;
                foreach (OrderMessage message in tag.Messages)
                {
                    ListViewItem item = this.lvMessages.Items.Add(message.DateTime.ToString());
                    item.Tag = message;
                    item.SubItems.Add(this.method_11(message.Message));
                }
            }
        }

        private string method_11(string string_1)
        {
            StringBuilder builder = new StringBuilder(string_1);
            Regex regex = new Regex("</?[\\w\\s='\"]*/?>");
            for (Match match = regex.Match(string_1); match.Success; match = match.NextMatch())
            {
                builder = builder.Replace(match.Value, "");
            }
            return builder.ToString();
        }

        private void method_12(object object_0)
        {
            ListViewItem item;
        Label_0042:
            item = object_0 as ListViewItem;
            Order tag = item.Tag as Order;
            Quote quote = MainModule.Instance.BrokerProvider.GetQuote(tag.Symbol);
            if ((quote == null) || (!(quote.TimeStamp.Date == DateTime.Now.Date) || (quote.Open <= 0.0)))
            {
                Thread.Sleep(100);
                goto Label_0042;
            }
            tag.OrderType = OrderType.Limit;
            tag.Price = quote.Open;
            base.Invoke(new Delegate62(this.method_7), new object[] { tag, item });
            MainModule.Instance.TradeManager.PlaceOrder(tag);
        }

        private void method_2(Order order_0)
        {
            IEnumerator enumerator = this.lvOrders.Items.GetEnumerator();
            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ListViewItem current = (ListViewItem)enumerator.Current;
                        if (current.Tag == order_0)
                        {
                            this.lvOrders.Items.Remove(current);
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        private void method_3()
        {
            int num = 0;
            int num2 = 0;
            if (this.lvOrders.Items.Count == 1)
            {
                this.statusOrders.Text = "1 Order";
            }
            else
            {
                this.statusOrders.Text = this.lvOrders.Items.Count + " Orders";
            }
            foreach (ListViewItem item in this.lvOrders.Items)
            {
                Order tag = (Order) item.Tag;
                if (tag.IsActive)
                {
                    num++;
                }
                if (tag.Status == OrderStatus.Filled)
                {
                    num2++;
                }
            }
            this.statusActive.Text = num + " Active";
            this.statusFilled.Text = num2 + " Filled";
        }

        private void method_4(string string_1)
        {
            this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(string_1);
        }

        private void method_5(string string_1)
        {
            this.lblStatusBar.Text = string_1;
            this.status.Refresh();
        }

        private bool method_6(Order order_0)
        {
            return (order_0.Account.StartsWith("Paper") || this.IsLoggedIn);
        }

        private void method_7(Order order_0, ListViewItem listViewItem_0)
        {
            if (listViewItem_0.SubItems.Count < 0x10)
            {
                while (listViewItem_0.SubItems.Count < this.lvOrders.Columns.Count)
                {
                    listViewItem_0.SubItems.Add("");
                }
                listViewItem_0.SubItems[3].Font = this.font_1;
            }
            listViewItem_0.ImageIndex = (int) order_0.AlertType;
            if (order_0.StateIndex >= 0)
            {
                listViewItem_0.StateImageIndex = order_0.StateIndex;
            }
            listViewItem_0.Text = order_0.Account;
            listViewItem_0.SubItems[1].Text = order_0.AlertDate.ToShortDateString() + " " + order_0.AlertDate.ToShortTimeString();
            listViewItem_0.SubItems[2].Text = order_0.Status.ToString();
            if (order_0.IsActive)
            {
                listViewItem_0.SubItems[2].Font = this.font_1;
            }
            else
            {
                listViewItem_0.SubItems[2].Font = this.font_0;
            }
            listViewItem_0.SubItems[3].Text = order_0.Symbol;
            listViewItem_0.SubItems[4].Text = order_0.AlertType.ToString();
            if (order_0.ExtendedOrderType != "")
            {
                listViewItem_0.SubItems[5].Text = order_0.ExtendedOrderType;
            }
            else
            {
                listViewItem_0.SubItems[5].Text = order_0.OrderType.ToString();
            }
            int pricingDecimalForSymbol = DecimalsManager.Instance.GetPricingDecimalForSymbol(order_0.Symbol);
            listViewItem_0.SubItems[6].Text = order_0.Shares.ToString();
            if (!(order_0.ExtendedOrderType != "") && (order_0.OrderType == OrderType.Market))
            {
                listViewItem_0.SubItems[7].Text = "";
            }
            else
            {
                listViewItem_0.SubItems[7].Text = order_0.Price.ToString("N" + pricingDecimalForSymbol);
            }
            listViewItem_0.SubItems[8].Text = order_0.FillQty.ToString();
            listViewItem_0.SubItems[9].Text = order_0.FillPrice.ToString("N" + pricingDecimalForSymbol);
            listViewItem_0.SubItems[10].Text = order_0.Route;
            if (order_0.Strategy != null)
            {
                listViewItem_0.SubItems[11].Text = order_0.Strategy.Name;
            }
            else
            {
                listViewItem_0.SubItems[11].Text = "";
            }
            listViewItem_0.SubItems[12].Text = order_0.SignalName;
            if (order_0.Strategy != null)
            {
                listViewItem_0.SubItems[13].Text = order_0.DataScale.ToString();
            }
            else
            {
                listViewItem_0.SubItems[13].Text = "";
            }
            if (order_0.Messages.Count > 0)
            {
                listViewItem_0.SubItems[14].Text = order_0.Messages[order_0.Messages.Count - 1].Message;
            }
            else
            {
                listViewItem_0.SubItems[14].Text = "";
            }
            listViewItem_0.SubItems[15].Text = order_0.TIF;
            listViewItem_0.SubItems[0x10].Text = order_0.AccountTradeType;
        }

        private void method_8()
        {
            this.list_0.Clear();
            foreach (ListViewItem item2 in this.lvOrders.SelectedItems)
            {
                this.list_0.Add((Order) item2.Tag);
            }
            string selectedAccountNumber = this.SelectedAccountNumber;
            this.lvOrders.BeginUpdate();
            try
            {
                this.lvOrders.Items.Clear();
                foreach (Order order in MainModule.Instance.TradeManager.GetOrdersForAccount(selectedAccountNumber))
                {
                    if ((selectedAccountNumber != "") || !order.Account.StartsWith("Paper"))
                    {
                        ListViewItem item = this.AddOrder(order);
                        if (this.list_0.Contains(order))
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
            finally
            {
                this.lvOrders.EndUpdate();
            }
            this.UpdateStatusBar();
            this.method_10();
        }

        private void method_9()
        {
            foreach (ListViewItem item in this.lvOrders.Items)
            {
                Order tag = (Order) item.Tag;
                this.method_7(tag, item);
            }
        }

        private void mniCopy_Click(object sender, EventArgs e)
        {
            MainModule.Instance.CopyListViewToClipboard(this.lvOrders);
        }

        private void mniPlaceLimit_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.lvOrders.SelectedItems)
            {
                new Thread(new ParameterizedThreadStart(this.method_12)) { IsBackground = true }.Start(item);
            }
        }

        private void mniPrint_Click(object sender, EventArgs e)
        {
            this.Print();
        }

        public void OrderAdded(Order order)
        {
            base.Invoke(new Delegate59(this.method_1), new object[] { order });
        }

        public void OrderRemoved(Order order)
        {
            base.Invoke(new Delegate59(this.method_2), new object[] { order });
        }

        private void OrdersAlertsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainModule.Instance.AutoTradingEnabled = AutoTradingMode.Off;
            Instance = null;
        }

        private void OrdersAlertsForm_Load(object sender, EventArgs e)
        {
            Instance = this;
            if (MainModule.Instance.BrokerProvider != null)
            {
                foreach (string str in MainModule.Instance.AccountNumbers)
                {
                    this.cmbAccount.Items.Add(str);
                }
                MainModule.Instance.BrokerProvider.GetStateImages(this.imageList_1);
            }
            if (MainModule.Instance.DefaultAccountNumber == "")
            {
                if (this.cmbAccount.Items.Count > 0)
                {
                    this.cmbAccount.SelectedIndex = 0;
                }
            }
            else
            {
                this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
            }
            this.cmbAccount.Items.Insert(0, string_0);
            this.ShowLoggedInState();
            this.cmbAutoTrading.SelectedIndex = (int) MainModule.Instance.AutoTradingEnabled;
        }

        public void OrderStatusUpdated(Order order)
        {
            base.Invoke(new Delegate58(this.method_0), new object[] { order });
        }

        public void Print()
        {
            DataObject obj2 = new DataObject();
            obj2.SetData(PrintReport.fmtBaseTitle.Name, MainModule.Instance.AuthProvider.ApplicationName);
            obj2.SetData(PrintReport.fmtTitle.Name, "Orders");
            obj2.SetData(PrintReport.fmtListView.Name, this.lvOrders);
            obj2.SetData(PrintReport.fmtDisclosure.Name, "1. Only equity orders are displayed in this Orders report.  Information related to orders in other types of securities can be viewed on Fidelity.com.\n2. The first Price column represents the Stop/Limit order price. For the Market Order Type, this field will be blank.\n3. The second Price column represents the average price at which the order was executed.");
            PrintReport report = new PrintReport {
                printObject = obj2
            };
            PageSettings defaultPageSettings = this.MyMainForm.DefaultPageSettings;
            report.ShowPrintPreview = !MainModule.Instance.Settings.Get("HidePrintPreview", false);
            report.ShowPrintDialog = !MainModule.Instance.Settings.Get("HidePrintDialog", false);
            report.PrintGraphicReport(defaultPageSettings);
        }

        public int SaveWorkspaceItems(IList<string> items)
        {
            return 1;
        }

        public void ShowLoggedInState()
        {
            base.Invoke(new Delegate57(this.ShowLoggedInStateThreadSafe));
        }

        public void ShowLoggedInStateThreadSafe()
        {
            this.btnCancelAll.Enabled = this.IsLoggedIn || this.PaperAccountSelected;
            if (this.IsLoggedIn)
            {
                if ((this.cmbAutoTrading.Items.Count == 2) && MainModule.Instance.AuthProvider.AllowAutoTrading)
                {
                    this.cmbAutoTrading.Items.Add("Live Accounts");
                }
            }
            else if (this.cmbAutoTrading.Items.Count > 2)
            {
                this.cmbAutoTrading.Items.RemoveAt(2);
                if (this.cmbAutoTrading.SelectedIndex == -1)
                {
                    this.cmbAutoTrading.SelectedIndex = 0;
                }
            }
            if (this.IsLoggedIn)
            {
                string text = this.cmbAccount.Text;
                this.cmbAccount.Items.Clear();
                foreach (string str2 in MainModule.Instance.AccountNumbers)
                {
                    this.cmbAccount.Items.Add(str2);
                }
                this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(text);
                if (this.cmbAccount.SelectedIndex == -1)
                {
                    this.cmbAccount.SelectedIndex = this.cmbAccount.Items.IndexOf(MainModule.Instance.DefaultAccountNumber);
                }
                if ((this.cmbAccount.SelectedIndex == -1) && (this.cmbAccount.Items.Count > 0))
                {
                    this.cmbAccount.SelectedIndex = 0;
                }
                this.cmbAccount.Items.Insert(0, string_0);
            }
        }

        public void SwitchToAccount(string account)
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate61(this.method_4), new object[] { account });
            }
            else
            {
                this.method_4(account);
            }
        }

        public void UpdateStatusBar()
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Delegate60(this.method_3));
            }
            else
            {
                this.method_3();
            }
        }

        public AutoTradingMode AutoTradingEnabled
        {
            get
            {
                return (AutoTradingMode) this.cmbAutoTrading.SelectedIndex;
            }
            set
            {
                if (MainModule.Instance.AuthProvider.AllowAutoTrading)
                {
                    this.cmbAutoTrading.SelectedIndex = (int) value;
                }
                else
                {
                    this.cmbAutoTrading.SelectedIndex = 0;
                }
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return ((MainModule.Instance.BrokerProvider != null) && MainModule.Instance.AuthProvider.LoggedIn);
            }
        }

        public MainForm MyMainForm
        {
            get
            {
                return (base.MdiParent as MainForm);
            }
        }

        private bool PaperAccountSelected
        {
            get
            {
                return this.cmbAccount.Text.StartsWith("Paper");
            }
        }

        public string SelectedAccountNumber
        {
            get
            {
                if (this.cmbAccount.Text == string_0)
                {
                    return "";
                }
                return this.cmbAccount.Text;
            }
        }

        public Order SelectedOrder
        {
            get
            {
                if (this.lvOrders.SelectedItems.Count != 1)
                {
                    return null;
                }
                return (Order) this.lvOrders.SelectedItems[0].Tag;
            }
        }

        public IList<Order> SelectedOrders
        {
            get
            {
                this.list_1.Clear();
                foreach (ListViewItem item in this.lvOrders.SelectedItems)
                {
                    this.list_1.Add((Order) item.Tag);
                }
                return this.list_1;
            }
        }

        private delegate void Delegate57();

        private delegate void Delegate58(Order order_0);

        private delegate void Delegate59(Order order_0);

        private delegate void Delegate60();

        private delegate void Delegate61(string string_0);

        private delegate void Delegate62(Order order_0, ListViewItem listViewItem_0);
    }
}

