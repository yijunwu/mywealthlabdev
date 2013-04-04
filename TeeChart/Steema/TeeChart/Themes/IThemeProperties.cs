namespace Steema.TeeChart.Themes
{
    using System;

    internal interface IThemeProperties
    {
        string AspectSmoothingMode { get; set; }

        string AspectTextRenderingHint { get; set; }

        bool AxesBottomGridCentered { get; set; }

        bool AxesBottomGridVisible { get; set; }

        bool AxesTopGridVisible { get; set; }

        string AxisAxisPenColor { get; set; }

        int AxisAxisPenWidth { get; set; }

        bool AxisGridCentered { get; set; }

        string AxisGridColor { get; set; }

        string AxisGridStyle { get; set; }

        bool AxisGridVisible { get; set; }

        int AxisGridWidth { get; set; }

        bool AxisLabelsFontBold { get; set; }

        string AxisLabelsFontColor { get; set; }

        string AxisLabelsFontName { get; set; }

        int AxisLabelsFontSize { get; set; }

        string AxisMinorGridColor { get; set; }

        bool AxisMinorGridVisible { get; set; }

        int AxisMinorTickCount { get; set; }

        int AxisMinorTicksLength { get; set; }

        bool AxisMinorTicksVisible { get; set; }

        string AxisTicksColor { get; set; }

        string AxisTicksInnerColor { get; set; }

        int AxisTicksInnerLength { get; set; }

        bool AxisTicksInnerVisible { get; set; }

        int AxisTicksLength { get; set; }

        string AxisTitleFontColor { get; set; }

        string AxisTitleFontName { get; set; }

        string Base64 { get; set; }

        string HeaderColor { get; set; }

        bool HeaderFontBold { get; set; }

        string HeaderFontColor { get; set; }

        string HeaderFontName { get; set; }

        int HeaderFontSize { get; set; }

        string HeaderGradientDirection { get; set; }

        string HeaderGradientEndColor { get; set; }

        string HeaderGradientMiddleColor { get; set; }

        bool HeaderGradientSigma { get; set; }

        float HeaderGradientSigmaFocus { get; set; }

        float HeaderGradientSigmaScale { get; set; }

        string HeaderGradientStartColor { get; set; }

        bool HeaderGradientVisible { get; set; }

        string HeaderPenColor { get; set; }

        bool HeaderPenVisible { get; set; }

        int HeaderPenWidth { get; set; }

        int HeaderShadowSize { get; set; }

        int HeaderShadowTransparency { get; set; }

        int HeaderTransparency { get; set; }

        string LegendDividingLinesColor { get; set; }

        bool LegendDividingLinesVisible { get; set; }

        string LegendFontColor { get; set; }

        string LegendFontName { get; set; }

        int LegendFontSize { get; set; }

        string LegendGradientDirection { get; set; }

        string LegendGradientEndColor { get; set; }

        string LegendGradientMiddleColor { get; set; }

        bool LegendGradientSigma { get; set; }

        float LegendGradientSigmaFocus { get; set; }

        float LegendGradientSigmaScale { get; set; }

        string LegendGradientStartColor { get; set; }

        bool LegendGradientVisible { get; set; }

        string LegendPenColor { get; set; }

        string LegendPenStyle { get; set; }

        bool LegendPenVisible { get; set; }

        int LegendPenWidth { get; set; }

        string LegendShadowColor { get; set; }

        int LegendShadowHeight { get; set; }

        int LegendShadowTransparency { get; set; }

        int LegendShadowWidth { get; set; }

        bool LegendSymbolDefaultPen { get; set; }

        bool LegendSymbolPenVisible { get; set; }

        bool LegendSymbolSquared { get; set; }

        bool LegendTransparent { get; set; }

        string[] Palette { get; set; }

        string PanelBevelInner { get; set; }

        string PanelBevelOuter { get; set; }

        int PanelBevelWidth { get; set; }

        int PanelBorderRound { get; set; }

        string PanelColor { get; set; }

        string PanelGradientDirection { get; set; }

        string PanelGradientEndColor { get; set; }

        string PanelGradientMiddleColor { get; set; }

        bool PanelGradientSigma { get; set; }

        float PanelGradientSigmaFocus { get; set; }

        float PanelGradientSigmaScale { get; set; }

        string PanelGradientStartColor { get; set; }

        bool PanelGradientVisible { get; set; }

        string PanelPenColor { get; set; }

        string PanelPenStyle { get; set; }

        bool PanelPenVisible { get; set; }

        int PanelPenWidth { get; set; }

        string PanelShadowColor { get; set; }

        int PanelShadowSize { get; set; }

        bool PanelShadowVisible { get; set; }

        string SeriesMarksArrowColor { get; set; }

        string SeriesMarksFontColor { get; set; }

        string SeriesMarksFontName { get; set; }

        int SeriesMarksFontSize { get; set; }

        string SeriesMarksGradientDirection { get; set; }

        string SeriesMarksGradientEndColor { get; set; }

        string SeriesMarksGradientMiddleColor { get; set; }

        bool SeriesMarksGradientSigma { get; set; }

        float SeriesMarksGradientSigmaFocus { get; set; }

        float SeriesMarksGradientSigmaScale { get; set; }

        string SeriesMarksGradientStartColor { get; set; }

        bool SeriesMarksGradientVisible { get; set; }

        bool SeriesMarksTransparent { get; set; }

        bool WallApplyDark { get; set; }

        string WallGradientDirection { get; set; }

        string WallGradientEndColor { get; set; }

        string WallGradientMiddleColor { get; set; }

        bool WallGradientSigma { get; set; }

        float WallGradientSigmaFocus { get; set; }

        float WallGradientSigmaScale { get; set; }

        string WallGradientStartColor { get; set; }

        bool WallGradientVisible { get; set; }

        string WallPenColor { get; set; }

        string WallPenStyle { get; set; }

        bool WallPenVisible { get; set; }

        int WallPenWidth { get; set; }

        string WallsBackColor { get; set; }

        string WallsBackGradientDirection { get; set; }

        string WallsBackGradientEndColor { get; set; }

        string WallsBackGradientMiddleColor { get; set; }

        bool WallsBackGradientSigma { get; set; }

        float WallsBackGradientSigmaFocus { get; set; }

        float WallsBackGradientSigmaScale { get; set; }

        string WallsBackGradientStartColor { get; set; }

        bool WallsBackGradientVisible { get; set; }

        bool WallsBackTransparent { get; set; }

        string WallsBottomColor { get; set; }

        int WallSize { get; set; }

        string WallsLeftColor { get; set; }

        string WallsRightColor { get; set; }
    }
}

