using System.Text;

namespace NaturalShift.Model.SolutionModel
{
    public class Solution : ISolution
    {
        public double Fitness { get; set; }
        public int?[,] Allocations { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Fitness: ");
            sb.AppendLine(Fitness.ToString());
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