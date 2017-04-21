//-----------------------------------------------------------------------
// <copyright file="IConfigurableProblem.cs" company="supix">
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

namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface IConfigurableProblem
    {
        /// <summary>
        /// Set first day of the problem
        /// </summary>
        /// <param name="day">The first day</param>
        /// <returns>The fluent interface</returns>
        IConfigurableProblem WithFirstDay(DateTime day);

        /// <summary>
        /// Start configuring compatible slots
        /// </summary>
        /// <param name="slot">The first compatible slot</param>
        /// <returns>The fluent interface</returns>
        ICfgSlotCompatibility MakingSlot(int slot);

        /// <summary>
        /// Start configuring slot closure
        /// </summary>
        ICfgSlotClosure Closing { get; }

        /// <summary>
        /// Start configuring item or slot weight
        /// </summary>
        /// <param name="weight">The weight</param>
        /// <returns>The fluent interface</returns>
        ICfgWeight AssigningWeight(Single weight);

        /// <summary>
        /// Start configuring slot value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The fluent interface</returns>
        ICfgSlotValue AssigningValue(Single value);

        /// <summary>
        /// Start configuring slot length
        /// </summary>
        /// <param name="length">The length</param>
        /// <returns>The fluent interface</returns>
        ICfgSlotLength AssigningLength(int length);

        /// <summary>
        /// Start configuring item startup effort
        /// </summary>
        /// <param name="effort">The startup effort</param>
        /// <returns>The fluent interface</returns>
        ICfgItemStartupEffort AssigningStartupEffort(Single effort);

        /// <summary>
        /// Start configuring unavailable items
        /// </summary>
        ICfgItemForUnavailItem Making { get; }

        /// <summary>
        /// Start configuring cross-item aptitude
        /// </summary>
        ICfgCrossItemAptitude ConsideringThat { get; }

        /// <summary>
        /// Start configuring aptitude
        /// </summary>
        /// <param name="aptitude">The aptitude</param>
        /// <returns>The fluent interface</returns>
        ICfgItemForAptitude AssigningAptitude(Single aptitude);

        /// <summary>
        /// Start configuring consecutive slot aptitude
        /// </summary>
        /// <returns>The fluent interface</returns>
        ICfgConsecutiveSlotAptitude Multiplying { get; }

        /// <summary>
        /// Set max consecutive working days
        /// </summary>
        /// <param name="maxDays">The maximum number of working days</param>
        /// <returns>The fluent interface</returns>
        IConfigurableProblem WithMaxConsecutiveWorkingDaysEqualTo(int maxDays);

        /// <summary>
        /// Set rest days after maximum working days reached
        /// </summary>
        /// <param name="days">Rest days</param>
        /// <returns>The fluent interface</returns>
        IConfigurableProblem RestAfterMaxWorkingDaysReached(int days);

        /// <summary>
        /// Set default aptitude
        /// </summary>
        /// <param name="aptitude">The default aptitude</param>
        /// <returns>The fluent interface</returns>
        IConfigurableProblem WithDefaultAptitude(Single aptitude);

        /// <summary>
        /// Builds the configured problem
        /// </summary>
        /// <returns>The configured problem</returns>
        Problem Build();
    }
}