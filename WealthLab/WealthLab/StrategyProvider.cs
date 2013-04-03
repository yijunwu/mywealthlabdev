namespace WealthLab
{
    using Fidelity.Components;
    using System;
    using System.Runtime.CompilerServices;

    public abstract class StrategyProvider
    {
        [CompilerGenerated]
        private ISettingsHost isettingsHost_0;
        [CompilerGenerated]
        private IStrategyHost istrategyHost_0;

        protected StrategyProvider()
        {
        }

        public abstract void CancelDownload();
        public abstract void DownloadStrategies(StrategyPubType strategyPubTypes, DateTime startDate);
        public override string ToString()
        {
            return this.FriendlyName;
        }

        public abstract string Description { get; }

        public abstract string FriendlyName { get; }

        public abstract DateTime LastDownload { get; set; }

        public ISettingsHost SettingsHost
        {
            [CompilerGenerated]
            get
            {
                return this.isettingsHost_0;
            }
            [CompilerGenerated]
            set
            {
                this.isettingsHost_0 = value;
            }
        }

        public IStrategyHost StrategyHost
        {
            [CompilerGenerated]
            get
            {
                return this.istrategyHost_0;
            }
            [CompilerGenerated]
            set
            {
                this.istrategyHost_0 = value;
            }
        }

        public abstract string StrategyProviderURL { get; }

        public abstract StrategyPubType StrategyTypesProvided { get; }
    }
}

