namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;

    public class AxisScrollEditor : AxisToolEdit
    {
        private IContainer components;
        private AxisScroll tool;

        public AxisScrollEditor()
        {
            this.InitializeComponent();
        }

        public AxisScrollEditor(Tool t) : this()
        {
            this.tool = (AxisScroll) t;
            base.SetTool(this.tool);
            base.BPen.Enabled = false;
            base.BPen.Visible = false;
            EditorUtils.Translate(this);
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
            this.components = new Container();
        }
    }
}

