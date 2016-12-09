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

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal class ItemIsBusyForSlotLength : IConstraintEnforcer
    {
        private readonly int[] lengths;

        public ItemIsBusyForSlotLength(int[] lengths)
        {
            this.lengths = lengths;
        }

        public void EnforceConstraint(ShiftMatrix matrix, int day, int slot)
        {
            if (matrix[day, slot].ChosenItem.HasValue)
            {
                var untilDay = day + lengths[slot] - 1;
                if (untilDay >= matrix.Days)
                    untilDay = matrix.Days - 1;

                for (int d = day + 1; d <= untilDay; d++)
                    for (int s = 0; s < matrix.Slots; s++)
                    {
                        var all = matrix[d, s];
                        if (!all.Processed)
                            all.CurrentAptitudes[matrix[day, slot].ChosenItem.Value] = 0;
                    }
            }
        }
    }
}