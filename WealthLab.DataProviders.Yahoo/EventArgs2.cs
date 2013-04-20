using System;
using WealthLab;

///WYJ fix, original name EventArgs2, candidate name: StaticDataEventArgs
internal class StaticDataEventArgs : RequestResultEventArgs
{
    public readonly Bars bars;
    public readonly SplitAndDividend splitAndDividend;

    public StaticDataEventArgs(DataRequest request, Bars bars_1, SplitAndDividend snd_1)
        : base(request)
    {
        this.bars = bars_1;
        this.splitAndDividend = snd_1;
    }
}

