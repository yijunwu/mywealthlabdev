namespace WealthLab
{
    using System;

    [Flags]
    public enum VisualizerAppliesTo
    {
        All = 15,
        CombinationStrategy = 0x10,
        MultiSymbol = 2,
        PortfolioSim = 8,
        RawProfit = 4,
        SingleSymbol = 1
    }
}

