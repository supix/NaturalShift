namespace NaturalShift.Model.SolutionModel
{
    public interface ISolution
    {
        /// <summary>
        /// The matrix [item, day] whose value is the slot index covered (if any).
        /// </summary>
        int?[,] Allocations { get; set; }

        /// <summary>
        /// The fitness of the solution
        /// </summary>
        double Fitness { get; set; }

        /// <summary>
        /// Returns solution in a human readable format
        /// </summary>
        /// <returns>The string to be printed</returns>
        string ToString();
    }
}