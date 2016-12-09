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
    public interface IConfigurableSolvingEnvironment
    {
        /// <summary>
        /// Build the solving environment
        /// </summary>
        /// <returns>The configured solving environment</returns>
        ISolvingEnvironment Build();

        /// <summary>
        /// Close current population and start a new one after a number of epochs
        /// </summary>
        /// <param name="epochs">Maximum number of epochs</param>
        /// <returns>The fluent interface</returns>
        IConfigurableSolvingEnvironment RenewingPopulationAfterEpochs(int epochs);

        /// <summary>
        /// Close current population and start a new one after a consecutive epochs without fitness improvement
        /// </summary>
        /// <param name="epochs">Maximum number of epochs</param>
        /// <returns>The fluent interface</returns>
        IConfigurableSolvingEnvironment RenewingPopulationAfterSameFitnessEpochs(int epochs);

        /// <summary>
        /// Start configuring duration of computation
        /// </summary>
        /// <param name="after">Elapsed time</param>
        /// <returns>The fluent interface</returns>
        IConfiguringTime StoppingComputationAfter(int after);

        /// <summary>
        /// Start configuring multithreading
        /// </summary>
        /// <returns>The fluent interface</returns>
        IConfigurableSolvingEnvironment UsingExactlyANumberOfThreadsEqualTo(int threads);

        /// <summary>
        /// Set population size
        /// </summary>
        /// <param name="size">Number of individuals</param>
        /// <returns>The fluent interface</returns>
        IConfigurableSolvingEnvironment WithPopulationSize(int size);
    }
}