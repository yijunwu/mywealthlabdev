namespace WealthLab
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class Optimizer
    {
        [CompilerGenerated]
        private IOptimizationHost ioptimizationHost;
        [CompilerGenerated]
        private IPrintHost iprintHost;
        [CompilerGenerated]
        private WealthLab.Strategy strategy;
        [CompilerGenerated]
        private WealthLab.WealthScript wealthScript;

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
                return this.ioptimizationHost;
            }
            [CompilerGenerated]
            set
            {
                this.ioptimizationHost = value;
            }
        }

        public abstract double NumberOfRuns { get; }

        public IPrintHost PrintHost
        {
            [CompilerGenerated]
            get
            {
                return this.iprintHost;
            }
            [CompilerGenerated]
            set
            {
                this.iprintHost = value;
            }
        }

        public WealthLab.Strategy Strategy
        {
            [CompilerGenerated]
            get
            {
                return this.strategy;
            }
            [CompilerGenerated]
            set
            {
                this.strategy = value;
            }
        }

        public WealthLab.WealthScript WealthScript
        {
            [CompilerGenerated]
            get
            {
                return this.wealthScript;
            }
            [CompilerGenerated]
            set
            {
                this.wealthScript = value;
            }
        }
    }
}

