namespace TreeMaps.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class TreeMaps : ItemsControl
    {
        public static DependencyProperty MaxDepthProperty = DependencyProperty.Register("MaxDepth", typeof(int), typeof(TreeMaps), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MinAreaProperty = DependencyProperty.Register("MinArea", typeof(int), typeof(TreeMaps), new FrameworkPropertyMetadata(0x40, FrameworkPropertyMetadataOptions.AffectsRender));
        public static DependencyProperty TreeMapModeProperty = DependencyProperty.Register("TreeMapMode", typeof(TreeMapAlgo), typeof(TreeMaps), new FrameworkPropertyMetadata(TreeMapAlgo.Squarified, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static DependencyProperty ValuePropertyNameProperty = DependencyProperty.Register("ValuePropertyName", typeof(string), typeof(TreeMaps), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        static TreeMaps()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeMaps), new FrameworkPropertyMetadata(typeof(TreeMaps)));
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeMapItem(1, this.MaxDepth, this.MinArea, this.ValuePropertyName);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is TreeMapItem);
        }

        public int MaxDepth
        {
            get
            {
                return (int) base.GetValue(MaxDepthProperty);
            }
            set
            {
                base.SetValue(MaxDepthProperty, value);
            }
        }

        public int MinArea
        {
            get
            {
                return (int) base.GetValue(MinAreaProperty);
            }
            set
            {
                base.SetValue(MinAreaProperty, value);
            }
        }

        public TreeMapAlgo TreeMapMode
        {
            get
            {
                return (TreeMapAlgo) base.GetValue(TreeMapModeProperty);
            }
            set
            {
                base.SetValue(TreeMapModeProperty, value);
            }
        }

        public string ValuePropertyName
        {
            get
            {
                return (string) base.GetValue(ValuePropertyNameProperty);
            }
            set
            {
                base.SetValue(ValuePropertyNameProperty, value);
            }
        }
    }
}

