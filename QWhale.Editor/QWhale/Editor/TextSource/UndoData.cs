namespace QWhale.Editor.TextSource
{
    using System;
    using System.Drawing;

    public class UndoData : IUndoData
    {
        private object data;
        private UndoOperation operation;
        private Point position;
        private UpdateReason reason;
        private UndoFlags undoFlag;
        private int updateCount;

        public UndoData(UndoOperation operation, object data)
        {
            this.Operation = operation;
            this.Data = data;
            this.Reason = UpdateReason.Other;
        }

        public virtual object Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        public virtual UndoOperation Operation
        {
            get
            {
                return this.operation;
            }
            set
            {
                this.operation = value;
            }
        }

        public virtual Point Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        public virtual UpdateReason Reason
        {
            get
            {
                return this.reason;
            }
            set
            {
                this.reason = value;
            }
        }

        public virtual UndoFlags UndoFlag
        {
            get
            {
                return this.undoFlag;
            }
            set
            {
                this.undoFlag = value;
            }
        }

        public virtual int UpdateCount
        {
            get
            {
                return this.updateCount;
            }
            set
            {
                this.updateCount = value;
            }
        }
    }
}

