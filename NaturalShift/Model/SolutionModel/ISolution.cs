namespace NaturalShift.Model.SolutionModel
{
    public interface ISolution
    {
        /// <summary>
        /// The matrix [item, day] whose value is the slot index covered (if any).
        /// </summary>
        int?[,] Allocations { get; }

        /// <summary>
        /// The fitness of the solution
        /// </summary>
        double Fitness { get; }

        /// <summary>
        /// Indicates how many solutions have been approximately evaluated to compute the current instance
        /// </summary>
        int EvaluatedSolutions { get; }

        /// <summary>
        /// Returns solution in a human readable format
        /// </summary>
        /// <returns>The string to be printed</returns>
        string ToString();
    }
}