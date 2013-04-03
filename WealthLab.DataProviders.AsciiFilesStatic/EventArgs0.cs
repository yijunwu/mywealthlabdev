using System;
using WealthLab.DataProviders.AsciiFilesStatic;

internal class EventArgs0 : EventArgs
{
    private AsciiCache asciiCache_0;

    public EventArgs0(AsciiCache asciiCache_1)
    {
        this.asciiCache_0 = asciiCache_1;
    }

    public AsciiCache method_0()
    {
        return this.asciiCache_0;
    }
}

