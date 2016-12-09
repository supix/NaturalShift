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
    internal class ConsecutiveSlotAptitudeEnforcer : IConstraintEnforcer
    {
        private readonly Single[,] multipliers;
        private readonly int[] slotLengths;

        public ConsecutiveSlotAptitudeEnforcer(Single[,] multipliers, int[] slotLengths)
        {
            this.multipliers = multipliers;
            this.slotLengths = slotLengths;
        }

        public void EnforceConstraint(ShiftMatrix matrix, int day, int slot)
        {
            int? chosenItem = matrix[day, slot].ChosenItem;
            if (chosenItem.HasValue)
            {
                int nextDayToEnforce = day + slotLengths[slot];
                if (nextDayToEnforce < matrix.Days)
                {
                    for (int s = 0; s < matrix.Slots; s++)
                    {
                        var all = matrix[nextDayToEnforce, s];
                        if (!all.Processed)
                        {
                            all.CurrentAptitudes[chosenItem.Value] =
                                all.CurrentAptitudes[chosenItem.Value]
                                * multipliers[slot, s];
                        }
                    }
                }
            }
        }
    }
}