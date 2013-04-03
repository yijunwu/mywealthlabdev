namespace WealthLab.International
{
    using System;

    public class TrialMethodParams : BaseMethodParams
    {
        private bool bool_2;
        private DateTime dateTime_1;
        private int int_3;
        private string string_10;
        private string string_11;

        public DateTime ActivationTrialDate
        {
            get
            {
                return this.dateTime_1;
            }
            set
            {
                this.dateTime_1 = value;
            }
        }

        public int DaysFromActivation
        {
            get
            {
                return this.int_3;
            }
            set
            {
                this.int_3 = value;
            }
        }

        public string Password
        {
            get
            {
                return this.string_11;
            }
            set
            {
                this.string_11 = value;
            }
        }

        public bool TrialAlreadyActivated
        {
            get
            {
                return this.bool_2;
            }
            set
            {
                this.bool_2 = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.string_10;
            }
            set
            {
                this.string_10 = value;
            }
        }
    }
}

