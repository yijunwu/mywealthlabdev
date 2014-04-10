namespace WealthLab.ChartControl
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;

    [ToolboxBitmap(typeof(DrawingObjectManager), "DrawingObjectManager")]
    public class DrawingObjectManager : Component
    {
        private byte byte_0;
        private WealthLab.ChartControl.Chart chart;
        private ChartDrawingObject selectedDrawingObject;
        private ChartDrawingObjectHandle selectedDrawingObjectHandle;
        private WealthLab.DataStore dataStore;
        private IContainer components;
        private ISettingsHost isettingsHost;
        private List<ChartDrawingObject> drawingObjects;
        private List<DrawingObjectHelper> drawingObjectHelpers;
        private List<string> list_2;
        private static SettingsManager settingsManager;
        private string rootPath;
        private string chartBookName;

        private EventHandler eventHandler_0;

        public event EventHandler NewDrawingObjectAdded
        {
            add
            {
                EventHandler eventHandler;
                EventHandler eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler eventHandler1 = (EventHandler)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler eventHandler;
                EventHandler eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler eventHandler1 = (EventHandler)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }

        public DrawingObjectManager()
        {
            this.drawingObjects = new List<ChartDrawingObject>();
            this.chartBookName = "Standard";
            this.drawingObjectHelpers = new List<DrawingObjectHelper>();
            this.list_2 = new List<string>();
            this.method_0();
            this.method_1();
        }

        public DrawingObjectManager(IContainer container)
        {
            this.drawingObjects = new List<ChartDrawingObject>();
            this.chartBookName = "Standard";
            this.drawingObjectHelpers = new List<DrawingObjectHelper>();
            this.list_2 = new List<string>();
            container.Add(this);
            this.method_0();
            this.method_1();
        }

        public bool ChangeDrawingObjectSettings(ChartDrawingObject chartDrawingObject_1)
        {
            if (chartDrawingObject_1 is ICustomSettings)
            {
                ICustomSettings settings = chartDrawingObject_1 as ICustomSettings;
                UserControl settingsUI = settings.GetSettingsUI();
                DrawingObjectProperties properties = new DrawingObjectProperties {
                    Text = chartDrawingObject_1.Helper.FriendlyName + " Properties"
                };
                properties.AddUserControl(settingsUI);
                if (properties.ShowDialog(this.Chart.FindForm()) == DialogResult.OK)
                {
                    settings.ChangeSettings(settingsUI);
                    settings.WriteSettings(this.SettingsHost);
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            this.drawingObjects.Clear();
        }

        public ChartDrawingObject CreateDrawingObject(ChartPane pane, System.Type typeOfDrawingObject, DateTime dateTime_0, double value)
        {
            ChartDrawingObject obj2 = null;
            try
            {
                obj2 = (ChartDrawingObject) Activator.CreateInstance(typeOfDrawingObject, new object[] { pane, dateTime_0, value });
                if (obj2 != null)
                {
                    this.method_2(obj2);
                }
            }
            catch (Exception)
            {
                obj2 = null;
            }
            return obj2;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public ChartDrawingObject FindByName(string name)
        {
            ChartDrawingObject obj3;
            using (List<ChartDrawingObject>.Enumerator enumerator = this.drawingObjects.GetEnumerator())
            {
                ChartDrawingObject current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.Name == name)
                    {
                        goto Label_0030;
                    }
                }
                return null;
            Label_0030:
                obj3 = current;
            }
            return obj3;
        }

        public void LoadDrawingObjects(Bars bars)
        {
            this.Clear();
            this.method_5();
            if (this.dataStore.ContainsSymbol(bars.Symbol, bars.Scale, bars.BarInterval))
            {
                Stream input = new FileStream(this.dataStore.FileNameForBars(bars), FileMode.Open);
                try
                {
                    BinaryReader reader = new BinaryReader(input);
                    if (reader.ReadDouble() >= 2.0)
                    {
                        for (int i = reader.ReadInt32(); i > 0; i--)
                        {
                            string str2 = reader.ReadString();
                            System.Type type = this.method_6(str2);
                            if (type == null)
                            {
                                return;
                            }
                            ChartDrawingObject item = (ChartDrawingObject) Activator.CreateInstance(type);
                            if (this.Chart != null)
                            {
                                item.Renderer = this.Chart.Renderer;
                                item.HandleFont = this.Chart.HandleFont;
                            }
                            item.Manager = this;
                            item.Read(reader);
                            if (this.Chart != null)
                            {
                                item.Pane = this.Chart.Renderer.FindPane(item.PaneDescription);
                            }
                            this.drawingObjects.Add(item);
                        }
                    }
                }
                finally
                {
                    if (input != null)
                    {
                        input.Close();
                    }
                }
            }
        }

        private void method_0()
        {
            this.components = new Container();
        }

        private void method_1()
        {
            if (!base.DesignMode)
            {
                AssemblyLoader loader = new AssemblyLoader {
                    BaseClass = "DrawingObjectHelper",
                    Path = Path.GetDirectoryName(Application.ExecutablePath)
                };
                foreach (System.Type type in loader.Types)
                {
                    DrawingObjectHelper item = (DrawingObjectHelper) loader.CreateInstance(type);
                    this.drawingObjectHelpers.Add(item);
                }
            }
        }

        internal void method_2(ChartDrawingObject chartDrawingObject_1)
        {
            if (!chartDrawingObject_1.ConfineToPricePane || chartDrawingObject_1.Pane.IsPricePane)
            {
                chartDrawingObject_1.HandleFont = this.Chart.HandleFont;
                chartDrawingObject_1.Manager = this;
                chartDrawingObject_1.Renderer = this.Chart.Renderer;
                DrawingObjectHelper helper = this.method_7(chartDrawingObject_1.GetType());
                int num = 1;
                do
                {
                    chartDrawingObject_1.Name = helper.FriendlyName + "_" + num;
                    num++;
                }
                while (this.FindByName(chartDrawingObject_1.Name) != null);
                if (chartDrawingObject_1 is ICustomSettings)
                {
                    (chartDrawingObject_1 as ICustomSettings).ReadSettings(this.SettingsHost);
                }
                this.drawingObjects.Add(chartDrawingObject_1);
                if (this.eventHandler_0 != null)
                {
                    this.eventHandler_0(this, null);
                }
            }
        }

        internal void method_3(Graphics graphics_0)
        {
            if (this.Chart.Renderer.PaneCreationCounter != this.byte_0)
            {
                this.byte_0 = this.Chart.Renderer.PaneCreationCounter;
                foreach (ChartDrawingObject obj3 in this.drawingObjects)
                {
                    obj3.Pane = this.Chart.Renderer.FindPane(obj3.PaneDescription);
                }
            }
            if (this.DrawingObjects.Count > 0)
            {
                foreach (ChartDrawingObject obj2 in this.DrawingObjects)
                {
                    if (obj2.Pane != null)
                    {
                        if (obj2.ClipToPane)
                        {
                            this.Chart.Renderer.ClipToPane(graphics_0, obj2.Pane);
                        }
                        else
                        {
                            this.Chart.Renderer.ClipToPane(graphics_0, null);
                        }
                        obj2.RenderHandles(graphics_0);
                        obj2.Render(graphics_0);
                        if (obj2.ClipToPane)
                        {
                            this.Chart.Renderer.ClipToPane(graphics_0, null);
                        }
                        obj2.RenderNotClipped(graphics_0);
                    }
                }
                this.Chart.Renderer.ClipToPane(graphics_0, null);
            }
        }

        internal bool method_4(ChartPane chartPane_0, int int_0, int int_1)
        {
            this.SelectedHandle = null;
            using (List<ChartDrawingObject>.Enumerator enumerator = this.drawingObjects.GetEnumerator())
            {
                ChartDrawingObject current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if ((chartPane_0 == current.Pane) && current.IsMouseOver(int_0, int_1))
                    {
                        ///goto  Label_003F; ///WYJ fix, simplify the flow
                        using (List<ChartDrawingObjectHandle>.Enumerator enumerator2 = current.Handles.GetEnumerator())
                        {
                            ChartDrawingObjectHandle handle;
                            while (enumerator2.MoveNext())
                            {
                                handle = enumerator2.Current;
                                if ((Math.Abs((int)(int_0 - handle.X)) <= 4) && (Math.Abs((int)(int_1 - handle.Y)) <= 4))
                                {
                                    ///goto  Label_0084;  ///WYJ fix, simplify the flow
                                    this.SelectedHandle = handle;
                                    break;
                                }
                            }
                        }
                        if (current != this.selectedDrawingObject)
                        {
                            if (this.selectedDrawingObject != null)
                            {
                                this.selectedDrawingObject.Selected = false;
                            }
                            current.Selected = true;
                            current.OnSelected(int_0, int_1);
                            this.selectedDrawingObject = current;
                            return true;
                        }
                        return false;
                    }
                }
            }
            if (this.selectedDrawingObject != null)
            {
                this.selectedDrawingObject.Selected = false;
                this.selectedDrawingObject = null;
                return true;
            }
            return false;
        }

        private void method_5()
        {
            if (this.dataStore == null)
            {
                if (this.rootPath == null)
                {
                    throw new InvalidOperationException("RootPath must be assigned before saving/loading Drawing Objects");
                }
                this.dataStore = new WealthLab.DataStore(this.RootPath, "DrawingObjects." + this.ChartBookName, "DRW");
            }
        }

        private System.Type method_6(string string_2)
        {
            System.Type type2;
            using (List<DrawingObjectHelper>.Enumerator enumerator = this.drawingObjectHelpers.GetEnumerator())
            {
                System.Type drawingObjectType;
                while (enumerator.MoveNext())
                {
                    DrawingObjectHelper current = enumerator.Current;
                    drawingObjectType = current.DrawingObjectType;
                    if (drawingObjectType.Name == string_2)
                    {
                        goto Label_0037;
                    }
                }
                return null;
            Label_0037:
                type2 = drawingObjectType;
            }
            return type2;
        }

        private DrawingObjectHelper method_7(System.Type type_0)
        {
            using (List<DrawingObjectHelper>.Enumerator enumerator = this.drawingObjectHelpers.GetEnumerator())
            {
                DrawingObjectHelper current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.DrawingObjectType == type_0)
                    {
                        ///goto  Label_0030;  ///WYJ fix, simplify the flow
                        return current;
                    }
                }
                return null;
            }
        }

        public void RemoveDrawingObject(ChartDrawingObject chartDrawingObject_1)
        {
            this.drawingObjects.Remove(chartDrawingObject_1);
        }

        public void SaveDrawingObjects(Bars bars)
        {
            if (bars != null)
            {
                this.method_5();
                if (this.DrawingObjects.Count == 0)
                {
                    if (this.dataStore.ContainsSymbol(bars.Symbol, bars.Scale, bars.BarInterval))
                    {
                        this.dataStore.RemoveFile(bars);
                    }
                }
                else
                {
                    Stream output = new FileStream(this.dataStore.FileNameForBars(bars), FileMode.Create, FileAccess.Write, FileShare.None);
                    try
                    {
                        BinaryWriter writer = new BinaryWriter(output);
                        writer.Write((double) 2.0);
                        writer.Write(this.DrawingObjects.Count);
                        foreach (ChartDrawingObject obj2 in this.DrawingObjects)
                        {
                            System.Type type = obj2.GetType();
                            writer.Write(type.Name);
                            obj2.Write(writer);
                        }
                    }
                    finally
                    {
                        if (output != null)
                        {
                            output.Close();
                        }
                    }
                }
            }
        }

        public void SplitAdjustDrawingObjects(string symbol, double splitFactor, DateTime exDate)
        {
            string path = this.rootPath + @"\DrawingObjectSplitAdjustements.dat";
            if (settingsManager == null)
            {
                settingsManager = new SettingsManager();
                settingsManager.RootPath = this.rootPath;
                settingsManager.FileName = "SplitAdjustments.txt";
            }
            lock (settingsManager)
            {
                if (settingsManager.Settings.ContainsKey(symbol) && (settingsManager.Settings[symbol] == exDate.ToShortDateString()))
                {
                    return;
                }
            }
            bool flag = false;
            DrawingObjectManager manager = null;
            Bars bars = null;
            foreach (string str2 in this.ChartBookNames)
            {
                manager = new DrawingObjectManager {
                    RootPath = this.RootPath,
                    ChartBookName = str2
                };
                manager.method_5();
                IList<BarDataScale> existingBarScales = manager.dataStore.GetExistingBarScales();
                for (int i = 0; i < existingBarScales.Count; i++)
                {
                    BarDataScale scale = existingBarScales[i];
                    BarDataScale scale2 = existingBarScales[i];
                    if (manager.dataStore.ContainsSymbol(symbol, scale.Scale, scale2.BarInterval))
                    {
                        BarDataScale scale3 = existingBarScales[i];
                        BarDataScale scale4 = existingBarScales[i];
                        bars = new Bars(symbol, scale3.Scale, scale4.BarInterval);
                        manager.LoadDrawingObjects(bars);
                        foreach (ChartDrawingObject obj2 in manager.DrawingObjects)
                        {
                            flag = true;
                            foreach (ChartDrawingObjectHandle handle in obj2.Handles)
                            {
                                handle.Value /= splitFactor;
                            }
                        }
                        manager.SaveDrawingObjects(bars);
                    }
                }
            }
            if (flag)
            {
                lock (settingsManager)
                {
                    settingsManager.Settings[symbol] = exDate.ToShortDateString();
                    settingsManager.SaveSettings();
                    File.Create(path);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public WealthLab.ChartControl.Chart Chart
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

        public string ChartBookName
        {
            get
            {
                return this.chartBookName;
            }
            set
            {
                if ((value != null) && (value != ""))
                {
                    this.chartBookName = SymbolFileNameConverter.SymbolToFileName(value);
                    this.dataStore = null;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<string> ChartBookNames
        {
            get
            {
                this.list_2.Clear();
                foreach (string str in Directory.GetDirectories(this.RootPath))
                {
                    if (str.Contains("DrawingObjects."))
                    {
                        int index = str.IndexOf("DrawingObjects.");
                        string item = str.Substring(index + 15);
                        this.list_2.Add(item);
                    }
                }
                if (this.list_2.Count == 0)
                {
                    this.list_2.Add("Standard");
                }
                return this.list_2;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public List<ChartDrawingObject> DrawingObjects
        {
            get
            {
                return this.drawingObjects;
            }
        }

        public bool HasObjectsThatCanTriggerAlerts
        {
            get
            {
                using (List<ChartDrawingObject>.Enumerator enumerator = this.DrawingObjects.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ChartDrawingObject current = enumerator.Current;
                        if (current.CanTriggerAlerts)
                        {
                            ///goto  Label_002A;  ///WYJ fix, simplify the flow
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        public string RootPath
        {
            get
            {
                return this.rootPath;
            }
            set
            {
                this.rootPath = value;
                this.dataStore = null;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ChartDrawingObject SelectedDrawingObject
        {
            get
            {
                return this.selectedDrawingObject;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ChartDrawingObjectHandle SelectedHandle
        {
            get
            {
                return this.selectedDrawingObjectHandle;
            }
            set
            {
                if (this.selectedDrawingObjectHandle != value)
                {
                    this.selectedDrawingObjectHandle = value;
                    if (this.Chart != null)
                    {
                        if (this.selectedDrawingObjectHandle != null)
                        {
                            this.Chart.Cursor = Cursors.Cross;
                        }
                        else if ((WealthLab.ChartControl.Chart.TypeOfObjectToDraw == null) && (this.Chart.Cursor != Cursors.WaitCursor))
                        {
                            this.Chart.Cursor = Cursors.Default;
                        }
                    }
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISettingsHost SettingsHost
        {
            get
            {
                return this.isettingsHost;
            }
            set
            {
                this.isettingsHost = value;
            }
        }
    }
}

