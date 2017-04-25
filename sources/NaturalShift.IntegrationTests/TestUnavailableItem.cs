//-----------------------------------------------------------------------
// <copyright file="TestUnavailableItem.cs" company="supix">
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
using NaturalShift.Model.ProblemModel.FluentInterfaces;
using NaturalShift.SolvingEnvironment;
using NUnit.Framework;

namespace NaturalShift.IntegrationTests
{
    [TestFixture]
    public class TestUnavailableItem
    {
        private Random rnd = new Random();

        [Test]
        [Repeat(10)]
        public void UnavailableItemDoesntShift()
        {
            const int days = 5;
            const int slots = 5;
            const int items = 8;
            var fromSlot = rnd.Next(slots);
            var toSlot = rnd.Next(slots);
            if (fromSlot > toSlot)
            {
                var temp = fromSlot;
                fromSlot = toSlot;
                toSlot = temp;
            }

            var fromDay = rnd.Next(days);
            var toDay = rnd.Next(days);
            if (fromDay > toDay)
            {
                var temp = fromDay;
                fromDay = toDay;
                toDay = temp;
            }

            var fromItem = rnd.Next(items);
            var toItem = rnd.Next(items);
            if (fromItem > toItem)
            {
                var temp = fromItem;
                fromItem = toItem;
                toItem = temp;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(slots)
                .WithItems(items)
                .Making.Items().From(fromItem).To(toItem).UnavailableForSlots().From(fromSlot).To(toSlot).InDays().From(fromDay).To(toDay)
                .Build();

            var solvingEnvironment = SolvingEnvironmentBuilder.Configure()
                .ForProblem(problem)
                .WithPopulationSize(100)
                .RenewingPopulationAfterSameFitnessEpochs(10)
                .StoppingComputationAfter(1).Milliseconds
                .Build();

            var solution = solvingEnvironment.Solve();

            for (int day = fromDay; day <= toDay; day++)
                for (int slot = fromSlot; slot <= toSlot; slot++)
                    Assert.That(solution.Allocations[day, slot], Is.Null.Or.Not.InRange(fromItem, toItem));
        }
    }
}