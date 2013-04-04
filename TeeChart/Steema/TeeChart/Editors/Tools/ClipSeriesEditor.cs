namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Tools;
    using System;
    using System.ComponentModel;

    public class ClipSeriesEditor : ToolSeriesEditor
    {
        private IContainer components;
        private ClipSeries tool;

        public ClipSeriesEditor()
        {
            this.InitializeComponent();
        }

        public ClipSeriesEditor(Tool t) : this()
        {
            base.setting = true;
            this.tool = (ClipSeries) t;
            base.SetTool(this.tool, null);
            base.setting = false;
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

