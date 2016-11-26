using NaturalShift.Model.ProblemModel;
using NaturalShift.Model.SolutionModel;
using System.Threading;

namespace NaturalShift.SolvingEnvironment
{
    internal class SolverThread
    {
        private readonly SimpleSolvingEnvironment solvingEnvironment;
        private Thread thread;
        public ISolution bestSolution;

        public bool IsActive { get { return thread != null; } }
        public SimpleSolvingEnvironment SolvingEnvironment { get { return solvingEnvironment; } }
        public ISolution BestSolution { get { return this.bestSolution; } }

        internal SolverThread(Problem problem, EnvironmentConfig environmentConfig)
        {
            this.solvingEnvironment = new SimpleSolvingEnvironment(problem, environmentConfig, false);
            this.solvingEnvironment.OnFitnessImprovement += SolvingEnvironment_OnFitnessImprovement;
            this.thread = null;
        }

        private void SolvingEnvironment_OnFitnessImprovement(double bestFitness, double averageFitness)
        {
            OnFitnessImprovement?.Invoke(bestFitness, averageFitness);
        }

        internal void Solve()
        {
            thread = new Thread(new ThreadStart(DoIt));
            thread.Start();
        }

        internal void DoIt()
        {
            this.bestSolution = solvingEnvironment.Solve();
            this.thread = null;
        }

        public delegate void FitnessImprovement(double bestFitness, double averageFitness);

        public event FitnessImprovement OnFitnessImprovement;
    }
}