namespace WealthLab.ChartControl
{
    using Fidelity.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using WealthLab;
    using WealthLab.WSDrawingObjects;

    [ToolboxBitmap(typeof(IndicatorDragDropManager), "IndicatorDragDropManager")]
    public class IndicatorDragDropManager : Component
    {
        internal Chart chart_0;
        private FundamentalsLoader fundamentalsLoader_0;
        private IContainer icontainer_0;
        private static IndicatorHelper indicatorHelper_0;
        private int int_0;
        private int int_1;
        private int int_2;
        private int int_3;
        private int int_4;
        private static int int_5 = 40;
        private ISettingsHost isettingsHost_0;
        private List<string> list_0;
        private List<string> list_1;
        private List<string> list_2;
        private List<string> list_3;
        private List<string> list_4;
        private List<IndicatorDescriptor> list_5;
        private List<PaneDescriptor> list_6;
        private List<System.Type> list_7;
        private List<IndicatorHelper> list_8;
        private PlottedIndicator plottedIndicator_0;
        private const string string_0 = "<#Using_Statments>\n";
        private const string string_1 = "<#StrategyParameter_Statments>\n";
        private const string string_2 = "<#CTor_Statments>\n";
        private const string string_3 = "<#CreateParameter_Statments>\n";
        private const string string_4 = "<#ChartPane_Statments>\n";
        private const string string_5 = "<#PlotSeries_Statments>\n";
        private string string_6;
        private string string_7;

