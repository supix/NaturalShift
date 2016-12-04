namespace NaturalShift.SolvingEnvironment
{
    internal class EnvironmentConfig
    {
        /// <summary>
        /// Default constructor setting reasonable defaults for configuration values
        /// </summary>
        public EnvironmentConfig()
        {
            this.MaxExecutionTimeMilliseconds = 0;
            this.MaxEpochs = 0;
            this.PopulationSize = 100;
            this.MaxEpochsWithoutFitnessImprovement = 0;
        }

        /// <summary>
        /// Maximum allowed time for a computation. When the time expires, the best found solution is returned.
        /// </summary>
        public int MaxExecutionTimeMilliseconds { get; set; }

        /// <summary>
        /// Number of chromosomes created for each population
        /// </summary>
        public int PopulationSize { get; set; }

        /// <summary>
        /// Maximum allowed number of epochs for a population. Zero allows unlimited epochs.
        /// </summary>
        public int MaxEpochs { get; set; }

        /// <summary>
        /// Maximum allowed epochs without fitness improvement, after which a new population is created.
        /// </summary>
        public int MaxEpochsWithoutFitnessImprovement { get; set; }
    }
}