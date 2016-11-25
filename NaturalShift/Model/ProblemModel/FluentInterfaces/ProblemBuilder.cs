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
        ICfgLastFollowingSlotForConsSlotApt
    {
        private ItemAptitude itemAptitude;
        private ItemUnavailability itemUnavailability;
        private Problem problem;
        private SlotClosure slotClosure;

        #region problem days, slots, items
        private int days;
        private int slots;
        private int items;
        #endregion

        #region CfgConsecutiveSlotAptitude

        private Single consecutiveSlotAptitudes_aptitude;
        private int consecutiveSlotAptitudes_fromFollowingSlot;
        private int consecutiveSlotAptitudes_fromPreceedingSlot;
        private int consecutiveSlotAptitudes_toFollowingSlot;
        private int consecutiveSlotAptitudes_toPreceedingSlot;
        #endregion CfgConsecutiveSlotAptitude

        #region CfgWeight

        private int slotOrItemWeight_idx;
        private Single slotOrItemWeight_weight;

        #endregion CfgWeight

        #region CfgCompatibleSlots

        private int compatibleSlots_slot1;
        private int compatibleSlots_slot2;

        #endregion CfgCompatibleSlots

        #region CfgSlotValue

        private int cfgSlotValue_slot;
        private Single cfgSlotValue_value;
        #endregion CfgSlotValue

        #region CfgStartupEffort

        private float cfgItemStartupEffort_effort;
        private int cfgItemStartupEffort_item;

        #endregion CfgStartupEffort

        #region CfgSlotLength

        private int cfgSlotLength_length;
        private int cfgSlotLength_slot;

        #endregion CfgSlotLength

        #region CfgCrossItemAptitude

        private int cfgCrossItemAptitude_item1;
        private int cfgCrossItemAptitude_item2;
        private Single cfgCrossItemAptitude_multiplier;
        private int cfgCrossItemAptitude_slot1;
        private int cfgCrossItemAptitude_slot2;
        #endregion CfgCrossItemAptitude

        ICfgSlotClosure IConfigurableProblem.Closing
        {
            get
            {
                this.slotClosure = new SlotClosure();

                return this;
            }
        }

        ICfgCrossItemAptitude IConfigurableProblem.ConsideringThat
        {
            get
            {
                return this;
            }
        }

        ICfgItemForUnavailItem IConfigurableProblem.Making
        {
            get
            {
                itemUnavailability = new ItemUnavailability();
                return this;
            }
        }

        ICfgConsecutiveSlotAptitude IConfigurableProblem.Multiplying
        {
            get
            {
                return this;
            }
        }

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
            this.cfgSlotLength_slot = slot;
            this.resolveSlotLengthConfiguration();

            return this;
        }

        IConfigurableProblem ICfgWeight.ToItem(int item)
        {
            this.slotOrItemWeight_idx = item;
            this.resolveItemWeightConfiguration();

            return this;
        }

        IConfigurableProblem ICfgSlotValue.ToSlot(int slot)
        {
            this.cfgSlotValue_slot = slot;
            this.resolveSlotValueConfiguration();

            return this;
        }

        IConfigurableProblem ICfgWeight.ToSlot(int slot)
        {
            this.slotOrItemWeight_idx = slot;
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
        ICfgItemForAptitude IConfigurableProblem.AssigningAptitude(float aptitude)
        {
            this.itemAptitude = new ItemAptitude();
            this.itemAptitude.Aptitude = aptitude;

            return this;
        }

        ICfgSlotLength IConfigurableProblem.AssigningLength(int length)
        {
            this.cfgSlotLength_length = length;

            return this;
        }

        ICfgItemStartupEffort IConfigurableProblem.AssigningStartupEffort(Single effort)
        {
            this.cfgItemStartupEffort_effort = effort;

            return this;
        }

        ICfgSlotValue IConfigurableProblem.AssigningValue(Single value)
        {
            this.cfgSlotValue_value = value;

            return this;
        }

        ICfgWeight IConfigurableProblem.AssigningWeight(float weight)
        {
            this.slotOrItemWeight_weight = weight;

            return this;
        }

        Problem IConfigurableProblem.Build()
        {
            return this.problem;
        }

        ICfgSlotCompatibility IConfigurableProblem.MakingSlot(int slot)
        {
            this.compatibleSlots_slot1 = slot;

            return this;
        }

        IConfigurableProblem IConfigurableProblem.RestAfterMaxWorkingDaysReached(int days)
        {
            this.problem.RestAfterMaxWorkingDaysReached = days;

            return this;
        }

        IConfigurableProblem IConfigurableProblem.WithDefaultAptitude(float aptitude)
        {
            this.problem.DefaultAptitude = aptitude;
            return this;
        }

        IConfigurableProblem IConfigurableProblem.WithFirstDay(DateTime day)
        {
            this.problem.FirstDay = day;
            return this;
        }

        IConfigurableProblem IConfigurableProblem.WithMaxConsecutiveWorkingDaysEqualTo(int maxDays)
        {
            this.problem.MaxConsecutiveWorkingDays = maxDays;
            return this;
        }

        IProblemWithoutSlots IProblemWithoutDays.WithDays(int days)
        {
            this.days = days;
            return this;
        }

        IConfigurableProblem IProblemWithoutItems.WithItems(int items)
        {
            this.items = items;
            this.problem = new Problem(this.days, this.slots, this.items);

            return this;
        }

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
            this.problem.SetItemWeight(this.slotOrItemWeight_idx, this.slotOrItemWeight_weight);
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
            this.problem.SetSlotLength(this.cfgSlotLength_length, this.cfgSlotLength_slot);
        }

        private void resolveSlotValueConfiguration()
        {
            this.problem.SetSlotValue(this.cfgSlotValue_value, this.cfgSlotValue_slot);
        }

        private void resolveSlotWeightConfiguration()
        {
            this.problem.SetSlotWeight(this.slotOrItemWeight_idx, this.slotOrItemWeight_weight);
        }
        #endregion Private methods for resolution
    }
}