        private EventHandler<DroppedIndicatorEventArgs> eventHandler_0;
        public event EventHandler<DroppedIndicatorEventArgs> IndicatorDropped
        {
            add
            {
                EventHandler<DroppedIndicatorEventArgs> eventHandler;
                EventHandler<DroppedIndicatorEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<DroppedIndicatorEventArgs> eventHandler1 = (EventHandler<DroppedIndicatorEventArgs>)Delegate.Combine(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<DroppedIndicatorEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
            remove
            {
                EventHandler<DroppedIndicatorEventArgs> eventHandler;
                EventHandler<DroppedIndicatorEventArgs> eventHandler0 = this.eventHandler_0;
                do
                {
                    eventHandler = eventHandler0;
                    EventHandler<DroppedIndicatorEventArgs> eventHandler1 = (EventHandler<DroppedIndicatorEventArgs>)Delegate.Remove(eventHandler, value);
                    eventHandler0 = Interlocked.CompareExchange<EventHandler<DroppedIndicatorEventArgs>>(ref this.eventHandler_0, eventHandler1, eventHandler);
                }
                while (eventHandler0 != eventHandler);
            }
        }


        public IndicatorDragDropManager()
        {
            this.int_0 = -1;
            this.int_1 = -1;
            this.int_2 = -1;
            this.int_3 = -1;
            this.int_4 = -1;
            this.list_0 = new List<string>();
            this.list_1 = new List<string>();
            this.list_2 = new List<string>();
            this.list_3 = new List<string>();
            this.list_4 = new List<string>();
            this.list_5 = new List<IndicatorDescriptor>();
            this.list_6 = new List<PaneDescriptor>();
            this.list_7 = new List<System.Type>();
            this.list_8 = new List<IndicatorHelper>();
            this.method_0();
            this.method_8();
        }

        public IndicatorDragDropManager(IContainer container)
        {
            this.int_0 = -1;
            this.int_1 = -1;
            this.int_2 = -1;
            this.int_3 = -1;
            this.int_4 = -1;
            this.list_0 = new List<string>();
            this.list_1 = new List<string>();
            this.list_2 = new List<string>();
            this.list_3 = new List<string>();
            this.list_4 = new List<string>();
            this.list_5 = new List<IndicatorDescriptor>();
            this.list_6 = new List<PaneDescriptor>();
            this.list_7 = new List<System.Type>();
            this.list_8 = new List<IndicatorHelper>();
            container.Add(this);
            this.method_0();
            this.method_8();
        }

        public static int CalculateDecimalsFromDefaultValue(double value)
        {
            int num = 2;
            value *= 100.0;
            value -= (int) value;
            while (value > 0.0)
            {
                value *= 10.0;
                value -= (int) value;
                num++;
                if (num == 6)
                {
                    return num;
                }
            }
            return num;
        }

        public void Clear()
        {
            this.list_5.Clear();
            this.method_20();
            this.chart_0.Renderer.ClearDragDropIndicators();
        }

        public void CreateDragDropIndicators()
        {
            if (this.chart_0.Renderer != null)
            {
                this.chart_0.Renderer.ClearDragDropIndicators();
                foreach (PaneDescriptor descriptor2 in this.list_6)
                {
                    if (this.chart_0.Renderer.FindPane(descriptor2.Description) == null)
                    {
                        int resizedRawHeight = this.chart_0.Renderer.GetResizedRawHeight(descriptor2.Description, int_5);
                        new ChartPane(this.chart_0.Renderer, descriptor2.AbovePricePane, resizedRawHeight, descriptor2.Description);
                    }
                }
                foreach (IndicatorDescriptor descriptor in this.list_5)
                {
                    ChartPane pane = this.chart_0.Renderer.FindPane(descriptor.PaneDescription);
                    if (pane != null)
                    {
                        PlottedIndicator item = this.method_9(descriptor);
                        if (item != null)
                        {
                            pane.PlottedIndicators.Add(item);
                        }
                        this.method_11(descriptor, pane);
                    }
                }
            }
        }

        public static bool CreateIndicatorParameterUI(IndicatorHelper helper, Control control_0, ChartPane pane, string symbol, bool includeVolume)
        {
            return CreateIndicatorParameterUI(helper, control_0, pane, symbol, includeVolume, (float) 1f);
        }

        public static bool CreateIndicatorParameterUI(IndicatorHelper helper, Control control_0, ChartPane pane, string symbol, bool includeVolume, int yOffset)
        {
            return smethod_0(helper, control_0, pane, symbol, includeVolume, 1f, yOffset);
        }

        public static bool CreateIndicatorParameterUI(IndicatorHelper helper, Control control_0, ChartPane pane, string symbol, bool includeVolume, float scaleFactor)
        {
            return smethod_0(helper, control_0, pane, symbol, includeVolume, scaleFactor, 20);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer_0 != null))
            {
                this.icontainer_0.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool EditIndicator(PlottedIndicator plottedIndicator_1)
        {
            IndicatorDescriptor item = this.method_14(plottedIndicator_1);
            if (item == null)
            {
                return false;
            }
            if (item.IsFundamental)
            {
                IndicatorParametersForm form = new IndicatorParametersForm();
                form.Initialize(item.FundamentalItemName);
                form.IndicatorColor = item.Color;
                form.IndicatorStyle = item.Style;
                form.IndicatorWidth = item.Width;
                if (form.ShowDialog(this.chart_0.FindForm()) == DialogResult.OK)
                {
                    this.method_17(form, plottedIndicator_1);
                    item.Color = form.IndicatorColor;
                    item.Style = form.IndicatorStyle;
                    item.Width = form.IndicatorWidth;
                    this.CreateDragDropIndicators();
                    return true;
                }
                return false;
            }
            int index = this.list_5.IndexOf(item);
            IndicatorHelper helper = this.method_15(plottedIndicator_1);
            IndicatorParametersForm form2 = new IndicatorParametersForm();
            form2.Initialize(plottedIndicator_1, item, helper, this.chart_0, this.chart_0.Renderer.FindPane(item.PaneDescription));
            if (form2.ShowDialog(this.chart_0.FindForm()) != DialogResult.OK)
            {
                return false;
            }
            this.method_17(form2, plottedIndicator_1);
            this.list_5[index] = form2.GetIndicatorDescriptor();
            this.list_5[index].LinkDescription = item.PlottedIndicator.Series.Description;
            foreach (IndicatorDescriptor descriptor3 in this.list_5)
            {
                if ((descriptor3 != this.list_5[index]) && (descriptor3.PlottedIndicator != null))
                {
                    descriptor3.LinkDescription = descriptor3.PlottedIndicator.Series.Description;
                }
            }
            this.CreateDragDropIndicators();
            foreach (IndicatorDescriptor descriptor2 in this.list_5)
            {
                descriptor2.LinkDescription = "";
            }
            return true;
        }

        public static string GetIndicatorString(IndicatorHelper helper, Control control_0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(helper.IndicatorType.Name);
            builder.Append(".Series(");
            for (int i = 0; i < helper.ParameterDefaultValues.Count; i++)
            {
                object obj2 = helper.ParameterDefaultValues[i];
                System.Type type = obj2.GetType();
                Control control = control_0.Controls[(i * 2) + 1];
                string text = "";
                if (obj2 is Enum)
                {
                    if ((obj2 is CoreDataSeries) || (obj2 is BarDataType))
                    {
                        text = control.Text;
                    }
                    else
                    {
                        text = type.Name + "." + control.Text;
                    }
                }
                else if (type == typeof(bool))
                {
                    CheckBox box = control as CheckBox;
                    if (box.Checked)
                    {
                        text = "true";
                    }
                    else
                    {
                        text = "false";
                    }
                }
                else if (type == typeof(string))
                {
                    text = "\"" + control.Text + "\"";
                }
                else
                {
                    text = control.Text;
                }
                builder.Append(text);
                if (i < (helper.ParameterDefaultValues.Count - 1))
                {
                    builder.Append(",");
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        private void method_0()
        {
            this.icontainer_0 = new Container();
        }

        private void method_1()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            List<string> list = new List<string>();
            foreach (IndicatorDescriptor descriptor in this.list_5)
            {
                string str2;
                string str4;
                string newValue = this.method_3(dictionary, descriptor);
                if (descriptor.IsFundamental)
                {
                    str2 = "PlotFundamentalItems(<#PaneName>,<#IndicatorSeries>,<#Color>,LineStyle.<#LineStyle>,<#LineWidth>);";
                    str4 = "\"" + descriptor.FundamentalItemName + "\"";
                }
                else
                {
                    string str = "using " + descriptor.IndicatorType.Namespace + ";";
                    if (!this.string_6.Contains(str) && !this.list_0.Contains(str))
                    {
                        this.list_0.Add(str);
                    }
                    str4 = this.method_2(list, dictionary2, descriptor);
                    if (descriptor.PlotBandPairIndicator)
                    {
                        string str7;
                        string str6 = descriptor.BandPairIndicatorType.Name + str4.Substring(str4.IndexOf('.'));
                        if (descriptor.FillBand)
                        {
                            str7 = smethod_2(descriptor.BandFillColor);
                        }
                        else
                        {
                            str7 = smethod_2(Color.Transparent);
                        }
                        str2 = "PlotSeriesFillBand(<#PaneName>,<#IndicatorSeries>," + str6 + ",<#Color>," + str7 + ",LineStyle.<#LineStyle>,<#LineWidth>);";
                    }
                    else if (descriptor.PlotOscillator)
                    {
                        str2 = "PlotSeriesOscillator(<#PaneName>,<#IndicatorSeries>," + descriptor.OverboughtLevel.ToString() + "," + descriptor.OversoldLevel.ToString() + "," + smethod_2(descriptor.OverboughtColor) + "," + smethod_2(descriptor.OversoldColor) + ",<#Color>,LineStyle.<#LineStyle>,<#LineWidth>);";
                    }
                    else
                    {
                        str2 = "PlotSeries(<#PaneName>,<#IndicatorSeries>,<#Color>,LineStyle.<#LineStyle>,<#LineWidth>);";
                    }
                }
                str2 = str2.Replace("<#PaneName>", newValue).Replace("<#IndicatorSeries>", str4);
                string str5 = smethod_2(descriptor.Color);
                str2 = str2.Replace("<#Color>", str5).Replace("<#LineStyle>", descriptor.Style.ToString()).Replace("<#LineWidth>", descriptor.Width.ToString());
                this.list_2.Add(str2);
            }
        }

        private object[] method_10(IndicatorDescriptor indicatorDescriptor_0)
        {
            object[] objArray = (object[]) indicatorDescriptor_0.Parameters.Clone();
            for (int i = 0; i < objArray.Length; i++)
            {
                object obj2 = objArray[i];
                if (obj2 is CoreDataSeries)
                {
                    switch (((CoreDataSeries) obj2))
                    {
                        case CoreDataSeries.Close:
                            objArray[i] = this.chart_0.Bars.Close;
                            break;

                        case CoreDataSeries.Open:
                            objArray[i] = this.chart_0.Bars.Open;
                            break;

                        case CoreDataSeries.High:
                            objArray[i] = this.chart_0.Bars.High;
                            break;

                        case CoreDataSeries.Low:
                            objArray[i] = this.chart_0.Bars.Low;
                            break;
                    }
                    continue;
                }
                if (!(obj2 is IndicatorDescriptionString))
                {
                    ///goto  Label_0173; ///WYJ fix, simplify the flow
                    if (obj2 is BarsDescriptorString)
                    {
                        objArray[i] = this.chart_0.Bars;
                    }
                    continue;
                }
                IndicatorDescriptionString str = (IndicatorDescriptionString) obj2;
                if (this.chart_0.Renderer.FindPane(indicatorDescriptor_0.PaneDescription) == null)
                {
                    return null;
                }
                PlottedIndicator indicator = this.chart_0.Renderer.FindPlottedIndicator(str.Description);
                if (indicator != null)
                {
                    ///goto  Label_0168;
                    objArray[i] = indicator.Series;
                    continue;
                }
                using (List<IndicatorDescriptor>.Enumerator enumerator = this.list_5.GetEnumerator())
                {
                    IndicatorDescriptor current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.LinkDescription == str.Description)
                        {
                            ///goto  Label_0135;  ///WYJ fix, simplify the flow
                            indicator = current.PlottedIndicator;
                            str.Description = current.PlottedIndicator.Series.Description;
                            break;
                        }
                    }
                }
                if (indicator == null)
                {
                    return null;
                }
                objArray[i] = indicator.Series;
            }
            return objArray;
        }

        private void method_11(IndicatorDescriptor indicatorDescriptor_0, ChartPane chartPane_0)
        {
            if (indicatorDescriptor_0.IsFundamental)
            {
                this.chart_0.Renderer.PlotFundamentalItem(chartPane_0, indicatorDescriptor_0.FundamentalItemName, indicatorDescriptor_0.Color, indicatorDescriptor_0.Style, indicatorDescriptor_0.Width, true);
            }
            if (indicatorDescriptor_0.PlotOscillator)
            {
                WSDFilledOscillator oscillator = new WSDFilledOscillator(indicatorDescriptor_0.PlottedIndicator.Series, indicatorDescriptor_0.OverboughtLevel, indicatorDescriptor_0.OversoldLevel, new SolidBrush(indicatorDescriptor_0.OverboughtColor), new SolidBrush(indicatorDescriptor_0.OversoldColor));
                this.chart_0.Renderer.PlotWealthScriptObject(chartPane_0, oscillator, true, true);
            }
            if (indicatorDescriptor_0.PlotBandPairIndicator)
            {
                object[] parameters = this.method_10(indicatorDescriptor_0);
                MethodInfo method = indicatorDescriptor_0.BandPairIndicatorType.GetMethod("Series");
                if (method != null)
                {
                    DataSeries series = method.Invoke(null, parameters) as DataSeries;
                    PlottedIndicator item = new PlottedIndicator(this.chart_0.Renderer, series) {
                        Color = indicatorDescriptor_0.Color,
                        Style = indicatorDescriptor_0.Style,
                        Width = indicatorDescriptor_0.Width,
                        DragAndDrop = true
                    };
                    indicatorDescriptor_0.PlottedPartner = item;
                    chartPane_0.PlottedIndicators.Add(item);
                    if (indicatorDescriptor_0.FillBand)
                    {
                        WSDFilledBand band = new WSDFilledBand(item.Series, indicatorDescriptor_0.PlottedIndicator.Series, new SolidBrush(indicatorDescriptor_0.BandFillColor));
                        this.chart_0.Renderer.PlotWealthScriptObject(chartPane_0, band, true, true);
                    }
                }
            }
        }

        private void method_12(IndicatorDescriptor indicatorDescriptor_0, List<IndicatorDescriptor> list_9)
        {
            DataSeries series = indicatorDescriptor_0.PlottedIndicator.Series;
            foreach (IndicatorDescriptor descriptor in this.list_5)
            {
                foreach (object obj2 in descriptor.Parameters)
                {
                    if (obj2 is IndicatorDescriptionString)
                    {
                        IndicatorDescriptionString str = (IndicatorDescriptionString) obj2;
                        if (str.Description == series.Description)
                        {
                            goto Label_0067;
                        }
                    }
                }
                continue;
            Label_0067:
                list_9.Add(descriptor);
                this.method_12(descriptor, list_9);
            }
        }

        private List<IndicatorDescriptor> method_13(IndicatorDescriptor indicatorDescriptor_0)
        {
            List<IndicatorDescriptor> list = new List<IndicatorDescriptor>();
            this.method_12(indicatorDescriptor_0, list);
            return list;
        }

        private IndicatorDescriptor method_14(PlottedIndicator plottedIndicator_1)
        {
            IndicatorDescriptor descriptor2;
            using (List<IndicatorDescriptor>.Enumerator enumerator = this.list_5.GetEnumerator())
            {
                IndicatorDescriptor current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if ((current.PlottedIndicator == plottedIndicator_1) || (current.PlottedPartner == plottedIndicator_1))
                    {
                        goto Label_0036;
                    }
                }
                return null;
            Label_0036:
                descriptor2 = current;
            }
            return descriptor2;
        }

        private IndicatorHelper method_15(PlottedIndicator plottedIndicator_1)
        {
            using (List<IndicatorHelper>.Enumerator enumerator = this.list_8.GetEnumerator())
            {
                IndicatorHelper current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.IndicatorType == plottedIndicator_1.Series.GetType())
                    {
                        ///goto  Label_003A;  ///WYJ fix, simplify the flow
                        return current; 
                    }
                }
                return null;
            }
        }

