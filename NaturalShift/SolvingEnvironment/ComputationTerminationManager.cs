using NaturalShift.SolvingEnvironment.Utils;
using System;

namespace NaturalShift.SolvingEnvironment
{
    /// <summary>
    /// Controls the computation termination.
    /// </summary>
    internal class ComputationTerminationManager
    {
        private static readonly IInternalLogger log = LoggerProvider.LoggerFor(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly int maxComputationTimeInMilliseconds;
        private readonly int maxEpochs;
        private readonly int maxEpochsWithoutFitnessImprovement;

        /// <summary>
        /// Initializes the termination manager.
        /// </summary>
        /// <param name="maxComputationTimeInMilliseconds">Maximum computation time allowed. Zero means no limits.</param>
        /// <param name="maxEpochs">Maximum computed epochs. Zero means no limits.</param>
        /// <param name="maxEpochsWithoutFitnessImprovement">Maximum consecutive epochs without fitness improvement. Zero means no limits.</param>
        public ComputationTerminationManager(int maxComputationTimeInMilliseconds,
            int maxEpochs,
            int maxEpochsWithoutFitnessImprovement)
        {
            if (maxComputationTimeInMilliseconds < 0)
                throw new ArgumentException(nameof(maxComputationTimeInMilliseconds), "Must be non negative");

            if (maxEpochs < 0)
                throw new ArgumentException(nameof(maxEpochs), "Must be non negative");

            if (maxEpochsWithoutFitnessImprovement < 0)
                throw new ArgumentException(nameof(maxEpochsWithoutFitnessImprovement), "Must be non negative");

            if ((maxComputationTimeInMilliseconds == 0) && (maxEpochs == 0) && (maxEpochsWithoutFitnessImprovement == 0))
                throw new ArgumentException(string.Format("{0}, {1} and {2} cannot be all null since computation would never end",
                    nameof(maxComputationTimeInMilliseconds),
                    nameof(maxEpochs),
                    nameof(maxEpochsWithoutFitnessImprovement)));

            this.maxComputationTimeInMilliseconds = maxComputationTimeInMilliseconds;
            this.maxEpochs = maxEpochs;
            this.maxEpochsWithoutFitnessImprovement = maxEpochsWithoutFitnessImprovement;
        }

        /// <summary>
        /// Indicates whether the evolution must terminate by checking the elapsed time,
        /// the current epoch index and the number of epochs without fitness improvement.
        /// </summary>
        /// <param name="elapsedMilliseconds">The execution time, so far</param>
        /// <param name="epochIndex">Index of the current epoch</param>
        /// <param name="epochsWithoutFitnessImprovement">Number of epochs without fitness improvement</param>
        /// <returns>The predicate about termination</returns>
        public bool Terminated(int elapsedMilliseconds, int epochIndex, int epochsWithoutFitnessImprovement = 0)
        {
            if ((this.maxComputationTimeInMilliseconds > 0) && (elapsedMilliseconds >= maxComputationTimeInMilliseconds))
            {
                log.Debug("Max computation time reached. Evolution terminated.");
                return true;
            }

            if ((this.maxEpochs > 0) && (epochIndex >= maxEpochs))
            {
                log.Debug("Max epochs reached. Evolution terminated.");
                return true;
            }

            if ((this.maxEpochsWithoutFitnessImprovement > 0) && (epochsWithoutFitnessImprovement >= maxEpochsWithoutFitnessImprovement))
            {
                log.Debug("Max epochs without fitness improvement reached. Evolution terminated.");
                return true;
            }

            return false;
        }
    }
}