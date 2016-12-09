// NaturalShift is an AI based engine to compute workshifts.
// Copyright (C) 2016 - Marcello Esposito (esposito.marce@gmail.com)
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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