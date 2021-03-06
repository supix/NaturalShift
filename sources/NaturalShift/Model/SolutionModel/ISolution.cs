﻿//-----------------------------------------------------------------------
// <copyright file="ISolution.cs" company="supix">
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
namespace NaturalShift.Model.SolutionModel
{
    public interface ISolution
    {
        /// <summary>
        /// The matrix [day, slot] whose value is the item index covering the slot in the day (if any).
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