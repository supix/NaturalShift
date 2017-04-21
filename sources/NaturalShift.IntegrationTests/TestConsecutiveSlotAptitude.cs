//-----------------------------------------------------------------------
// <copyright file="TestConsecutiveSlotAptitude.cs" company="supix">
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
using NaturalShift.Model.ProblemModel.FluentInterfaces;
using NaturalShift.SolvingEnvironment;
using NUnit.Framework;
using System;

namespace NaturalShift.IntegrationTests
{
    [TestFixture]
    public class TestConsecutiveSlotAptitude
    {
        private Random rnd = new Random();

        [Test]
        [Repeat(10)]
        public void ZeroConsecutiveSlotAptitudeIsRespected()
        {
            const int days = 5;
            const int slots = 5;
            const int items = 5;

            var fromSlot1 = rnd.Next(slots);
            var toSlot1 = rnd.Next(slots);
            if (fromSlot1 > toSlot1)
            {
                var temp = fromSlot1;
                fromSlot1 = toSlot1;
                toSlot1 = temp;
            }

            var fromSlot2 = rnd.Next(slots);
            var toSlot2 = rnd.Next(slots);
            if (fromSlot2 > toSlot2)
            {
                var temp = fromSlot2;
                fromSlot2 = toSlot2;
                toSlot2 = temp;
            }

            var problem = ProblemBuilder.Configure()
                .WithDays(days)
                .WithSlots(slots)
                .WithItems(items)
                .Multiplying.AptitudeBy(0).WhenSlots().From(fromSlot1).To(toSlot1).IsFollowedBySlots().From(fromSlot2).To(toSlot2)
                .Build();

            var solvingEnvironment = SolvingEnvironmentBuilder.Configure()
                .ForProblem(problem)
                .WithPopulationSize(100)
                .RenewingPopulationAfterSameFitnessEpochs(10)
                .StoppingComputationAfter(1).Milliseconds
                .Build();

            var solution = solvingEnvironment.Solve();

            for (int item = 0; item < problem.Items; item++)
                for (int day = 0; day < problem.Days - 1; day++)
                    if ((solution.Allocations[item, day] >= fromSlot1) &&
                        ((solution.Allocations[item, day] <= toSlot1)))
                        Assert.That(solution.Allocations[item, day + 1], Is.Null.Or.Not.InRange(fromSlot2, toSlot2));
        }
    }
}