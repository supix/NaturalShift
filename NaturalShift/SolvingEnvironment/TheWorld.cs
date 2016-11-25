using AForge.Genetic;
using NaturalShift.Model.ProblemModel;
using NaturalShift.Model.SolutionModel;
using NaturalShift.SolvingEnvironment.Chromosomes;
using NaturalShift.SolvingEnvironment.Constraints;
using NaturalShift.SolvingEnvironment.Fitness;
using NaturalShift.SolvingEnvironment.Matrix;
using NaturalShift.SolvingEnvironment.MatrixEnumerators;
using NaturalShift.SolvingEnvironment.Utils;
using System;

namespace NaturalShift.SolvingEnvironment
{
    /// <summary>
    /// This class hosts the evolution of a single population, until reaching the termination condition.
    /// </summary>
    internal class TheWorld
    {
        private static readonly IInternalLogger log = LoggerProvider.LoggerFor(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Problem problem;
        private readonly int populationSize;

        public TheWorld(Problem problem, int populationSize)
        {
            if (problem == null)
                throw new ArgumentNullException(nameof(problem));

            if (populationSize <= 1)
                throw new ArgumentException(nameof(populationSize), "Must be greater than 1");

            this.problem = problem;
            this.populationSize = populationSize;
        }

        /// <summary>
        /// Creates a population and let it evolve until termination condition is reached.
        /// </summary>
        /// <param name="terminationCondition">Evolution termination condition evaluated on each new epoch</param>
        /// <returns>The best solution found across the evolution</returns>
        public ISolution EvolveUntil(TerminationCondition terminationCondition)
        {
            var shiftMatrix = MatrixBuilder.Build(this.problem);
            var enumerator = new IncreasingRowsRandomColumns(this.problem.Days, this.problem.Slots);
            var constraints = ConstraintsBuilder.Build(this.problem);
            var chromoProcessor = new ChromosomeProcessor(shiftMatrix, enumerator, constraints);
            var evaluator = new FitnessEvaluator(problem);
            var fitnessFunction = new FitnessFunction(chromoProcessor, evaluator, shiftMatrix);
            var epochIndex = 1;
            var epochsWithoutFitnessImprovement = 0;

            var population = new Population(
                this.populationSize,
                new ThreadSafeShortArrayChromosome(shiftMatrix.GetNumberOfUnforcedSlots()),
                fitnessFunction,
                new RouletteWheelSelection());
            population.RunEpoch();

            var overallBestChromosome = population.BestChromosome;
            var overallBestFitness = overallBestChromosome.Fitness;
            log.DebugFormat("Starting population. Best fitness: {0} - average {1}", overallBestFitness, population.FitnessAvg);

            while (!terminationCondition(epochIndex, epochsWithoutFitnessImprovement))
            {
                population.RunEpoch();
                epochIndex++;

                var bestChromosome = population.BestChromosome;
                var bestFitness = bestChromosome.Fitness;
                //log.DebugFormat("Epoch {0}. Best fitness: {1} - average {2} - Fitness stable from epochs: {3}",
                //    epochIndex,
                //    bestFitness,
                //    population.FitnessAvg,
                //    epochsWithoutFitnessImprovement);

                if (bestFitness > overallBestFitness)
                {
                    log.DebugFormat("Fitness improvement! Epoch {0}. Best fitness: {1} - average {2} - Fitness stable from epochs: {3}",
                        epochIndex,
                        bestFitness,
                        population.FitnessAvg,
                        epochsWithoutFitnessImprovement);
                    overallBestChromosome = bestChromosome;
                    overallBestFitness = bestFitness;
                    epochsWithoutFitnessImprovement = 0;
                }
                else
                    epochsWithoutFitnessImprovement++;
            }

            overallBestChromosome.Evaluate(fitnessFunction);
            return SolutionBuilder.Build(overallBestFitness, shiftMatrix);
        }

        public delegate bool TerminationCondition(int epochs, int epochsWithoutFitnessImprovement);
    }
}