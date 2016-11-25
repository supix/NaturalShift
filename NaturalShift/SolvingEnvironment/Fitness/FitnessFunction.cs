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