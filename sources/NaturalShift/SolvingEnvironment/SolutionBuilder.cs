//-----------------------------------------------------------------------
// <copyright file="SolutionBuilder.cs" company="supix">
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
using System;
using System.Diagnostics;
using NaturalShift.Model.SolutionModel;
using NaturalShift.SolvingEnvironment.Matrix;
using NaturalShift.SolvingEnvironment.Utils;

namespace NaturalShift.SolvingEnvironment
{
    internal static class SolutionBuilder
    {
        private static readonly IInternalLogger log = LoggerProvider.LoggerFor(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ISolution Build(Double fitness, ShiftMatrix m, int evaluatedSolutions, int numberOfItems)
        {
            var sw = new Stopwatch();
            sw.Start();

            var r = new int?[m.Days, m.Slots];

            for (int day = 0; day < m.Days; day++)
                for (int slot = 0; slot < m.Slots; slot++)
                    r[day, slot] = null;

            for (int day = 0; day < m.Days; day++)
                for (int slot = 0; slot < m.Slots; slot++)
                {
                    r[day, slot] = m[day, slot].ChosenItem;
                }

            sw.Stop();
            log.DebugFormat("Solution built in {0} ms", sw.ElapsedMilliseconds);

            return new Solution(numberOfItems)
            {
                Fitness = fitness,
                Allocations = r,
                EvaluatedSolutions = evaluatedSolutions
            };
        }
    }
}