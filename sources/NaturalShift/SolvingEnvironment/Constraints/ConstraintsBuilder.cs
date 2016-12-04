using NaturalShift.Model.ProblemModel;
using System.Collections.Generic;

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