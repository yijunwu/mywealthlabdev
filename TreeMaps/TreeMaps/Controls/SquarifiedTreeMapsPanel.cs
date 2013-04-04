namespace TreeMaps.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public class SquarifiedTreeMapsPanel : TreeMapsPanel
    {
        private void AddRowToLayout(List<TreeMapsPanel.WeightUIElement> row)
        {
            base.ComputeTreeMaps(row);
        }

        protected override void ComputeBounds()
        {
            this.Squarify(base.ManagedItems, new List<TreeMapsPanel.WeightUIElement>(), base.GetShortestSide());
        }

        protected override void ComputeNextPosition(TreeMapsPanel.RowOrientation orientation, ref double xPos, ref double yPos, double width, double height)
        {
            if (orientation == TreeMapsPanel.RowOrientation.Horizontal)
            {
                yPos += height;
            }
            else
            {
                xPos += width;
            }
        }

        protected override Rect GetRectangle(TreeMapsPanel.RowOrientation orientation, TreeMapsPanel.WeightUIElement item, double x, double y, double width, double height)
        {
            if (orientation == TreeMapsPanel.RowOrientation.Horizontal)
            {
                return new Rect(x, y, width, item.RealArea / width);
            }
            return new Rect(x, y, item.RealArea / height, height);
        }

        private void Squarify(List<TreeMapsPanel.WeightUIElement> items, List<TreeMapsPanel.WeightUIElement> row, double sideLength)
        {
            if (items.Count == 0)
            {
                this.AddRowToLayout(row);
            }
            else
            {
                TreeMapsPanel.WeightUIElement element = items[0];
                List<TreeMapsPanel.WeightUIElement> list = new List<TreeMapsPanel.WeightUIElement>(row) {
                    element
                };
                List<TreeMapsPanel.WeightUIElement> list2 = new List<TreeMapsPanel.WeightUIElement>(items);
                list2.RemoveAt(0);
                double num = this.Worst(row, sideLength);
                double num2 = this.Worst(list, sideLength);
                if ((row.Count == 0) || (num > num2))
                {
                    this.Squarify(list2, list, sideLength);
                }
                else
                {
                    this.AddRowToLayout(row);
                    this.Squarify(items, new List<TreeMapsPanel.WeightUIElement>(), base.GetShortestSide());
                }
            }
        }

        private double Worst(List<TreeMapsPanel.WeightUIElement> row, double sideLength)
        {
            if (row.Count == 0)
            {
                return 0.0;
            }
            double num = 0.0;
            double maxValue = double.MaxValue;
            double num3 = 0.0;
            foreach (TreeMapsPanel.WeightUIElement element in row)
            {
                num = Math.Max(num, element.RealArea);
                maxValue = Math.Min(maxValue, element.RealArea);
                num3 += element.RealArea;
            }
            if (maxValue == double.MaxValue)
            {
                maxValue = 0.0;
            }
            double num4 = ((sideLength * sideLength) * num) / (num3 * num3);
            double num5 = (num3 * num3) / ((sideLength * sideLength) * maxValue);
            return Math.Max(num4, num5);
        }
    }
}

