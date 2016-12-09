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

        public static ISolution Build(Double fitness, ShiftMatrix m, int evaluatedSolutions)
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
                Allocations = r,
                EvaluatedSolutions = evaluatedSolutions
            };
        }
    }
}