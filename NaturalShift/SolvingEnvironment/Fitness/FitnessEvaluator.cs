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
            dimensions.Add(new AptitudeFulfilled());
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
            var d0 = dimensions[0].Evaluate(matrix);
            var d1 = dimensions[1].Evaluate(matrix);
            var d2 = dimensions[2].Evaluate(matrix);
            var d3 = dimensions[3].Evaluate(matrix);

            return .6 * d0 +
                .2 * d1 +
                .2 * d2 +
                .2 * d3;
        }
    }
}