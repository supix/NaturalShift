using NaturalShift.Model.ProblemModel;
using NaturalShift.SolvingEnvironment.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class FitnessEvaluator
    {
        private readonly List<IFitnessDimension> dimensions;

        public FitnessEvaluator(Problem problem)
        {
            dimensions = new List<IFitnessDimension>();
            dimensions.Add(new CurrentAptitudeFulfilled());
            dimensions.Add(new InitialAptitudeFulfilled());
            dimensions.Add(new BusySlotsAreGood(problem.SlotValues));
            dimensions.Add(new ItemEffortsFairness(
                problem.ItemStartupEfforts,
                problem.ItemWeights,
                problem.SlotWeights,
                problem.Days, problem.Slots, problem.Items));
            dimensions.Add(new SlotMixFairness(problem.Items));
        }

        public double Evaluate(ShiftMatrix matrix)
        {
            return dimensions.Average(x => x.Evaluate(matrix));
        }
    }
}