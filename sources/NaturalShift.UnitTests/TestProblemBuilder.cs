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
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using NaturalShift.Model.ProblemModel.FluentInterfaces;
using NUnit.Framework;
using System;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class TestProblemBuilder
    {
        private Random rnd = new Random();

        [Test]
        [Repeat(1000)]
        public void AllClosedSlotsIsCorrectlyAdded()
        {
            var slots = rnd.Next(50) + 2;
            var closedSlot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Closing.AllSlots().InDay(5)
                .Build();

            var sc = problem.SlotClosures[0];
            Assert.That(sc.Slots.From, Is.EqualTo(0));
            Assert.That(sc.Slots.To, Is.EqualTo(slots - 1));
        }

        [Test]
        [Repeat(1000)]
        public void AllDaysSlotClosureIsCorrectlyAdded()
        {
            var days = rnd.Next() + 1;

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Closing.Slot(5).Always()
                .Build();

            var sc = problem.SlotClosures[0];
            Assert.That(sc.Days.From, Is.EqualTo(0));
            Assert.That(sc.Days.To, Is.EqualTo(days - 1));
        }

        [Test]
        [Repeat(1000)]
        public void AllDaysUnavailabilityIsCorrectlyConfigured()
        {
            var days = rnd.Next(50) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Making.Item(5).UnavailableForSlot(5).Always()
                .Build();

            Assert.That(problem.ItemsUnavailabilities[0].Days.From, Is.EqualTo(0));
            Assert.That(problem.ItemsUnavailabilities[0].Days.To, Is.EqualTo(days - 1));
        }

        [Test]
        [Repeat(1000)]
        public void AllItemsAptitudeIsCorrectlyConfigured()
        {
            var items = rnd.Next(20) + 2;

            var aptitude = (Single)(rnd.NextDouble() * 2);
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .AssigningAptitude(aptitude).ToAllItems().ForSlot(5).InDay(5)
                .Build();

            Assert.That(problem.Aptitudes[0].Items.From, Is.EqualTo(0));
            Assert.That(problem.Aptitudes[0].Items.To, Is.EqualTo(items - 1));
            Assert.That(problem.Aptitudes[0].Aptitude, Is.EqualTo(aptitude));
        }

        [Test]
        [Repeat(1000)]
        public void AllItemsUnavailabilityIsCorrectlyConfigured()
        {
            var items = rnd.Next(20) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .Making.AllItems().UnavailableForSlot(5).InDay(5)
                .Build();

            Assert.That(problem.ItemsUnavailabilities[0].Items.From, Is.EqualTo(0));
            Assert.That(problem.ItemsUnavailabilities[0].Items.To, Is.EqualTo(items - 1));
        }

        [Test]
        [Repeat(100)]
        public void AllSlotsAreNotCompatibleWhenTheFirstCompatibilityIsAdded()
        {
            var slots = rnd.Next(50) + 2;
            var slot1 = 0;
            var slot2 = 1;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .MakingSlot(slot1).CompatibleWithSlot(slot2)
                .Build();

            for (var s1 = 0; s1 < slots; s1++)
                for (var s2 = 0; s2 < slots; s2++)
                    if (!((s1 == slot1) && (s2 == slot2)) && !((s1 == slot2) && (s2 == slot1)))
                        Assert.That(problem.CompatibleSlots[s1, s2], Is.False);
                    else
                        Assert.That(problem.CompatibleSlots[s1, s2], Is.True);
        }

        [Test]
        [Repeat(1000)]
        public void AllSlotsUnavailabilityIsCorrectlyConfigured()
        {
            var slots = rnd.Next(20) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Making.Item(5).UnavailableForAllSlots().InDay(5)
                .Build();

            Assert.That(problem.ItemsUnavailabilities[0].Slots.From, Is.EqualTo(0));
            Assert.That(problem.ItemsUnavailabilities[0].Slots.To, Is.EqualTo(slots - 1));
        }

        [Test]
        public void ByDefaultAllConsecutiveSlotAptitudesIsNull()
        {
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Build();

            Assert.That(problem.ConsecutiveSlotAptitudes, Is.Null);
        }

        [Test]
        public void ByDefaultAptitudeListIsEmpty()
        {
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Build();

            Assert.That(problem.Aptitudes, Is.Empty);
        }

        [Test]
        public void ByDefaultCompatibleSlotsMatrixIsNull()
        {
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Build();

            Assert.That(problem.CompatibleSlots, Is.Null);
        }

        [Test]
        public void ByDefaultCrossItemAptitudesIsNull()
        {
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Build();

            Assert.That(problem.CrossItemAptitudes, Is.Null);
        }

        [Test]
        [Repeat(1000)]
        public void ByDefaultItemStartupEffortsIsNull()
        {
            var items = rnd.Next(50) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .Build();

            Assert.That(problem.ItemStartupEfforts, Is.Null);
        }

        [Test]
        public void ByDefaultItemUnavailabilityListIsEmpty()
        {
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Build();

            Assert.That(problem.ItemsUnavailabilities, Is.Empty);
        }

        [Test]
        [Repeat(1000)]
        public void ByDefaultItemWeightsIsNull()
        {
            var items = rnd.Next(50) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .Build();

            Assert.That(problem.ItemWeights, Is.Null);
        }

        [Test]
        public void ByDefaultSlotClosureListIsEmpty()
        {
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Build();

            Assert.That(problem.SlotClosures, Is.Empty);
        }

        [Test]
        [Repeat(1000)]
        public void ByDefaultSlotLengthsIsNotNull()
        {
            var slots = rnd.Next(50) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Build();

            Assert.That(problem.SlotLengths, Is.Not.Null);
        }

        [Test]
        [Repeat(1000)]
        public void ByDefaultSlotsValuesIsNull()
        {
            var slots = rnd.Next(50) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Build();

            Assert.That(problem.SlotValues, Is.Null);
        }

        [Test]
        [Repeat(1000)]
        public void ByDefaultSlotWeightsIsNotNull()
        {
            var slots = rnd.Next(50) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Build();

            Assert.That(problem.SlotWeights, Is.Not.Null);
        }

        [Test]
        [Repeat(1000)]
        public void ClosedDayRangeIsCorrectlyAdded()
        {
            var days = rnd.Next(50) + 1;
            var closedDayMin = rnd.Next(days);
            var closedDayMax = rnd.Next(days);
            if (closedDayMin > closedDayMax)
            {
                var swap = closedDayMin;
                closedDayMin = closedDayMax;
                closedDayMax = swap;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Closing.Slot(5).InDays().From(closedDayMin).To(closedDayMax)
                .Build();

            var sc = problem.SlotClosures[0];
            Assert.That(sc.Days.From, Is.EqualTo(closedDayMin));
            Assert.That(sc.Days.To, Is.EqualTo(closedDayMax));
        }

        [Test]
        [Repeat(1000)]
        public void ClosedSlotRangeIsCorrectlyAdded()
        {
            var slots = rnd.Next(50) + 2;
            var closedSlotMin = rnd.Next(slots);
            var closedSlotMax = rnd.Next(slots);
            if (closedSlotMin > closedSlotMax)
            {
                var swap = closedSlotMin;
                closedSlotMin = closedSlotMax;
                closedSlotMax = swap;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Closing.Slots().From(closedSlotMin).To(closedSlotMax).InDay(5)
                .Build();

            var sc = problem.SlotClosures[0];
            Assert.That(sc.Slots.From, Is.EqualTo(closedSlotMin));
            Assert.That(sc.Slots.To, Is.EqualTo(closedSlotMax));
        }

        [Test]
        [Repeat(1000)]
        public void CompatibleSlotIsCorrectlySet()
        {
            var slots = rnd.Next(50) + 2;
            var slot1 = rnd.Next(slots);
            int slot2;
            do
            {
                slot2 = rnd.Next(slots);
            } while (slot2 == slot1);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .MakingSlot(slot1).CompatibleWithSlot(slot2)
                .Build();

            Assert.That(problem.CompatibleSlots[slot1, slot2], Is.True);
        }

        [Test]
        public void ConfigurableProblemIsNotNull()
        {
            var configurableProblem = ProblemBuilder.Configure();

            Assert.That(configurableProblem, Is.Not.Null);
        }

        [Test]
        [Repeat(1000)]
        public void ConfigurationOfDaysSlotAndItemsDoesWork()
        {
            var days = this.RandomFrom5To15();
            var slots = this.RandomFrom5To15();
            var items = this.RandomFrom5To15();

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(slots)
                .WithItems(items)
                .Build();

            Assert.That(problem.Days, Is.EqualTo(days));
            Assert.That(problem.Slots, Is.EqualTo(slots));
            Assert.That(problem.Items, Is.EqualTo(items));
        }

        [Test]
        [Repeat(1000)]
        public void ConsecutiveSlotAptitudesIsCorrectlyConfigured()
        {
            var multiplier = (Single)(rnd.NextDouble() * 2);
            var slots = rnd.Next(20) + 2;
            var slot1 = rnd.Next(slots);
            var slot2 = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Multiplying.AptitudeBy(multiplier).WhenSlot(slot1).IsFollowedBySlot(slot2)
                .Build();

            Assert.That(problem.ConsecutiveSlotAptitudes[slot1, slot2], Is.EqualTo(multiplier));
        }

        [Test]
        [Repeat(1000)]
        public void PreceedingRangeConsecutiveSlotAptitudesIsCorrectlyConfigured()
        {
            var multiplier = (Single)(rnd.NextDouble() * 2);
            var slots = rnd.Next(20) + 2;
            var fromPreceedingSlot = rnd.Next(slots);
            var toPreceedingSlot = rnd.Next(slots);
            if (fromPreceedingSlot > toPreceedingSlot)
            {
                var swap = fromPreceedingSlot;
                fromPreceedingSlot = toPreceedingSlot;
                toPreceedingSlot = swap;
            }
            var followingSlot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Multiplying.AptitudeBy(multiplier).WhenSlots().From(fromPreceedingSlot).To(toPreceedingSlot).IsFollowedBySlot(followingSlot)
                .Build();

            for (var slot = fromPreceedingSlot; slot <= toPreceedingSlot; slot++)
                Assert.That(problem.ConsecutiveSlotAptitudes[slot, followingSlot], Is.EqualTo(multiplier));
        }

        [Test]
        [Repeat(1000)]
        public void PreceedingRangeConsecutiveSlotAptitudesDoesNotChangeUnaffectedSlots()
        {
            var multiplier = (Single)(rnd.NextDouble() * 2);
            var slots = rnd.Next(20) + 2;
            var fromPreceedingSlot = rnd.Next(slots);
            var toPreceedingSlot = rnd.Next(slots);
            if (fromPreceedingSlot > toPreceedingSlot)
            {
                var swap = fromPreceedingSlot;
                fromPreceedingSlot = toPreceedingSlot;
                toPreceedingSlot = swap;
            }
            var followingSlot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Multiplying.AptitudeBy(multiplier).WhenSlots().From(fromPreceedingSlot).To(toPreceedingSlot).IsFollowedBySlot(followingSlot)
                .Build();

            for (var slot = 0; slot < fromPreceedingSlot; slot++)
                for (var follSlot = 0; follSlot < slots; follSlot++)
                    Assert.That(problem.ConsecutiveSlotAptitudes[slot, follSlot], Is.EqualTo(1F));
            for (var slot = toPreceedingSlot + 1; slot < slots; slot++)
                for (var follSlot = 0; follSlot < slots; follSlot++)
                    Assert.That(problem.ConsecutiveSlotAptitudes[slot, follSlot], Is.EqualTo(1F));
        }

        [Test]
        [Repeat(1000)]
        public void AllPreceedingConsecutiveSlotAptitudesIsCorrectlyConfigured()
        {
            var multiplier = (Single)(rnd.NextDouble() * 2);
            var slots = rnd.Next(20) + 2;
            var followingSlot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Multiplying.AptitudeBy(multiplier).WhenAnySlot().IsFollowedBySlot(followingSlot)
                .Build();

            for (var slot = 0; slot < slots; slot++)
                Assert.That(problem.ConsecutiveSlotAptitudes[slot, followingSlot], Is.EqualTo(multiplier));
        }

        [Test]
        [Repeat(1000)]
        public void FollowingRangeConsecutiveSlotAptitudesIsCorrectlyConfigured()
        {
            var multiplier = (Single)(rnd.NextDouble() * 2);
            var slots = rnd.Next(20) + 2;
            var preceedingSlot = rnd.Next(slots);
            var fromFollowingSlot = rnd.Next(slots);
            var toFollowingSlot = rnd.Next(slots);
            if (fromFollowingSlot > toFollowingSlot)
            {
                var swap = fromFollowingSlot;
                fromFollowingSlot = toFollowingSlot;
                toFollowingSlot = swap;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Multiplying.AptitudeBy(multiplier).WhenSlot(preceedingSlot).IsFollowedBySlots().From(fromFollowingSlot).To(toFollowingSlot)
                .Build();

            for (var slot = fromFollowingSlot; slot <= toFollowingSlot; slot++)
                Assert.That(problem.ConsecutiveSlotAptitudes[preceedingSlot, slot], Is.EqualTo(multiplier));
        }

        [Test]
        [Repeat(1000)]
        public void FollowingRangeConsecutiveSlotAptitudesDoesNotChangeUnaffectedSlots()
        {
            var multiplier = (Single)(rnd.NextDouble() * 2);
            var slots = rnd.Next(20) + 2;
            var preceedingSlot = rnd.Next(slots);
            var fromFollowingSlot = rnd.Next(slots);
            var toFollowingSlot = rnd.Next(slots);
            if (fromFollowingSlot > toFollowingSlot)
            {
                var swap = fromFollowingSlot;
                fromFollowingSlot = toFollowingSlot;
                toFollowingSlot = swap;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Multiplying.AptitudeBy(multiplier).WhenSlot(preceedingSlot).IsFollowedBySlots().From(fromFollowingSlot).To(toFollowingSlot)
                .Build();

            for (var slot = 0; slot < fromFollowingSlot; slot++)
                for (var precSlot = 0; precSlot < slots; precSlot++)
                    Assert.That(problem.ConsecutiveSlotAptitudes[precSlot, slot], Is.EqualTo(1F));
            for (var slot = toFollowingSlot + 1; slot < slots; slot++)
                for (var precSlot = 0; precSlot < slots; precSlot++)
                    Assert.That(problem.ConsecutiveSlotAptitudes[precSlot, slot], Is.EqualTo(1F));
        }

        [Test]
        [Repeat(1000)]
        public void AllFollowingConsecutiveSlotAptitudesIsCorrectlyConfigured()
        {
            var multiplier = (Single)(rnd.NextDouble() * 2);
            var slots = rnd.Next(20) + 2;
            var preceedingSlot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Multiplying.AptitudeBy(multiplier).WhenSlot(preceedingSlot).IsFollowedByAnySlot()
                .Build();

            for (var slot = 0; slot < slots; slot++)
                Assert.That(problem.ConsecutiveSlotAptitudes[preceedingSlot, slot], Is.EqualTo(multiplier));
        }

        [Test]
        [Repeat(100)]
        public void ConsecutiveSlotMultipliersAreAlways1WhenTheFirstRuleIsConfigured()
        {
            var slots = rnd.Next(20) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Multiplying.AptitudeBy(1).WhenSlot(0).IsFollowedBySlot(0)
                .Build();

            for (int s1 = 0; s1 < slots; s1++)
                for (int s2 = 0; s2 < slots; s2++)
                    Assert.That(problem.ConsecutiveSlotAptitudes[s1, s2], Is.EqualTo(1));
        }

        [Test]
        [Repeat(100)]
        public void CrossItemMultipliersAreAlways1WhenTheFirstRuleIsConfigured()
        {
            var slots = rnd.Next(10) + 2;
            var items = rnd.Next(10) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(items)
                .ConsideringThat.WhenItem(0).CoversSlot(0).AptitudeIsMultipliedBy(1).ForItem(1).CoveringSlot(1)
                .Build();

            for (int s1 = 0; s1 < slots; s1++)
                for (int s2 = 0; s2 < slots; s2++)
                    for (int i1 = 0; i1 < items; i1++)
                        for (int i2 = 0; i2 < items; i2++)
                            Assert.That(problem.CrossItemAptitudes[s1, s2, i1, i2], Is.EqualTo(1F));
        }

        [Test]
        [Repeat(100)]
        public void CrossItemtAptitudesIsCorrectlyConfigured()
        {
            var multiplier = (Single)(rnd.NextDouble() * 2);
            var slots = rnd.Next(20) + 2;
            var slot1 = rnd.Next(slots);
            var slot2 = rnd.Next(slots);
            var items = rnd.Next(20) + 2;
            var item1 = rnd.Next(items);
            var item2 = rnd.Next(items);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(items)
                .ConsideringThat.WhenItem(item1).CoversSlot(slot1).AptitudeIsMultipliedBy(multiplier).ForItem(item2).CoveringSlot(slot2)
                .Build();

            Assert.That(problem.CrossItemAptitudes[slot1, slot2, item1, item2], Is.EqualTo(multiplier));
        }

        [Test]
        [Repeat(1000)]
        public void DayRangeUnavailabilityIsCorrectlyConfigured()
        {
            var days = rnd.Next(50) + 2;
            var unavailableDayMin = rnd.Next(days);
            var unavailablDayMax = rnd.Next(days);
            if (unavailableDayMin > unavailablDayMax)
            {
                var swap = unavailableDayMin;
                unavailableDayMin = unavailablDayMax;
                unavailablDayMax = swap;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Making.Item(5).UnavailableForSlot(5).InDays().From(unavailableDayMin).To(unavailablDayMax)
                .Build();

            Assert.That(problem.ItemsUnavailabilities[0].Days.From, Is.EqualTo(unavailableDayMin));
            Assert.That(problem.ItemsUnavailabilities[0].Days.To, Is.EqualTo(unavailablDayMax));
        }

        [Test]
        [Repeat(1000)]
        public void DefaultAptitudeIsCorrectlySet()
        {
            var aptitude = (Single)(rnd.NextDouble() * 3);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .WithDefaultAptitude(aptitude)
                .Build();

            Assert.That(problem.DefaultAptitude, Is.EqualTo(aptitude));
        }

        [Test]
        [Repeat(1000)]
        public void FirstDayIsCorrectlySet()
        {
            var firstDay = DateTime.Now.AddDays(rnd.Next(1000));

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .WithFirstDay(firstDay)
                .Build();

            Assert.That(problem.FirstDay, Is.EqualTo(firstDay));
        }

        [Test]
        [Repeat(1000)]
        public void InitializedCompatibleSlotsDimensionsAreEqualToSlotsXSlots()
        {
            var slots = rnd.Next(20) + 2;
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .MakingSlot(0).CompatibleWithSlot(1)
                .Build();

            Assert.That(problem.CompatibleSlots.GetLength(0), Is.EqualTo(problem.Slots));
            Assert.That(problem.CompatibleSlots.GetLength(1), Is.EqualTo(problem.Slots));
        }

        [Test]
        [Repeat(1000)]
        public void InitializedConsecutiveSlotAptitudesLengthIsEqualToSlots()
        {
            var slots = rnd.Next(20) + 2;
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Multiplying.AptitudeBy(1F).WhenSlot(0).IsFollowedBySlot(1)
                .Build();

            Assert.That(problem.ConsecutiveSlotAptitudes.GetLength(0), Is.EqualTo(problem.Slots));
            Assert.That(problem.ConsecutiveSlotAptitudes.GetLength(1), Is.EqualTo(problem.Slots));
        }

        [Test]
        [Repeat(100)]
        public void InitializedCrossItemAptitudesDimensionsAreEqualToSlotsXSlotsXItemsXItems()
        {
            var items = rnd.Next(20) + 2;
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .ConsideringThat.WhenItem(0).CoversSlot(0).AptitudeIsMultipliedBy(1F).ForItem(1).CoveringSlot(1)
                .Build();

            Assert.That(problem.CrossItemAptitudes.GetLength(0), Is.EqualTo(problem.Slots));
            Assert.That(problem.CrossItemAptitudes.GetLength(1), Is.EqualTo(problem.Slots));
            Assert.That(problem.CrossItemAptitudes.GetLength(2), Is.EqualTo(problem.Items));
            Assert.That(problem.CrossItemAptitudes.GetLength(3), Is.EqualTo(problem.Items));
        }

        [Test]
        [Repeat(1000)]
        public void InitializedItemStartupEffortsLengthIsEqualToItems()
        {
            var items = rnd.Next(20) + 2;
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .AssigningStartupEffort(1F).ToItem(0)
                .Build();

            Assert.That(problem.ItemStartupEfforts.Length, Is.EqualTo(problem.Items));
        }

        [Test]
        [Repeat(1000)]
        public void InitializedItemWeightsLengthIsEqualToItems()
        {
            var items = rnd.Next(20) + 2;
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .AssigningWeight(1f).ToItem(0)
                .Build();

            Assert.That(problem.ItemWeights.Length, Is.EqualTo(problem.Items));
        }

        [Test]
        [Repeat(1000)]
        public void InitializedSlotLengthsLengthIsEqualToSlots()
        {
            var slots = rnd.Next(20) + 2;
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningLength(2).ToSlot(0)
                .Build();

            Assert.That(problem.SlotLengths.Length, Is.EqualTo(problem.Slots));
        }

        [Test]
        [Repeat(1000)]
        public void InitializedSlotValuesLengthIsEqualToSlots()
        {
            var slots = rnd.Next(20) + 2;
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningValue(1F).ToSlot(0)
                .Build();

            Assert.That(problem.SlotValues.Length, Is.EqualTo(problem.Slots));
        }

        [Test]
        [Repeat(1000)]
        public void InitializedSlotWeightsLengthIsEqualToSlots()
        {
            var slots = rnd.Next(20) + 2;
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningWeight(1F).ToSlot(0)
                .Build();

            Assert.That(problem.SlotWeights.Length, Is.EqualTo(problem.Slots));
        }

        [Test]
        [Repeat(1000)]
        public void ItemRangeAptitudeIsCorrectlyConfigured()
        {
            var items = rnd.Next(20) + 2;
            var itemMin = rnd.Next(items);
            int itemMax = rnd.Next(items);
            if (itemMin > itemMax)
            {
                var swap = itemMin;
                itemMin = itemMax;
                itemMax = swap;
            }

            var aptitude = (Single)(rnd.NextDouble() * 2);
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .AssigningAptitude(aptitude).ToItems().From(itemMin).To(itemMax).ForSlot(5).InDay(5)
                .Build();

            Assert.That(problem.Aptitudes[0].Items.From, Is.EqualTo(itemMin));
            Assert.That(problem.Aptitudes[0].Items.To, Is.EqualTo(itemMax));
            Assert.That(problem.Aptitudes[0].Aptitude, Is.EqualTo(aptitude));
        }

        [Test]
        [Repeat(1000)]
        public void ItemStartupEffortIsCorrectlySet()
        {
            var items = rnd.Next(50) + 2;
            var effort = (Single)(rnd.NextDouble() * 2);
            var item = rnd.Next(items);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .AssigningStartupEffort(effort).ToItem(item)
                .Build();

            Assert.That(problem.ItemStartupEfforts[item], Is.EqualTo(effort));
        }

        [Test]
        [Repeat(1000)]
        public void ItemUnavailabilityRangeIsCorrectlyConfigured()
        {
            var items = rnd.Next(20) + 2;
            var unavailableItemMin = rnd.Next(items);
            var unavailableItemMax = rnd.Next(items);
            if (unavailableItemMin > unavailableItemMax)
            {
                var swap = unavailableItemMin;
                unavailableItemMin = unavailableItemMax;
                unavailableItemMax = swap;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .Making.Items().From(unavailableItemMin).To(unavailableItemMax).UnavailableForSlot(5).InDay(5)
                .Build();

            Assert.That(problem.ItemsUnavailabilities[0].Items.From, Is.EqualTo(unavailableItemMin));
            Assert.That(problem.ItemsUnavailabilities[0].Items.To, Is.EqualTo(unavailableItemMax));
        }

        [Test]
        [Repeat(1000)]
        public void ItemWeightIsCorrectlySet()
        {
            var items = rnd.Next(50) + 2;
            var weight = (Single)(rnd.NextDouble() * 2);
            var item = rnd.Next(items);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .AssigningWeight(weight).ToItem(item)
                .Build();

            Assert.That(problem.ItemWeights[item], Is.EqualTo(weight));
        }

        [Test]
        [Repeat(1000)]
        public void MultipleItemWeightIsCorrectlySet()
        {
            var items = rnd.Next(50) + 2;
            var weight = (Single)(rnd.NextDouble() * 2);
            var fromItem = rnd.Next(items);
            var toItem = rnd.Next(items);
            if (fromItem > toItem)
            {
                var temp = fromItem;
                fromItem = toItem;
                toItem = temp;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .AssigningWeight(weight).ToItems().From(fromItem).To(toItem)
                .Build();

            for (int i = fromItem; i <= toItem; i++)
                Assert.That(problem.ItemWeights[i], Is.EqualTo(weight));
        }

        [Test]
        [Repeat(1000)]
        public void AllItemsWeightIsCorrectlySet()
        {
            var items = rnd.Next(50) + 2;
            var weight = (Single)(rnd.NextDouble() * 2);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .AssigningWeight(weight).ToAllItems()
                .Build();

            for (int i = 0; i < problem.Items; i++)
                Assert.That(problem.ItemWeights[i], Is.EqualTo(weight));
        }

        [Test]
        [Repeat(1000)]
        public void SlotWeightIsCorrectlySet()
        {
            var slots = rnd.Next(50) + 2;
            var weight = (Single)(rnd.NextDouble() * 2);
            var slot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningWeight(weight).ToSlot(slot)
                .Build();

            Assert.That(problem.SlotWeights[slot], Is.EqualTo(weight));
        }

        [Test]
        [Repeat(1000)]
        public void MultipleSlotWeightIsCorrectlySet()
        {
            var slots = rnd.Next(50) + 2;
            var weight = (Single)(rnd.NextDouble() * 2);
            var fromSlot = rnd.Next(slots - 2);
            var toSlot = rnd.Next(slots - 2);
            if (fromSlot > toSlot)
            {
                var temp = fromSlot;
                fromSlot = toSlot;
                toSlot = temp;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningWeight(weight).ToSlots().From(fromSlot).To(toSlot)
                .Build();

            for (int i = fromSlot; i <= toSlot; i++)
                Assert.That(problem.SlotWeights[i], Is.EqualTo(weight));
        }

        [Test]
        [Repeat(1000)]
        public void AllSlotsWeightIsCorrectlySet()
        {
            var slots = rnd.Next(50) + 2;
            var weight = (Single)(rnd.NextDouble() * 2);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningWeight(weight).ToAllSlots()
                .Build();

            for (int i = 0; i < problem.Slots; i++)
                Assert.That(problem.SlotWeights[i], Is.EqualTo(weight));
        }

        [Test]
        [Repeat(1000)]
        public void MaxConsecutiveWorkingDaysIsCorrectlySet()
        {
            var max = rnd.Next(10);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .WithMaxConsecutiveWorkingDaysEqualTo(max)
                .Build();

            Assert.That(problem.MaxConsecutiveWorkingDays, Is.EqualTo(max));
        }

        [Test]
        [Repeat(1000)]
        public void RestAfterMaxWorkingDaysReachedIsCorrectlySet()
        {
            var rest = rnd.Next(10);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .RestAfterMaxWorkingDaysReached(rest)
                .Build();

            Assert.That(problem.RestAfterMaxWorkingDaysReached, Is.EqualTo(rest));
        }

        [Test]
        [Repeat(1000)]
        public void SingleDayUnavailabilityIsCorrectlyConfigured()
        {
            var days = rnd.Next(50) + 2;
            var unavailableDay = rnd.Next(days);

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Making.Item(5).UnavailableForSlot(5).InDay(unavailableDay)
                .Build();

            Assert.That(problem.ItemsUnavailabilities[0].Days.From, Is.EqualTo(unavailableDay));
            Assert.That(problem.ItemsUnavailabilities[0].Days.To, Is.EqualTo(unavailableDay));
        }

        [Test]
        [Repeat(1000)]
        public void SingleItemAptitudeIsCorrectlyConfigured()
        {
            var items = rnd.Next(20) + 2;
            var item = rnd.Next(items);

            var aptitude = (Single)(rnd.NextDouble() * 2);
            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .AssigningAptitude(aptitude).ToItem(item).ForSlot(5).InDay(5)
                .Build();

            Assert.That(problem.Aptitudes[0].Items.From, Is.EqualTo(item));
            Assert.That(problem.Aptitudes[0].Items.To, Is.EqualTo(item));
            Assert.That(problem.Aptitudes[0].Aptitude, Is.EqualTo(aptitude));
        }

        [Test]
        [Repeat(1000)]
        public void SingleItemUnavailabilityIsCorrectlyConfigured()
        {
            var items = rnd.Next(20) + 2;
            var unavailableItem = rnd.Next(items);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(items)
                .Making.Item(unavailableItem).UnavailableForSlot(5).InDay(5)
                .Build();

            Assert.That(problem.ItemsUnavailabilities[0].Items.From, Is.EqualTo(unavailableItem));
            Assert.That(problem.ItemsUnavailabilities[0].Items.To, Is.EqualTo(unavailableItem));
        }

        [Test]
        [Repeat(1000)]
        public void SingleSlotClosureInSingleDayIsCorrectlyAdded()
        {
            var closedSlot = rnd.Next(10);
            var closedDay = rnd.Next(10);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .Closing.Slot(closedSlot).InDay(closedDay)
                .Build();

            var sc = problem.SlotClosures[0];
            Assert.That(sc.Slots.From, Is.EqualTo(closedSlot));
            Assert.That(sc.Slots.To, Is.EqualTo(closedSlot));
            Assert.That(sc.Days.From, Is.EqualTo(closedDay));
            Assert.That(sc.Days.To, Is.EqualTo(closedDay));
        }

        [Test]
        [Repeat(1000)]
        public void SingleSlotUnavailabilityIsCorrectlyConfigured()
        {
            var slots = rnd.Next(20) + 2;
            var unavailableSlot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(this.RandomFrom5To15())
                .WithItems(slots)
                .Making.Item(5).UnavailableForSlot(unavailableSlot).InDay(5)
                .Build();

            Assert.That(problem.ItemsUnavailabilities[0].Slots.From, Is.EqualTo(unavailableSlot));
            Assert.That(problem.ItemsUnavailabilities[0].Slots.To, Is.EqualTo(unavailableSlot));
        }

        [Test]
        [Repeat(1000)]
        public void SlotCompatibilityMatrixIsSimmetric()
        {
            var slots = rnd.Next(10) + 2;
            var slot1 = 0;
            var slot2 = 1;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .MakingSlot(slot1).CompatibleWithSlot(slot2)
                .Build();

            for (var s1 = 0; s1 < slots; s1++)
                for (var s2 = s1 + 1; s2 < slots; s2++)
                    Assert.That(problem.CompatibleSlots[s1, s2], Is.EqualTo(problem.CompatibleSlots[s2, s1]));
        }

        [Test]
        [Repeat(1000)]
        public void SlotLengthIsCorrectlySet()
        {
            var slots = rnd.Next(50) + 2;
            var length = rnd.Next(5);
            var slot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningLength(length).ToSlot(slot)
                .Build();

            Assert.That(problem.SlotLengths[slot], Is.EqualTo(length));
        }

        [Test]
        [Repeat(1000)]
        public void SlotRangeLengthIsCorrectlySet()
        {
            var slots = rnd.Next(50) + 2;
            var length = rnd.Next(5);
            var fromSlot = rnd.Next(slots);
            var toSlot = rnd.Next(slots);
            if (fromSlot > toSlot)
            {
                var temp = fromSlot;
                fromSlot = toSlot;
                toSlot = temp;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningLength(length).ToSlots().From(fromSlot).To(toSlot)
                .Build();

            for (int i = fromSlot; i <= toSlot; i++)
                Assert.That(problem.SlotLengths[i], Is.EqualTo(length));
        }

        [Test]
        [Repeat(1000)]
        public void SlotLengthOutOfSlotRangeIsUnaffected()
        {
            var slots = rnd.Next(50) + 2;
            var length = rnd.Next(5);
            var fromSlot = rnd.Next(slots);
            var toSlot = rnd.Next(slots);
            if (fromSlot > toSlot)
            {
                var temp = fromSlot;
                fromSlot = toSlot;
                toSlot = temp;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningLength(length).ToSlots().From(fromSlot).To(toSlot)
                .Build();

            for (int i = 0; i < fromSlot; i++)
                Assert.That(problem.SlotLengths[i], Is.EqualTo(1));
            for (int i = toSlot + 1; i < problem.Slots; i++)
                Assert.That(problem.SlotLengths[i], Is.EqualTo(1));
        }

        [Test]
        [Repeat(1000)]
        public void AllSlotLengthsIsCorrectlyConfigured()
        {
            var slots = rnd.Next(50) + 2;
            var length = rnd.Next(5);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningLength(length).ToAllSlots()
                .Build();

            for (int i = 0; i < problem.Slots; i++)
                Assert.That(problem.SlotLengths[i], Is.EqualTo(length));
        }

        [Test]
        [Repeat(1000)]
        public void SlotRangeUnavailabilityIsCorrectlyConfigured()
        {
            var slots = rnd.Next(20) + 2;
            var unavailableSlotsMin = rnd.Next(slots);
            var unavailableSlotsMax = rnd.Next(slots);
            if (unavailableSlotsMin > unavailableSlotsMax)
            {
                var swap = unavailableSlotsMin;
                unavailableSlotsMin = unavailableSlotsMax;
                unavailableSlotsMax = swap;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .Making.Item(5).UnavailableForSlots().From(unavailableSlotsMin).To(unavailableSlotsMax).InDay(5)
                .Build();

            Assert.That(problem.ItemsUnavailabilities[0].Slots.From, Is.EqualTo(unavailableSlotsMin));
            Assert.That(problem.ItemsUnavailabilities[0].Slots.To, Is.EqualTo(unavailableSlotsMax));
        }

        [Test]
        [Repeat(1000)]
        public void SingleSlotValueIsCorrectlyConfigured()
        {
            var slots = rnd.Next(50) + 2;
            var value = (Single)(rnd.NextDouble() * 2);
            var slot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningValue(value).ToSlot(slot)
                .Build();

            Assert.That(problem.SlotValues[slot], Is.EqualTo(value));
        }

        [Test]
        [Repeat(1000)]
        public void SlotRangeValuesIsCorrectlyConfigured()
        {
            var slots = rnd.Next(50) + 3;
            var fromSlot = rnd.Next(slots - 2) + 2;
            var toSlot = rnd.Next(slots - 2) + 2;
            if (fromSlot > toSlot)
            {
                var temp = fromSlot;
                fromSlot = toSlot;
                toSlot = temp;
            }
            var value = (Single)(rnd.NextDouble() * 2);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningValue(value).ToSlots().From(fromSlot).To(toSlot)
                .Build();

            for (int i = fromSlot; i <= toSlot; i++)
                Assert.That(problem.SlotValues[i], Is.EqualTo(value));
        }

        [Test]
        [Repeat(1000)]
        public void SlotValueOutOfRangeIsUnaffected()
        {
            var slots = rnd.Next(50) + 3;
            var fromSlot = rnd.Next(slots - 2) + 2;
            var toSlot = rnd.Next(slots - 2) + 2;
            if (fromSlot > toSlot)
            {
                var temp = fromSlot;
                fromSlot = toSlot;
                toSlot = temp;
            }
            var value = (Single)(rnd.NextDouble() * 2);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningValue(value).ToSlots().From(fromSlot).To(toSlot)
                .Build();

            for (int i = 0; i < fromSlot; i++)
                Assert.That(problem.SlotValues[i], Is.EqualTo(1F));
            for (int i = toSlot + 1; i < problem.Slots; i++)
                Assert.That(problem.SlotValues[i], Is.EqualTo(1F));
        }

        [Test]
        [Repeat(1000)]
        public void AllSlotValuesIsCorrectlyConfigured()
        {
            var slots = rnd.Next(50) + 3;
            var value = (Single)(rnd.NextDouble() * 2);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningValue(value).ToAllSlots()
                .Build();

            for (int i = 0; i < slots; i++)
                Assert.That(problem.SlotValues[i], Is.EqualTo(value));
        }

        [Test]
        [Repeat(1000)]
        public void SlotRangeAptitudeIsCorrectlyConfigured()
        {
            var slots = rnd.Next(20) + 2;
            var slotMin = rnd.Next(slots);
            var slotMax = rnd.Next(slots);
            if (slotMin > slotMax)
            {
                var swap = slotMin;
                slotMin = slotMax;
                slotMax = swap;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningAptitude(1F).ToItem(5).ForSlots().From(slotMin).To(slotMax).InDay(5)
                .Build();

            Assert.That(problem.Aptitudes[0].Slots.From, Is.EqualTo(slotMin));
            Assert.That(problem.Aptitudes[0].Slots.To, Is.EqualTo(slotMax));
        }

        [Test]
        [Repeat(1000)]
        public void AllSlotsAptitudeIsCorrectlyConfigured()
        {
            var slots = rnd.Next(20) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningAptitude(1F).ToItem(5).ForAllSlots().InDay(5)
                .Build();

            Assert.That(problem.Aptitudes[0].Slots.From, Is.EqualTo(0));
            Assert.That(problem.Aptitudes[0].Slots.To, Is.EqualTo(problem.Slots - 1));
        }

        [Test]
        [Repeat(1000)]
        public void SingleSlotAptitudeIsCorrectlyConfigured()
        {
            var slots = rnd.Next(20) + 2;
            var slot = rnd.Next(slots);

            var problem = ProblemBuilder.Configure()
                .WithDays(this.RandomFrom5To15())
                .WithSlots(slots)
                .WithItems(this.RandomFrom5To15())
                .AssigningAptitude(1F).ToItem(5).ForSlot(slot).InDay(5)
                .Build();

            Assert.That(problem.Aptitudes[0].Slots.From, Is.EqualTo(slot));
            Assert.That(problem.Aptitudes[0].Slots.To, Is.EqualTo(slot));
        }

        [Test]
        [Repeat(1000)]
        public void DayRangeAptitudeIsCorrectlyConfigured()
        {
            var days = rnd.Next(20) + 2;
            var dayMin = rnd.Next(days);
            var dayMax = rnd.Next(days);
            if (dayMin > dayMax)
            {
                var swap = dayMin;
                dayMin = dayMax;
                dayMax = swap;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .AssigningAptitude(1F).ToItem(5).ForSlot(5).InDays().From(dayMin).To(dayMax)
                .Build();

            Assert.That(problem.Aptitudes[0].Days.From, Is.EqualTo(dayMin));
            Assert.That(problem.Aptitudes[0].Days.To, Is.EqualTo(dayMax));
        }

        [Test]
        [Repeat(1000)]
        public void SingleDayAptitudeIsCorrectlyConfigured()
        {
            var days = rnd.Next(20) + 2;
            var day = rnd.Next(days);

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .AssigningAptitude(1F).ToItem(5).ForSlot(5).InDay(day)
                .Build();

            Assert.That(problem.Aptitudes[0].Days.From, Is.EqualTo(day));
            Assert.That(problem.Aptitudes[0].Days.To, Is.EqualTo(day));
        }

        [Test]
        [Repeat(1000)]
        public void AllDaysAptitudeIsCorrectlyConfigured()
        {
            var days = rnd.Next(20) + 2;

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(this.RandomFrom5To15())
                .WithItems(this.RandomFrom5To15())
                .AssigningAptitude(1F).ToItem(5).ForSlot(5).Always()
                .Build();

            Assert.That(problem.Aptitudes[0].Days.From, Is.EqualTo(0));
            Assert.That(problem.Aptitudes[0].Days.To, Is.EqualTo(problem.Days - 1));
        }

        private int RandomFrom5To15()
        {
            return rnd.Next(11) + 5;
        }
    }
}