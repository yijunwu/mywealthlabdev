namespace Steema.TeeChart.Editors
{
    using Steema.TeeChart;
    using Steema.TeeChart.Languages;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class DesignTimeOptions : Form
    {
        private Button button1;
        private CheckBox checkBox1;
        private ComboBox comboBox1;
        private Container components;
        private Label label1;
        private Label label3;
        private Label labelLang;
        internal static int language;

        public DesignTimeOptions()
        {
            this.InitializeComponent();
            this.checkBox1.Checked = Utils.RememberEditor();
            this.comboBox1.SelectedIndex = Utils.GalleryStyle();
            this.SetLabelLang();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int language = Utils.Language();
            if (Texts.Translator.AskLanguage(ref language))
            {
                Utils.Registry_SetValue(Utils.TeeChartKeyName, "Language", language);
                this.SetLabelLang();
            }
        }

        public static int CurrentLanguage()
        {
            return Utils.Language();
        }

        private void DesignTimeOptions_Load(object sender, EventArgs e)
        {
            this.button1.Enabled = this.TryLoadTranslator();
            EditorUtils.Translate(this);
            if (this.button1.Enabled && (Utils.Language() == 0))
            {
                this.button1.Text = "&Change...";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DoChangeLanguage()
        {
            if (language == 0)
            {
                English.SetTexts();
            }
            else if (this.TryLoadTranslator())
            {
                Texts.Translator.InitLanguage(Utils.Language());
            }
        }

        public static DesignTimeOptions GetInstance()
        {
            return new DesignTimeOptions();
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.labelLang = new Label();
            this.button1 = new Button();
            this.checkBox1 = new CheckBox();
            this.label3 = new Label();
            this.comboBox1 = new ComboBox();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x3a, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Language:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label1.UseMnemonic = false;
            this.labelLang.AutoSize = true;
            this.labelLang.Location = new Point(0x4c, 0x10);
            this.labelLang.Name = "labelLang";
            this.labelLang.Size = new Size(0x29, 0x10);
            this.labelLang.TabIndex = 1;
            this.labelLang.Text = "English";
            this.labelLang.UseMnemonic = false;
            this.button1.FlatStyle = FlatStyle.Flat;
            this.button1.Location = new Point(0xa8, 12);
            this.button1.Name = "button1";
            this.button1.TabIndex = 2;
            this.button1.Text = "&Change...";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.checkBox1.FlatStyle = FlatStyle.Flat;
            this.checkBox1.Location = new Point(13, 0x35);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0xe3, 0x18);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "&Remember editor position";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x20, 100);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x4a, 0x10);
            this.label3.TabIndex = 5;
            this.label3.Text = "Chart &Gallery:";
            this.label3.TextAlign = ContentAlignment.TopRight;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] { "By category", "By series type" });
            this.comboBox1.Location = new Point(0x70, 0x60);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x79, 0x15);
            this.comboBox1.TabIndex = 6;
            base.ClientSize = new Size(0x110, 0x85);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.labelLang);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.button1);
            base.Name = "DesignTimeOptions";
            this.Text = "Design time options";
            base.Load += new EventHandler(this.DesignTimeOptions_Load);
            base.ResumeLayout(false);
        }

        internal static void InitLanguage(bool forced)
        {
            InitLanguage(false, forced);
        }

        internal static void InitLanguage(bool designMode, bool forced)
        {
            if (forced || !English.TextsOk)
            {
                if (designMode)
                {
                    language = CurrentLanguage();
                }
                GetInstance().DoChangeLanguage();
            }
        }

        private void SetLabelLang()
        {
            if (Texts.Translator != null)
            {
                this.labelLang.Text = Texts.Translator.LanguageToString(Utils.Language());
            }
        }

        public void StoreSettings()
        {
            Utils.Registry_SetValue(Utils.TeeChartKeyName, "RememberEditor", this.checkBox1.Checked);
            Utils.Registry_SetValue(Utils.TeeChartKeyName, "GalleryStyle", this.comboBox1.SelectedIndex);
            this.DoChangeLanguage();
        }

        private bool TryLoadTranslator()
        {
            string str = "TeeChart.Languages";
            if (Texts.Translator == null)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.Load("TeeChart.Languages");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
                if ((assembly != null) && Utils.AutoLanguage())
                {
                    string directoryName = Path.GetDirectoryName(assembly.CodeBase);
                    string path = directoryName.Substring(directoryName.IndexOf(@"file:\") + @"file:\".Length) + @"\" + str + ".dll";
                    try
                    {
                        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int) stream.Length);
                        stream.Close();
                        assembly = Assembly.Load(buffer);
                    }
                    catch (Exception exception2)
                    {
                        Console.WriteLine(exception2.Message);
                    }
                }
                if (assembly != null)
                {
                    System.Type type = assembly.GetType("Steema.TeeChart.Languages.Translator");
                    if (type != null)
                    {
                        object obj2 = Activator.CreateInstance(type);
                        if ((obj2 != null) && (obj2 is ITranslator))
                        {
                            Texts.Translator = obj2 as ITranslator;
                        }
                    }
                }
            }
            return (Texts.Translator != null);
        }
    }
}

