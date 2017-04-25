//-----------------------------------------------------------------------
// <copyright file="FitnessEvaluator.cs" company="supix">
//
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
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using NaturalShift.Model.ProblemModel;
using NaturalShift.SolvingEnvironment.Matrix;

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