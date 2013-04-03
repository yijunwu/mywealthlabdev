namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class Optimizer
    {
        [CompilerGenerated]
        private IOptimizationHost ioptimizationHost_0;
        [CompilerGenerated]
        private IPrintHost iprintHost_0;
        [CompilerGenerated]
        private WealthLab.Strategy strategy_0;
        [CompilerGenerated]
        private WealthLab.WealthScript wealthScript_0;

        protected Optimizer()
        {
        }

        public abstract void FirstRun();
        public abstract void Initialize();
        public abstract bool NextRun(SystemPerformance systemPerformance_0, OptimizationResult optimizationResult_0);
        public virtual void RefreshViews()
        {
        }

        public virtual void RunCompleted(OptimizationResultList results)
        {
        }

        public override string ToString()
        {
            return this.FriendlyName;
        }

        public abstract string Description { get; }

        public abstract string FriendlyName { get; }

        public IOptimizationHost Host
        {
            [CompilerGenerated]
            get
            {
                return this.ioptimizationHost_0;
            }
            [CompilerGenerated]
            set
            {
                this.ioptimizationHost_0 = value;
            }
        }

        public abstract double NumberOfRuns { get; }

        public IPrintHost PrintHost
        {
            [CompilerGenerated]
            get
            {
                return this.iprintHost_0;
            }
            [CompilerGenerated]
            set
            {
                this.iprintHost_0 = value;
            }
        }

        public WealthLab.Strategy Strategy
        {
            [CompilerGenerated]
            get
            {
                return this.strategy_0;
            }
            [CompilerGenerated]
            set
            {
                this.strategy_0 = value;
            }
        }

        public WealthLab.WealthScript WealthScript
        {
            [CompilerGenerated]
            get
            {
                return this.wealthScript_0;
            }
            [CompilerGenerated]
            set
            {
                this.wealthScript_0 = value;
            }
        }
    }
}

