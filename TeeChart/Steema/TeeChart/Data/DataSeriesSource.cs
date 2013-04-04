namespace Steema.TeeChart.Data
{
    using Steema.TeeChart.Styles;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Web.UI;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public abstract class DataSeriesSource : SeriesSource
    {
        private static List<object> bindingSources = new List<object>();
        private static List<object> dataSets = new List<object>();
        internal static bool HasBindingSource = false;
        private static List<object> tableAdapters = new List<object>();

        protected DataSeriesSource()
        {
        }

        private static void AddColumns(DataColumnCollection co, ListControl combo, ArrayList combos)
        {
            foreach (DataColumn column in co)
            {
                if (combo is ListBox)
                {
                    ((ListBox) combo).Items.Add(column.ColumnName);
                }
                else if (combo is ComboBox)
                {
                    ((ComboBox) combo).Items.Add(column.ColumnName);
                }
                if (combos != null)
                {
                    foreach (ComboBox box in combos)
                    {
                        box.Items.Add(column.ColumnName);
                    }
                }
            }
        }

        private static void AddFromAdapter(Series s, IDataAdapter d)
        {
            DataSet dataSet = new DataSet();
            d.Fill(dataSet);
            if (dataSet.Tables.Count > 0)
            {
                AddTable(s, dataSet.Tables[0]);
            }
        }

        private static void AddFromBindingSource(BindingSource bindingSource, Series s)
        {
            DataTable tableFromBindingSource = GetTableFromBindingSource(bindingSource);
            if (tableFromBindingSource != null)
            {
                AddTable(s, tableFromBindingSource);
            }
        }

        private static void AddTable(Series s, DataTable t)
        {
            s.BeginUpdate();
            s.Clear();
            s.Add(t);
            s.EndUpdate();
        }

        internal static void CheckSeriesSource(Series s, ComboBox c)
        {
            ArrayList list = new ArrayList();
            DataTable table = null;
            if (s.DataSource is DataView)
            {
                DataView dataSource = (DataView) s.DataSource;
                if (dataSource.Table != null)
                {
                    table = dataSource.Table;
                }
                list.Add(table);
            }
            else if (s.DataSource is DataTable)
            {
                table = (DataTable) s.DataSource;
                list.Add(table);
            }
            else if (s.DataSource is DataSet)
            {
                DataSet set = (DataSet) s.DataSource;
                foreach (DataTable table2 in set.Tables)
                {
                    list.Add(table2);
                }
            }
            else if (s.DataSource is IDbDataAdapter)
            {
                IDbDataAdapter adapter = (IDbDataAdapter) s.DataSource;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                foreach (DataTable table3 in dataSet.Tables)
                {
                    list.Add(table3);
                }
            }
            foreach (DataTable table4 in list)
            {
                foreach (object obj2 in c.Items)
                {
                    if (table4 == obj2)
                    {
                        c.SelectedItem = obj2;
                        break;
                    }
                }
            }
        }

        internal static void FillComponentLists(Series s)
        {
            IContainer chartContainer = null;
            chartContainer = s.chart.ChartContainer;
            if (chartContainer != null)
            {
                foreach (object obj2 in chartContainer.Components)
                {
                    if (obj2.GetType().Name.Contains("TableAdapter"))
                    {
                        tableAdapters.Add(obj2);
                    }
                    else if (obj2 is BindingSource)
                    {
                        bindingSources.Add(obj2);
                        if ((obj2 as BindingSource).DataSource is DataSet)
                        {
                            dataSets.Add((obj2 as BindingSource).DataSource);
                        }
                        HasBindingSource = true;
                    }
                }
            }
        }

        internal static void FillFields(object source, ListControl combo, ArrayList combos)
        {
            if (source is BindingSource)
            {
                DataTable tableFromBindingSource = GetTableFromBindingSource(source as BindingSource);
                if (tableFromBindingSource != null)
                {
                    AddColumns(tableFromBindingSource.Columns, combo, combos);
                }
            }
            if (source is IDataReader)
            {
                AddColumns((source as IDataReader).GetSchemaTable().Columns, combo, combos);
            }
            else if (source is DataAdapter)
            {
                DataAdapter adapter = source as DataAdapter;
                DataSet dataSet = new DataSet();
                DataTable[] tableArray = adapter.FillSchema(dataSet, SchemaType.Mapped);
                if (tableArray.Length > 0)
                {
                    AddColumns(tableArray[0].Columns, combo, combos);
                }
            }
            else if (source is DataSet)
            {
                DataTableCollection tables = (source as DataSet).Tables;
                if (tables.Count > 0)
                {
                    AddColumns(tables[0].Columns, combo, combos);
                }
            }
            else if (source is DataTable)
            {
                AddColumns(((DataTable) source).Columns, combo, combos);
            }
            else if (source is DataView)
            {
                AddColumns((source as DataView).Table.Columns, combo, combos);
            }
        }

        internal static void FillSeries(Series s, IDataReader r)
        {
            if (!r.IsClosed)
            {
                int ordinal;
                if (s.mandatory.DataMember.Length == 0)
                {
                    ordinal = 0;
                }
                else
                {
                    ordinal = r.GetOrdinal(s.mandatory.DataMember);
                }
                while (r.Read())
                {
                    if (r.IsDBNull(ordinal))
                    {
                        s.Add();
                    }
                    else
                    {
                        double d = Convert.ToDouble(r.GetValue(ordinal));
                        if (double.IsInfinity(d) || double.IsNaN(d))
                        {
                            s.Add();
                            continue;
                        }
                        s.Add(d);
                    }
                }
            }
        }

        internal static void FillSources(Series s, ComboBox c)
        {
            IContainer chartContainer = null;
            c.Items.Clear();
            chartContainer = s.chart.ChartContainer;
            FillComponentLists(s);
            if (chartContainer != null)
            {
                foreach (object obj2 in chartContainer.Components)
                {
                    if (IsValidSource(obj2))
                    {
                        if (HasBindingSource && (obj2 is BindingSource))
                        {
                            c.Items.Add(obj2);
                        }
                        else if (((obj2 is DataSet) && (((DataSet) obj2).Tables.Count > 0)) && !HasBindingSource)
                        {
                            foreach (DataTable table in ((DataSet) obj2).Tables)
                            {
                                if (c.Items.IndexOf(table) == -1)
                                {
                                    c.Items.Add(table);
                                }
                            }
                        }
                        else if (!HasBindingSource)
                        {
                            c.Items.Add(obj2);
                        }
                    }
                }
            }
            else
            {
                FillSourcesReflection(s, c);
            }
        }

        private static void FillSourcesReflection(Series s, ComboBox c)
        {
            if (s.chart.parent != null)
            {
                object obj2 = s.Chart.parent.FindParentForm();
                if (obj2 != null)
                {
                    System.Type baseType = obj2.GetType();
                    do
                    {
                        foreach (FieldInfo info in baseType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                        {
                            object obj3 = info.GetValue(obj2);
                            if (IsValidSource(obj3))
                            {
                                if ((obj3 is DataSet) && (((DataSet) obj3).Tables.Count > 0))
                                {
                                    foreach (DataTable table in ((DataSet) obj3).Tables)
                                    {
                                        if (c.Items.IndexOf(table) == -1)
                                        {
                                            c.Items.Add(table);
                                        }
                                    }
                                }
                                else if (obj3 is DataTable)
                                {
                                    if (c.Items.IndexOf((DataTable) obj3) == -1)
                                    {
                                        c.Items.Add((DataTable) obj3);
                                    }
                                }
                                else if (c.Items.IndexOf(obj3) == -1)
                                {
                                    c.Items.Add(obj3);
                                }
                            }
                        }
                        baseType = baseType.BaseType;
                    }
                    while (!baseType.Equals(typeof(TemplateControl)) && !baseType.Equals(typeof(ContainerControl)));
                }
                if (c.Items.Count > 0)
                {
                    CheckSeriesSource(s, c);
                }
            }
        }

        internal static DataTable FillTable(string TableName)
        {
            DataTable table = null;
            object tableAdapter = GetTableAdapter(TableName);
            object dataTable = GetDataTable(TableName);
            if ((tableAdapter != null) && (dataTable != null))
            {
                tableAdapter.GetType().GetMethod("Fill").Invoke(tableAdapter, new object[] { dataTable });
                table = dataTable as DataTable;
            }
            return table;
        }

        internal static DataTable FillTable(string TableName, Series s)
        {
            FillComponentLists(s);
            return FillTable(TableName);
        }

        internal static object GetDataTable(string TableName)
        {
            object obj2 = null;
            object obj3 = null;
            PropertyInfo property = null;
            if (dataSets.Count <= 0)
            {
                return obj2;
            }
            foreach (object obj4 in dataSets)
            {
                property = obj4.GetType().GetProperty(TableName);
                if (property != null)
                {
                    obj3 = obj4;
                    break;
                }
            }
            return Activator.CreateInstance(property.GetValue(obj3, null).GetType());
        }

        internal static object GetTableAdapter(string TableName)
        {
            object obj2 = null;
            if (tableAdapters.Count > 0)
            {
                foreach (object obj3 in tableAdapters)
                {
                    if (obj3.GetType().Name.Contains(TableName))
                    {
                        obj2 = obj3;
                    }
                }
            }
            return obj2;
        }

        private static DataTable GetTableFromBindingSource(BindingSource bindingSource)
        {
            DataTable table = null;
            if (((bindingSource.DataSource != null) && (bindingSource.DataMember != null)) && (bindingSource.DataSource is DataSet))
            {
                table = (bindingSource.DataSource as DataSet).Tables[bindingSource.DataMember];
            }
            return table;
        }

        internal static bool HasTableAdapter(string TableName)
        {
            bool flag = false;
            foreach (object obj2 in tableAdapters)
            {
                if (obj2.GetType().Name.Contains(TableName))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public static bool IsTeeDataSet(object o)
        {
            bool flag = false;
            if ((o is DataSet) && ((o as DataSet).DataSetName == "TeeDataSet"))
            {
                flag = true;
            }
            return flag;
        }

        public static bool IsValidSource(object c)
        {
            return ((((c is DataSet) || (c is DataAdapter)) || ((c is DataTable) || (c is DataView))) || (c is BindingSource));
        }

        internal static bool TryRefreshData(Series s)
        {
            object dataSource = s.DataSource;
            if (dataSource is BindingSource)
            {
                AddFromBindingSource(dataSource as BindingSource, s);
                return true;
            }
            if (dataSource is DataTable)
            {
                AddTable(s, (DataTable) dataSource);
                return true;
            }
            if (dataSource is DataSet)
            {
                DataSet set = (DataSet) dataSource;
                if (set.Tables.Count > 0)
                {
                    AddTable(s, set.Tables[0]);
                }
                return true;
            }
            if (dataSource is DataView)
            {
                s.BeginUpdate();
                s.Clear();
                s.Add((DataView) dataSource);
                s.EndUpdate();
                return true;
            }
            if (dataSource is IDataReader)
            {
                s.BeginUpdate();
                s.Clear();
                s.Add((IDataReader) dataSource);
                s.EndUpdate();
                return true;
            }
            if (dataSource is IBindingList)
            {
                IBindingList list1 = (IBindingList) dataSource;
            }
            else
            {
                if (dataSource is IList)
                {
                    s.BeginUpdate();
                    s.Clear();
                    s.Add((IList) dataSource);
                    s.EndUpdate();
                    return true;
                }
                if (dataSource is IDbDataAdapter)
                {
                    IDbDataAdapter d = (IDbDataAdapter) dataSource;
                    if (d.SelectCommand != null)
                    {
                        AddFromAdapter(s, d);
                    }
                    return true;
                }
                if (dataSource is DataAdapter)
                {
                    AddFromAdapter(s, (DataAdapter) dataSource);
                    return true;
                }
            }
            return false;
        }
    }
}

