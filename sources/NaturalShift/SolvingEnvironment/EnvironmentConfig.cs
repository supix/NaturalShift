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