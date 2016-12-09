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

using NaturalShift.SolvingEnvironment.Matrix;

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal class NoUbiquityOverIncompatibleSlots : IConstraintEnforcer
    {
        private readonly bool[,] compatibleSlots;
        private readonly int[] slotLengths;

        public NoUbiquityOverIncompatibleSlots(bool[,] compatibleSlots, int[] slotLengths, int slots)
        {
            this.compatibleSlots = compatibleSlots;
            this.slotLengths = slotLengths;

            if (this.compatibleSlots == null)
                this.compatibleSlots = new bool[slots, slots];

            if (this.slotLengths == null)
            {
                this.slotLengths = new int[slots];
                for (int i = 0; i < slots; i++)
                    this.slotLengths[i] = 1;
            }
        }

        public void EnforceConstraint(ShiftMatrix matrix, int day, int slot)
        {
            if (matrix[day, slot].ChosenItem.HasValue)
            {
                var chosenItem = matrix[day, slot].ChosenItem.Value;
                for (int d = day; d < day + slotLengths[slot]; d++)
                {
                    for (int s = 0; s < matrix.Slots; s++)
                    {
                        var all = matrix[day, s];
                        if ((!all.Processed) && (!compatibleSlots[slot, s]))
                            all.CurrentAptitudes[chosenItem] = 0;
                    }
                }
            }
        }
    }
}