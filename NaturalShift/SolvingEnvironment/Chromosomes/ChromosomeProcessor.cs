using NaturalShift.SolvingEnvironment.Constraints;
using NaturalShift.SolvingEnvironment.Matrix;
using NaturalShift.SolvingEnvironment.MatrixEnumerators;
using System;
using System.Collections.Generic;

namespace NaturalShift.SolvingEnvironment.Chromosomes
{
    internal class ChromosomeProcessor
    {
        private readonly ShiftMatrix matrix;
        private readonly IMatrixEnumerator enumerator;
        private readonly IList<IConstraintEnforcer> rules;
        private Single[] a = null;

        public ChromosomeProcessor(
            ShiftMatrix matrix,
            IMatrixEnumerator enumerator,
            IList<IConstraintEnforcer> rules)
        {
            this.matrix = matrix;
            this.enumerator = enumerator;
            this.rules = rules;
        }

        public void ProcessChromosome(Double[] fromZeroToOneArray)
        {
            if (a == null)
                a = new float[fromZeroToOneArray.Length];
            for (int i = 0; i < fromZeroToOneArray.Length; i++)
                a[i] = (Single)fromZeroToOneArray[i];
            ProcessChromosome(a);
        }

        public void ProcessChromosome(Single[] fromZeroToOneArray)
        {
            int idx = 0;
            int day, slot;
            matrix.ResetUnforcedAllocationStates();
            enumerator.Reset();
            while (enumerator.GetNext(out day, out slot))
            {
                if (!matrix[day, slot].Forced)
                {
                    Single gene = fromZeroToOneArray[idx++];
                    if (!matrix[day, slot].Processed)
                    {
                        int? chosenItem = matrix[day, slot].Process(gene);
                        foreach (var r in rules)
                            r.EnforceConstraint(matrix, day, slot);
                    }
                }
            }
            ////Test whether just the right number of genes have been provided (and used)
            //if (idx != fromZeroToOneArray.Length)
            //    throw new InvalidOperationException("Chromosome too long");
        }
    }
}