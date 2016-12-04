using NaturalShift.Model.ProblemModel.FluentInterfaces;
using NaturalShift.SolvingEnvironment;
using NUnit.Framework;
using System;

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

            for (int item = fromItem; item <= toItem; item++)
                for (int day = fromDay; day <= toDay; day++)
                    Assert.That(solution.Allocations[item, day], Is.Null.Or.Not.InRange(fromSlot, toSlot));
        }
    }
}