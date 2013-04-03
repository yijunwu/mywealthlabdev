namespace WealthLab.Strategies
{
    using System;
    using WealthLab;

    public abstract class WebServiceStrategyHelper : StrategyHelper
    {
        protected WebServiceStrategyHelper()
        {
        }

        public abstract string BenefitsIssues { get; }

        public abstract string Category { get; }

        public abstract string CategoryBenefitsIssues { get; }

        public abstract string CategoryDescription { get; }

        public abstract string CategoryEReview { get; }

        public abstract string CategoryExampleImageDescription { get; }

        public abstract string CategoryExampleImageURI { get; }

        public abstract string CategoryFooter { get; }

        public abstract string EReview { get; }

        public abstract string ExampleImageDescription { get; }

        public abstract string ExampleImageURI { get; }

        public abstract string Footer { get; }

        public abstract string New { get; }

        public abstract string Spotlight { get; }
    }
}

