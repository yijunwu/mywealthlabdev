namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class FlexOptions : Form
    {
        private TextBox AC_OETags_js;
        private Button bCompile;
        private Button button1;
        private Button button2;
        public CheckBox cbDelete;
        private CheckBox cbEmbed;
        private CheckBox cbPreview;
        private TextBox Chart1_html;
        private IContainer components;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label1;
        private Label label2;
        private Chart panel;
        private TextBox tbFlex;
        public TextBox tbTemp;

        public FlexOptions(Chart c)
        {
            this.panel = c;
            this.InitializeComponent();
            this.ReadOptions();
        }

        public void bCompile_Click(object sender, EventArgs e)
        {
            string text = this.tbTemp.Text;
            if (!Directory.Exists(text) && Utils.YesNo("Folder \"" + text + "\" does not exist.\n\rDo you want to create it?"))
            {
                Directory.CreateDirectory(text);
            }
            string name = this.Panel.Parent.GetControl().Name;
            if (Utils.IsNullOrEmpty(name))
            {
                name = "Chart1";
            }
            if (text.EndsWith(@"\"))
            {
                text = text + name + ".mxml";
            }
            else
            {
                text = text + @"\" + name + ".mxml";
            }
            this.Panel.Export.Image.Flex.EmbeddedImages = this.cbEmbed.Checked;
            this.Panel.Export.Image.Flex.Save(text);
            this.Check_TeeSWC_Library(this.tbTemp.Text + @"\tee.swc");
            bool flag = this.Compile(text);
            if (this.cbDelete.Checked)
            {
                if (File.Exists(this.tbTemp.Text + @"\tee.swc"))
                {
                    File.Delete(this.tbTemp.Text + @"\tee.swc");
                }
                if (File.Exists(text))
                {
                    File.Delete(text);
                }
                if (this.cbEmbed.Checked)
                {
                    foreach (string str3 in Directory.GetFiles(this.tbTemp.Text, "TeeChart_Flex_Temp_*"))
                    {
                        File.Delete(str3);
                    }
                }
            }
            if (flag && this.cbPreview.Checked)
            {
                this.GenerateHTML(this.tbTemp.Text, name);
                this.Preview(this.tbTemp.Text + @"\" + name + ".html");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.Description = "Select temporary folder";
            this.folderBrowserDialog1.ShowNewFolderButton = true;
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.tbTemp.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.Description = "Select folder with Adobe Flex mxmlc.exe compiler";
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.tbFlex.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void cbDelete_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void cbEmbed_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void cbPreview_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void Check_TeeSWC_Library(string teeSWC)
        {
            if (!File.Exists(teeSWC))
            {
                Stream resource = Utils.GetResource("Steema.TeeChart.Export.tee.swc");
                if (resource != null)
                {
                    FileStream stream2 = File.Create(teeSWC);
                    try
                    {
                        byte[] buffer = new byte[resource.Length];
                        resource.Read(buffer, 0, (int) resource.Length);
                        stream2.Write(buffer, 0, (int) resource.Length);
                    }
                    finally
                    {
                        stream2.Close();
                        resource.Close();
                    }
                }
            }
        }

        private void CheckCompile()
        {
            this.bCompile.Enabled = ((this.panel != null) && !Utils.IsNullOrEmpty(this.tbTemp.Text)) && !Utils.IsNullOrEmpty(this.tbFlex.Text);
        }

        private bool Compile(string targetFile)
        {
            Process process = new Process();
            string directoryName = Path.GetDirectoryName(targetFile);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = this.tbFlex.Text + @"\mxmlc.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.Arguments = " -use-network=false -library-path+=\"" + directoryName + "\\tee.swc\" \"" + targetFile + "\"";
            Cursor c = this.Panel.Parent.GetCursor();
            this.Panel.Parent.SetCursor(Cursors.WaitCursor);
            process.Start();
            string str2 = process.StandardOutput.ReadToEnd();
            string s = process.StandardError.ReadToEnd();
            process.WaitForExit();
            int exitCode = process.ExitCode;
            process.Close();
            char ch = '\0';
            str2 = str2.Replace(ch.ToString(), "");
            s = s.Replace(ch.ToString(), "");
            this.Panel.Parent.SetCursor(c);
            if (exitCode == 0)
            {
                return true;
            }
            Utils.ErrorMessage(s);
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FlexOptions_Load(object sender, EventArgs e)
        {
            this.CheckCompile();
        }

        private void GenerateHTML(string path, string movie)
        {
            FileStream stream;
            StreamWriter writer;
            if (!File.Exists(path + @"\AC_OETags.js"))
            {
                stream = File.Create(path + @"\AC_OETags.js");
                writer = new StreamWriter(stream);
                try
                {
                    writer.Write(this.AC_OETags_js.Text);
                }
                finally
                {
                    writer.Close();
                    stream.Close();
                }
            }
            string[] lines = this.Chart1_html.Lines;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf("%MOVIE%") > 0)
                {
                    lines[i] = lines[i].Replace("%MOVIE%", movie);
                }
            }
            stream = File.Create(path + @"\" + movie + ".html");
            writer = new StreamWriter(stream);
            try
            {
                foreach (string str in lines)
                {
                    writer.Write(str + "\r\n");
                }
            }
            finally
            {
                writer.Close();
                stream.Close();
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(FlexOptions));
            this.bCompile = new Button();
            this.cbPreview = new CheckBox();
            this.label1 = new Label();
            this.tbTemp = new TextBox();
            this.button1 = new Button();
            this.button2 = new Button();
            this.tbFlex = new TextBox();
            this.label2 = new Label();
            this.cbDelete = new CheckBox();
            this.cbEmbed = new CheckBox();
            this.AC_OETags_js = new TextBox();
            this.Chart1_html = new TextBox();
            this.folderBrowserDialog1 = new FolderBrowserDialog();
            base.SuspendLayout();
            this.bCompile.FlatStyle = FlatStyle.Flat;
            this.bCompile.Location = new Point(12, 12);
            this.bCompile.Name = "bCompile";
            this.bCompile.Size = new Size(0x4b, 0x17);
            this.bCompile.TabIndex = 0;
            this.bCompile.Text = "&Compile...";
            this.bCompile.UseVisualStyleBackColor = true;
            this.bCompile.Click += new EventHandler(this.bCompile_Click);
            this.cbPreview.Checked = true;
            this.cbPreview.CheckState = CheckState.Checked;
            this.cbPreview.Location = new Point(0x69, 0x10);
            this.cbPreview.Name = "cbPreview";
            this.cbPreview.Size = new Size(0x7f, 0x11);
            this.cbPreview.TabIndex = 1;
            this.cbPreview.Text = "&Preview after compile";
            this.cbPreview.AutoSize = true;
            this.cbPreview.UseVisualStyleBackColor = true;
            this.cbPreview.CheckedChanged += new EventHandler(this.cbPreview_CheckedChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x31);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x59, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "&Temporary folder:";
            this.tbTemp.Location = new Point(12, 0x41);
            this.tbTemp.Name = "tbTemp";
            this.tbTemp.Size = new Size(220, 20);
            this.tbTemp.TabIndex = 3;
            this.tbTemp.TextChanged += new EventHandler(this.tbTemp_TextChanged);
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0xee, 0x3f);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x1a, 0x17);
            this.button1.TabIndex = 4;
            this.button1.Text = "...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.FlatStyle = FlatStyle.Flat;
            this.button2.Location = new Point(0xee, 0x66);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x1a, 0x17);
            this.button2.TabIndex = 7;
            this.button2.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.tbFlex.Location = new Point(12, 0x68);
            this.tbFlex.Name = "tbFlex";
            this.tbFlex.Size = new Size(220, 20);
            this.tbFlex.TabIndex = 6;
            this.tbFlex.TextChanged += new EventHandler(this.tbFlex_TextChanged);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 0x58);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x8e, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Path to Adobe &Flex compiler:";
            this.cbDelete.Checked = true;
            this.cbDelete.CheckState = CheckState.Checked;
            this.cbDelete.Location = new Point(12, 130);
            this.cbDelete.Name = "cbDelete";
            this.cbDelete.Size = new Size(0xcf, 0x11);
            this.cbDelete.TabIndex = 8;
            this.cbDelete.Text = "&Delete temporary files after compilation";
            this.cbDelete.AutoSize = true;
            this.cbDelete.UseVisualStyleBackColor = true;
            this.cbDelete.CheckedChanged += new EventHandler(this.cbDelete_CheckedChanged);
            this.cbEmbed.Checked = true;
            this.cbEmbed.CheckState = CheckState.Checked;
            this.cbEmbed.Location = new Point(12, 0x99);
            this.cbEmbed.Name = "cbEmbed";
            this.cbEmbed.Size = new Size(0x71, 0x11);
            this.cbEmbed.TabIndex = 9;
            this.cbEmbed.Text = "&Embedded images";
            this.cbEmbed.AutoSize = true;
            this.cbEmbed.UseVisualStyleBackColor = true;
            this.cbEmbed.CheckedChanged += new EventHandler(this.cbEmbed_CheckedChanged);
            this.AC_OETags_js.Location = new Point(0x68, 0xb0);
            this.AC_OETags_js.Multiline = true;
            this.AC_OETags_js.Name = "AC_OETags_js";
            this.AC_OETags_js.Size = new Size(0x4d, 0x43);
            this.AC_OETags_js.TabIndex = 10;
            this.AC_OETags_js.Text = manager.GetString("AC_OETags_js.Text");
            this.AC_OETags_js.Visible = false;
            this.Chart1_html.Location = new Point(0xbb, 0xb0);
            this.Chart1_html.Multiline = true;
            this.Chart1_html.Name = "Chart1_html";
            this.Chart1_html.Size = new Size(0x4d, 0x43);
            this.Chart1_html.TabIndex = 11;
            this.Chart1_html.Text = manager.GetString("Chart1_html.Text");
            this.Chart1_html.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x13b, 0xf2);
            base.Controls.Add(this.Chart1_html);
            base.Controls.Add(this.AC_OETags_js);
            base.Controls.Add(this.cbEmbed);
            base.Controls.Add(this.cbDelete);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.tbFlex);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.tbTemp);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.cbPreview);
            base.Controls.Add(this.bCompile);
            base.Name = "FlexOptions";
            this.Text = "FlexOptions";
            base.Load += new EventHandler(this.FlexOptions_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void Preview(string target)
        {
            Process.Start(target);
        }

        private void ReadOptions()
        {
            this.tbTemp.Text = Utils.Registry_GetValue(Utils.TeeChartKeyName, "FlexCanvas_Temp", "").ToString();
            if (Utils.IsNullOrEmpty(this.tbTemp.Text))
            {
                this.tbTemp.Text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            this.tbFlex.Text = Utils.Registry_GetValue(Utils.TeeChartKeyName, "FlexCanvas_Compiler", "").ToString();
        }

        private void tbFlex_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(this.tbFlex.Text + @"\mxmlc.exe"))
            {
                this.CheckCompile();
                Utils.Registry_SetValue(Utils.TeeChartKeyName, "FlexCanvas_Compiler", this.tbFlex.Text);
            }
            else
            {
                Utils.ErrorMessage("Cannot find Flex compiler mxmlc.exe in folder: \r\n" + this.tbFlex.Text);
            }
        }

        private void tbTemp_TextChanged(object sender, EventArgs e)
        {
            this.CheckCompile();
            Utils.Registry_SetValue(Utils.TeeChartKeyName, "FlexCanvas_Temp", this.tbTemp.Text);
        }

        public Chart Panel
        {
            get
            {
                return this.panel;
            }
            set
            {
                this.panel = value;
            }
        }
    }
}

