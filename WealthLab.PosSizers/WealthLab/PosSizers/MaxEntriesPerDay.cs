namespace WealthLab.PosSizers
{
    using Fidelity.Components;
    using System;
    using System.Windows.Forms;
    using WealthLab;

    public class MaxEntriesPerDay : BasicPosSizer, ICustomSettings
    {
        private int int_0 = 2;
        private MaxEntriesPerDaySettings maxEntriesPerDaySettings_0;

        public override void ApplyConfigString(string config)
        {
            base.ApplyConfigString(config);
            string[] strArray = config.Split(new char[] { '|' });
            this.int_0 = int.Parse(strArray[4]);
        }

        public void ChangeSettings(UserControl userControl_0)
        {
            this.int_0 = this.maxEntriesPerDaySettings_0.MaxEntries;
            base.ChangeBasicSettings(this.maxEntriesPerDaySettings_0);
        }

        public override string GetConfigString()
        {
            return (base.GetConfigString() + this.int_0 + "|");
        }

        public UserControl GetSettingsUI()
        {
            if (this.maxEntriesPerDaySettings_0 == null)
            {
                this.maxEntriesPerDaySettings_0 = new MaxEntriesPerDaySettings();
            }
            this.maxEntriesPerDaySettings_0.MaxEntries = this.int_0;
            base.InitializeSettings(this.maxEntriesPerDaySettings_0);
            return this.maxEntriesPerDaySettings_0;
        }

        public void ReadSettings(ISettingsHost host)
        {
            this.int_0 = host.Get("MaxEntriesPerDay.MaxEntries", 2);
            base.ReadBasicSettings(host);
        }

        public override double SizePosition(Position currentPos, Bars bars, int int_1, double basisPrice, PositionType positionType_0, double riskStopLevel, double equity, double cash)
        {
            int num = 0;
            for (int i = base.Positions.Count - 1; i >= 0; i--)
            {
                DateTime time = bars.Date[int_1 + 1];
                if (base.Positions[i].EntryDate == time.Date)
                {
                    num++;
                    if (num >= this.int_0)
                    {
                        break;
                    }
                }
            }
            if (num >= this.int_0)
            {
                return 0.0;
            }
            return base.CalcBasicPositionSize(bars, int_1, positionType_0, basisPrice, riskStopLevel, equity);
        }

        public void WriteSettings(ISettingsHost host)
        {
            host.Set("MaxEntriesPerDay.MaxEntries", this.int_0);
            base.WriteBasicSettings(host);
        }

        public override string FriendlyName
        {
            get
            {
                return "Max Entries per Day";
            }
        }
    }
}

