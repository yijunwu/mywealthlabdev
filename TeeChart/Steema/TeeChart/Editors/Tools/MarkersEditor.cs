namespace Steema.TeeChart.Editors.Tools
{
    using Steema.TeeChart.Tools;
    using System;
    using System.Windows.Forms;

    public class MarkersEditor : ToolsEditor
    {
        public MarkersEditor(ToolsCollection c, Control parent, System.Type toolType) : base(c, parent, toolType)
        {
        }

        protected override Steema.TeeChart.Tools.Tool GetNewTool()
        {
            return (base.Tools as MarkersCollection).Add("Marker");
        }
    }
}

