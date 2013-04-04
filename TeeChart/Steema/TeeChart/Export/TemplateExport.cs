namespace Steema.TeeChart.Export
{
    using Steema.TeeChart;
    using Steema.TeeChart.Data;
    using Steema.TeeChart.Functions;
    using Steema.TeeChart.Styles;
    using Steema.TeeChart.Tools;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;

    public sealed class TemplateExport : ExportFormat
    {
        internal Chart chart;
        private bool includeData = true;

        public TemplateExport(Chart c)
        {
            this.chart = c;
            base.FileExtension = Texts.TeeFilesExtension;
        }

        private void AddValueToSerialization(SerializationInfo info, string s, object o)
        {
            this.AddValueToSerialization(info, s, o, o.GetType());
        }

        private void AddValueToSerialization(SerializationInfo info, string s, object o, Type type)
        {
            info.AddValue(s, o, type);
        }

        internal static string FileFilter()
        {
            return (Texts.TeeFiles + " (" + Texts.TeeFilesExtension + ")|*." + Texts.TeeFilesExtension);
        }

        internal override string FilterFiles()
        {
            return (Texts.TeeFiles + " (" + Texts.TeeFilesExtension + ")|*." + Texts.TeeFilesExtension);
        }

        public void Save(Stream stream)
        {
            this.Serialize(stream);
        }

        public void Save(string FileName)
        {
            using (FileStream stream = new FileStream(FileName, FileMode.Create))
            {
                this.Save(stream);
                stream.Flush();
                stream.Close();
            }
        }

        public void Serialize(Stream stream)
        {
            new BinaryFormatter { AssemblyFormat = FormatterAssemblyStyle.Simple }.Serialize(stream, this.chart);
        }

        public void Serialize(SerializationInfo info, StreamingContext context)
        {
            this.SerializeObject("", this.chart, info);
        }

        private void SerializeObject(string Prefix, object value, SerializationInfo info)
        {
            if (value != null)
            {
                if (value is CollectionBase)
                {
                    CollectionBase base2 = (CollectionBase) value;
                    Prefix = Prefix.Remove(0, 1);
                    int num = 0;
                    foreach (object obj2 in base2)
                    {
                        string s = Prefix + "." + num.ToString();
                        this.AddValueToSerialization(info, s, obj2.GetType().ToString());
                        this.SerializeObject("." + s, obj2, info);
                        num++;
                    }
                }
                else if (value is Array)
                {
                    this.AddValueToSerialization(info, Prefix, value);
                }
                else if (value is StringList)
                {
                    if (this.IncludeData)
                    {
                        this.AddValueToSerialization(info, Prefix, value);
                    }
                }
                else if (value is ColorList)
                {
                    this.AddValueToSerialization(info, Prefix, value);
                }
                else if (value is Image)
                {
                    this.AddValueToSerialization(info, Prefix, value);
                }
                else
                {
                    if ((value is Steema.TeeChart.Styles.ValueList) && !this.IncludeData)
                    {
                        return;
                    }
                    if (value is Steema.TeeChart.Styles.ValueList)
                    {
                        Steema.TeeChart.Styles.ValueList list = (Steema.TeeChart.Styles.ValueList) value;
                        list.Trim();
                        this.AddValueToSerialization(info, Prefix + ".Value", list.Value);
                        this.AddValueToSerialization(info, Prefix + ".Count", list.Count);
                    }
                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(value))
                    {
                        this.SerializeProperty(Prefix, value, descriptor, info);
                    }
                }
                if (value is ICustomSerialization)
                {
                    (value as ICustomSerialization).Serialize(info);
                }
            }
        }

        private void SerializeProperty(string Title, object value, PropertyDescriptor i, SerializationInfo info)
        {
            if (i.ShouldSerializeValue(value) && (i.SerializationVisibility != DesignerSerializationVisibility.Hidden))
            {
                string s = Title + "." + i.DisplayName;
                if (i.PropertyType == typeof(string))
                {
                    object o = i.GetValue(value);
                    this.AddValueToSerialization(info, s, o, o.GetType());
                }
                else if (i.PropertyType.IsClass)
                {
                    object obj3 = i.GetValue(value);
                    if (obj3 is Series)
                    {
                        if (obj3 is PolygonSeries)
                        {
                            this.AddValueToSerialization(info, s.Remove(0, 1), obj3.GetType().ToString());
                            this.SerializeObject(s, obj3, info);
                        }
                        else
                        {
                            int index = this.chart.series.IndexOf((Series) obj3);
                            if (index >= 0)
                            {
                                this.AddValueToSerialization(info, s, "Series." + index);
                            }
                        }
                    }
                    else if (obj3 is DrawLineItem)
                    {
                        this.AddValueToSerialization(info, s.Remove(0, 1), obj3.GetType().ToString());
                        this.SerializeObject(s, obj3, info);
                    }
                    else if ((value is Tool) && (obj3 is Axis))
                    {
                        this.AddValueToSerialization(info, s, "Axis." + this.chart.axes.IndexOf((Axis) obj3));
                    }
                    else if ((value is Series) && (obj3 is Axis))
                    {
                        this.AddValueToSerialization(info, s, "CustomAxes." + this.chart.axes.IndexOf((Axis) obj3));
                    }
                    else if (obj3 is MarksItems)
                    {
                        this.SerializeObject(s, obj3, info);
                    }
                    else if (obj3 is Steema.TeeChart.Functions.Function)
                    {
                        this.AddValueToSerialization(info, s, obj3.GetType().ToString());
                        this.SerializeObject(s, i.GetValue(value), info);
                    }
                    else if ((obj3 is object[]) && (i.Name == "DataSource"))
                    {
                        string[] strArray = new string[((object[]) obj3).Length];
                        Array array = (Array) obj3;
                        int num2 = 0;
                        foreach (object obj4 in array)
                        {
                            strArray.SetValue("Series." + this.chart.Series.IndexOf((Series) obj4).ToString(), num2);
                            num2++;
                        }
                        this.AddValueToSerialization(info, s, strArray);
                    }
                    else if (!(obj3 is SeriesSource) && !(obj3 is DataSet))
                    {
                        this.SerializeObject(s, i.GetValue(value), info);
                    }
                }
                else
                {
                    object obj5 = i.GetValue(value);
                    this.AddValueToSerialization(info, s, obj5, obj5.GetType());
                }
            }
        }

        public bool IncludeData
        {
            get
            {
                return this.includeData;
            }
            set
            {
                if (this.includeData != value)
                {
                    this.includeData = value;
                }
            }
        }

        public interface ICustomSerialization
        {
            void DeSerialize(SerializationInfo info);
            void Serialize(SerializationInfo info);
        }
    }
}

