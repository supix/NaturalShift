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

using NaturalShift.SolvingEnvironment.Matrix;
using System;
using System.Diagnostics;
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

            //compute normalizer as upper bound
            Array.Copy(this.itemStartupEfforts, this._itemEfforts, this.itemStartupEfforts.Length);
            Single maxSlotWeight = this.slotWeights.Max();
            for (int i = 0; i < this.itemStartupEfforts.Length; i++)
                _itemEfforts[i] += this.itemWeights[i] * maxSlotWeight * days;
            normalizer = Utils.MathUtils.StandardDeviation(new Single[] { 0, _itemEfforts.Max() });
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

            var result = 1 - (Utils.MathUtils.StandardDeviation(_itemEfforts) / normalizer);

            Debug.Assert(result >= 0 && result <= 1);

            return result;
        }
    }
}