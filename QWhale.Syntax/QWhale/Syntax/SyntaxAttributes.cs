namespace QWhale.Syntax
{
    using QWhale.Common;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;

    public class SyntaxAttributes : SortList<ISyntaxAttribute>, ISyntaxAttributes, ISortList<ISyntaxAttribute>, IList<ISyntaxAttribute>, ICollection<ISyntaxAttribute>, IEnumerable<ISyntaxAttribute>, IEnumerable
    {
        public virtual bool BlockDeleting(Rectangle rect)
        {
            bool flag = false;
            for (int i = base.Count - 1; i >= 0; i--)
            {
                if (SortList<ISyntaxAttribute>.InsideRange(base[i].Position, rect))
                {
                    base.RemoveAt(i);
                    flag = true;
                }
            }
            return flag;
        }

        public virtual bool PositionChanged(int x, int y, int deltaX, int deltaY)
        {
            bool flag = false;
            foreach (ISyntaxAttribute attribute in this)
            {
                Point position = attribute.Position;
                if ((deltaY == 0) && (position.Y > y))
                {
                    return flag;
                }
                if (QWhale.Common.Range.UpdatePos(x, y, deltaX, deltaY, ref position, attribute.Name == SyntaxConsts.DefinitionScopeEnd))
                {
                    attribute.Position = position;
                    flag = true;
                }
            }
            return flag;
        }

        void ISyntaxAttributes.Sort()
        {
            base.Sort();
        }
    }
}

