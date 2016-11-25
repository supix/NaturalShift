using NaturalShift.SolvingEnvironment.ItemSelectors;
using NUnit.Framework;
using System;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class TestRouletteWheel
    {
        private Random rnd = new Random();

        [Test]
        public void SelectSecondItemWhenRouletteFallsOnTheRightBoundaryOfTheSecondSector()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[] { 1F, 1F, 0.1F, 1F, 1F };

            //second/third boundary is 2 / 4.1 = 0.4878048
            int? selItem = itemSelector.SelectItem(array, .4878F);

            Assert.That(selItem, Is.EqualTo(1));
        }

        [Test]
        public void SelectThirdItemWhenRouletteFallsOnTheLeftBoundaryOfTheThirdSector()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[] { 1F, 1F, 0.1F, 1F, 1F };

            //second/third boundary is 2 / 4.1 = 0.4878048
            int? selItem = itemSelector.SelectItem(array, .4879F);

            Assert.That(selItem, Is.EqualTo(2));
        }

        [Test]
        public void SelectThirdItemWhenRouletteFallsOnTheRightBoundaryOfTheThirdSector()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[] { 1F, 1F, 0.1F, 1F, 1F };

            //third/fourth boundary is 2.1 / 4.1 = 0.5121951
            int? selItem = itemSelector.SelectItem(array, .5121F);

            Assert.That(selItem, Is.EqualTo(2));
        }

        [Test]
        public void SelectFourthItemWhenRouletteFallsOnTheLeftBoundaryOfTheFourthSector()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[] { 1F, 1F, 0.1F, 1F, 1F };

            //third/fourth boundary is 2.1 / 4.1 = 0.5121951
            int? selItem = itemSelector.SelectItem(array, .5122F);

            Assert.That(selItem, Is.EqualTo(3));
        }

        [Test]
        [Repeat(1000)]
        public void ShouldSelectAnItem()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[1 + rnd.Next(10)];
            for (int i = 0; i < array.Length; i++)
                array[i] = 1F;
            var rouletteValue = (float)rnd.NextDouble();

            int? selItem = itemSelector.SelectItem(array, rouletteValue);

            Assert.That(selItem, Is.GreaterThanOrEqualTo(0).And.LessThanOrEqualTo(array.Length));
        }

        [Test]
        [Repeat(1000)]
        public void ShouldNotSelectAnItemWithZeroAptitude()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[2 + rnd.Next(10)];
            for (int i = 0; i < array.Length; i++)
                array[i] = 1F;
            var zeroItemIndex = rnd.Next(array.Length);
            array[zeroItemIndex] = 0F;
            var rouletteValue = (float)rnd.NextDouble();

            int? selItem = itemSelector.SelectItem(array, rouletteValue);

            Assert.That(selItem,
                Is.GreaterThanOrEqualTo(0)
                .And
                .LessThanOrEqualTo(array.Length)
                .And.Not.EqualTo(zeroItemIndex));
        }

        [Test]
        [Repeat(1000)]
        public void ShouldSelectTheOnlyItemWithNonZeroAptitude()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[1 + rnd.Next(10)];
            for (int i = 0; i < array.Length; i++)
                array[i] = 0F;
            var nonZeroItemIndex = rnd.Next(array.Length);
            array[nonZeroItemIndex] = 1F;
            var rouletteValue = (float)rnd.NextDouble();

            int? selItem = itemSelector.SelectItem(array, rouletteValue);

            Assert.That(selItem, Is.EqualTo(nonZeroItemIndex));
        }

        [Test]
        [Repeat(1000)]
        public void ShouldSelectNoItemsWhenAptitudeAreAllZero()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[1 + rnd.Next(10)];
            for (int i = 0; i < array.Length; i++)
                array[i] = 0F;
            var rouletteValue = (float)rnd.NextDouble();

            int? selItem = itemSelector.SelectItem(array, rouletteValue);

            Assert.That(selItem, Is.Null);
        }

        [Test]
        public void SelectionThrowsWhenRouletteValueIsGreaterThanOne()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[] { 1F, 1F, 1F, 1F, 1F };

            Assert.Throws<IndexOutOfRangeException>(() => itemSelector.SelectItem(array, 1.0001F));
        }

        [Test]
        public void DoNotSelectFirstCandidateWhenHasZeroAptitudeEvenWithZeroRouletteValue()
        {
            var itemSelector = new RouletteWheel();
            var array = new Single[] { 0F, 1F, 1F, 1F, 1F };

            var selected = itemSelector.SelectItem(array, 0F);

            Assert.That(selected, Is.Not.EqualTo(0));
        }
    }
}