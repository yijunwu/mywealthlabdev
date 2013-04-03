namespace WealthLab.International
{
    using System;

    public class KeyMethodParams : BaseMethodParams
    {
        private bool bool_2;
        private bool bool_3;
        private bool bool_4;
        private int int_3;
        private int int_4;
        private int int_5;
        private DateTime? nullable_0;
        private string string_10;
        private string string_11;
        private string string_12;
        private string string_13;
        private string string_14;
        private string string_15;
        private string string_16;
        private string string_17;

        public string ActivationKey
        {
            get
            {
                return this.string_13;
            }
            set
            {
                this.string_13 = value;
            }
        }

        public bool CompromisedOldHwid
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

        public string ComputerName
        {
            get
            {
                return this.string_14;
            }
            set
            {
                this.string_14 = value;
            }
        }

        public string ComputerNameRegistered
        {
            get
            {
                return this.string_16;
            }
            set
            {
                this.string_16 = value;
            }
        }

        public int DaysFromLastRegistrationHwid
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

        public int DaysToExpiration
        {
            get
            {
                return this.int_5;
            }
            set
            {
                this.int_5 = value;
            }
        }

        public DateTime? ExpirationDate
        {
            get
            {
                return this.nullable_0;
            }
            set
            {
                this.nullable_0 = value;
            }
        }

        public bool Expired
        {
            get
            {
                return this.bool_4;
            }
            set
            {
                this.bool_4 = value;
            }
        }

        public string FirstName
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

        public string HwidRegistered
        {
            get
            {
                return this.string_15;
            }
            set
            {
                this.string_15 = value;
            }
        }

        public string LastName
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

        public string MaintenanceUrl
        {
            get
            {
                return this.string_17;
            }
            set
            {
                this.string_17 = value;
            }
        }

        public string OrderStatusDescription
        {
            get
            {
                return this.string_12;
            }
            set
            {
                this.string_12 = value;
            }
        }

        public int OrderStatusId
        {
            get
            {
                return this.int_4;
            }
            set
            {
                this.int_4 = value;
            }
        }

        public bool SmallDaysFromLastRegistrationHwid
        {
            get
            {
                return this.bool_3;
            }
            set
            {
                this.bool_3 = value;
            }
        }
    }
}

