namespace QWhale.Editor
{
    using QWhale.Common;
    using System;
    using System.Drawing;

    public interface IWhiteSpace : IUpdate
    {
        void Assign(IWhiteSpace source);
        void ResetEofSymbol();
        void ResetEolSymbol();
        void ResetSpaceSymbol();
        void ResetSymbolColor();
        void ResetTabSymbol();
        void ResetVisible();
        void ResetWordWrapSymbol();

        string EofString { get; }

        char EofSymbol { get; set; }

        string EolString { get; }

        char EolSymbol { get; set; }

        ISerializationInfo SerializationInfo { get; set; }

        string SpaceString { get; }

        char SpaceSymbol { get; set; }

        Color SymbolColor { get; set; }

        string TabString { get; }

        char TabSymbol { get; set; }

        bool Visible { get; set; }

        string WordWrapString { get; }

        char WordWrapSymbol { get; set; }
    }
}

