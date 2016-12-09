// NaturalShift is an AI based engine to compute workshifts.
// Copyright (C) 2016 - Marcello Esposito (esposito.marce@gmail.com)
// 
// This file is part of NaturalShift.
// NaturalShift is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// NaturalShift is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using GAF;
using GAF.Operators;
using GAF.Threading;
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
    /// This class hosts the evolution of a single population, until reaching the termination condition. This class encapsulates genetic algorithm functionalities.
    /// </summary>
    internal class TheWorld
    {
        private static readonly IInternalLogger log = LoggerProvider.LoggerFor(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Problem problem;
        private readonly int populationSize;
        private double overallBestFitness;
        private int epochs;
        private int epochsWithoutFitnessImprovement;

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
            var rnd = RandomProvider.GetThreadRandom();
            var shiftMatrix = MatrixBuilder.Build(this.problem);
            var enumerator = new IncreasingRowsRandomColumns(this.problem.Days, this.problem.Slots);
            var constraints = ConstraintsBuilder.Build(this.problem);
            var chromoProcessor = new ChromosomeProcessor(shiftMatrix, enumerator, constraints);
            var evaluator = new FitnessEvaluator(problem);
            var fitnessFunction = new Fitness.FitnessFunction(chromoProcessor, evaluator, shiftMatrix);
            var chromosomeLength = shiftMatrix.GetNumberOfUnforcedSlots();
            const double crossoverProbability = 0.90;
            const double mutationProbability = 0.05;
            const int elitismPercentage = 5;
            epochs = 1;
            epochsWithoutFitnessImprovement = 0;
            overallBestFitness = -1;

            log.Debug("Starting population.");
            var population = new Population();
            for (var i = 0; i < populationSize; i++)
            {
                var c = new Double[chromosomeLength];
                for (var k = 0; k < chromosomeLength; k++)
                    c[k] = rnd.NextDouble();
                var ch = new Chromosome(c);
                population.Solutions.Add(ch);
            }

            //create the genetic operators
            var elite = new Elite(elitismPercentage);

            var crossover = new Crossover(crossoverProbability, true)
            {
                CrossoverType = CrossoverType.SinglePoint
            };

            var mutation = new SwapMutate(mutationProbability);

            //create the GA itself
            var ga = new GeneticAlgorithm(population, fitnessFunction.Evaluate);

            //subscribe to the GAs Generation Complete event
            ga.OnGenerationComplete += Ga_OnGenerationComplete;

            //add the operators to the ga process pipeline
            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutation);

            //run the GA
            ga.Run((pop, currentGeneration, currentEvaluation) =>
            {
                return terminationCondition(currentGeneration, epochsWithoutFitnessImprovement, population.MaximumFitness, population.AverageFitness);
            });

            population.GetTop(1)[0].Evaluate(fitnessFunction.Evaluate);
            return SolutionBuilder.Build(overallBestFitness, shiftMatrix, epochs * population.PopulationSize);
        }

        private void Ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            epochs++;
            if (e.Population.MaximumFitness > overallBestFitness)
            {
                //log.DebugFormat("Fitness improvement! Epoch {0}. Best fitness: {1} - average {2} - Fitness stable from epochs: {3}",
                //    e.Generation,
                //    e.Population.MaximumFitness,
                //    e.Population.AverageFitness,
                //    epochsWithoutFitnessImprovement);
                overallBestFitness = e.Population.MaximumFitness;
                epochsWithoutFitnessImprovement = 0;
            }
            else
                epochsWithoutFitnessImprovement++;
        }

        public delegate bool TerminationCondition(int epochs, int epochsWithoutFitnessImprovement, double bestFitness, double averageFitness);
    }
}