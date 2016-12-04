using NaturalShift.Model.ProblemModel;

namespace NaturalShift.SolvingEnvironment.Matrix
{
    internal static class MatrixBuilder
    {
        public static ShiftMatrix Build(Problem problem)
        {
            var m = new ShiftMatrix(problem.Days, problem.Slots, problem.Items, problem.DefaultAptitude);

            //SlotClosures
            if ((problem.SlotClosures != null) && (problem.SlotClosures.Count > 0))
            {
                //foreach (var sc in p.SlotClosures)
                for (var i = 0; i < problem.SlotClosures.Count; i++) //for instead of foreach for thread-safety
                {
                    var sc = problem.SlotClosures[i];
                    for (int day = sc.Days.From; day <= sc.Days.To; day++)
                        for (int slot = sc.Slots.From; slot <= sc.Slots.To; slot++)
                            m[day, slot].Force(null);
                }
            }

            //Aptitudes
            if ((problem.Aptitudes != null) && (problem.Aptitudes.Count > 0))
            {
                for (var i = 0; i < problem.Aptitudes.Count; i++) //for instead of foreach for thread-safety
                {
                    var apt = problem.Aptitudes[i];
                    for (int day = apt.Days.From; day <= apt.Days.To; day++)
                        for (int slot = apt.Slots.From; slot <= apt.Slots.To; slot++)
                            for (int item = apt.Items.From; item <= apt.Items.To; item++)
                            {
                                m[day, slot].InitialAptitudes[item] = apt.Aptitude;
                            }
                }
            }

            //ItemsUnavailabilities
            if ((problem.ItemsUnavailabilities != null) && (problem.ItemsUnavailabilities.Count > 0))
            {
                //foreach (var iu in p.ItemsUnavailabilities)
                for (var i = 0; i < problem.ItemsUnavailabilities.Count; i++) //for instead of foreach for thread-safety
                {
                    var iu = problem.ItemsUnavailabilities[i];
                    for (int slot = iu.Slots.From; slot <= iu.Slots.To; slot++)
                    {
                        //effective starting day decreases according to slot length (to prevent long slots overlapping unavailabilities)
                        var fromDay = iu.Days.From - problem.SlotLengths[slot] + 1;
                        if (fromDay < 0)
                            fromDay = 0;

                        for (int day = fromDay; day <= iu.Days.To; day++)
                            for (int item = iu.Items.From; item <= iu.Items.To; item++)
                                m[day, slot].InitialAptitudes[item] = 0;
                    }
                }
            }

            return m;
        }
    }
}