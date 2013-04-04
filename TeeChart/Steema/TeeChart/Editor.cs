namespace Steema.TeeChart
{
    using Steema.TeeChart.Editors;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    [DesignTimeVisible(true), ToolboxBitmap(typeof(Steema.TeeChart.Editor), "Images.Editor.bmp"), Editor(typeof(Steema.TeeChart.Editor.EditorEditor), typeof(System.ComponentModel.ComponentEditor)), DefaultProperty("Chart"), Designer(typeof(Steema.TeeChart.Editor.EditorDesigner))]
    public class Editor : Control
    {
        private bool autoRepaint;
        private TChart chart;
        private Container components;
        private ChartEditorTabs defaultTab;
        private string helpFileName;
        private ChartEditorTabs[] hideTabs;
        private bool highLightTabs;
        private ChartEditorOptions[] options;
        private string title;

        [Description("Event fired when the Chart editor dialog is closed.")]
        public event EventHandler CloseEditor;

        [Description("Occurs just before the Chart editor dialog is about to display.")]
        public event EventHandler ShowEditor;

        public Editor()
        {
            this.title = "";
            this.autoRepaint = true;
            this.helpFileName = "";
            this.InitializeComponent();
        }

        public Editor(TChart c) : this()
        {
            this.chart = c;
        }

        public Editor(IContainer container)
        {
            this.title = "";
            this.autoRepaint = true;
            this.helpFileName = "";
            container.Add(this);
            this.InitializeComponent();
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

        public static DialogResult Show(TChart c)
        {
            using (Steema.TeeChart.Editor editor = new Steema.TeeChart.Editor(c))
            {
                return editor.ShowModal();
            }
        }

        public DialogResult ShowModal()
        {
            return this.ShowModal(null);
        }

        public DialogResult ShowModal(Control owner)
        {
            DialogResult none = DialogResult.None;
            if (this.chart != null)
            {
                using (ChartEditor editor = new ChartEditor(this.chart.Chart))
                {
                    if (this.title.Length != 0)
                    {
                        editor.Text = this.title;
                    }
                    if (this.helpFileName.Length != 0)
                    {
                        editor.HelpFileName = this.helpFileName;
                    }
                    bool autoRepaint = this.chart.AutoRepaint;
                    if (!this.autoRepaint)
                    {
                        this.chart.AutoRepaint = false;
                    }
                    try
                    {
                        if (this.highLightTabs)
                        {
                            editor.SetHighLightTabs();
                        }
                        if ((this.hideTabs != null) && (this.hideTabs.Length > 0))
                        {
                            editor.SetHideTabs(this.hideTabs);
                        }
                        if ((this.options != null) && (this.options.Length > 0))
                        {
                            editor.SetOptions(this.options);
                        }
                        editor.SetDefaultTab(this.defaultTab);
                        if (this.ShowEditor != null)
                        {
                            this.ShowEditor(this, EventArgs.Empty);
                        }
                        none = editor.ShowDialog(owner);
                    }
                    finally
                    {
                        if (!this.autoRepaint)
                        {
                            this.chart.AutoRepaint = autoRepaint;
                        }
                        if (this.CloseEditor != null)
                        {
                            this.CloseEditor(this, EventArgs.Empty);
                        }
                    }
                }
            }
            return none;
        }

        [DefaultValue(true), Description("Enables or disables Chart auto-repainting while modifying properties at Chart editor.")]
        public bool AutoRepaint
        {
            get
            {
                return this.autoRepaint;
            }
            set
            {
                this.autoRepaint = value;
            }
        }

        [Description("Gets or sets the Chart control to be edited."), DefaultValue((string) null)]
        public TChart Chart
        {
            get
            {
                return this.chart;
            }
            set
            {
                this.chart = value;
            }
        }

        [Description("Gets or sets the initial tab to display when showing the Chart editor."), DefaultValue(0)]
        public ChartEditorTabs DefaultTab
        {
            get
            {
                return this.defaultTab;
            }
            set
            {
                this.defaultTab = value;
            }
        }

        [DefaultValue(""), Description("Path of custom help file to show when clicking Help button.")]
        public string HelpFileName
        {
            get
            {
                return this.helpFileName;
            }
            set
            {
                this.helpFileName = value;
            }
        }

        [DefaultValue((string) null)]
        public ChartEditorTabs[] HideTabs
        {
            get
            {
                return this.hideTabs;
            }
            set
            {
                this.hideTabs = value;
            }
        }

        public bool HighLightTabs
        {
            get
            {
                return this.highLightTabs;
            }
            set
            {
                this.highLightTabs = value;
            }
        }

        public ChartEditorOptions[] Options
        {
            get
            {
                return this.options;
            }
            set
            {
                this.options = value;
            }
        }

        [DefaultValue(""), Description("The text to show as caption of Chart editor dialog.")]
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        internal sealed class EditorDesigner : ComponentDesigner
        {
            public EditorDesigner()
            {
                this.Verbs.Add(new DesignerVerb("&Edit...", new EventHandler(this.OnEdit)));
            }

            private void OnEdit(object sender, EventArgs e)
            {
                if (this.Editor.ShowModal() == DialogResult.OK)
                {
                    base.RaiseComponentChanged(null, null, null);
                }
            }

            private Steema.TeeChart.Editor Editor
            {
                get
                {
                    return (Steema.TeeChart.Editor) base.Component;
                }
            }
        }

        internal class EditorEditor : System.ComponentModel.ComponentEditor
        {
            public override bool EditComponent(ITypeDescriptorContext context, object component)
            {
                ((Steema.TeeChart.Editor) component).ShowModal();
                return true;
            }
        }
    }
}

