//-----------------------------------------------------------------------
// <copyright file="ConstraintsBuilder.cs" company="supix">
//
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
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using NaturalShift.Model.ProblemModel;

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal static class ConstraintsBuilder
    {
        public static IList<IConstraintEnforcer> Build(Problem problem)
        {
            var enforcers = new List<IConstraintEnforcer>();

            //ConsecutiveSlotAptitude
            if ((problem.ConsecutiveSlotAptitudes != null) && (problem.SlotLengths != null))
                enforcers.Add(new ConsecutiveSlotAptitudeEnforcer(
                    problem.ConsecutiveSlotAptitudes,
                    problem.SlotLengths));

            //CrossItemAptitude
            if (problem.CrossItemAptitudes != null)
                enforcers.Add(new CrossItemAptitudeEnforcer(problem.CrossItemAptitudes));

            //ItemIsBusyForSlotLength
            if (problem.SlotLengths != null)
                enforcers.Add(new ItemIsBusyForSlotLength(problem.SlotLengths));

            //MaxWorkingDays
            if ((problem.MaxConsecutiveWorkingDays > 0) && (problem.RestAfterMaxWorkingDaysReached > 0))
                enforcers.Add(
                    new MaxWorkingDaysEnforcer(
                        problem.MaxConsecutiveWorkingDays,
                        problem.RestAfterMaxWorkingDaysReached));

            //NoUbiquityOverIncompatibleSlots
            enforcers.Add(
                new NoUbiquityOverIncompatibleSlots(
                    problem.CompatibleSlots,
                    problem.SlotLengths,
                    problem.Slots));

            return enforcers;
        }
    }
}