        private IndicatorHelper method_16(IndicatorDescriptor indicatorDescriptor_0)
        {
            using (List<IndicatorHelper>.Enumerator enumerator = this.list_8.GetEnumerator())
            {
                IndicatorHelper current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (current.IndicatorType == indicatorDescriptor_0.IndicatorType)
                    {
                        ///goto  Label_0035;
                        return current;
                    }
                }
                return null;
            }
        }

        private void method_17(IndicatorParametersForm indicatorParametersForm_0, PlottedIndicator plottedIndicator_1)
        {
            string str;
            if (plottedIndicator_1.IsFundamental)
            {
                str = "DrawFund." + plottedIndicator_1.FundamentalItemName + ".";
            }
            else
            {
                str = "DrawInd." + plottedIndicator_1.Series.GetType().Name + ".";
            }
            this.SettingsHost.Set(str + "Color", indicatorParametersForm_0.IndicatorColor);
            this.SettingsHost.Set(str + "Style", indicatorParametersForm_0.IndicatorStyle.ToString());
            this.SettingsHost.Set(str + "Width", indicatorParametersForm_0.IndicatorWidth);
            this.SettingsHost.Set(str + "FillBands", indicatorParametersForm_0.FillBand);
            if (indicatorParametersForm_0.FillBand)
            {
                this.SettingsHost.Set(str + "BandFillColor", indicatorParametersForm_0.BandFillColor);
                this.SettingsHost.Set(str + "BandTransparency", indicatorParametersForm_0.BandFillTransparency);
            }
            this.SettingsHost.Set(str + "Oscillator", indicatorParametersForm_0.PlotAsOscillator);
            if (indicatorParametersForm_0.PlotAsOscillator)
            {
                this.SettingsHost.Set(str + "OscOverbought", indicatorParametersForm_0.OscillatorOverboughtLevel);
                this.SettingsHost.Set(str + "OscOverboughtColor", indicatorParametersForm_0.OscillatorOverboughtColor);
                this.SettingsHost.Set(str + "OscOversold", indicatorParametersForm_0.OscillatorOversoldLevel);
                this.SettingsHost.Set(str + "OscOversoldColor", indicatorParametersForm_0.OscillatorOversoldColor);
                this.SettingsHost.Set(str + "OscTransparency", indicatorParametersForm_0.OscillatorFillTransparency);
            }
            for (int i = 0; i < indicatorParametersForm_0.IndicatorParameterCount; i++)
            {
                string indicatorParameterValue = indicatorParametersForm_0.GetIndicatorParameterValue(i);
                if (indicatorParameterValue != "")
                {
                    this.SettingsHost.Set(str + "Param" + i, indicatorParameterValue);
                }
            }
        }

        private void method_18(System.Type type_0, IndicatorParametersForm indicatorParametersForm_0)
        {
            string str = "DrawInd." + type_0.Name + ".";
            this.method_19(str, indicatorParametersForm_0);
        }

        private void method_19(string string_8, IndicatorParametersForm indicatorParametersForm_0)
        {
            if (this.SettingsHost.ContainsKey(string_8 + "Color"))
            {
                indicatorParametersForm_0.IndicatorColor = this.SettingsHost.Get(string_8 + "Color", Color.Red);
                indicatorParametersForm_0.IndicatorStyle = (LineStyle) Enum.Parse(typeof(LineStyle), this.SettingsHost.Get(string_8 + "Style", "Solid"));
                indicatorParametersForm_0.IndicatorWidth = this.SettingsHost.Get(string_8 + "Width", 1);
                indicatorParametersForm_0.PlotBandPairIndicator = this.SettingsHost.Get(string_8 + "Bands", false);
                if (indicatorParametersForm_0.PlotBandPairIndicator)
                {
                    indicatorParametersForm_0.FillBand = this.SettingsHost.Get(string_8 + "FillBands", false);
                    indicatorParametersForm_0.BandFillColor = this.SettingsHost.Get(string_8 + "BandFillColor", Color.Blue);
                    indicatorParametersForm_0.BandFillTransparency = this.SettingsHost.Get(string_8 + "BandTransparency", 80);
                }
                indicatorParametersForm_0.PlotAsOscillator = this.SettingsHost.Get(string_8 + "Oscillator", false);
                if (indicatorParametersForm_0.PlotAsOscillator)
                {
                    indicatorParametersForm_0.OscillatorOverboughtLevel = this.SettingsHost.Get(string_8 + "OscOverbought", (double) 0.0);
                    indicatorParametersForm_0.OscillatorOverboughtColor = this.SettingsHost.Get(string_8 + "OscOverboughtColor", Color.Blue);
                    indicatorParametersForm_0.OscillatorOversoldLevel = this.SettingsHost.Get(string_8 + "OscOversold", (double) 0.0);
                    indicatorParametersForm_0.OscillatorOversoldColor = this.SettingsHost.Get(string_8 + "OscOversoldColor", Color.Red);
                    indicatorParametersForm_0.OscillatorFillTransparency = this.SettingsHost.Get(string_8 + "OscTransparency", 80);
                }
                for (int i = 0; i < indicatorParametersForm_0.IndicatorParameterCount; i++)
                {
                    string str = string_8 + "Param" + i;
                    if (this.SettingsHost.ContainsKey(str))
                    {
                        string str2 = this.SettingsHost.Get(str, "");
                        indicatorParametersForm_0.SetIndicatorParameterValue(i, str2);
                    }
                }
            }
        }

        ///WYJ fix, code from Reflector, deprecated because of having too many goto statements. Try code from ILSpy
        /*
        private string method_2(List<string> list_9, Dictionary<string, string> dictionary_0, IndicatorDescriptor indicatorDescriptor_0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(indicatorDescriptor_0.IndicatorType.Name);
            builder.Append(".Series(");
            StringBuilder builder2 = new StringBuilder();
            builder2.Append(indicatorDescriptor_0.IndicatorType.Name);
            builder2.Append("(");
            int num = 0;
            IndicatorHelper helper = this.method_16(indicatorDescriptor_0);
            for (int i = 0; i < indicatorDescriptor_0.Parameters.Length; i++)
            {
                int num3;
                string str2;
                double minimumValue;
                double maximumValue;
                IndicatorDescriptor descriptor2;
                System.Type type = helper.ParameterDefaultValues[i].GetType();
                object obj2 = indicatorDescriptor_0.Parameters[i];
                string str = indicatorDescriptor_0.Parameters[i].ToString();
                if (obj2 is BarsDescriptorString)
                {
                    str = "Bars";
                    goto Label_04A2;
                }
                builder2.Append((num++ > 0) ? "," : "");
                builder2.Append(str);
                if ((obj2 is Enum) && (type != typeof(CoreDataSeries)))
                {
                    if (type == typeof(BarDataType))
                    {
                        str = "Bars";
                    }
                    else
                    {
                        str = type.Name + "." + str;
                    }
                    goto Label_04A2;
                }
                if (type == typeof(string))
                {
                    str = "\"" + indicatorDescriptor_0.Parameters[i].ToString() + "\"";
                    goto Label_04A2;
                }
                if ((type == typeof(RangeBoundInt32)) || (type == typeof(RangeBoundDouble)))
                {
                    goto Label_0275;
                }
                if (!(obj2 is IndicatorDescriptionString))
                {
                    goto Label_025B;
                }
                IndicatorDescriptionString str5 = (IndicatorDescriptionString) obj2;
                if (dictionary_0.ContainsKey(str5.Description))
                {
                    str = dictionary_0[str5.Description];
                    goto Label_04A2;
                }
                PlottedIndicator indicator = this.chart_0.Renderer.FindPlottedIndicator(str5.Description);
                if (indicator != null)
                {
                    goto Label_0240;
                }
                using (List<IndicatorDescriptor>.Enumerator enumerator = this.list_5.GetEnumerator())
                {
                    IndicatorDescriptor current;
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        if (current.LinkDescription == str5.Description)
                        {
                            goto Label_0208;
                        }
                    }
                    goto Label_0239;
                Label_0208:
                    indicator = current.PlottedIndicator;
                    str5.Description = current.PlottedIndicator.Series.Description;
                }
            Label_0239:
                if (indicator == null)
                {
                    return null;
                }
            Label_0240:
                descriptor2 = this.method_14(indicator);
                str = this.method_2(list_9, dictionary_0, descriptor2);
                goto Label_04A2;
            Label_025B:
                if (obj2 is bool)
                {
                    str = str.ToLower();
                }
                goto Label_04A2;
            Label_0275:
                num3 = list_9.Count;
                goto Label_028E;
            Label_027F:
                if (!this.string_6.Contains(str2))
                {
                    goto Label_02B3;
                }
            Label_028E:
                num3++;
                str2 = "slider" + num3.ToString();
                if (list_9.Contains(str2))
                {
                    goto Label_028E;
                }
                goto Label_027F;
            Label_02B3:
                list_9.Add(str2);
                this.list_3.Add("private StrategyParameter " + str2 + ";");
                string str3 = "\"" + indicatorDescriptor_0.IndicatorType.Name + "_" + helper.ParameterDescriptions[i] + "_" + num3.ToString() + "\"";
                double gridIncrement = 1.0;
                if (type == typeof(RangeBoundInt32))
                {
                    RangeBoundInt32 num5 = (RangeBoundInt32) helper.ParameterDefaultValues[i];
                    minimumValue = num5.MinimumValue;
                    maximumValue = num5.MaximumValue;
                }
                else
                {
                    RangeBoundDouble num8 = (RangeBoundDouble) helper.ParameterDefaultValues[i];
                    minimumValue = num8.MinimumValue;
                    maximumValue = num8.MaximumValue;
                }
                GridLines lines = new GridLines {
                    WholeNumbersOnly = (minimumValue == ((int) minimumValue)) && (maximumValue == ((int) maximumValue)),
                    LinesDesired = 15,
                    RangeMin = minimumValue,
                    RangeMax = maximumValue
                };
                gridIncrement = lines.GridIncrement;
                string item = str2 + " = CreateParameter(" + str3 + "," + str + "," + minimumValue.ToString() + "," + maximumValue.ToString() + "," + gridIncrement.ToString() + ");";
                this.list_4.Add(item);
                str = str2 + "." + ((type == typeof(RangeBoundInt32)) ? "ValueInt" : "Value");
            Label_04A2:
                builder.Append((i > 0) ? "," : "");
                builder.Append(str);
            }
            builder.Append(")");
            builder2.Append(")");
            string str6 = builder2.ToString();
            dictionary_0[str6] = builder.ToString();
            return builder.ToString();
        } */

        ///WYJ fix, code from ILSpy
        // WealthLab.ChartControl.IndicatorDragDropManager
        private string method_2(List<string> list_9, Dictionary<string, string> dictionary_0, IndicatorDescriptor indicatorDescriptor_0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(indicatorDescriptor_0.IndicatorType.Name);
            stringBuilder.Append(".Series(");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append(indicatorDescriptor_0.IndicatorType.Name);
            stringBuilder2.Append("(");
            int num = 0;
            IndicatorHelper indicatorHelper = this.method_16(indicatorDescriptor_0);
            for (int i = 0; i < indicatorDescriptor_0.Parameters.Length; i++)
            {
                Type type = indicatorHelper.ParameterDefaultValues[i].GetType();
                object obj = indicatorDescriptor_0.Parameters[i];
                string text = indicatorDescriptor_0.Parameters[i].ToString();
                if (obj is BarsDescriptorString)
                {
                    text = "Bars";
                }
                else
                {
                    stringBuilder2.Append((num++ > 0) ? "," : "");
                    stringBuilder2.Append(text);
                    if (obj is Enum && type != typeof(CoreDataSeries))
                    {
                        if (type == typeof(BarDataType))
                        {
                            text = "Bars";
                        }
                        else
                        {
                            text = type.Name + "." + text;
                        }
                    }
                    else
                    {
                        if (type == typeof(string))
                        {
                            text = "\"" + indicatorDescriptor_0.Parameters[i].ToString() + "\"";
                        }
                        else
                        {
                            if (!(type == typeof(RangeBoundInt32)) && !(type == typeof(RangeBoundDouble)))
                            {
                                if (obj is IndicatorDescriptionString)
                                {
                                    IndicatorDescriptionString indicatorDescriptionString = (IndicatorDescriptionString)obj;
                                    if (dictionary_0.ContainsKey(indicatorDescriptionString.Description))
                                    {
                                        text = dictionary_0[indicatorDescriptionString.Description];
                                    }
                                    else
                                    {
                                        PlottedIndicator plottedIndicator = this.chart_0.Renderer.FindPlottedIndicator(indicatorDescriptionString.Description);
                                        if (plottedIndicator == null)
                                        {
                                            foreach (IndicatorDescriptor current in this.list_5)
                                            {
                                                if (current.LinkDescription == indicatorDescriptionString.Description)
                                                {
                                                    plottedIndicator = current.PlottedIndicator;
                                                    indicatorDescriptionString.Description = current.PlottedIndicator.Series.Description;
                                                    break;
                                                }
                                            }
                                            if (plottedIndicator == null)
                                            {
                                                return null;
                                            }
                                        }
                                        IndicatorDescriptor indicatorDescriptor_ = this.method_14(plottedIndicator);
                                        text = this.method_2(list_9, dictionary_0, indicatorDescriptor_);
                                    }
                                }
                                else
                                {
                                    if (obj is bool)
                                    {
                                        text = text.ToLower();
                                    }
                                }
                            }
                            else
                            {
                                int num2 = list_9.Count;
                                string text2;
                                do
                                {
                                    num2++;
                                    text2 = "slider" + num2.ToString();
                                }
                                while (list_9.Contains(text2) || this.string_6.Contains(text2));
                                list_9.Add(text2);
                                this.list_3.Add("private StrategyParameter " + text2 + ";");
                                string text3 = string.Concat(new string[]
						        {
							        "\"",
							        indicatorDescriptor_0.IndicatorType.Name,
							        "_",
							        indicatorHelper.ParameterDescriptions[i],
							        "_",
							        num2.ToString(),
							        "\""
						        });
                                double num3;
                                double num4;
                                if (type == typeof(RangeBoundInt32))
                                {
                                    RangeBoundInt32 rangeBoundInt = (RangeBoundInt32)indicatorHelper.ParameterDefaultValues[i];
                                    num3 = (double)rangeBoundInt.MinimumValue;
                                    num4 = (double)rangeBoundInt.MaximumValue;
                                }
                                else
                                {
                                    RangeBoundDouble rangeBoundDouble = (RangeBoundDouble)indicatorHelper.ParameterDefaultValues[i];
                                    num3 = rangeBoundDouble.MinimumValue;
                                    num4 = rangeBoundDouble.MaximumValue;
                                }
                                double gridIncrement = new GridLines
                                {
                                    WholeNumbersOnly = num3 == (double)((int)num3) && num4 == (double)((int)num4),
                                    LinesDesired = 15,
                                    RangeMin = num3,
                                    RangeMax = num4
                                }.GridIncrement;
                                string item = string.Concat(new string[]
						        {
							        text2,
							        " = CreateParameter(",
							        text3,
							        ",",
							        text,
							        ",",
							        num3.ToString(),
							        ",",
							        num4.ToString(),
							        ",",
							        gridIncrement.ToString(),
							        ");"
						        });
                                this.list_4.Add(item);
                                text = text2 + "." + ((type == typeof(RangeBoundInt32)) ? "ValueInt" : "Value");
                            }
                        }
                    }
                }
                stringBuilder.Append((i > 0) ? "," : "");
                stringBuilder.Append(text);
            }
            stringBuilder.Append(")");
            stringBuilder2.Append(")");
            string key = stringBuilder2.ToString();
            dictionary_0[key] = stringBuilder.ToString();
            return stringBuilder.ToString();
        }


        private void method_20()
        {
            for (int i = this.list_6.Count - 1; i >= 0; i--)
            {
                PaneDescriptor descriptor = this.list_6[i];
                bool flag = true;
                using (List<IndicatorDescriptor>.Enumerator enumerator = this.list_5.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        IndicatorDescriptor current = enumerator.Current;
                        if (current.PaneDescription == descriptor.Description)
                        {
                            ///goto  Label_0059;  ///WYJ fix, simplify the flow
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    this.chart_0.Renderer.RemovePane(descriptor.Description);
                    this.list_6.RemoveAt(i);
                }
            }
        }

        private string method_3(Dictionary<string, string> dictionary_0, IndicatorDescriptor indicatorDescriptor_0)
        {
            string paneDescription = indicatorDescriptor_0.PaneDescription;
            string str2 = paneDescription;
            switch (paneDescription)
            {
                case "P":
                    return "PricePane";

                case "V":
                    return "VolumePane";
            }
            if (dictionary_0.ContainsKey(paneDescription))
            {
                return dictionary_0[paneDescription];
            }
            for (int i = 0; i < paneDescription.Length; i++)
            {
                if (!char.IsLetterOrDigit(paneDescription[i]) && (paneDescription[i] != ' '))
                {
                    str2 = str2.Replace(paneDescription[i], ' ');
                }
            }
            string[] strArray = str2.Split(new char[] { ' ' });
            StringBuilder builder = new StringBuilder();
            for (int j = 0; j < strArray.Length; j++)
            {
                if (!string.IsNullOrEmpty(strArray[j]))
                {
                    char ch = strArray[j][0];
                    builder.Append(ch.ToString().ToUpper());
                    if (strArray[j].Length > 1)
                    {
                        builder.Append(strArray[j].Substring(1));
                    }
                }
            }
            str2 = builder.ToString();
            using (List<PaneDescriptor>.Enumerator enumerator = this.list_6.GetEnumerator())
            {
                int num3;
                while (enumerator.MoveNext())
                {
                    PaneDescriptor current = enumerator.Current;
                    if (current.Description == paneDescription)
                    {
                        goto Label_0143;
                    }
                }
                return str2;
            Label_0143:
                num3 = 0;
                string str3 = str2;
                do
                {
                    num3++;
                    str2 = "pane" + str3 + num3.ToString();
                }
                while (this.string_6.Contains(str2 + " "));
                dictionary_0[paneDescription] = str2;
                string item = string.Concat(new object[] { "ChartPane ", str2, " = CreatePane(", int_5, ",true,true);" });
                this.list_1.Add(item);
            }
            return str2;
        }

        private string method_4(string string_8)
        {
            if (!this.method_5(string_8))
            {
                throw new InvalidDataException("Invalid strategy code: could not place code insertion points");
            }
            StringBuilder builder = new StringBuilder();
            int startIndex = 0;
            builder.Append(string_8.Substring(0, this.int_0));
            startIndex = this.int_0;
            builder.Append("<#Using_Statments>\n");
            if (this.int_1 < this.int_3)
            {
                if (this.int_1 > -1)
                {
                    builder.Append(string_8.Substring(startIndex, this.int_1 - startIndex));
                    builder.Append("<#StrategyParameter_Statments>\n");
                    startIndex = this.int_1;
                    if (this.int_2 > -1)
                    {
                        builder.Append(string_8.Substring(startIndex, this.int_2 - startIndex));
                        builder.Append("<#CreateParameter_Statments>\n");
                        startIndex = this.int_2;
                    }
                }
                builder.Append(string_8.Substring(startIndex, this.int_3 - startIndex));
                if (this.int_1 == -1)
                {
                    builder.Append("<#StrategyParameter_Statments>\n");
                    builder.AppendLine("<#CTor_Statments>\n");
                }
                startIndex = this.int_3;
                if (this.int_4 > -1)
                {
                    builder.Append(string_8.Substring(startIndex, this.int_4 - startIndex));
                    builder.AppendLine("<#ChartPane_Statments>\n");
                    builder.AppendLine("<#PlotSeries_Statments>\n");
                    startIndex = this.int_4;
                }
            }
            if (this.int_3 < this.int_1)
            {
                builder.Append(string_8.Substring(startIndex, this.int_3 - startIndex));
                startIndex = this.int_3;
                if (this.int_4 > -1)
                {
                    builder.Append(string_8.Substring(startIndex, this.int_4 - startIndex));
                    builder.AppendLine("<#ChartPane_Statments>\n");
                    builder.AppendLine("<#PlotSeries_Statments>\n");
                    startIndex = this.int_4;
                }
                builder.Append(string_8.Substring(startIndex, this.int_1 - startIndex));
                builder.AppendLine("<#StrategyParameter_Statments>\n");
                startIndex = this.int_1;
                if (this.int_2 > -1)
                {
                    builder.Append(string_8.Substring(startIndex, this.int_2 - startIndex));
                    builder.AppendLine("<#CreateParameter_Statments>\n");
                    startIndex = this.int_2;
                }
            }
            builder.Append(string_8.Substring(startIndex));
            return builder.ToString();
        }

        private bool method_5(string string_8)
        {
            this.int_0 = -1;
            this.int_1 = -1;
            this.int_2 = -1;
            this.int_3 = -1;
            this.int_4 = -1;
            bool flag = false;
            bool flag2 = false;
            string str = "MyStrategy";
            bool flag3 = false;
            int num = 0;
            bool flag4 = false;
            int num2 = 0;
            int num3 = 0;
            StringReader reader = new StringReader(string_8);
            string str2 = reader.ReadLine();
            int startIndex = 0;
            int length = 0;
            bool flag5 = false;
            while (str2 != null)
            {
                startIndex += length;
                startIndex += string_8.Substring(startIndex).IndexOf(str2);
                length = str2.Length;
                str2 = this.method_6(str2, ref flag5);
                str2 = this.method_7(str2);
                if (!string.IsNullOrEmpty(str2))
                {
                    if (!flag)
                    {
                        if (str2.StartsWith("namespace "))
                        {
                            flag = true;
                            this.int_0 = startIndex;
                        }
                    }
                    else if (!flag2)
                    {
                        if (str2.Contains("class ") && str2.Contains("WealthScript"))
                        {
                            flag2 = true;
                            string[] strArray = str2.Split(new char[] { ' ' });
                            int index = str2.StartsWith("public ") ? 2 : 1;
                            str = strArray[index];
                            this.string_7 = str;
                        }
                    }
                    else
                    {
                        if (!flag3 && str2.StartsWith("public " + str))
                        {
                            flag3 = true;
                            this.int_1 = startIndex;
                            num = 1;
                            num3 = 0;
                        }
                        else if (!flag4 && str2.StartsWith("protected override void Execute()"))
                        {
                            flag4 = true;
                            this.int_3 = startIndex;
                            num2 = 1;
                            num3 = 0;
                        }
                        if (num > 0)
                        {
                            if ((num == 1) && str2.Contains("{"))
                            {
                                num++;
                            }
                            else if (num == 2)
                            {
                                this.int_2 = startIndex;
                                num++;
                            }
                            else if (num == 3)
                            {
                                if (str2.Contains("}"))
                                {
                                    if (num3 == 0)
                                    {
                                        num = 0;
                                    }
                                    else
                                    {
                                        num3--;
                                    }
                                }
                                else if (str2.Contains("{"))
                                {
                                    num3++;
                                }
                            }
                        }
                        else if (num2 > 0)
                        {
                            if (str2.Contains("}"))
                            {
                                if (--num3 == 0)
                                {
                                    this.int_4 = startIndex;
                                    num2 = 0;
                                }
                            }
                            else if (str2.Contains("{"))
                            {
                                num3++;
                            }
                        }
                    }
                    if (((flag && flag2) && (flag3 && (num == 0))) && (flag4 && (num2 == 0)))
                    {
                        break;
                    }
                }
                str2 = reader.ReadLine();
            }
            if (((this.int_0 <= -1) || (this.int_3 <= -1)) || (this.int_4 <= -1))
            {
                return false;
            }
            if (this.int_1 != -1)
            {
                return (this.int_2 > -1);
            }
            return true;
        }

        private string method_6(string string_8, ref bool bool_0)
        {
            StringBuilder builder = new StringBuilder();
            while (string_8.Length > 0)
            {
                if (!bool_0)
                {
                    int length = -1;
                    int index = -1;
                    if (string_8.Contains("//"))
                    {
                        length = string_8.IndexOf("//");
                    }
                    if (string_8.Contains("/*"))
                    {
                        index = string_8.IndexOf("/*");
                    }
                    if ((length <= -1) && (index <= -1))
                    {
                        builder.Append(string_8);
                        break;
                    }
                    if ((length > -1) && (((index > -1) && (length < index)) || (index == -1)))
                    {
                        builder.Append(string_8.Substring(0, length));
                        break;
                    }
                    if (index > -1)
                    {
                        builder.Append(string_8.Substring(0, index));
                        string_8 = string_8.Substring(index);
                        bool_0 = true;
                    }
                }
                else
                {
                    if (!string_8.Contains("*/"))
                    {
                        break;
                    }
                    int num2 = string_8.IndexOf("*/");
                    string_8 = string_8.Substring(num2 + 2);
                    bool_0 = false;
                }
            }
            return builder.ToString();
        }

        private string method_7(string string_8)
        {
            string_8 = string_8.Trim();
            while (string_8.Contains("\t"))
            {
                string_8 = string_8.Replace('\t', ' ');
            }
            while (string_8.Contains("  "))
            {
                string_8 = string_8.Replace("  ", " ");
            }
            return string_8;
        }

        private void method_8()
        {
            if (!base.DesignMode)
            {
                AssemblyLoader loader = new AssemblyLoader {
                    BaseClass = "IndicatorHelper",
                    Path = Path.GetDirectoryName(Application.ExecutablePath)
                };
                foreach (System.Type type in loader.Types)
                {
                    IndicatorHelper item = (IndicatorHelper) loader.CreateInstance(type);
                    this.list_8.Add(item);
                    System.Type indicatorType = item.IndicatorType;
                    this.list_7.Add(indicatorType);
                }
            }
        }

        private PlottedIndicator method_9(IndicatorDescriptor indicatorDescriptor_0)
        {
            DataSeries series = null;
            if (indicatorDescriptor_0.FundamentalItemName != "")
            {
                if (this.Fundamentals != null)
                {
                    series = this.Fundamentals.RequestSymbolDataSeries(this.chart_0.Bars, this.chart_0.Bars.Symbol, indicatorDescriptor_0.FundamentalItemName);
                    if (series == null)
                    {
                        series = this.Fundamentals.RequestNonSymbolDataSeries(this.chart_0.Bars, indicatorDescriptor_0.FundamentalItemName);
                    }
                }
            }
            else
            {
                object[] parameters = this.method_10(indicatorDescriptor_0);
                if (parameters == null)
                {
                    return null;
                }
                MethodInfo method = indicatorDescriptor_0.IndicatorType.GetMethod("Series");
                if (method == null)
                {
                    return null;
                }
                series = method.Invoke(null, parameters) as DataSeries;
            }
            PlottedIndicator indicator = new PlottedIndicator(this.chart_0.Renderer, series) {
                Color = indicatorDescriptor_0.Color,
                Style = indicatorDescriptor_0.Style,
                Width = indicatorDescriptor_0.Width,
                DragAndDrop = true,
                FundamentalItemName = indicatorDescriptor_0.FundamentalItemName
            };
            if (indicator.IsFundamental)
            {
                indicator.Style = LineStyle.Invisible;
            }
            indicatorDescriptor_0.PlottedIndicator = indicator;
            return indicator;
        }

        public void ProcessDroppedFundamentalItem(DraggedFundamentalItem draggedFundamental)
        {
            IndicatorParametersForm form = new IndicatorParametersForm();
            form.Initialize(draggedFundamental.ItemName);
            this.method_19("DrawFund." + draggedFundamental.ItemName + ".", form);
            if (form.ShowDialog(this.chart_0.FindForm()) == DialogResult.OK)
            {
                IndicatorDescriptor fundamentalDescriptor = form.GetFundamentalDescriptor();
                PlottedIndicator indicator = this.method_9(fundamentalDescriptor);
                if (indicator != null)
                {
                    this.method_17(form, indicator);
                    this.list_5.Add(fundamentalDescriptor);
                    string description = draggedFundamental.Provider.ItemPane(draggedFundamental.ItemName);
                    ChartPane pane = this.chart_0.Renderer.FindPane(description);
                    if (pane == null)
                    {
                        pane = new ChartPane(this.chart_0.Renderer, true, int_5, description);
                        PaneDescriptor item = new PaneDescriptor {
                            AbovePricePane = true,
                            Height = int_5,
                            Description = description
                        };
                        this.list_6.Add(item);
                    }
                    fundamentalDescriptor.PaneDescription = pane.Description;
                    pane.PlottedIndicators.Add(indicator);
                    this.method_11(fundamentalDescriptor, pane);
                    indicator.Style = LineStyle.Invisible;
                    this.chart_0.DoInvalidate();
                    if (this.eventHandler_0 != null)
                    {
                        this.eventHandler_0(this, new DroppedIndicatorEventArgs(fundamentalDescriptor));
                    }
                }
            }
        }

        public void ProcessDroppedIndicatorHelper(IndicatorHelper indHelper, ChartPane pane)
        {
            IndicatorParametersForm form = new IndicatorParametersForm();
            if (form.Initialize(indHelper, this.chart_0, pane))
            {
                this.method_18(indHelper.IndicatorType, form);
                if (form.ShowDialog(this.chart_0.FindForm()) == DialogResult.OK)
                {
                    IndicatorDescriptor indicatorDescriptor = form.GetIndicatorDescriptor();
                    PlottedIndicator indicator = this.method_9(indicatorDescriptor);
                    if (indicator != null)
                    {
                        this.method_17(form, indicator);
                        this.list_5.Add(indicatorDescriptor);
                        if (indHelper.TargetPane != "")
                        {
                            string targetPane = indHelper.TargetPane;
                            if (targetPane.StartsWith("?"))
                            {
                                targetPane = form.UseIndicatorPane ? targetPane.Substring(1) : pane.Description;
                            }
                            if (targetPane == "P")
                            {
                                pane = this.chart_0.Renderer.PricePane;
                            }
                            else if (targetPane == "V")
                            {
                                pane = this.chart_0.Renderer.VolumePane;
                            }
                            else
                            {
                                ChartPane pane2 = this.chart_0.Renderer.FindPane(targetPane);
                                if (pane2 == null)
                                {
                                    pane = new ChartPane(this.chart_0.Renderer, true, int_5, targetPane);
                                    PaneDescriptor item = new PaneDescriptor {
                                        AbovePricePane = true,
                                        Height = int_5,
                                        Description = targetPane
                                    };
                                    this.list_6.Add(item);
                                }
                                else
                                {
                                    pane = pane2;
                                }
                            }
                        }
                        indicatorDescriptor.PaneDescription = pane.Description;
                        pane.PlottedIndicators.Add(indicator);
                        this.method_11(indicatorDescriptor, pane);
                        this.chart_0.DoInvalidate();
                        if (this.eventHandler_0 != null)
                        {
                            this.eventHandler_0(this, new DroppedIndicatorEventArgs(indicatorDescriptor));
                        }
                    }
                }
            }
        }

        public string PushIndicatorsCode(string strategyCode)
        {
            string str;
            if (this.list_5.Count == 0)
            {
                MessageBox.Show("No drag-drop indicators found on the chart");
                return strategyCode;
            }
            try
            {
                strategyCode = this.method_4(strategyCode);
                if (string.IsNullOrEmpty(this.string_7))
                {
                    throw new InvalidDataException("Invalid strategy code: Class name not found");
                }
                this.string_6 = strategyCode;
                this.list_0.Clear();
                this.list_1.Clear();
                this.list_2.Clear();
                this.list_3.Clear();
                this.list_4.Clear();
                if (!strategyCode.Contains("using WealthLab.Indicators;"))
                {
                    this.list_0.Add("using WealthLab.Indicators;");
                }
                this.method_1();
                if (strategyCode.Contains("<#CTor_Statments>\n"))
                {
                    if (this.list_4.Count > 0)
                    {
                        string newValue = "\t\tpublic " + this.string_7 + "()\n\t\t{\n<#CreateParameter_Statments>\n\n\t\t}";
                        strategyCode = strategyCode.Replace("<#CTor_Statments>\n", newValue);
                    }
                    else
                    {
                        strategyCode = strategyCode.Replace("<#CTor_Statments>\n", "");
                    }
                }
                strategyCode = smethod_1(strategyCode, this.list_0, "<#Using_Statments>\n", "Pushed indicator using statements", "");
                strategyCode = smethod_1(strategyCode, this.list_1, "<#ChartPane_Statments>\n", "Pushed indicator ChartPane statements", "\t\t\t");
                strategyCode = smethod_1(strategyCode, this.list_2, "<#PlotSeries_Statments>\n", "Pushed indicator PlotSeries statements", "\t\t\t");
                strategyCode = smethod_1(strategyCode, this.list_3, "<#StrategyParameter_Statments>\n", "Pushed indicator StrategyParameter statements", "\t\t");
                strategyCode = smethod_1(strategyCode, this.list_4, "<#CreateParameter_Statments>\n", "Pushed indicator CreateParameter statements", "\t\t\t");
                return strategyCode;
            }
            catch (InvalidDataException exception)
            {
                MessageBox.Show(exception.Message);
                str = strategyCode;
            }
            return str;
        }

        public void ReadFromStream(Stream stream)
        {
            int num;
            BinaryReader reader = new BinaryReader(stream);
            this.Clear();
            for (num = reader.ReadInt32(); num > 0; num--)
            {
                PaneDescriptor item = new PaneDescriptor();
                item.Read(reader);
                this.list_6.Add(item);
            }
            for (num = reader.ReadInt32(); num > 0; num--)
            {
                IndicatorDescriptor descriptor = new IndicatorDescriptor(0);
                descriptor.method_0(reader, this.list_7);
                IndicatorHelper helper = this.method_16(descriptor);
                if (helper != null)
                {
                    descriptor.BandPairIndicatorType = helper.PartnerBandIndicatorType;
                    for (int i = 0; i < helper.ParameterDefaultValues.Count; i++)
                    {
                        if (((helper.ParameterDefaultValues[i] is Enum) && !(helper.ParameterDefaultValues[i] is CoreDataSeries)) && !(helper.ParameterDefaultValues[i] is BarDataType))
                        {
                            string str = descriptor.Parameters[i] as string;
                            descriptor.Parameters[i] = Enum.Parse(helper.ParameterDefaultValues[i].GetType(), str);
                        }
                    }
                }
                this.list_5.Add(descriptor);
            }
        }

        public void RemoveIndicator(PlottedIndicator plottedIndicator_1)
        {
            IndicatorDescriptor descriptor = this.method_14(plottedIndicator_1);
            if (descriptor != null)
            {
                foreach (IndicatorDescriptor descriptor2 in this.method_13(descriptor))
                {
                    this.list_5.Remove(descriptor2);
                }
                this.list_5.Remove(descriptor);
                this.method_20();
                string description = plottedIndicator_1.Series.Description;
                if (this.chart_0.Bars.Cache.ContainsKey(description))
                {
                    this.chart_0.Bars.Cache.Remove(description);
                }
                this.CreateDragDropIndicators();
            }
        }

        public void SaveToStream(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(this.list_6.Count);
            foreach (PaneDescriptor descriptor2 in this.list_6)
            {
                descriptor2.Write(writer);
            }
            writer.Write(this.list_5.Count);
            foreach (IndicatorDescriptor descriptor in this.list_5)
            {
                descriptor.method_1(writer);
            }
        }

        public static void SetIndicatorUIValues(IndicatorHelper helper, string indicatorString, Control control_0)
        {
            if (control_0.Controls.Count == 0)
            {
                CreateIndicatorParameterUI(helper, control_0, null, "Bars", false);
            }
            int index = indicatorString.IndexOf('(');
            indicatorString = indicatorString.Substring(index + 1);
            for (int i = indicatorString.Length - 1; i >= 0; i--)
            {
                if (indicatorString[i] == ')')
                {
                    indicatorString = indicatorString.Substring(0, i);
                    break;
                }
            }
            List<string> list = new List<string>();
            bool flag = false;
            string item = "";
            for (int j = 0; j < indicatorString.Length; j++)
            {
                if (flag)
                {
                    if (indicatorString[j] == '"')
                    {
                        flag = false;
                        list.Add(item);
                        item = "";
                        j++;
                    }
                    else
                    {
                        item = item + indicatorString[j];
                    }
                }
                else if (indicatorString[j] == '"')
                {
                    flag = true;
                    item = "";
                }
                else if (indicatorString[j] == ',')
                {
                    list.Add(item);
                    item = "";
                }
                else
                {
                    item = item + indicatorString[j];
                }
            }
            list.Add(item);
            int num3 = 0;
            foreach (object obj2 in helper.ParameterDefaultValues)
            {
                obj2.GetType();
                Control control = control_0.Controls[(num3 * 2) + 1];
                string str2 = list[num3];
                if (obj2 is Enum)
                {
                    index = str2.IndexOf('.');
                    if (index > 0)
                    {
                        str2 = str2.Substring(index + 1);
                    }
                    control.Text = str2;
                }
                else if (obj2 is bool)
                {
                    CheckBox box = control as CheckBox;
                    box.Checked = str2 == "true";
                }
                else
                {
                    control.Text = str2;
                }
                num3++;
            }
        }

        private static bool smethod_0(IndicatorHelper indicatorHelper_1, Control control_0, ChartPane chartPane_0, string string_8, bool bool_0, float float_0, int int_6)
        {
            indicatorHelper_0 = indicatorHelper_1;
            IList<string> parameterDescriptions = indicatorHelper_1.ParameterDescriptions;
            IList<object> parameterDefaultValues = indicatorHelper_1.ParameterDefaultValues;
            control_0.Controls.Clear();
            int x = (int) (118f * float_0);
            for (int i = 0; i < parameterDefaultValues.Count; i++)
            {
                Control control2;
                if ((indicatorHelper_1.ParameterDisplayNames != null) && (indicatorHelper_1.ParameterDisplayNames.Count > 0))
                {
                    LinkLabel label = new LinkLabel();
                    label.Click += new EventHandler(IndicatorDragDropManager.smethod_3);
                    label.Tag = i;
                    control2 = label;
                }
                else
                {
                    control2 = new Label();
                }
                control2.AutoSize = true;
                if ((indicatorHelper_1.ParameterDisplayNames != null) && (indicatorHelper_1.ParameterDisplayNames.Count > 0))
                {
                    control2.Text = indicatorHelper_1.ParameterDisplayNames[i];
                }
                else
                {
                    control2.Text = parameterDescriptions[i];
                }
                int y = (int) ((int_6 + (0x19 * i)) * float_0);
                control2.Location = new Point(7, y);
                control_0.Controls.Add(control2);
                System.Type enumType = parameterDefaultValues[i].GetType();
                Control control = null;
                if (enumType == typeof(CoreDataSeries))
                {
                    ComboBox box3 = new ComboBox {
                        Location = new Point(x, y),
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };
                    box3.Items.Add(CoreDataSeries.Close);
                    box3.Items.Add(CoreDataSeries.High);
                    box3.Items.Add(CoreDataSeries.Low);
                    box3.Items.Add(CoreDataSeries.Open);
                    if (bool_0)
                    {
                        box3.Items.Add(CoreDataSeries.Volume);
                    }
                    if (chartPane_0 != null)
                    {
                        foreach (PlottedIndicator indicator in chartPane_0.PlottedIndicators)
                        {
                            box3.Items.Add(indicator.Series.Description);
                        }
                    }
                    if (box3.Items.Count == 0)
                    {
                        return false;
                    }
                    box3.Text = parameterDefaultValues[i].ToString();
                    if (((chartPane_0 != null) && (chartPane_0.SelectedIndicator != null)) && box3.Items.Contains(chartPane_0.SelectedIndicator.Series.Description))
                    {
                        box3.Tag = "I";
                        box3.Text = chartPane_0.SelectedIndicator.Series.Description;
                    }
                    if (box3.SelectedIndex == -1)
                    {
                        box3.SelectedIndex = 0;
                    }
                    control = box3;
                }
                else if (enumType == typeof(BarDataType))
                {
                    ComboBox box5 = new ComboBox {
                        Location = new Point(x, y),
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };
                    BarsDescriptorString item = new BarsDescriptorString(string_8);
                    box5.Items.Add(item);
                    box5.SelectedIndex = 0;
                    control = box5;
                }
                else if (parameterDefaultValues[i] is Enum)
                {
                    ComboBox box = new ComboBox {
                        Location = new Point(x, y),
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };
                    foreach (string str in Enum.GetNames(enumType))
                    {
                        box.Items.Add(str);
                    }
                    Enum enum2 = parameterDefaultValues[i] as Enum;
                    box.SelectedIndex = box.Items.IndexOf(enum2.ToString());
                    control = box;
                }
                else if (enumType == typeof(int))
                {
                    NumericUpDown down4 = new NumericUpDown {
                        Maximum = 2147483647M,
                        Minimum = -2147483648M,
                        Location = new Point(x, y),
                        Value = (int) parameterDefaultValues[i]
                    };
                    control = down4;
                }
                else if (enumType == typeof(RangeBoundInt32))
                {
                    RangeBoundInt32 num7 = (RangeBoundInt32) parameterDefaultValues[i];
                    NumericUpDown down3 = new NumericUpDown {
                        Maximum = 2147483647M,
                        Minimum = -2147483648M,
                        Location = new Point(x, y),
                        Value = num7.Value
                    };
                    control = down3;
                }
                else if (enumType == typeof(double))
                {
                    NumericUpDown down = new NumericUpDown();
                    double num3 = (double) parameterDefaultValues[i];
                    down.DecimalPlaces = CalculateDecimalsFromDefaultValue(num3);
                    down.Maximum = 79228162514264337593543950335M;
                    down.Minimum = -79228162514264337593543950335M;
                    down.Location = new Point(x, y);
                    down.Value = (decimal) num3;
                    control = down;
                }
                else if (enumType == typeof(RangeBoundDouble))
                {
                    NumericUpDown down2 = new NumericUpDown();
                    RangeBoundDouble num6 = (RangeBoundDouble) parameterDefaultValues[i];
                    down2.DecimalPlaces = CalculateDecimalsFromDefaultValue(num6.MinimumValue);
                    down2.Maximum = 79228162514264337593543950335M;
                    down2.Minimum = -79228162514264337593543950335M;
                    down2.Location = new Point(x, y);
                    down2.Value = (decimal) num6.Value;
                    control = down2;
                }
                else if (enumType == typeof(string))
                {
                    TextBox box4 = new TextBox {
                        Location = new Point(x, y),
                        Text = (string) parameterDefaultValues[i]
                    };
                    control = box4;
                }
                else if (enumType == typeof(bool))
                {
                    CheckBox box2 = new CheckBox {
                        AutoSize = true,
                        Location = control2.Location
                    };
                    control2.Visible = false;
                    box2.Text = parameterDescriptions[i];
                    box2.Checked = (bool) parameterDefaultValues[i];
                    control = box2;
                }
                else
                {
                    if (enumType != typeof(DateTime))
                    {
                        throw new InvalidOperationException("Invalid indicator parameter type: " + enumType.Name);
                    }
                    DateTimePicker picker = new DateTimePicker {
                        Location = new Point(x, y)
                    };
                    DateTime time = (DateTime) parameterDefaultValues[i];
                    if (time.Date == new DateTime(0, 0, 0))
                    {
                        picker.Format = DateTimePickerFormat.Time;
                        picker.ShowUpDown = true;
                    }
                    else
                    {
                        picker.Format = DateTimePickerFormat.Short;
                    }
                    picker.Value = time;
                    control = picker;
                }
                control_0.Controls.Add(control);
            }
            return true;
        }

        private static string smethod_1(string string_8, List<string> list_9, string string_9, string string_10, string string_11)
        {
            StringBuilder builder = new StringBuilder();
            if (list_9.Count > 0)
            {
                if (!string.IsNullOrEmpty(string_10))
                {
                    builder.AppendLine(string_11 + "//" + string_10);
                }
                foreach (string str in list_9)
                {
                    builder.AppendLine(string_11 + str);
                }
            }
            return string_8.Replace(string_9, builder.ToString());
        }

        private static string smethod_2(Color color_0)
        {
            if (color_0.IsKnownColor)
            {
                string str = color_0.ToString();
                int index = str.IndexOf('[');
                str = str.Substring(index + 1);
                return ("Color." + str.Substring(0, str.Length - 1));
            }
            return string.Concat(new object[] { "Color.FromArgb(", color_0.A, ",", color_0.R, ",", color_0.G, ",", color_0.B, ")" });
        }

        private static void smethod_3(object sender, EventArgs e)
        {
            LinkLabel label = sender as LinkLabel;
            string str = InputBox.Show("Rename Parameter", "Parameter Name", label.Text);
            if (str != "")
            {
                label.Text = str;
                int tag = (int) label.Tag;
                indicatorHelper_0.ParameterDisplayNames[tag] = label.Text;
            }
        }

        public FundamentalsLoader Fundamentals
        {
            get
            {
                return this.fundamentalsLoader_0;
            }
            set
            {
                this.fundamentalsLoader_0 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public bool HasDragDroppedIndicators
        {
            get
            {
                return (this.list_5.Count > 0);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public PlottedIndicator SelectedIndicator
        {
            get
            {
                return this.plottedIndicator_0;
            }
            set
            {
                this.plottedIndicator_0 = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public ISettingsHost SettingsHost
        {
            get
            {
                return this.isettingsHost_0;
            }
            set
            {
                this.isettingsHost_0 = value;
            }
        }
    }
}

