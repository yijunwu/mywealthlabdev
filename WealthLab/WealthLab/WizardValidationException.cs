namespace WealthLab
{
    using System;

    public class WizardValidationException : ArgumentException
    {
        public WizardValidationException(string message) : base(message)
        {
        }
    }
}

