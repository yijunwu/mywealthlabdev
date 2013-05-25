namespace WLDDatabaseProvider
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tChart1 = new Steema.TeeChart.TChart();
            this.candle1 = new Steema.TeeChart.Styles.Candle();
            this.textSource1 = new Steema.TeeChart.Data.TextSource();
            this.singleRecordSource1 = new Steema.TeeChart.Data.SingleRecordSource();
            this.cursorTool1 = new Steema.TeeChart.Tools.CursorTool();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(2, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(823, 426);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tChart1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(815, 400);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tChart1
            // 
            // 
            // 
            // 
            this.tChart1.Aspect.View3D = false;
            this.tChart1.Aspect.ZOffset = 0D;
            this.tChart1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // 
            // 
            this.tChart1.Legend.Visible = false;
            this.tChart1.Location = new System.Drawing.Point(6, 6);
            this.tChart1.Name = "tChart1";
            this.tChart1.Series.Add(this.candle1);
            this.tChart1.Size = new System.Drawing.Size(803, 388);
            this.tChart1.TabIndex = 0;
            this.tChart1.Tools.Add(this.cursorTool1);
            this.tChart1.Click += new System.EventHandler(this.tChart1_Click_1);
            // 
            // candle1
            // 
            // 
            // 
            // 
            this.candle1.Brush.Color = System.Drawing.Color.White;
            this.candle1.CloseValues = this.candle1.YValues;
            this.candle1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.candle1.ColorEach = false;
            this.candle1.DataSource = this.singleRecordSource1;
            this.candle1.DateValues = this.candle1.XValues;
            // 
            // 
            // 
            this.candle1.LinePen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(51)))), ((int)(((byte)(82)))));
            // 
            // 
            // 
            // 
            // 
            // 
            this.candle1.Marks.Callout.ArrowHead = Steema.TeeChart.Styles.ArrowHeadStyles.None;
            this.candle1.Marks.Callout.ArrowHeadSize = 8;
            // 
            // 
            // 
            this.candle1.Marks.Callout.Brush.Color = System.Drawing.Color.Black;
            this.candle1.Marks.Callout.Distance = 0;
            this.candle1.Marks.Callout.Draw3D = false;
            this.candle1.Marks.Callout.Length = 10;
            this.candle1.Marks.Callout.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            this.candle1.Marks.Callout.Visible = false;
            // 
            // 
            // 
            this.candle1.Pointer.Draw3D = false;
            this.candle1.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            this.candle1.Title = "candle1";
            // 
            // textSource1
            // 
            this.textSource1.DecimalSeparator = '.';
            this.textSource1.FileName = "http://www.steema.com/test.txt";
            this.textSource1.Series = null;
            // 
            // singleRecordSource1
            // 
            this.singleRecordSource1.DataSource = null;
            this.singleRecordSource1.Series = this.candle1;
            this.singleRecordSource1.ValueMembers = new string[0];
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 440);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Steema.TeeChart.TChart tChart1;
        private Steema.TeeChart.Styles.Candle candle1;
        private Steema.TeeChart.Data.SingleRecordSource singleRecordSource1;
        private Steema.TeeChart.Data.TextSource textSource1;
        private Steema.TeeChart.Tools.CursorTool cursorTool1;
    }
}