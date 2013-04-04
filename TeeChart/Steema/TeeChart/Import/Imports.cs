namespace Steema.TeeChart.Import
{
    using Steema.TeeChart;
    using Steema.TeeChart.Editors;
    using Steema.TeeChart.Export;
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    public class Imports
    {
        protected Chart chart;
        private TemplateImport template;
        private ThemeImport theme;
        private ArrayList visitedCustomObjects;

        public Imports()
        {
            this.visitedCustomObjects = new ArrayList();
        }

        public Imports(Chart c)
        {
            this.visitedCustomObjects = new ArrayList();
            this.chart = c;
        }

        public MemoryStream DecodeBase64(string base64String)
        {
            byte[] buffer;
            MemoryStream stream = new MemoryStream {
                Position = 0L
            };
            try
            {
                buffer = Convert.FromBase64String(base64String);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
            }
            catch (Exception exception)
            {
                Console.WriteLine("{0}", exception.Message);
            }
            buffer = null;
            stream.Position = 0L;
            return stream;
        }

        public virtual void DeserializeFrom(SerializationInfo info, StreamingContext context)
        {
            Map map = null;
            OrgSeries series = null;
            DrawLine line = null;
            SubChartTool tool = null;
            int num = -1;
            ArrayList list = new ArrayList();
            new ArrayList();
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                string name = current.Name;
                if (!name.StartsWith("."))
                {
                    int index = name.IndexOf(".");
                    if (index >= 0)
                    {
                        string str2 = name.Substring(0, index);
                        name = name.Remove(0, index);
                        if (TypeDescriptor.GetProperties(this.chart).Find(str2, true) != null)
                        {
                            string typeName = current.Value.ToString();
                            Type type = this.FindType(typeName);
                            object[] args = new object[] { this.chart };
                            object obj2 = null;
                            if (((type != typeof(PolygonSeries)) && (type != typeof(OrgItem))) && ((type != typeof(DrawLineItem)) && (type.GetInterface("IWorldMaps") == null)))
                            {
                                obj2 = Activator.CreateInstance(type, args);
                            }
                            if (type == typeof(Axis))
                            {
                                this.chart.Axes.Custom.Add((Axis) obj2);
                            }
                            if (type == typeof(Polygon))
                            {
                                list.Add(obj2 as Polygon);
                            }
                            if (type == typeof(SubChart))
                            {
                                list.Add(obj2 as SubChart);
                            }
                            if (type == typeof(OrgItem))
                            {
                                if (series != null)
                                {
                                    args = new object[] { series.Items };
                                    obj2 = Activator.CreateInstance(type, args);
                                }
                                list.Add(obj2 as OrgItem);
                            }
                            if (type.GetInterface("IWorldMaps") != null)
                            {
                                args = new object[] { this.chart, bool.TrueString };
                                obj2 = Activator.CreateInstance(type, args);
                                map = obj2 as Map;
                            }
                            if (type == typeof(DrawLine))
                            {
                                line = obj2 as DrawLine;
                            }
                            if (type == typeof(DrawLineItem))
                            {
                                if (line != null)
                                {
                                    args = new object[] { line };
                                    obj2 = Activator.CreateInstance(type, args);
                                }
                                if (name.Contains("Selected"))
                                {
                                    num = list.Count - 1;
                                }
                                else
                                {
                                    list.Add(obj2 as DrawLineItem);
                                }
                            }
                            if (type == typeof(Map))
                            {
                                map = obj2 as Map;
                            }
                            if (type == typeof(OrgSeries))
                            {
                                series = obj2 as OrgSeries;
                            }
                            if (type == typeof(SubChartTool))
                            {
                                tool = obj2 as SubChartTool;
                            }
                        }
                    }
                }
            }
            if (list.Count > 0)
            {
                if (list[0] is Polygon)
                {
                    foreach (Polygon polygon in list)
                    {
                        polygon.ParentSeries = map;
                        polygon.ParentSeries.Add((double) 0.0, (double) 0.0);
                        map.Shapes.Add(polygon);
                    }
                }
                else if (list[0] is OrgItem)
                {
                    foreach (OrgItem item in list)
                    {
                        series.Items.Add(item);
                    }
                }
                else if (list[0] is SubChart)
                {
                    foreach (SubChart chart in list)
                    {
                        tool.Charts.Add(chart);
                    }
                }
                else if (list[0] is DrawLineItem)
                {
                    foreach (DrawLineItem item2 in list)
                    {
                        line.Lines.Add(item2);
                    }
                    if (num != -1)
                    {
                        line.Selected = list[num] as DrawLineItem;
                    }
                }
                list.Clear();
            }
            enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry entry2 = enumerator.Current;
                bool flag = true;
                string str4 = entry2.Name;
                if (str4.StartsWith("."))
                {
                    str4 = str4.Remove(0, 1);
                    object component = this.chart;
                    do
                    {
                        string str5;
                        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
                        int length = str4.IndexOf(".");
                        if (length >= 0)
                        {
                            str5 = str4.Substring(0, length);
                            PropertyDescriptor descriptor2 = properties.Find(str5, true);
                            if (descriptor2 == null)
                            {
                                if (component is SeriesCollection)
                                {
                                    component = ((SeriesCollection) component)[Convert.ToInt32(str5)];
                                }
                                else if (component is CustomAxes)
                                {
                                    component = ((CustomAxes) component)[Convert.ToInt32(str5)];
                                }
                                else if (component is MarksItems)
                                {
                                    component = ((MarksItems) component)[Convert.ToInt32(str5)];
                                }
                                else if (component is PolygonList)
                                {
                                    component = ((PolygonList) component)[Convert.ToInt32(str5)];
                                }
                                else if (component is ToolsCollection)
                                {
                                    component = ((ToolsCollection) component)[Convert.ToInt32(str5)];
                                }
                                else if (component is ChartCollection)
                                {
                                    component = ((ChartCollection) component)[Convert.ToInt32(str5)];
                                }
                                else if (component is DrawLines)
                                {
                                    component = ((DrawLines) component)[Convert.ToInt32(str5)];
                                }
                                if ((component is TemplateExport.ICustomSerialization) && (this.visitedCustomObjects.IndexOf(component) == -1))
                                {
                                    (component as TemplateExport.ICustomSerialization).DeSerialize(info);
                                    this.visitedCustomObjects.Add(component);
                                }
                            }
                            else
                            {
                                if (str5 != "DataSource")
                                {
                                    component = descriptor2.GetValue(component);
                                }
                                if ((component is TemplateExport.ICustomSerialization) && (this.visitedCustomObjects.IndexOf(component) == -1))
                                {
                                    (component as TemplateExport.ICustomSerialization).DeSerialize(info);
                                    this.visitedCustomObjects.Add(component);
                                }
                            }
                            str4 = str4.Remove(0, length + 1);
                            if (component == null)
                            {
                                flag = false;
                            }
                        }
                        else
                        {
                            str5 = str4;
                            PropertyDescriptor pd = properties.Find(str5, true);
                            if (pd != null)
                            {
                                if (pd.PropertyType == typeof(object))
                                {
                                    if (entry2.Value is Array)
                                    {
                                        object[] objArray2 = new object[((Array) entry2.Value).Length];
                                        int num4 = 0;
                                        foreach (object obj4 in (Array) entry2.Value)
                                        {
                                            objArray2.SetValue(this.GetLinkObject(obj4), num4);
                                            num4++;
                                        }
                                        this.SetValueToSerialization(pd, component, objArray2);
                                    }
                                    else if (entry2.Value.GetType() == typeof(string))
                                    {
                                        this.SetValueToSerialization(pd, component, this.GetLinkObject(entry2.Value));
                                    }
                                }
                                else if (pd.PropertyType == typeof(Axis))
                                {
                                    if (entry2.Value.GetType() == typeof(string))
                                    {
                                        this.SetValueToSerialization(pd, component, this.GetLinkObject(entry2.Value));
                                    }
                                }
                                else if (pd.PropertyType == typeof(Series))
                                {
                                    if (entry2.Value.GetType() == typeof(string))
                                    {
                                        this.SetValueToSerialization(pd, component, this.GetLinkObject(entry2.Value));
                                    }
                                }
                                else if (pd.PropertyType == typeof(Tool))
                                {
                                    if (entry2.Value.GetType() == typeof(string))
                                    {
                                        this.SetValueToSerialization(pd, component, this.GetLinkObject(entry2.Value));
                                    }
                                }
                                else if (pd.PropertyType == typeof(FastLine))
                                {
                                    if (entry2.Value.GetType() == typeof(string))
                                    {
                                        this.SetValueToSerialization(pd, component, this.GetLinkObject(entry2.Value));
                                    }
                                }
                                else if (pd.PropertyType == typeof(Volume))
                                {
                                    if (entry2.Value.GetType() == typeof(string))
                                    {
                                        this.SetValueToSerialization(pd, component, this.GetLinkObject(entry2.Value));
                                    }
                                }
                                else if (pd.PropertyType == typeof(StringList))
                                {
                                    StringList list2 = new StringList();
                                    if (entry2.Value is StringList)
                                    {
                                        list2.AddRange((StringList) entry2.Value);
                                    }
                                    else
                                    {
                                        object[] objArray3 = (object[]) entry2.Value;
                                        string[] array = new string[objArray3.Length];
                                        objArray3.CopyTo(array, 0);
                                        list2.AddRange(array);
                                    }
                                    if (list2.Count > 0)
                                    {
                                        this.SetValueToSerialization(pd, component, list2);
                                    }
                                }
                                else if (pd.PropertyType == typeof(ColorList))
                                {
                                    ColorList list3 = new ColorList();
                                    if (entry2.Value is ColorList)
                                    {
                                        list3.AddRange((ColorList) entry2.Value);
                                    }
                                    else
                                    {
                                        object[] objArray4 = (object[]) entry2.Value;
                                        Color[] colorArray = new Color[objArray4.Length];
                                        objArray4.CopyTo(colorArray, 0);
                                        list3.AddRange(colorArray);
                                    }
                                    if (list3.Count > 0)
                                    {
                                        this.SetValueToSerialization(pd, component, list3);
                                    }
                                }
                                else if (pd.PropertyType == typeof(ArrayList))
                                {
                                    ArrayList list4 = new ArrayList(((Array) entry2.Value).Length);
                                    list4.AddRange((Array) entry2.Value);
                                    if (list4.Count > 0)
                                    {
                                        this.SetValueToSerialization(pd, component, list4);
                                    }
                                }
                                else if (pd.PropertyType == typeof(Image))
                                {
                                    this.SetValueToSerialization(pd, component, entry2.Value);
                                }
                                else if (pd.PropertyType == entry2.ObjectType)
                                {
                                    this.SetValueToSerialization(pd, component, entry2.Value);
                                }
                                else if (pd.PropertyType == typeof(Function))
                                {
                                    Type f = this.FindType(entry2.Value.ToString());
                                    if (f == typeof(ADXFunction))
                                    {
                                        int num5 = this.chart.series.IndexOf(component as Series);
                                        if (this.chart.series.Count > (num5 + 2))
                                        {
                                            FastLine line2 = (FastLine) this.chart.series[num5 + 1];
                                            FastLine line3 = (FastLine) this.chart.series[num5 + 2];
                                            object[] initArgs = new object[] { line2, line3 };
                                            this.SetValueToSerialization(pd, component, Function.NewInstance(f, initArgs));
                                        }
                                        else
                                        {
                                            this.SetValueToSerialization(pd, component, Function.NewInstance(f));
                                        }
                                    }
                                    else if (f == typeof(Bollinger))
                                    {
                                        int num6 = this.chart.series.IndexOf(component as Series);
                                        if (this.chart.series.Count > (num6 + 1))
                                        {
                                            FastLine line4 = (FastLine) this.chart.series[num6 + 1];
                                            object[] objArray6 = new object[] { line4 };
                                            this.SetValueToSerialization(pd, component, Function.NewInstance(f, objArray6));
                                        }
                                        else
                                        {
                                            this.SetValueToSerialization(pd, component, Function.NewInstance(f));
                                        }
                                    }
                                    else if (f == typeof(MACDFunction))
                                    {
                                        int num7 = this.chart.series.IndexOf(component as Series);
                                        if (this.chart.series.Count > (num7 + 2))
                                        {
                                            FastLine line5 = (FastLine) this.chart.series[num7 + 1];
                                            Volume volume = (Volume) this.chart.series[num7 + 2];
                                            object[] objArray7 = new object[] { line5, volume };
                                            this.SetValueToSerialization(pd, component, Function.NewInstance(f, objArray7));
                                        }
                                        else
                                        {
                                            this.SetValueToSerialization(pd, component, Function.NewInstance(f));
                                        }
                                    }
                                    else
                                    {
                                        this.SetValueToSerialization(pd, component, Function.NewInstance(f));
                                    }
                                }
                                else if ((pd.PropertyType != typeof(FastLine)) && (entry2.Value.GetType() != typeof(string)))
                                {
                                    this.SetValueToSerialization(pd, component, Convert.ChangeType(entry2.Value, pd.PropertyType));
                                }
                            }
                            else if (component is Steema.TeeChart.Styles.ValueList)
                            {
                                ((Steema.TeeChart.Styles.ValueList) component).Deserialize(str5, entry2.Value);
                            }
                            flag = false;
                        }
                    }
                    while (flag);
                }
            }
            if (this.chart is TemplateExport.ICustomSerialization)
            {
                (this.chart as TemplateExport.ICustomSerialization).DeSerialize(info);
            }
        }

        private Type FindType(string typeName)
        {
            Type type = this.InternalFindType(typeName, base.GetType().Assembly);
            if (type == null)
            {
                type = this.InternalFindType(typeName, Assembly.GetExecutingAssembly());
            }
            if (type == null)
            {
                type = this.InternalFindType(typeName, Assembly.GetEntryAssembly());
            }
            if (type == null)
            {
                foreach (AssemblyName name in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
                {
                    type = this.InternalFindType(typeName, Assembly.Load(name));
                    if (type != null)
                    {
                        break;
                    }
                }
            }
            if (type == null)
            {
                foreach (AssemblyName name2 in Assembly.GetEntryAssembly().GetReferencedAssemblies())
                {
                    type = this.InternalFindType(typeName, Assembly.Load(name2));
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            return type;
        }

        private object GetLinkObject(object o)
        {
            string str = o.ToString();
            int index = str.IndexOf(".");
            if (index >= 0)
            {
                string str2 = str.Substring(0, index).ToUpper();
                int num2 = Convert.ToInt32(str.Remove(0, index + 1));
                switch (str2)
                {
                    case "SERIES":
                        return this.chart.series[num2];

                    case "AXIS":
                        return this.chart.axes[num2];

                    case "CUSTOMAXES":
                        return this.chart.axes.custom[num2];
                }
            }
            return null;
        }

        private Type InternalFindType(string typeName, Assembly a)
        {
            foreach (Type type in a.GetExportedTypes())
            {
                if (type.FullName == typeName)
                {
                    return type;
                }
            }
            return null;
        }

        public void InternalLoadViewState(MemoryStream savedState, ref Chart chart)
        {
            chart = chart.Import.Template.Load(savedState);
            savedState.Position = 0L;
            savedState.Flush();
            savedState.Close();
        }

        private void SetValueToSerialization(PropertyDescriptor pd, object component, object value)
        {
            pd.SetValue(component, value);
        }

        public void ShowImportDialog()
        {
            ImportEditor.ShowModal(this.chart);
        }

        private void testMethod(ref Type t)
        {
            t = base.GetType();
        }

        public TemplateImport Template
        {
            get
            {
                if (this.template == null)
                {
                    this.template = new TemplateImport(this.chart);
                }
                return this.template;
            }
        }

        public ThemeImport Theme
        {
            get
            {
                if (this.theme == null)
                {
                    this.theme = new ThemeImport(this.chart);
                }
                return this.theme;
            }
        }

        public class MyBinder : Binder
        {
            public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
            {
                if (match == null)
                {
                    throw new ArgumentNullException("match");
                }
                for (int i = 0; i < match.Length; i++)
                {
                    if (this.ChangeType(value, match[i].FieldType, culture) != null)
                    {
                        return match[i];
                    }
                }
                return null;
            }

            public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state)
            {
                BinderState state2 = new BinderState();
                object[] array = new object[args.Length];
                args.CopyTo(array, 0);
                state2.args = array;
                state = state2;
                if (match == null)
                {
                    throw new ArgumentNullException();
                }
                for (int i = 0; i < match.Length; i++)
                {
                    int num2 = 0;
                    ParameterInfo[] parameters = match[i].GetParameters();
                    if (args.Length == parameters.Length)
                    {
                        for (int j = 0; j < args.Length; j++)
                        {
                            if (names != null)
                            {
                                if (names.Length != args.Length)
                                {
                                    throw new ArgumentException("names and args must have the same number of elements.");
                                }
                                for (int k = 0; k < names.Length; k++)
                                {
                                    if (string.Compare(parameters[j].Name, names[k].ToString()) == 0)
                                    {
                                        args[j] = state2.args[k];
                                    }
                                }
                            }
                            if (this.ChangeType(args[j], parameters[j].ParameterType, culture) == null)
                            {
                                break;
                            }
                            num2++;
                        }
                        if (num2 == args.Length)
                        {
                            return match[i];
                        }
                    }
                }
                return null;
            }

            private bool CanConvertFrom(Type type1, Type type2)
            {
                return type1.Equals(type2);
            }

            public override object ChangeType(object value, Type myChangeType, CultureInfo culture)
            {
                if (this.CanConvertFrom(value.GetType(), myChangeType))
                {
                    return Convert.ChangeType(value, myChangeType);
                }
                return null;
            }

            public override void ReorderArgumentArray(ref object[] args, object state)
            {
                ((BinderState) state).args.CopyTo(args, 0);
            }

            public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
            {
                if (match == null)
                {
                    throw new ArgumentNullException("match");
                }
                for (int i = 0; i < match.Length; i++)
                {
                    int num2 = 0;
                    ParameterInfo[] parameters = match[i].GetParameters();
                    if (types.Length == parameters.Length)
                    {
                        for (int j = 0; j < types.Length; j++)
                        {
                            if (!this.CanConvertFrom(types[j], parameters[j].ParameterType))
                            {
                                break;
                            }
                            num2++;
                        }
                        if (num2 == types.Length)
                        {
                            return match[i];
                        }
                    }
                }
                return null;
            }

            public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
            {
                if (match == null)
                {
                    throw new ArgumentNullException("match");
                }
                for (int i = 0; i < match.Length; i++)
                {
                    int num2 = 0;
                    ParameterInfo[] indexParameters = match[i].GetIndexParameters();
                    if (indexes.Length == indexParameters.Length)
                    {
                        for (int j = 0; j < indexes.Length; j++)
                        {
                            if (!this.CanConvertFrom(indexes[j], indexParameters[j].ParameterType))
                            {
                                break;
                            }
                            num2++;
                        }
                        if ((num2 == indexes.Length) && this.CanConvertFrom(returnType, match[i].PropertyType))
                        {
                            return match[i];
                        }
                    }
                }
                return null;
            }

            private class BinderState
            {
                public object[] args;
            }
        }
    }
}

