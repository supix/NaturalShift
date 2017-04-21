//-----------------------------------------------------------------------
// <copyright file="Problem.cs" company="supix">
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
using System.Collections.Generic;

namespace NaturalShift.Model.ProblemModel
{
    public class Problem
    {
        private int days;
        private int items;
        private int slots;

        internal Problem()
        {
            this.SlotClosures = new List<SlotClosure>();
            this.ItemsUnavailabilities = new List<ItemUnavailability>();
            this.Aptitudes = new List<ItemAptitude>();
            this.MaxConsecutiveWorkingDays = 0;
            this.DefaultAptitude = 1F;
            this.FirstDay = DateTime.Today;
        }

        internal Problem(int days, int slots, int items) : this()
        {
            this.Days = days;
            this.Slots = slots;
            this.Items = items;

            this.SlotLengths = new int[this.Slots];
            for (int i = 0; i < this.Slots; i++)
                this.SlotLengths[i] = 1;
#warning the default must become a parameter

            this.SlotWeights = new Single[this.Slots];
            for (int i = 0; i < this.Slots; i++)
                this.SlotWeights[i] = 1F;
#warning the default must become a parameter
        }

        public IList<ItemAptitude> Aptitudes { get; set; }

        public bool[,] CompatibleSlots { get; set; }

        public Single[,] ConsecutiveSlotAptitudes { get; set; }

        /// <summary>
        /// Indexes are in the order slot1, slot2, item1, item2
        /// </summary>
        public Single[,,,] CrossItemAptitudes { get; set; }

        public int Days
        {
            get { return days; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(days), days, "Must be greater or equal to 1");

                days = value;
            }
        }

        public Single DefaultAptitude { get; set; }

        public DateTime FirstDay { get; set; }

        public int Items
        {
            get { return items; }
            set
            {
                if (value <= 1)
                    throw new ArgumentOutOfRangeException(nameof(items), items, "Must be greater or equal to 2");

                items = value;
            }
        }

        public Single[] ItemStartupEfforts { get; set; }

        public IList<ItemUnavailability> ItemsUnavailabilities { get; set; }

        public Single[] ItemWeights { get; set; }

        public int MaxConsecutiveWorkingDays { get; set; }

        public int RestAfterMaxWorkingDaysReached { get; set; }

        public IList<SlotClosure> SlotClosures { get; set; }

        public int[] SlotLengths { get; set; }

        public int Slots
        {
            get { return slots; }
            set
            {
                if (value <= 1)
                    throw new ArgumentOutOfRangeException(nameof(slots), slots, "Must be greater or equal to 2");

                slots = value;
            }
        }

        public Single[] SlotValues { get; set; }
        public Single[] SlotWeights { get; set; }

        #region Internal methods to set value optionally initializing structures

        internal void SetSlotWeight(int from, int to, float weight)
        {
            for (int i = from; i <= to; i++)
                this.SlotWeights[i] = weight;
        }

        internal void SetItemWeight(int from, int to, float weight)
        {
            if (ItemWeights == null)
            {
                this.ItemWeights = new Single[this.Items];
                for (int i = 0; i < this.Items; i++)
                    this.ItemWeights[i] = 1F;
#warning the default must become a parameter
            }

            for (int i = from; i <= to; i++)
                this.ItemWeights[i] = weight;
        }

        internal void SetSlotValue(Single value, int fromSlot, int toSlot)
        {
            if (SlotValues == null)
            {
                this.SlotValues = new Single[this.Slots];
                for (int i = 0; i < this.Slots; i++)
                    this.SlotValues[i] = 1F;
#warning the default must become a parameter
            }

            for (int i = fromSlot; i <= toSlot; i++)
                this.SlotValues[i] = value;
        }

        internal void SetCompatibleSlots(int slot1, int slot2)
        {
            if (slot1 == slot2)
                throw new ArgumentException("Cannot make a slot compatible with itself");

            if (this.CompatibleSlots == null)
            {
                this.CompatibleSlots = new bool[this.Slots, this.Slots];
                for (int s1 = 0; s1 < this.Slots; s1++)
                    for (int s2 = 0; s2 < this.Slots; s2++)
                        this.CompatibleSlots[s1, s2] = false;
            }

            this.CompatibleSlots[slot1, slot2] = true;
            this.CompatibleSlots[slot2, slot1] = true;
        }

        internal void SetItemStartupEffort(Single effort, int item)
        {
            if (ItemStartupEfforts == null)
            {
                this.ItemStartupEfforts = new Single[this.Items];
                for (int i = 0; i < this.Items; i++)
                    this.ItemStartupEfforts[i] = 1F;
#warning the default must become a parameter
            }

            this.ItemStartupEfforts[item] = effort;
        }

        internal void SetSlotLength(int length, int fromSlot, int toSlot)
        {
            for (int i = fromSlot; i <= toSlot; i++)
                this.SlotLengths[i] = length;
        }

        internal void SetConsecutiveSlotAptitudes(float aptitude,
            int fromPreceedingSlot,
            int toPreceedingSlot,
            int fromFollowingSlot,
            int toFollowingSlot)
        {
            if (this.ConsecutiveSlotAptitudes == null)
            {
                this.ConsecutiveSlotAptitudes = new Single[this.Slots, this.Slots];
                for (int s1 = 0; s1 < this.Slots; s1++)
                    for (int s2 = 0; s2 < this.Slots; s2++)
                        this.ConsecutiveSlotAptitudes[s1, s2] = 1F;
            }

            for (int prec = fromPreceedingSlot; prec <= toPreceedingSlot; prec++)
                for (int foll = fromFollowingSlot; foll <= toFollowingSlot; foll++)
                    this.ConsecutiveSlotAptitudes[prec, foll] = aptitude;
        }

        internal void SetCrossItemAptitudes(float multiplier,
            int slot1, int slot2, int item1, int item2)
        {
            if (this.CrossItemAptitudes == null)
            {
                this.CrossItemAptitudes = new Single[this.Slots, this.Slots, this.Items, this.Items];
                for (int s1 = 0; s1 < this.Slots; s1++)
                    for (int s2 = 0; s2 < this.Slots; s2++)
                        for (int i1 = 0; i1 < this.Items; i1++)
                            for (int i2 = 0; i2 < this.Items; i2++)
                                this.CrossItemAptitudes[s1, s2, i1, i2] = 1F;
            }

            this.CrossItemAptitudes[slot1, slot2, item1, item2] = multiplier;
        }

        #endregion Internal methods to set value optionally initializing structures
    }
}