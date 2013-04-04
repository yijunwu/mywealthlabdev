namespace Steema.TeeChart.Editors.Export
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    public class EmfOptions : Form
    {
        private ComboBox cbEmfType;
        private Container components;

        public EmfOptions()
        {
            this.InitializeComponent();
            base.Name = "EmfOptions";
            this.cbEmfType.Items.AddRange(Enum.GetNames(typeof(EmfType)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cbEmfType = new ComboBox();
            base.SuspendLayout();
            this.cbEmfType.Location = new Point(0x10, 0x10);
            this.cbEmfType.Name = "cbEmfType";
            this.cbEmfType.Size = new Size(0x79, 0x15);
            this.cbEmfType.TabIndex = 1;
            this.cbEmfType.Text = "EmfOnly";
            base.ClientSize = new Size(0x98, 0x4b);
            base.Controls.Add(this.cbEmfType);
            base.Name = "EmfOptions";
            base.ResumeLayout(false);
        }

        public int EMFType
        {
            get
            {
                return this.cbEmfType.SelectedIndex;
            }
        }
    }
}

