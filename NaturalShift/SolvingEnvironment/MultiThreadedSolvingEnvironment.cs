using NaturalShift.Model.ProblemModel;
using NaturalShift.Model.SolutionModel;
using NaturalShift.SolvingEnvironment.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NaturalShift.SolvingEnvironment
{
    internal class MultiThreadedSolvingEnvironment : ISolvingEnvironment
    {
        private static readonly IInternalLogger log = LoggerProvider.LoggerFor(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Problem problem;
        private readonly EnvironmentConfig environmentConfig;
        private readonly int numberOfThreads;

        private double bestFitnessSoFar = 0;
        private object lockObj = new Object();

        public MultiThreadedSolvingEnvironment(Problem problem, EnvironmentConfig environmentConfig, int numberOfThreads = 0)
        {
            if (problem == null)
                throw new ArgumentNullException(nameof(problem));

            if (environmentConfig == null)
                throw new ArgumentNullException(nameof(problem));

            if (numberOfThreads == 1)
                throw new ArgumentException("Cannot be equal to 1", nameof(numberOfThreads));

            this.problem = problem;
            this.environmentConfig = environmentConfig;
            this.numberOfThreads = numberOfThreads > 0 ? numberOfThreads : System.Environment.ProcessorCount;
        }

        public ISolution Solve()
        {
            var solvers = new List<SolverThread>();
            for (int i = 0; i < numberOfThreads; i++)
            {
                var solver = new SolverThread(this.problem, this.environmentConfig);
                solver.OnFitnessImprovement += Solver_OnFitnessImprovement;
                solvers.Add(solver);
            }

            solvers.ForEach(x => x.Solve());

            while (solvers.Any(s => s.IsActive))
                Thread.Sleep(250);

            var bestSolution = solvers.OrderByDescending(s => s.BestSolution.Fitness).First().BestSolution;

            return bestSolution;
        }

        private void Solver_OnFitnessImprovement(double bestFitness, double averageFitness)
        {
            lock (lockObj)
            {
                if (bestFitness > bestFitnessSoFar)
                {
                    bestFitnessSoFar = bestFitness;
                    log.DebugFormat("Best fitness found: {0}. Average: {1}", bestFitness, averageFitness);
                }
            }
        }

        internal int NumberOfThreads { get { return this.numberOfThreads; } }
    }
}