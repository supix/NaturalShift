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
        private readonly bool logFitnessImprovement;

        public SimpleSolvingEnvironment(Problem problem,
            EnvironmentConfig environmentConfig, bool logFitnessImprovement = true)
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
            this.logFitnessImprovement = logFitnessImprovement;
        }

        public ISolution Solve()
        {
            var totalElapsedEpochs = 0;
            ISolution bestSolutionSoFar = null;
            double bestFitnessSoFar = 0;
            int overallEvaluatedSolutions = 0;
            var sw = new Stopwatch();
            sw.Start();

            do
            {
                var theWorld = new TheWorld(this.problem, this.environmentConfig.PopulationSize);

                var bestSolution = theWorld.EvolveUntil((epochs, epochsWithoutFitnessImprovement, bestFitness, averageFitness) =>
                {
                    if (bestFitness > bestFitnessSoFar)
                    {
                        bestFitnessSoFar = bestFitness;
                        if (logFitnessImprovement)
                            log.DebugFormat("Fitness improvement! Epoch {0}. Best fitness: {1} - average {2} - Fitness stable from epochs: {3}",
                                epochs,
                                bestFitness,
                                averageFitness,
                                epochsWithoutFitnessImprovement);
                        OnFitnessImprovement?.Invoke(bestFitness, averageFitness);
                    }

                    totalElapsedEpochs++;
                    return computationTerminationManager.Terminated((int)sw.ElapsedMilliseconds, totalElapsedEpochs, epochsWithoutFitnessImprovement);
                });

                overallEvaluatedSolutions += bestSolution.EvaluatedSolutions;

                if ((bestSolutionSoFar == null) || (bestSolutionSoFar.Fitness < bestSolution.Fitness))
                    bestSolutionSoFar = bestSolution;
            } while (!computationTerminationManager.Terminated((int)sw.ElapsedMilliseconds, totalElapsedEpochs));

            return new Solution()
            {
                Fitness = bestSolutionSoFar.Fitness,
                Allocations = bestSolutionSoFar.Allocations,
                EvaluatedSolutions = overallEvaluatedSolutions
            };
        }

        public delegate void FitnessImprovement(double bestFitness, double averageFitness);

        public event FitnessImprovement OnFitnessImprovement;
    }
}