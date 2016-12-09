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

using System.Text;

namespace NaturalShift.Model.SolutionModel
{
    public class Solution : ISolution
    {
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
            for (int item = 0; item < Allocations.GetLength(0); item++)
            {
                for (int day = 0; day < Allocations.GetLength(1); day++)
                {
                    sb.Append(Allocations[item, day].HasValue ? Allocations[item, day].Value.ToString().PadLeft(3) : "  -");
                    if (day < Allocations.GetLength(1) - 1)
                        sb.Append(",");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}