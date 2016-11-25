using NaturalShift.SolvingEnvironment.Matrix;
using System;
using System.Linq;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class ItemEffortsFairness : IFitnessDimension
    {
        private readonly Single[] itemStartupEfforts;
        private readonly Single[] itemWeights;
        private readonly Single[] slotWeights;
        private Single[] _itemEfforts;
        private readonly Single normalizer;

        public ItemEffortsFairness(Single[] itemStartupEfforts, Single[] itemWeights, Single[] slotWeights, int days, int slots, int items)
        {
            this.itemStartupEfforts = itemStartupEfforts != null ? itemStartupEfforts : new Single[items];
            if (itemWeights != null)
                this.itemWeights = itemWeights;
            else
            {
                this.itemWeights = new Single[items];
                for (int i = 0; i < items; i++)
                    this.itemWeights[i] = 1;
            }

            if (slotWeights != null)
                this.slotWeights = slotWeights;
            else
            {
                this.slotWeights = new Single[slots];
                for (int i = 0; i < slots; i++)
                    this.slotWeights[i] = 1;
            }

            _itemEfforts = new Single[this.itemStartupEfforts.Length];

            //compute normalizer as estimated upper bound
            Array.Copy(this.itemStartupEfforts, this._itemEfforts, this.itemStartupEfforts.Length);
            Single maxSlotWeight = this.slotWeights.Max();
            for (int i = 0; i < this.itemStartupEfforts.Length; i++)
                _itemEfforts[i] += this.itemWeights[i] * maxSlotWeight * days;
            normalizer = Utils.MathUtils.StandardDeviation(new Single[] { 0, _itemEfforts.Max() }) / 10F;
        }

        public float Evaluate(ShiftMatrix matrix)
        {
            Array.Copy(itemStartupEfforts, _itemEfforts, itemStartupEfforts.Length);

            for (int day = 0; day < matrix.Days; day++)
                for (int slot = 0; slot < matrix.Slots; slot++)
                {
                    var all = matrix[day, slot];
                    if (all.ChosenItem.HasValue)
                    {
                        _itemEfforts[all.ChosenItem.Value] += slotWeights[slot] * itemWeights[all.ChosenItem.Value];
                    }
                }

            Single result = 1 - (Utils.MathUtils.StandardDeviation(_itemEfforts) / normalizer);
            return result > 0 ? result : 0F;
        }
    }
}