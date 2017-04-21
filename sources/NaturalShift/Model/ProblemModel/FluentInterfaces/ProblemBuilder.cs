//-----------------------------------------------------------------------
// <copyright file="ProblemBuilder.cs" company="supix">
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
    public class ProblemBuilder :
        IProblemWithoutDays,
        IProblemWithoutSlots,
        IProblemWithoutItems,
        IConfigurableProblem,
        ICfgSlotClosure,
        ICfgItemForUnavailItem,
        ICfgConsecutiveSlotAptitude,
        ICfgCrossItemAptitude,
        ICfgSlotForUnavailItem,
        ICfgDayForUnavailItem,
        ICfgMultipleSlotsForUnavailItem,
        ICfgMultipleDaysForUnavailItem,
        ICfgLastSlotForUnavailItem,
        ICfgLastDayForUnavailItem,
        ICfgMultipleItemsForUnavailItem,
        ICfgLastUnavailableItem,
        ICfgDayOfClosure,
        ICfgMultipleSlotClosure,
        ICfgMultipleDaysOfClosure,
        ICfgLastSlotClosure,
        ICfgLastDayOfClosure,
        ICfgWeight,
        ICfgSlotCompatibility,
        ICfgItemForAptitude,
        ICfgSlotForAptitude,
        ICfgMultipleItemsForAptitude,
        ICfgLastItemForAptitude,
        ICfgDayForAptitude,
        ICfgMultipleSlotsForAptitude,
        ICfgMultipleDaysForAptitude,
        ICfgLastSlotForAptitude,
        ICfgLastDayForAptitude,
        ICfgSlotValue,
        ICfgItemStartupEffort,
        ICfgSlotLength,
        ICfgPrecSlotForConsSlotAptitude,
        ICfgFollowingSlotForConsSlotApt,
        ICfgMultiplePrecSlotForConsSlotApt,
        ICfgFirstSlotForCrossItemApt,
        ICfgAptitudeForCrossItemApt,
        ICfgSecondItemForCrossItemApt,
        ICfgSecondSlotForCrossItemApt,
        ICfgLastPrecSlotForConsSlotApt,
        ICfgMultipleFollowingSlotForConsSlotApt,
        ICfgLastFollowingSlotForConsSlotApt,
        ICfgMultipleSlotsForSlotValue,
        ICfgLastSlotForSlotValue,
        ICfgMultipleSlotsForSlotWeight,
        ICfgMultipleItemsForItemWeight,
        ICfgLastItemForItemWeight,
        ICfgLastSlotForSlotWeight,
        ICfgMultipleSlotsForSlotLength,
        ICfgLastSlotForSlotLength
    {
        private ItemAptitude itemAptitude;
        private ItemUnavailability itemUnavailability;
        private Problem problem;
        private SlotClosure slotClosure;

        #region problem days, slots, items

        private int days;
        private int slots;
        private int items;

        #endregion problem days, slots, items

        #region CfgConsecutiveSlotAptitude

        private Single consecutiveSlotAptitudes_aptitude;
        private int consecutiveSlotAptitudes_fromFollowingSlot;
        private int consecutiveSlotAptitudes_fromPreceedingSlot;
        private int consecutiveSlotAptitudes_toFollowingSlot;
        private int consecutiveSlotAptitudes_toPreceedingSlot;

        #endregion CfgConsecutiveSlotAptitude

        #region CfgWeight

        private int slotOrItemWeight_from;
        private int slotOrItemWeight_to;
        private Single slotOrItemWeight_weight;

        #endregion CfgWeight

        #region CfgCompatibleSlots

        private int compatibleSlots_slot1;
        private int compatibleSlots_slot2;

        #endregion CfgCompatibleSlots

        #region CfgSlotValue

        private Single cfgSlotValue_value;
        private int cfgSlotValue_fromSlot;
        private int cfgSlotValue_toSlot;

        #endregion CfgSlotValue

        #region CfgSlotLength

        private int cfgSlotLength_length;
        private int cfgSlotLength_fromSlot;
        private int cfgSlotLength_toSlot;

        #endregion CfgSlotLength

        #region CfgStartupEffort

        private float cfgItemStartupEffort_effort;
        private int cfgItemStartupEffort_item;

        #endregion CfgStartupEffort

        #region CfgCrossItemAptitude

        private int cfgCrossItemAptitude_item1;
        private int cfgCrossItemAptitude_item2;
        private Single cfgCrossItemAptitude_multiplier;
        private int cfgCrossItemAptitude_slot1;
        private int cfgCrossItemAptitude_slot2;

        #endregion CfgCrossItemAptitude

        /// <summary>
        /// Gets the interface for configuring slot closures
        /// </summary>
        ICfgSlotClosure IConfigurableProblem.Closing
        {
            get
            {
                this.slotClosure = new SlotClosure();

                return this;
            }
        }

        /// <summary>
        /// Gets the interface for configuring cross-item aptitudes
        /// </summary>
        ICfgCrossItemAptitude IConfigurableProblem.ConsideringThat
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets the interface for configuring item unavailabilities
        /// </summary>
        ICfgItemForUnavailItem IConfigurableProblem.Making
        {
            get
            {
                itemUnavailability = new ItemUnavailability();
                return this;
            }
        }

        /// <summary>
        /// Gets the interface for configuring consecutive slot aptitudes
        /// </summary>
        ICfgConsecutiveSlotAptitude IConfigurableProblem.Multiplying
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets the interface for fluently configuring a problem
        /// </summary>
        /// <returns>The fluent interface for configuring number of days</returns>
        public static IProblemWithoutDays Configure()
        {
            return new ProblemBuilder();
        }

        ICfgSecondItemForCrossItemApt ICfgAptitudeForCrossItemApt.AptitudeIsMultipliedBy(float multiplier)
        {
            this.cfgCrossItemAptitude_multiplier = multiplier;

            return this;
        }

        ICfgPrecSlotForConsSlotAptitude ICfgConsecutiveSlotAptitude.AptitudeBy(float aptitude)
        {
            this.consecutiveSlotAptitudes_aptitude = aptitude;

            return this;
        }

        ICfgFirstSlotForCrossItemApt ICfgCrossItemAptitude.WhenItem(int item)
        {
            this.cfgCrossItemAptitude_item1 = item;

            return this;
        }

        IConfigurableProblem ICfgDayForAptitude.Always()
        {
            this.itemAptitude.Days.From = 0;
            this.itemAptitude.Days.To = this.problem.Days - 1;
            this.resolveItemAptitudeConfiguration();

            return this;
        }

        IConfigurableProblem ICfgDayForAptitude.InDay(int day)
        {
            this.itemAptitude.Days.From = day;
            this.itemAptitude.Days.To = day;
            this.resolveItemAptitudeConfiguration();

            return this;
        }

        ICfgMultipleDaysForAptitude ICfgDayForAptitude.InDays()
        {
            return this;
        }

        IConfigurableProblem ICfgDayForUnavailItem.Always()
        {
            this.itemUnavailability.Days.From = 0;
            this.itemUnavailability.Days.To = this.problem.Days - 1;
            this.resolveItemsUnavailabilityConfiguration();

            return this;
        }

        IConfigurableProblem ICfgDayForUnavailItem.InDay(int day)
        {
            this.itemUnavailability.Days.From = day;
            this.itemUnavailability.Days.To = day;
            this.resolveItemsUnavailabilityConfiguration();

            return this;
        }

        ICfgMultipleDaysForUnavailItem ICfgDayForUnavailItem.InDays()
        {
            return this;
        }

        IConfigurableProblem ICfgDayOfClosure.Always()
        {
            this.slotClosure.Days.From = 0;
            this.slotClosure.Days.To = this.problem.Days - 1;
            this.resolveSlotClosureConfiguration();

            return this;
        }

        IConfigurableProblem ICfgDayOfClosure.InDay(int day)
        {
            this.slotClosure.Days.From = day;
            this.slotClosure.Days.To = day;
            this.resolveSlotClosureConfiguration();

            return this;
        }

        ICfgMultipleDaysOfClosure ICfgDayOfClosure.InDays()
        {
            return this;
        }

        ICfgAptitudeForCrossItemApt ICfgFirstSlotForCrossItemApt.CoversSlot(int slot)
        {
            this.cfgCrossItemAptitude_slot1 = slot;

            return this;
        }

        IConfigurableProblem ICfgFollowingSlotForConsSlotApt.IsFollowedByAnySlot()
        {
            consecutiveSlotAptitudes_fromFollowingSlot = 0;
            consecutiveSlotAptitudes_toFollowingSlot = this.problem.Slots - 1;
            this.resolveItemConsecutiveSlotAptitudeConfiguration();

            return this;
        }

        IConfigurableProblem ICfgFollowingSlotForConsSlotApt.IsFollowedBySlot(int slot)
        {
            this.consecutiveSlotAptitudes_fromFollowingSlot = slot;
            this.consecutiveSlotAptitudes_toFollowingSlot = slot;
            this.resolveItemConsecutiveSlotAptitudeConfiguration();

            return this;
        }

        ICfgMultipleFollowingSlotForConsSlotApt ICfgFollowingSlotForConsSlotApt.IsFollowedBySlots()
        {
            return this;
        }

        ICfgSlotForAptitude ICfgItemForAptitude.ToAllItems()
        {
            this.itemAptitude.Items.From = 0;
            this.itemAptitude.Items.To = this.problem.Items - 1;

            return this;
        }

        ICfgSlotForAptitude ICfgItemForAptitude.ToItem(int item)
        {
            this.itemAptitude.Items.From = item;
            this.itemAptitude.Items.To = item;

            return this;
        }

        ICfgMultipleItemsForAptitude ICfgItemForAptitude.ToItems()
        {
            return this;
        }

        ICfgSlotForUnavailItem ICfgItemForUnavailItem.AllItems()
        {
            this.itemUnavailability.Items.From = 0;
            this.itemUnavailability.Items.To = this.problem.Items - 1;
            return this;
        }

        ICfgSlotForUnavailItem ICfgItemForUnavailItem.Item(int item)
        {
            this.itemUnavailability.Items.From = item;
            this.itemUnavailability.Items.To = item;
            return this;
        }

        ICfgMultipleItemsForUnavailItem ICfgItemForUnavailItem.Items()
        {
            return this;
        }

        IConfigurableProblem ICfgItemStartupEffort.ToItem(int item)
        {
            this.cfgItemStartupEffort_item = item;
            this.resolveItemStartupEffort();

            return this;
        }

        IConfigurableProblem ICfgLastDayForAptitude.To(int day)
        {
            this.itemAptitude.Days.To = day;
            this.resolveItemAptitudeConfiguration();

            return this;
        }

        IConfigurableProblem ICfgLastDayForUnavailItem.To(int day)
        {
            this.itemUnavailability.Days.To = day;
            this.resolveItemsUnavailabilityConfiguration();

            return this;
        }

        ICfgDayForUnavailItem ICfgLastSlotForUnavailItem.To(int slot)
        {
            this.itemUnavailability.Slots.To = slot;
            return this;
        }

        ICfgFollowingSlotForConsSlotApt ICfgLastPrecSlotForConsSlotApt.To(int slot)
        {
            consecutiveSlotAptitudes_toPreceedingSlot = slot;

            return this;
        }

        ICfgDayForAptitude ICfgLastSlotForAptitude.To(int slot)
        {
            this.itemAptitude.Slots.To = slot;

            return this;
        }

        IConfigurableProblem ICfgLastDayOfClosure.To(int day)
        {
            this.slotClosure.Days.To = day;
            this.resolveSlotClosureConfiguration();

            return this;
        }

        ICfgSlotForAptitude ICfgLastItemForAptitude.To(int item)
        {
            this.itemAptitude.Items.To = item;

            return this;
        }

        ICfgLastDayForUnavailItem ICfgMultipleDaysForUnavailItem.From(int day)
        {
            this.itemUnavailability.Days.From = day;
            return this;
        }

        ICfgLastDayForAptitude ICfgMultipleDaysForAptitude.From(int day)
        {
            this.itemAptitude.Days.From = day;

            return this;
        }

        ICfgLastItemForAptitude ICfgMultipleItemsForAptitude.From(int item)
        {
            this.itemAptitude.Items.From = item;

            return this;
        }

        ICfgLastUnavailableItem ICfgMultipleItemsForUnavailItem.From(int item)
        {
            this.itemUnavailability.Items.From = item;
            return this;
        }

        ICfgLastPrecSlotForConsSlotApt ICfgMultiplePrecSlotForConsSlotApt.From(int slot)
        {
            consecutiveSlotAptitudes_fromPreceedingSlot = slot;

            return this;
        }

        ICfgLastSlotForAptitude ICfgMultipleSlotsForAptitude.From(int slot)
        {
            this.itemAptitude.Slots.From = slot;

            return this;
        }

        ICfgFollowingSlotForConsSlotApt ICfgPrecSlotForConsSlotAptitude.WhenAnySlot()
        {
            consecutiveSlotAptitudes_fromPreceedingSlot = 0;
            consecutiveSlotAptitudes_toPreceedingSlot = this.problem.Slots - 1;

            return this;
        }

        ICfgFollowingSlotForConsSlotApt ICfgPrecSlotForConsSlotAptitude.WhenSlot(int slot)
        {
            this.consecutiveSlotAptitudes_fromPreceedingSlot = slot;
            this.consecutiveSlotAptitudes_toPreceedingSlot = slot;

            return this;
        }

        ICfgMultiplePrecSlotForConsSlotApt ICfgPrecSlotForConsSlotAptitude.WhenSlots()
        {
            return this;
        }

        ICfgSecondSlotForCrossItemApt ICfgSecondItemForCrossItemApt.ForItem(int item)
        {
            this.cfgCrossItemAptitude_item2 = item;

            return this;
        }

        IConfigurableProblem ICfgSecondSlotForCrossItemApt.CoveringSlot(int slot)
        {
            this.cfgCrossItemAptitude_slot2 = slot;
            this.resolveCrossItemAptitudeConfiguration();

            return this;
        }

        ICfgDayOfClosure ICfgSlotClosure.AllSlots()
        {
            this.slotClosure.Slots.From = 0;
            this.slotClosure.Slots.To = this.problem.Slots - 1;
            return this;
        }

        ICfgDayOfClosure ICfgSlotClosure.Slot(int slot)
        {
            this.slotClosure.Slots.From = slot;
            this.slotClosure.Slots.To = slot;
            return this;
        }

        ICfgMultipleSlotClosure ICfgSlotClosure.Slots()
        {
            return this;
        }

        IConfigurableProblem ICfgSlotCompatibility.CompatibleWithSlot(int slot2)
        {
            this.compatibleSlots_slot2 = slot2;
            this.resolveSlotCompatibilityConfiguration();

            return this;
        }

        ICfgDayForAptitude ICfgSlotForAptitude.ForAllSlots()
        {
            this.itemAptitude.Slots.From = 0;
            this.itemAptitude.Slots.To = this.problem.Slots - 1;

            return this;
        }

        ICfgDayForAptitude ICfgSlotForAptitude.ForSlot(int slot)
        {
            this.itemAptitude.Slots.From = slot;
            this.itemAptitude.Slots.To = slot;

            return this;
        }

        ICfgMultipleSlotsForAptitude ICfgSlotForAptitude.ForSlots()
        {
            return this;
        }

        ICfgDayForUnavailItem ICfgSlotForUnavailItem.UnavailableForAllSlots()
        {
            this.itemUnavailability.Slots.From = 0;
            this.itemUnavailability.Slots.To = this.problem.Slots - 1;
            return this;
        }

        ICfgDayForUnavailItem ICfgSlotForUnavailItem.UnavailableForSlot(int slot)
        {
            this.itemUnavailability.Slots.From = slot;
            this.itemUnavailability.Slots.To = slot;
            return this;
        }

        ICfgMultipleSlotsForUnavailItem ICfgSlotForUnavailItem.UnavailableForSlots()
        {
            return this;
        }

        IConfigurableProblem ICfgSlotLength.ToSlot(int slot)
        {
            this.cfgSlotLength_fromSlot = slot;
            this.cfgSlotLength_toSlot = slot;
            this.resolveSlotLengthConfiguration();

            return this;
        }

        IConfigurableProblem ICfgWeight.ToItem(int item)
        {
            this.slotOrItemWeight_from = item;
            this.slotOrItemWeight_to = item;
            this.resolveItemWeightConfiguration();

            return this;
        }

        IConfigurableProblem ICfgSlotValue.ToSlot(int slot)
        {
            this.cfgSlotValue_fromSlot = slot;
            this.cfgSlotValue_toSlot = slot;
            this.resolveSlotValueConfiguration();

            return this;
        }

        IConfigurableProblem ICfgWeight.ToSlot(int slot)
        {
            this.slotOrItemWeight_from = slot;
            this.slotOrItemWeight_to = slot;
            this.resolveSlotWeightConfiguration();

            return this;
        }

        IConfigurableProblem ICfgLastFollowingSlotForConsSlotApt.To(int slot)
        {
            consecutiveSlotAptitudes_toFollowingSlot = slot;
            this.resolveItemConsecutiveSlotAptitudeConfiguration();

            return this;
        }

        ICfgSlotForUnavailItem ICfgLastUnavailableItem.To(int item)
        {
            this.itemUnavailability.Items.To = item;
            return this;
        }

        ICfgLastDayOfClosure ICfgMultipleDaysOfClosure.From(int day)
        {
            this.slotClosure.Days.From = day;
            return this;
        }

        ICfgLastSlotForUnavailItem ICfgMultipleSlotsForUnavailItem.From(int slot)
        {
            this.itemUnavailability.Slots.From = slot;
            return this;
        }

        ICfgLastFollowingSlotForConsSlotApt ICfgMultipleFollowingSlotForConsSlotApt.From(int slot)
        {
            consecutiveSlotAptitudes_fromFollowingSlot = slot;

            return this;
        }

        /// <summary>
        /// Gets the interface for configuring item aptitudes
        /// </summary>
        /// <param name="aptitude">The item aptitude</param>
        /// <returns>The fluent interface</returns>
        ICfgItemForAptitude IConfigurableProblem.AssigningAptitude(float aptitude)
        {
            this.itemAptitude = new ItemAptitude();
            this.itemAptitude.Aptitude = aptitude;

            return this;
        }

        /// <summary>
        /// Gets the interface for configuring slot lenghts
        /// </summary>
        /// <param name="length">The slot length</param>
        /// <returns>The fluent interface</returns>
        ICfgSlotLength IConfigurableProblem.AssigningLength(int length)
        {
            this.cfgSlotLength_length = length;

            return this;
        }

        /// <summary>
        /// Gets the interface for configuring item startup efforts
        /// </summary>
        /// <param name="effort">The startup effort</param>
        /// <returns>The fluent interface</returns>
        ICfgItemStartupEffort IConfigurableProblem.AssigningStartupEffort(Single effort)
        {
            this.cfgItemStartupEffort_effort = effort;

            return this;
        }

        /// <summary>
        /// Gets the interface for configuring slot values
        /// </summary>
        /// <param name="value">The slot value</param>
        /// <returns>The fluent interface</returns>
        ICfgSlotValue IConfigurableProblem.AssigningValue(Single value)
        {
            this.cfgSlotValue_value = value;

            return this;
        }

        /// <summary>
        /// Gets the interface for configuring slot weights
        /// </summary>
        /// <param name="weight">The slot weight</param>
        /// <returns>The fluent interface</returns>
        ICfgWeight IConfigurableProblem.AssigningWeight(float weight)
        {
            this.slotOrItemWeight_weight = weight;

            return this;
        }

        /// <summary>
        /// Buils a problem instance
        /// </summary>
        /// <returns>The problem instance</returns>
        Problem IConfigurableProblem.Build()
        {
            return this.problem;
        }

        /// <summary>
        /// Gets the interface for configuring compatible slots
        /// </summary>
        /// <param name="slot">The first compatible slot</param>
        /// <returns>The fluent interface</returns>
        ICfgSlotCompatibility IConfigurableProblem.MakingSlot(int slot)
        {
            this.compatibleSlots_slot1 = slot;

            return this;
        }

        /// <summary>
        /// Gets the interface for configuring rest after max working days reached
        /// </summary>
        /// <param name="days">The number of rest days</param>
        /// <returns>The fluent interface</returns>
        IConfigurableProblem IConfigurableProblem.RestAfterMaxWorkingDaysReached(int days)
        {
            this.problem.RestAfterMaxWorkingDaysReached = days;

            return this;
        }

        /// <summary>
        /// Gets the interface for configuring the default items aptitude
        /// </summary>
        /// <param name="aptitude">The default aptitude</param>
        /// <returns>The fluent interface</returns>
        IConfigurableProblem IConfigurableProblem.WithDefaultAptitude(float aptitude)
        {
            this.problem.DefaultAptitude = aptitude;
            return this;
        }

        /// <summary>
        /// Gets the interface for configuring the date of the first day in the workshift
        /// </summary>
        /// <param name="day">The date of the first day</param>
        /// <returns>The fluent interface</returns>
        IConfigurableProblem IConfigurableProblem.WithFirstDay(DateTime day)
        {
            this.problem.FirstDay = day;
            return this;
        }

        /// <summary>
        /// Gets the interface for configuring the maximum number of consecutive working days
        /// </summary>
        /// <param name="maxDays">The maximum</param>
        /// <returns>The fluent interface</returns>
        IConfigurableProblem IConfigurableProblem.WithMaxConsecutiveWorkingDaysEqualTo(int maxDays)
        {
            this.problem.MaxConsecutiveWorkingDays = maxDays;
            return this;
        }

        /// <summary>
        /// Gets the interface for configuring the number of days in the workshift
        /// </summary>
        /// <param name="days">The number of days</param>
        /// <returns>The fluent interface for configuring number of slots</returns>
        IProblemWithoutSlots IProblemWithoutDays.WithDays(int days)
        {
            this.days = days;
            return this;
        }

        /// <summary>
        /// Gets the interface for configuring the number of items in the workshift
        /// </summary>
        /// <param name="items">The number of items</param>
        /// <returns>The fluent interface</returns>
        IConfigurableProblem IProblemWithoutItems.WithItems(int items)
        {
            this.items = items;
            this.problem = new Problem(this.days, this.slots, this.items);

            return this;
        }

        /// <summary>
        /// Gets the interface for configuring the number of slots in the workshift
        /// </summary>
        /// <param name="slots">The number of slots</param>
        /// <returns>The fluent interface for configuring the number of items</returns>
        IProblemWithoutItems IProblemWithoutSlots.WithSlots(int slots)
        {
            this.slots = slots;
            return this;
        }

        ICfgLastSlotClosure ICfgMultipleSlotClosure.From(int slot)
        {
            this.slotClosure.Slots.From = slot;
            return this;
        }

        ICfgDayOfClosure ICfgLastSlotClosure.To(int slot)
        {
            this.slotClosure.Slots.To = slot;
            return this;
        }

        #region Private methods for resolution

        private void resolveCrossItemAptitudeConfiguration()
        {
            this.problem.SetCrossItemAptitudes(
                this.cfgCrossItemAptitude_multiplier,
                this.cfgCrossItemAptitude_slot1,
                this.cfgCrossItemAptitude_slot2,
                this.cfgCrossItemAptitude_item1,
                this.cfgCrossItemAptitude_item2);
        }

        private void resolveItemAptitudeConfiguration()
        {
            this.problem.Aptitudes.Add(this.itemAptitude);
            this.itemAptitude = null;
        }

        private void resolveItemConsecutiveSlotAptitudeConfiguration()
        {
            this.problem.SetConsecutiveSlotAptitudes(
                this.consecutiveSlotAptitudes_aptitude,
                this.consecutiveSlotAptitudes_fromPreceedingSlot,
                this.consecutiveSlotAptitudes_toPreceedingSlot,
                this.consecutiveSlotAptitudes_fromFollowingSlot,
                this.consecutiveSlotAptitudes_toFollowingSlot);
        }

        private void resolveItemStartupEffort()
        {
            this.problem.SetItemStartupEffort(this.cfgItemStartupEffort_effort, this.cfgItemStartupEffort_item);
        }

        private void resolveItemsUnavailabilityConfiguration()
        {
            this.problem.ItemsUnavailabilities.Add(itemUnavailability);
            this.itemUnavailability = null;
        }

        private void resolveItemWeightConfiguration()
        {
            this.problem.SetItemWeight(this.slotOrItemWeight_from, this.slotOrItemWeight_to, this.slotOrItemWeight_weight);
        }

        private void resolveSlotClosureConfiguration()
        {
            this.problem.SlotClosures.Add(slotClosure);
            this.slotClosure = null;
        }

        private void resolveSlotCompatibilityConfiguration()
        {
            this.problem.SetCompatibleSlots(this.compatibleSlots_slot1, this.compatibleSlots_slot2);
        }

        private void resolveSlotLengthConfiguration()
        {
            this.problem.SetSlotLength(this.cfgSlotLength_length, this.cfgSlotLength_fromSlot, this.cfgSlotLength_toSlot);
        }

        private void resolveSlotValueConfiguration()
        {
            this.problem.SetSlotValue(this.cfgSlotValue_value, this.cfgSlotValue_fromSlot, this.cfgSlotValue_toSlot);
        }

        private void resolveSlotWeightConfiguration()
        {
            this.problem.SetSlotWeight(this.slotOrItemWeight_from, this.slotOrItemWeight_to, this.slotOrItemWeight_weight);
        }

        #endregion Private methods for resolution

        ICfgMultipleSlotsForSlotValue ICfgSlotValue.ToSlots()
        {
            return this;
        }

        IConfigurableProblem ICfgSlotValue.ToAllSlots()
        {
            cfgSlotValue_fromSlot = 0;
            cfgSlotValue_toSlot = this.problem.Slots - 1;
            this.resolveSlotValueConfiguration();

            return this;
        }

        ICfgLastSlotForSlotValue ICfgMultipleSlotsForSlotValue.From(int slot)
        {
            cfgSlotValue_fromSlot = slot;

            return this;
        }

        IConfigurableProblem ICfgLastSlotForSlotValue.To(int slot)
        {
            cfgSlotValue_toSlot = slot;
            this.resolveSlotValueConfiguration();

            return this;
        }

        ICfgMultipleSlotsForSlotWeight ICfgWeight.ToSlots()
        {
            return this;
        }

        IConfigurableProblem ICfgWeight.ToAllSlots()
        {
            this.slotOrItemWeight_from = 0;
            this.slotOrItemWeight_to = this.problem.Slots - 1;
            this.resolveSlotWeightConfiguration();

            return this;
        }

        ICfgMultipleItemsForItemWeight ICfgWeight.ToItems()
        {
            return this;
        }

        IConfigurableProblem ICfgWeight.ToAllItems()
        {
            this.slotOrItemWeight_from = 0;
            this.slotOrItemWeight_to = this.problem.Items - 1;
            this.resolveItemWeightConfiguration();

            return this;
        }

        ICfgLastSlotForSlotWeight ICfgMultipleSlotsForSlotWeight.From(int slot)
        {
            this.slotOrItemWeight_from = slot;

            return this;
        }

        ICfgLastItemForItemWeight ICfgMultipleItemsForItemWeight.From(int item)
        {
            this.slotOrItemWeight_from = item;

            return this;
        }

        IConfigurableProblem ICfgLastItemForItemWeight.To(int item)
        {
            this.slotOrItemWeight_to = item;
            this.resolveItemWeightConfiguration();

            return this;
        }

        IConfigurableProblem ICfgLastSlotForSlotWeight.To(int slot)
        {
            this.slotOrItemWeight_to = slot;
            this.resolveSlotWeightConfiguration();

            return this;
        }

        ICfgMultipleSlotsForSlotLength ICfgSlotLength.ToSlots()
        {
            return this;
        }

        ICfgLastSlotForSlotLength ICfgMultipleSlotsForSlotLength.From(int slot)
        {
            this.cfgSlotLength_fromSlot = slot;

            return this;
        }

        IConfigurableProblem ICfgLastSlotForSlotLength.To(int slot)
        {
            this.cfgSlotLength_toSlot = slot;
            this.resolveSlotLengthConfiguration();

            return this;
        }

        IConfigurableProblem ICfgSlotLength.ToAllSlots()
        {
            this.cfgSlotLength_fromSlot = 0;
            this.cfgSlotLength_toSlot = problem.Slots - 1;
            this.resolveSlotLengthConfiguration();

            return this;
        }
    }
}