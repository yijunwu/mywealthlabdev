namespace Steema.TeeChart
{
    using System;
    using System.ComponentModel;

    internal class FrAccess : License
    {
        private string key;
        private FrAccessProvider owner;

        internal FrAccess(FrAccessProvider owner, string key)
        {
            this.owner = owner;
            this.key = key;
        }

        public override void Dispose()
        {
        }

        public override string LicenseKey
        {
            get
            {
                return this.key;
            }
        }
    }
}

