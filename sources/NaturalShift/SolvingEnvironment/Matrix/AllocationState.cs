//-----------------------------------------------------------------------
// <copyright file="AllocationState.cs" company="supix">
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
using NaturalShift.SolvingEnvironment.ItemSelectors;
using System;

namespace NaturalShift.SolvingEnvironment.Matrix
{
    /// <summary>
    /// This class contains the basic information about allocation state in a single day for a single slot.
    /// Initial aptitudes are referred to the workshifters, as the aptitude for the i-th item to cover the slot in the day.
    /// CurrentAptitudes vector is copied from InitialAptitudes for the sake of perfomance, to avoid allocation and deallocation of vectors.
    /// The Forced flag indicates that an allocation is resolved by configuration: a forced slot does non participate in the item selection.
    /// </summary>
    internal class AllocationState
    {
        private readonly IItemSelector itemSelector;

        public AllocationState(int Day, int Slot, Single[] initialAptitudes, IItemSelector itemSelector)
        {
            this.Day = Day;
            this.Slot = Slot;
            this.itemSelector = itemSelector;

            this.InitialAptitudes = initialAptitudes;
            this.CurrentAptitudes = new Single[initialAptitudes.Length];
            this.Forced = false;
            this.Processed = false;
            this.ChosenItem = null;
        }

        public int Day { get; set; }
        public int Slot { get; set; }
        public bool Forced { get; set; }
        public bool Processed { get; set; }
        public int? ChosenItem { get; set; }
        public Single[] InitialAptitudes { get; set; }
        public Single[] CurrentAptitudes { get; set; }

        public void Force(int? i)
        {
            ChosenItem = i;
            Forced = true;
            Processed = true;
        }

        public void Reset()
        {
            this.Forced = false;
            this.Processed = false;
            this.ChosenItem = null;
            Array.Copy(this.InitialAptitudes, this.CurrentAptitudes, this.InitialAptitudes.Length);
        }

        public void ResetIfNotForced()
        {
            if (!Forced)
                Reset();
        }

        public int? Process(Single x)
        {
            ChosenItem = itemSelector.SelectItem(CurrentAptitudes, x);
            Processed = true;
            return ChosenItem;
        }
    }
}