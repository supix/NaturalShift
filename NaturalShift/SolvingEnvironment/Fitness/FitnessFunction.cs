using AForge.Genetic;
using NaturalShift.SolvingEnvironment.Chromosomes;
using NaturalShift.SolvingEnvironment.Matrix;
using System;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class FitnessFunction : IFitnessFunction
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

        public double Evaluate(IChromosome chromosome)
        {
            var dac = ((ThreadSafeShortArrayChromosome)chromosome);

            //bound chromosome to 1
#warning use a chromosome intrinsically bounded to 1
            if (zeroToOneArray == null)
                zeroToOneArray = new Single[dac.Length];

            for (int i = 0; i < zeroToOneArray.Length; i++)
            {
                zeroToOneArray[i] = dac.Value[i] / (Single)ushort.MaxValue;
            }

            processor.ProcessChromosome(zeroToOneArray);
            return evaluator.Evaluate(matrix);
        }
    }
}