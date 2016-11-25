using System;
using System.Linq;

namespace NaturalShift.SolvingEnvironment.Utils
{
    internal static class MathUtils
    {
        /// <summary>
        /// Get the variance
        /// </summary>
        /// <param name="nums">The Single series to calculate the variance from</param>
        /// <returns>The variance</returns>
        public static Single Variance(Single[] nums)
        {
            if (nums.Length > 1)
            {
                // Get the average of the values
                Single avg = nums.Average();

                // Now figure out how far each point is from the mean
                // So we subtract from the number the average
                // Then raise it to the power of 2
                Single sumOfSquares = nums.Select(val => (val - avg) * (val - avg)).Sum();

                // Finally divide it by n - 1 (for standard deviation variance)
                // Or use length without subtracting one ( for population standard deviation variance)
                return sumOfSquares / (Single)(nums.Length - 1);
            }
            else { return 0.0F; }
        }

        /// <summary>
        /// Get the variance
        /// </summary>
        /// <param name="nums">The int series to calculate the variance from</param>
        /// <returns>The variance</returns>
        public static Single Variance(int[] nums)
        {
            if (nums.Length > 1)
            {
                // Get the average of the values
                Single avg = (Single)nums.Average();

                // Now figure out how far each point is from the mean
                // So we subtract from the number the average
                // Then raise it to the power of 2
                Single sumOfSquares = 0.0F;

                foreach (int num in nums)
                {
                    sumOfSquares += (Single)Math.Pow((num - avg), 2.0);
                }

                // Finally divide it by n - 1 (for standard deviation variance)
                // Or use length without subtracting one ( for population standard deviation variance)
                return sumOfSquares / (Single)(nums.Length - 1);
            }
            else { return 0.0F; }
        }

        /// <summary>
        ///  Get the standard deviation
        /// </summary>
        /// <param name="nums">The Single series to calculate the variance from</param>
        /// <returns>The standard deviation</returns>
        public static Single StandardDeviation(Single[] nums)
        {
            return (Single)Math.Sqrt(Variance(nums));
        }

        /// <summary>
        ///  Get the standard deviation
        /// </summary>
        /// <param name="nums">The int series to calculate the variance from</param>
        /// <returns>The standard deviation</returns>
        public static Single StandardDeviation(int[] nums)
        {
            return (Single)Math.Sqrt(Variance(nums));
        }
    }
}