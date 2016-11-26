using NaturalShift.Model.SolutionModel;
using NaturalShift.SolvingEnvironment.Matrix;
using NaturalShift.SolvingEnvironment.Utils;
using System;
using System.Diagnostics;

namespace NaturalShift.SolvingEnvironment
{
    internal static class SolutionBuilder
    {
        private static readonly IInternalLogger log = LoggerProvider.LoggerFor(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ISolution Build(Double fitness, ShiftMatrix m)
        {
            var sw = new Stopwatch();
            sw.Start();

            var r = new int?[m.Items, m.Days];

            for (int item = 0; item < m.Items; item++)
                for (int day = 0; day < m.Days; day++)
                    r[item, day] = null;

            for (int day = 0; day < m.Days; day++)
                for (int slot = 0; slot < m.Slots; slot++)
                {
                    var chosenItem = m[day, slot].ChosenItem;
                    if (chosenItem.HasValue)
                        r[chosenItem.Value, day] = slot;
                }

            sw.Stop();
            log.DebugFormat("Solution built in {0} ms", sw.ElapsedMilliseconds);

            return new Solution()
            {
                Fitness = fitness,
                Allocations = r
            };
        }
    }
}