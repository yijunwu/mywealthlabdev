namespace WealthLab.Optimizers
{
    using System;
    using WealthLab;

    public class Exhaustive : Optimizer
    {
        private OptResultsGraph1D _1D = new OptResultsGraph1D();
        private OptResultsGraph2D _2D = new OptResultsGraph2D();

        protected bool AdvanceParameter(WealthScript ws, int n)
        {
            if (n < 0)
            {
                return false;
            }
            if (ws.Parameters[n].Value >= ws.Parameters[n].Stop)
            {
                if (n == 0)
                {
                    return false;
                }
                ws.Parameters[n].Value = ws.Parameters[n].Start;
                return this.AdvanceParameter(ws, n - 1);
            }
            ws.Parameters[n].Value = double.Parse((decimal.Parse(ws.Parameters[n].Value.ToString()) + decimal.Parse(ws.Parameters[n].Step.ToString())).ToString());
            return true;
        }

        public override void FirstRun()
        {
            foreach (StrategyParameter parameter in base.WealthScript.Parameters)
            {
                parameter.Value = parameter.Start;
            }
        }

        public override void Initialize()
        {
            base.Host.CreateTab("1 Parameter Graph", this._1D);
            base.Host.CreateTab("2 Parameter Graph", this._2D);
        }

        public override bool NextRun(SystemPerformance sp, OptimizationResult or)
        {
            return this.AdvanceParameter(base.WealthScript, base.WealthScript.Parameters.Count - 1);
        }

        public override void RefreshViews()
        {
            this._1D.RefreshView();
            this._2D.RefreshView();
        }

        public override void RunCompleted(OptimizationResultList results)
        {
            this._1D.UpdateResults(results, base.WealthScript);
            this._1D.PrintHost = base.PrintHost;
            this._2D.UpdateResults(results, base.WealthScript);
            this._2D.PrintHost = base.PrintHost;
        }

        public override string Description
        {
            get
            {
                return "Brute force optimization that executes the Strategy using every possibly combination of Parameter values.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "Exhaustive";
            }
        }

        public override double NumberOfRuns
        {
            get
            {
                if (base.WealthScript == null)
                {
                    return 0.0;
                }
                double num = 1.0;
                foreach (StrategyParameter parameter in base.WealthScript.Parameters)
                {
                    num *= parameter.NumberOfRuns;
                }
                return num;
            }
        }
    }
}

