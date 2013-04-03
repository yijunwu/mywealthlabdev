using System;
using System.Runtime.CompilerServices;
using WealthLab;

public class StrategyEventArgs : EventArgs
{
    [CompilerGenerated]
    private WealthLab.Strategy strategy_0;
    private string string_0 = "";

    public StrategyEventArgs(string strategyID)
    {
        this.string_0 = strategyID;
    }

    public WealthLab.Strategy Strategy
    {
        [CompilerGenerated]
        get
        {
            return this.strategy_0;
        }
        [CompilerGenerated]
        set
        {
            this.strategy_0 = value;
        }
    }

    public string StrategyID
    {
        get
        {
            return this.string_0;
        }
    }
}

