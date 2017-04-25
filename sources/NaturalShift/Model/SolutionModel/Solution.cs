//-----------------------------------------------------------------------
// <copyright file="Solution.cs" company="supix">
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
using System.Collections.Generic;
using System.Text;

namespace NaturalShift.Model.SolutionModel
{
    public class Solution : ISolution
    {
        private readonly int numberOfItems;

        public Solution(int numberOfItems)
        {
            this.numberOfItems = numberOfItems;
        }

        public double Fitness { get; set; }
        public int?[,] Allocations { get; set; }
        public int EvaluatedSolutions { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Fitness: ");
            sb.AppendLine(Fitness.ToString());
            sb.Append("Evaluated solutions: ");
            sb.AppendLine(EvaluatedSolutions.ToString());

            for (int item = 0; item < numberOfItems; item++)
            {
                for (int day = 0; day < Allocations.GetLength(0); day++)
                {
                    var shifts = new List<int>();
                    for (int slot = 0; slot < Allocations.GetLength(1); slot++)
                    {
                        if (Allocations[day, slot] == item)
                            shifts.Add(slot);
                    }

                    sb.Append(shiftsToString(shifts));

                    if (day < Allocations.GetLength(0) - 1)
                        sb.Append(", ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string shiftsToString(List<int> shifts)
        {
            if (shifts.Count == 0)
                return " -";

            if (shifts.Count == 1)
                return shifts[0].ToString().PadLeft(2);

            var joinedShifts = string.Join(",", shifts);
            return string.Format("({0})", joinedShifts);
        }
    }
}