using NaturalShift.Model.ProblemModel;
using NaturalShift.Model.SolutionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NaturalShift.SolvingEnvironment
{
    internal class MultiThreadedSolvingEnvironment : ISolvingEnvironment
    {
        private readonly Problem problem;
        private readonly EnvironmentConfig environmentConfig;
        private readonly int numberOfThreads;

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
                solvers.Add(solver);
            }

            solvers.ForEach(x => x.Solve());

            while (solvers.Any(s => s.IsActive))
                Thread.Sleep(250);

            var bestSolution = solvers.OrderByDescending(s => s.BestSolution.Fitness).First().BestSolution;

            return bestSolution;
        }

        internal int NumberOfThreads { get { return this.numberOfThreads; } }
    }
}