using NaturalShift.Model.ProblemModel;
using NUnit.Framework;
using System;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class TestIntRange
    {
        private Random rnd = new Random();

        [Test]
        public void ParameterlessConstructorShouldGiveZeroRange()
        {
            var r = new IntRange();

            Assert.That(r.From, Is.Zero);
            Assert.That(r.To, Is.Zero);
        }

        [Test]
        [Repeat(1000)]
        public void OneParameterConstructorShouldGiveRange()
        {
            var value = rnd.Next();
            var r = new IntRange(value);

            Assert.That(r.From, Is.EqualTo(value));
            Assert.That(r.To, Is.EqualTo(value));
        }

        [Test]
        [Repeat(1000)]
        public void TwoParametersConstructorShouldGiveRange()
        {
            var value1 = rnd.Next();
            var value2 = rnd.Next();
            if (value1 > value2)
            {
                var swap = value1;
                value1 = value2;
                value2 = swap;
            }
            var r = new IntRange(value1, value2);

            Assert.That(r.From, Is.EqualTo(value1));
            Assert.That(r.To, Is.EqualTo(value2));
        }
    }
}