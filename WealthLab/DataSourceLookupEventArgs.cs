using System;
using System.Runtime.CompilerServices;
using WealthLab;

public class DataSourceLookupEventArgs : EventArgs
{
    [CompilerGenerated]
    private WealthLab.DataSource dataSource_0;
    private string string_0;

    public DataSourceLookupEventArgs(string dsName)
    {
        this.string_0 = dsName;
    }

    public WealthLab.DataSource DataSource
    {
        [CompilerGenerated]
        get
        {
            return this.dataSource_0;
        }
        [CompilerGenerated]
        set
        {
            this.dataSource_0 = value;
        }
    }

    public string DataSourceName
    {
        get
        {
            return this.string_0;
        }
    }
}

