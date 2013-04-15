using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WealthLab.DataProviders.AsciiFilesStatic;

internal class TextFilesWizardPageData : UserControl
{
    public AsciiFilesDataSet asciiFilesDataSet_0;
    private GroupBox grpData;
    private IContainer icontainer_0;
    public const int int_0 = 0;
    public const int int_1 = 1;
    public const int int_2 = 2;
    public const int int_3 = 3;
    public const int int_4 = 4;
    public const int int_5 = 5;
    private Label lblFile;
    private ListView lstData;

    public TextFilesWizardPageData()
    {
        this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (this.icontainer_0 != null))
        {
            this.icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }

    [DllImport("user32.dll", CharSet=CharSet.Auto)]
    public static extern IntPtr FindWindowEx(IntPtr intptr_0, IntPtr intptr_1, string string_0, string string_1);
    [DllImport("user32.dll")]
    public static extern IntPtr GetParent(IntPtr intptr_0);
    [DllImport("user32.dll")]
    public static extern IntPtr GetWindow(IntPtr intptr_0, int int_6);
    private void InitializeComponent()
    {
        this.grpData = new GroupBox();
        this.lblFile = new Label();
        this.lstData = new ListView();
        this.grpData.SuspendLayout();
        base.SuspendLayout();
        this.grpData.Controls.Add(this.lblFile);
        this.grpData.Controls.Add(this.lstData);
        this.grpData.Location = new Point(7, 7);
        this.grpData.Name = "grpData";
        this.grpData.Size = new Size(550, 0x157);
        this.grpData.TabIndex = 0;
        this.grpData.TabStop = false;
        this.grpData.Text = "Preview";
        this.lblFile.AutoSize = true;
        this.lblFile.Location = new Point(3, 0x10);
        this.lblFile.Name = "lblFile";
        this.lblFile.Size = new Size(0, 13);
        this.lblFile.TabIndex = 8;
        this.lstData.FullRowSelect = true;
        this.lstData.Location = new Point(6, 0x24);
        this.lstData.Name = "lstData";
        this.lstData.Size = new Size(0x21a, 0x12d);
        this.lstData.TabIndex = 7;
        this.lstData.UseCompatibleStateImageBehavior = false;
        this.lstData.View = View.Details;
        this.lstData.VirtualMode = true;
        this.lstData.RetrieveVirtualItem += new RetrieveVirtualItemEventHandler(this.lstData_RetrieveVirtualItem);
        base.AutoScaleDimensions = new SizeF(6f, 13f);
        base.AutoScaleMode = AutoScaleMode.Font;
        base.Controls.Add(this.grpData);
        base.Name = "TextFilesWizardPageData";
        base.Size = new Size(560, 0x161);
        base.Load += new EventHandler(this.TextFilesWizardPageData_Load);
        this.grpData.ResumeLayout(false);
        this.grpData.PerformLayout();
        base.ResumeLayout(false);
    }

    private void lstData_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
        if (e.ItemIndex <= this.asciiFilesDataSet_0.Rows.Count)
        {
            object[] objArray = this.asciiFilesDataSet_0.Rows[e.ItemIndex];
            string[] items = new string[objArray.Length];
            for (int i = 0; i < objArray.Length; i++)
            {
                if (objArray[i] is DateTime)
                {
                    DateTime time = (DateTime) objArray[i];
                    if ((this.asciiFilesDataSet_0.Fields.Items[i] as Field).Type == FieldType.Time)
                    {
                        items[i] = time.ToLongTimeString();
                        continue;
                    }
                    if (time.TimeOfDay.Ticks == 0L)
                    {
                        items[i] = time.ToShortDateString();
                        continue;
                    }
                }
                items[i] = objArray[i].ToString();
            }
            e.Item = new ListViewItem(items);
        }
    }

    public void method_0()
    {
    }

    private void method_1()
    {
        try
        {
            IntPtr window = GetWindow(GetParent(base.Handle), 2);
            IntPtr zero = IntPtr.Zero;
            while (((int) window) != 0)
            {
                zero = FindWindowEx(window, IntPtr.Zero, null, "<- Previous");
                if (((int) zero) != 0)
                {
                    ///goto  Label_0048;  ///WYJ fix, simplify the flow
                    SendMessage((int)zero, 0xf5, 0, IntPtr.Zero);
                    return;
                }
                window = GetWindow(window, 2);
            }
            return;
        }
        catch
        {
        }
    }

    [DllImport("user32.dll", CharSet=CharSet.Auto)]
    public static extern int SendMessage(int int_6, int int_7, int int_8, IntPtr intptr_0);
    private void TextFilesWizardPageData_Load(object sender, EventArgs e)
    {
        if (this.asciiFilesDataSet_0 != null)
        {
            List<string> list = AsciiFilesDataSet.GetFilesFromDir(this.asciiFilesDataSet_0.Folder, this.asciiFilesDataSet_0.Extension, true, true);
            if (list.Count > 0)
            {
                this.lblFile.Text = string.Format("File: \"{0}\"", list[0]);
                Application.DoEvents();
                this.asciiFilesDataSet_0.Parse(list[0]);
                this.lstData.BeginUpdate();
                this.lstData.Columns.Clear();
                this.lstData.Items.Clear();
                foreach (Field field in this.asciiFilesDataSet_0.Fields.Items)
                {
                    ColumnHeader header = new ColumnHeader {
                        Text = field.Name
                    };
                    if (header.Text == "Filler")
                    {
                        header.Width = 0;
                    }
                    else
                    {
                        header.Width = 90;
                    }
                    this.lstData.Columns.Add(header);
                }
                this.lstData.EndUpdate();
                this.lstData.VirtualListSize = this.asciiFilesDataSet_0.Rows.Count;
                if (this.asciiFilesDataSet_0.ParseError)
                {
                    this.method_1();
                }
            }
        }
    }
}

