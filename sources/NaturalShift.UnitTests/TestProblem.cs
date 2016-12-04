using NaturalShift.Model.ProblemModel;
using NUnit.Framework;
using System;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class TestProblem
    {
        private Random rnd = new Random();

        [Test]
        [Repeat(1000)]
        public void ThreeParametersConstructorDoesInitializeValues()
        {
            var days = rnd.Next(50) + 1;
            var slots = rnd.Next(50) + 2;
            var items = rnd.Next(50) + 2;

            var problem = new Problem(days, slots, items);

            Assert.That(problem.Days, Is.EqualTo(days));
            Assert.That(problem.Slots, Is.EqualTo(slots));
            Assert.That(problem.Items, Is.EqualTo(items));
        }
    }
}