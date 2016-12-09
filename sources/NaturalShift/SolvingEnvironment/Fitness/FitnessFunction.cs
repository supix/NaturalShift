// Copyright (c) 2016 - esposito.marce@gmail.com
// This file is part of NaturalShift.
// 
// NaturalShift is free software: you can redistribute it and/or modify
// it under the terms of the Affero GNU General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// Foobar is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

using GAF;
using NaturalShift.SolvingEnvironment.Chromosomes;
using NaturalShift.SolvingEnvironment.Matrix;
using System;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class FitnessFunction
    {
        private readonly ChromosomeProcessor processor;
        private readonly FitnessEvaluator evaluator;
        private readonly ShiftMatrix matrix;
        private Single[] zeroToOneArray = null;

        public FitnessFunction(ChromosomeProcessor processor, FitnessEvaluator evaluator, ShiftMatrix matrix)
        {
            this.processor = processor;
            this.evaluator = evaluator;
            this.matrix = matrix;
        }

        public double Evaluate(Chromosome chromosome)
        {
            if (zeroToOneArray == null)
                zeroToOneArray = new Single[chromosome.Count];

            int i = 0;
            foreach (var g in chromosome.Genes)
            {
                zeroToOneArray[i++] = (Single)g.RealValue;
            }

            processor.ProcessChromosome(zeroToOneArray);
            return evaluator.Evaluate(matrix);
        }
    }
}