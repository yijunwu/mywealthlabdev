namespace WLDDatabaseProvider
{
    partial class DataProviderUserControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn = new System.Windows.Forms.Button();
            this.lblTable = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.txtTable = new System.Windows.Forms.TextBox();
            this.txtDateColumn = new System.Windows.Forms.TextBox();
            this.txtSymbolColumn = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHighColumn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOpenColumn = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLowColumn = new System.Windows.Forms.TextBox();
            this.lable7 = new System.Windows.Forms.Label();
            this.txtCloseColumn = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtConnectionStr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtVolumeColumn = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(0, 0);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(75, 23);
            this.btn.TabIndex = 0;
            this.btn.Text = "this is  a Button";
            this.btn.UseVisualStyleBackColor = true;
            // 
            // lblTable
            // 
            this.lblTable.AutoSize = true;
            this.lblTable.Location = new System.Drawing.Point(177, 140);
            this.lblTable.Name = "lblTable";
            this.lblTable.Size = new System.Drawing.Size(30, 13);
            this.lblTable.TabIndex = 1;
            this.lblTable.Text = "table";
            this.lblTable.Visible = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(31, 166);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(65, 13);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "date column";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Location = new System.Drawing.Point(96, 137);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(75, 20);
            this.txtDatabase.TabIndex = 3;
            this.txtDatabase.Visible = false;
            // 
            // txtTable
            // 
            this.txtTable.Location = new System.Drawing.Point(249, 140);
            this.txtTable.Name = "txtTable";
            this.txtTable.Size = new System.Drawing.Size(75, 20);
            this.txtTable.TabIndex = 4;
            this.txtTable.Visible = false;
            // 
            // txtDateColumn
            // 
            this.txtDateColumn.Location = new System.Drawing.Point(96, 166);
            this.txtDateColumn.Name = "txtDateColumn";
            this.txtDateColumn.Size = new System.Drawing.Size(75, 20);
            this.txtDateColumn.TabIndex = 5;
            this.txtDateColumn.Text = "Date";
            this.txtDateColumn.TextChanged += new System.EventHandler(this.txtDateColumn_TextChanged);
            // 
            // txtSymbolColumn
            // 
            this.txtSymbolColumn.Location = new System.Drawing.Point(249, 166);
            this.txtSymbolColumn.Name = "txtSymbolColumn";
            this.txtSymbolColumn.Size = new System.Drawing.Size(75, 20);
            this.txtSymbolColumn.TabIndex = 7;
            this.txtSymbolColumn.Text = "Symbol";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(172, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "symbol column";
            // 
            // txtHighColumn
            // 
            this.txtHighColumn.Location = new System.Drawing.Point(249, 192);
            this.txtHighColumn.Name = "txtHighColumn";
            this.txtHighColumn.Size = new System.Drawing.Size(75, 20);
            this.txtHighColumn.TabIndex = 11;
            this.txtHighColumn.Text = "High";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "high column";
            // 
            // txtOpenColumn
            // 
            this.txtOpenColumn.Location = new System.Drawing.Point(96, 192);
            this.txtOpenColumn.Name = "txtOpenColumn";
            this.txtOpenColumn.Size = new System.Drawing.Size(75, 20);
            this.txtOpenColumn.TabIndex = 9;
            this.txtOpenColumn.Text = "Open";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "open column";
            // 
            // txtLowColumn
            // 
            this.txtLowColumn.Location = new System.Drawing.Point(249, 218);
            this.txtLowColumn.Name = "txtLowColumn";
            this.txtLowColumn.Size = new System.Drawing.Size(75, 20);
            this.txtLowColumn.TabIndex = 15;
            this.txtLowColumn.Text = "Low";
            // 
            // lable7
            // 
            this.lable7.AutoSize = true;
            this.lable7.Location = new System.Drawing.Point(188, 218);
            this.lable7.Name = "lable7";
            this.lable7.Size = new System.Drawing.Size(60, 13);
            this.lable7.TabIndex = 14;
            this.lable7.Text = "low column";
            // 
            // txtCloseColumn
            // 
            this.txtCloseColumn.Location = new System.Drawing.Point(96, 218);
            this.txtCloseColumn.Name = "txtCloseColumn";
            this.txtCloseColumn.Size = new System.Drawing.Size(75, 20);
            this.txtCloseColumn.TabIndex = 13;
            this.txtCloseColumn.Text = "Close";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "close column";
            // 
            // txtConnectionStr
            // 
            this.txtConnectionStr.Location = new System.Drawing.Point(96, 4);
            this.txtConnectionStr.Multiline = true;
            this.txtConnectionStr.Name = "txtConnectionStr";
            this.txtConnectionStr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConnectionStr.Size = new System.Drawing.Size(346, 61);
            this.txtConnectionStr.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "connection string";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(96, 71);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtQuery.Size = new System.Drawing.Size(346, 89);
            this.txtQuery.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "query";
            // 
            // txtVolumeColumn
            // 
            this.txtVolumeColumn.Location = new System.Drawing.Point(96, 244);
            this.txtVolumeColumn.Name = "txtVolumeColumn";
            this.txtVolumeColumn.Size = new System.Drawing.Size(75, 20);
            this.txtVolumeColumn.TabIndex = 17;
            this.txtVolumeColumn.Text = "Volume";
            this.txtVolumeColumn.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 247);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "volume column";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // DataProviderUserControl
            // 
            this.Controls.Add(this.txtVolumeColumn);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtConnectionStr);
            this.Controls.Add(this.txtLowColumn);
            this.Controls.Add(this.lable7);
            this.Controls.Add(this.txtCloseColumn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtHighColumn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOpenColumn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSymbolColumn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDateColumn);
            this.Controls.Add(this.txtTable);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblTable);
            this.Controls.Add(this.txtDatabase);
            this.Name = "DataProviderUserControl";
            this.Size = new System.Drawing.Size(445, 275);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Label lblTable;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.TextBox txtTable;
        private System.Windows.Forms.TextBox txtDateColumn;
        private System.Windows.Forms.TextBox txtSymbolColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHighColumn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOpenColumn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLowColumn;
        private System.Windows.Forms.Label lable7;
        private System.Windows.Forms.TextBox txtCloseColumn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtConnectionStr;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtVolumeColumn;
        private System.Windows.Forms.Label label7;
    }
}
