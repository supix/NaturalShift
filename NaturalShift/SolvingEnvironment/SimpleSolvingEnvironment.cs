using NaturalShift.Model.ProblemModel;
using NaturalShift.Model.SolutionModel;
using NaturalShift.SolvingEnvironment.Utils;
using System;
using System.Diagnostics;

namespace NaturalShift.SolvingEnvironment
{
    internal class SimpleSolvingEnvironment : ISolvingEnvironment
    {
        private static readonly IInternalLogger log = LoggerProvider.LoggerFor(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ComputationTerminationManager computationTerminationManager;
        private readonly EnvironmentConfig environmentConfig;
        private readonly Problem problem;

        public SimpleSolvingEnvironment(Problem problem,
            EnvironmentConfig environmentConfig)
        {
            if (problem == null)
                throw new ArgumentNullException(nameof(problem));

            if (environmentConfig == null)
                throw new ArgumentNullException(nameof(environmentConfig));

            this.computationTerminationManager = new ComputationTerminationManager(
                environmentConfig.MaxExecutionTimeMilliseconds,
                environmentConfig.MaxEpochs,
                environmentConfig.MaxEpochsWithoutFitnessImprovement);
            this.environmentConfig = environmentConfig;
            this.problem = problem;
        }

        public ISolution Solve()
        {
            var totalElapsedEpochs = 0;
            ISolution bestSolutionSoFar = null;
            var sw = new Stopwatch();
            sw.Start();

            do
            {
                var theWorld = new TheWorld(this.problem, this.environmentConfig.PopulationSize);

                var bestSolution = theWorld.EvolveUntil((epochs, epochsWithoutFitnessImprovement) =>
                {
                    totalElapsedEpochs++;
                    return computationTerminationManager.Terminated((int)sw.ElapsedMilliseconds, totalElapsedEpochs, epochsWithoutFitnessImprovement);
                });

                if ((bestSolutionSoFar == null) || (bestSolutionSoFar.Fitness < bestSolution.Fitness))
                    bestSolutionSoFar = bestSolution;
            } while (!computationTerminationManager.Terminated((int)sw.ElapsedMilliseconds, totalElapsedEpochs));

            return bestSolutionSoFar;
        }
    }
}