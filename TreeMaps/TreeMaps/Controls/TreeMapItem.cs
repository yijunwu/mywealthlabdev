namespace TreeMaps.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    [TemplatePart(Name="PART_Header", Type=typeof(FrameworkElement))]
    public class TreeMapItem : HeaderedItemsControl
    {
        private double _area;
        private TreeMaps _parentTreeMaps;
        private const string HeaderPartName = "PART_Header";
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register("Level", typeof(int), typeof(TreeMapItem), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MaxDepthProperty = DependencyProperty.Register("MaxDepth", typeof(int), typeof(TreeMapItem), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty MinAreaProperty = DependencyProperty.Register("MinArea", typeof(int), typeof(TreeMapItem), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty ShouldRecurseProperty = DependencyProperty.Register("ShouldRecurse", typeof(bool), typeof(TreeMapItem), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        public static DependencyProperty TreeMapModeProperty = DependencyProperty.Register("TreeMapMode", typeof(TreeMapAlgo), typeof(TreeMapItem), new FrameworkPropertyMetadata(TreeMapAlgo.Squarified, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty ValuePropertyNameProperty = DependencyProperty.Register("ValuePropertyName", typeof(string), typeof(TreeMapItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        static TreeMapItem()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeMapItem), new FrameworkPropertyMetadata(typeof(TreeMapItem)));
        }

        public TreeMapItem()
        {
            base.VerticalAlignment = VerticalAlignment.Stretch;
            base.HorizontalAlignment = HorizontalAlignment.Stretch;
            base.VerticalContentAlignment = VerticalAlignment.Stretch;
            base.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            base.SnapsToDevicePixels = true;
        }

        public TreeMapItem(int level, int maxDepth, int minArea, string valuePropertyName) : this()
        {
            this.ValuePropertyName = valuePropertyName;
            this.Level = level;
            this.MaxDepth = maxDepth;
            this.MinArea = minArea;
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Size size = base.ArrangeOverride(arrangeBounds);
            if (this.IsValidSize(size))
            {
                this._area = size.Width * size.Height;
            }
            else
            {
                this._area = 0.0;
            }
            this.UpdateShouldRecurse();
            return size;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeMapItem(this.Level + 1, this.MaxDepth, this.MinArea, this.ValuePropertyName);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is TreeMapItem);
        }

        private bool IsValidSize(Size size)
        {
            return (((!size.IsEmpty && (size.Width > 0.0)) && ((size.Width != double.NaN) && (size.Height > 0.0))) && (size.Height != double.NaN));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if ((e.Property == ValuePropertyNameProperty) || (e.Property == FrameworkElement.DataContextProperty))
            {
                if ((this.ValuePropertyName != null) && (base.DataContext != null))
                {
                    Binding binding = new Binding(this.ValuePropertyName) {
                        Source = base.DataContext
                    };
                    BindingOperations.SetBinding(this, TreeMapsPanel.WeightProperty, binding);
                }
            }
            else if (e.Property == MaxDepthProperty)
            {
                this.UpdateShouldRecurse();
            }
            else if (e.Property == MinAreaProperty)
            {
                this.UpdateShouldRecurse();
            }
            else if (e.Property == LevelProperty)
            {
                this.UpdateShouldRecurse();
            }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            this._parentTreeMaps = this.ParentTreeMap;
            if (this._parentTreeMaps != null)
            {
                Binding binding = new Binding(TreeMaps.TreeMapModeProperty.Name) {
                    Source = this._parentTreeMaps
                };
                BindingOperations.SetBinding(this, TreeMapModeProperty, binding);
                Binding binding2 = new Binding(TreeMaps.ValuePropertyNameProperty.Name) {
                    Source = this._parentTreeMaps
                };
                BindingOperations.SetBinding(this, ValuePropertyNameProperty, binding2);
                Binding binding3 = new Binding(TreeMaps.MinAreaProperty.Name) {
                    Source = this._parentTreeMaps
                };
                BindingOperations.SetBinding(this, MinAreaProperty, binding3);
                Binding binding4 = new Binding(TreeMaps.MaxDepthProperty.Name) {
                    Source = this._parentTreeMaps
                };
                BindingOperations.SetBinding(this, MaxDepthProperty, binding4);
            }
        }

        private void UpdateShouldRecurse()
        {
            if (!base.HasHeader)
            {
                this.ShouldRecurse = false;
            }
            else
            {
                this.ShouldRecurse = ((this.MaxDepth == 0) || (this.Level < this.MaxDepth)) && ((this.MinArea == 0) || (this.Area > this.MinArea));
            }
        }

        internal double Area
        {
            get
            {
                return this._area;
            }
        }

        public int Level
        {
            get
            {
                return (int) base.GetValue(LevelProperty);
            }
            set
            {
                base.SetValue(LevelProperty, value);
            }
        }

        public int MaxDepth
        {
            get
            {
                return (int) base.GetValue(MaxDepthProperty);
            }
            internal set
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
            internal set
            {
                base.SetValue(MinAreaProperty, value);
            }
        }

        internal ItemsControl ParentItemsControl
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this);
            }
        }

        internal TreeMaps ParentTreeMap
        {
            get
            {
                for (ItemsControl control = this.ParentItemsControl; control != null; control = ItemsControl.ItemsControlFromItemContainer(control))
                {
                    TreeMaps maps = control as TreeMaps;
                    if (maps != null)
                    {
                        return maps;
                    }
                }
                return null;
            }
        }

        internal TreeMapItem ParentTreeMapItem
        {
            get
            {
                return (this.ParentItemsControl as TreeMapItem);
            }
        }

        public bool ShouldRecurse
        {
            get
            {
                return (bool) base.GetValue(ShouldRecurseProperty);
            }
            internal set
            {
                base.SetValue(ShouldRecurseProperty, value);
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

