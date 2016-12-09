// NaturalShift is an AI based engine to compute workshifts.
// Copyright (C) 2016 - Marcello Esposito (esposito.marce@gmail.com)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using NaturalShift.SolvingEnvironment.Matrix;
using System;

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal class CrossItemAptitudeEnforcer : IConstraintEnforcer
    {
        private readonly float[,,,] multipliers; //indexes are slot1, slot2, item1, item2

        public CrossItemAptitudeEnforcer(Single[,,,] multipliers)
        {
            this.multipliers = multipliers;
        }

        public void EnforceConstraint(ShiftMatrix matrix, int day, int slot)
        {
            if (matrix[day, slot].ChosenItem.HasValue)
            {
                int item = matrix[day, slot].ChosenItem.Value;
                for (int s = 0; s < matrix.Slots; s++)
                    for (int i = 0; i < matrix.Items; i++)
                    {
                        if ((item != i) && (slot != s))
                        {
                            var all = matrix[day, s];
                            if (!all.Processed && multipliers[slot, s, item, i] != 1)
                            {
                                all.CurrentAptitudes[i] =
                                    all.CurrentAptitudes[i]
                                    * multipliers[slot, s, item, i];
                            }
                        }
                    }
            }
        }
    }
}