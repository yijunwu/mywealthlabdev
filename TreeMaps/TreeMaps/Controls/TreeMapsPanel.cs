namespace TreeMaps.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    public class TreeMapsPanel : Panel
    {
        private Rect _emptyArea;
        private List<WeightUIElement> _items = new List<WeightUIElement>();
        private double _weightSum = 0.0;
        public static readonly DependencyProperty WeightProperty = DependencyProperty.RegisterAttached("Weight", typeof(double), typeof(TreeMapsPanel), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (WeightUIElement element in this.ManagedItems)
            {
                try
                {
                    element.UIElement.Arrange(new Rect(element.ComputedLocation, element.ComputedSize));
                }
                catch (Exception)
                {
                }
            }
            return arrangeSize;
        }

        protected virtual void ComputeBounds()
        {
            this.ComputeTreeMaps(this.ManagedItems);
        }

        protected virtual void ComputeNextPosition(RowOrientation orientation, ref double xPos, ref double yPos, double width, double height)
        {
            if (orientation == RowOrientation.Horizontal)
            {
                xPos += width;
            }
            else
            {
                yPos += height;
            }
        }

        protected void ComputeTreeMaps(List<WeightUIElement> items)
        {
            Rect rect;
            RowOrientation orientation = this.GetOrientation();
            double num = 0.0;
            foreach (WeightUIElement element in items)
            {
                num += element.RealArea;
            }
            if (orientation == RowOrientation.Horizontal)
            {
                rect = new Rect(this._emptyArea.X, this._emptyArea.Y, num / this._emptyArea.Height, this._emptyArea.Height);
                double width = Math.Max((double) 0.0, (double) (this._emptyArea.Width - rect.Width));
                this._emptyArea = new Rect(this._emptyArea.X + rect.Width, this._emptyArea.Y, width, this._emptyArea.Height);
            }
            else
            {
                rect = new Rect(this._emptyArea.X, this._emptyArea.Y, this._emptyArea.Width, num / this._emptyArea.Width);
                this._emptyArea = new Rect(this._emptyArea.X, this._emptyArea.Y + rect.Height, this._emptyArea.Width, Math.Max((double) 0.0, (double) (this._emptyArea.Height - rect.Height)));
            }
            double x = rect.X;
            double y = rect.Y;
            foreach (WeightUIElement element in items)
            {
                Rect rect2 = this.GetRectangle(orientation, element, x, y, rect.Width, rect.Height);
                element.AspectRatio = rect2.Width / rect2.Height;
                element.ComputedSize = rect2.Size;
                element.ComputedLocation = rect2.Location;
                this.ComputeNextPosition(orientation, ref x, ref y, rect2.Width, rect2.Height);
            }
        }

        protected RowOrientation GetOrientation()
        {
            return ((this.EmptyArea.Width > this.EmptyArea.Height) ? RowOrientation.Horizontal : RowOrientation.Vertical);
        }

        protected virtual Rect GetRectangle(RowOrientation orientation, WeightUIElement item, double x, double y, double width, double height)
        {
            if (orientation == RowOrientation.Horizontal)
            {
                return new Rect(x, y, item.RealArea / height, height);
            }
            return new Rect(x, y, width, item.RealArea / width);
        }

        protected double GetShortestSide()
        {
            return Math.Min(this.EmptyArea.Width, this.EmptyArea.Height);
        }

        public static double GetWeight(DependencyObject uiElement)
        {
            if (uiElement == null)
            {
                return 0.0;
            }
            return (double) uiElement.GetValue(WeightProperty);
        }

        private bool IsValidItem(WeightUIElement item)
        {
            return (((item != null) && (item.Weight != double.NaN)) && (Math.Round(item.Weight, 0) != 0.0));
        }

        private bool IsValidSize(Size size)
        {
            return (((!size.IsEmpty && (size.Width > 0.0)) && ((size.Width != double.NaN) && (size.Height > 0.0))) && (size.Height != double.NaN));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this.EmptyArea = new Rect(0.0, 0.0, constraint.Width, constraint.Height);
            this.PrepareItems();
            double num = this.EmptyArea.Width * this.EmptyArea.Height;
            foreach (WeightUIElement element in this.ManagedItems)
            {
                element.RealArea = (num * element.Weight) / this._weightSum;
            }
            this.ComputeBounds();
            foreach (WeightUIElement element2 in this.ManagedItems)
            {
                if (this.IsValidSize(element2.ComputedSize))
                {
                    element2.UIElement.Measure(element2.ComputedSize);
                }
                else
                {
                    element2.UIElement.Measure(new Size(0.0, 0.0));
                }
            }
            return constraint;
        }

        private void PrepareItems()
        {
            this._weightSum = 0.0;
            this.ManagedItems.Clear();
            foreach (UIElement element in base.Children)
            {
                WeightUIElement item = new WeightUIElement(element, GetWeight(element));
                if (this.IsValidItem(item))
                {
                    this._weightSum += item.Weight;
                    this.ManagedItems.Add(item);
                }
                else
                {
                    item.ComputedSize = Size.Empty;
                    item.ComputedLocation = new Point(0.0, 0.0);
                    item.UIElement.Measure(item.ComputedSize);
                    item.UIElement.Visibility = Visibility.Collapsed;
                }
            }
            this.ManagedItems.Sort(new Comparison<WeightUIElement>(WeightUIElement.CompareByValueDecreasing));
        }

        public static void SetWeight(DependencyObject uiElement, double value)
        {
            if (uiElement != null)
            {
                uiElement.SetValue(WeightProperty, value);
            }
        }

        protected Rect EmptyArea
        {
            get
            {
                return this._emptyArea;
            }
            set
            {
                this._emptyArea = value;
            }
        }

        protected List<WeightUIElement> ManagedItems
        {
            get
            {
                return this._items;
            }
        }

        protected enum RowOrientation
        {
            Horizontal,
            Vertical
        }

        protected class WeightUIElement
        {
            private double _area;
            private Point _desiredLocation;
            private Size _desiredSize;
            private System.Windows.UIElement _element;
            private double _ratio;
            private double _weight;

            public WeightUIElement(System.Windows.UIElement element, double weight)
            {
                this._element = element;
                this._weight = weight;
            }

            public static int CompareByValueDecreasing(TreeMapsPanel.WeightUIElement x, TreeMapsPanel.WeightUIElement y)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return -1;
                    }
                    return 0;
                }
                if (y == null)
                {
                    return 1;
                }
                return (x.Weight.CompareTo(y.Weight) * -1);
            }

            public double AspectRatio
            {
                get
                {
                    return this._ratio;
                }
                set
                {
                    this._ratio = value;
                }
            }

            internal Point ComputedLocation
            {
                get
                {
                    return this._desiredLocation;
                }
                set
                {
                    this._desiredLocation = value;
                }
            }

            internal Size ComputedSize
            {
                get
                {
                    return this._desiredSize;
                }
                set
                {
                    this._desiredSize = value;
                }
            }

            public double RealArea
            {
                get
                {
                    return this._area;
                }
                set
                {
                    this._area = value;
                }
            }

            public System.Windows.UIElement UIElement
            {
                get
                {
                    return this._element;
                }
            }

            public double Weight
            {
                get
                {
                    return this._weight;
                }
            }
        }
    }
}

