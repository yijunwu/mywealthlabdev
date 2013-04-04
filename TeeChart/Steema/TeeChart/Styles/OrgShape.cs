namespace Steema.TeeChart.Styles
{
    using Steema.TeeChart;
    using Steema.TeeChart.Drawing;
    using System;

    public class OrgShape : TextShapePosition
    {
        public OrgShape() : this(null)
        {
        }

        public OrgShape(Chart c) : base(c)
        {
            base.bCustomPosition = true;
        }

        protected override bool ShouldSerializeLeft()
        {
            return false;
        }

        protected override bool ShouldSerializeTop()
        {
            return false;
        }
    }
}

