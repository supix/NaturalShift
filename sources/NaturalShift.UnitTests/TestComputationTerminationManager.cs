using NaturalShift.SolvingEnvironment;
using NUnit.Framework;
using System;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class TestComputationTerminationManager
    {
        private Random rnd = new Random();

        [Test]
        [Repeat(1000)]
        public void MaxElapsedTimeConstraintShouldWorkWhenTimeExeeds()
        {
            var maxTime = rnd.Next(1000) + 1;
            var elapsedTime = maxTime + 1 + rnd.Next(1000);
            var manager = new ComputationTerminationManager(maxTime, 0, 0);

            Assert.That(manager.Terminated(elapsedTime, 100, 100), Is.True);
        }

        [Test]
        [Repeat(1000)]
        public void MaxElapsedTimeConstraintShouldWorkWhenTimeDoesNotExeed()
        {
            var elapsedTime = rnd.Next(1000) + 1;
            var maxTime = elapsedTime + 1 + rnd.Next(1000);
            var manager = new ComputationTerminationManager(maxTime, 0, 0);

            Assert.That(manager.Terminated(elapsedTime, 100, 100), Is.False);
        }

        [Test]
        [Repeat(1000)]
        public void MaxEpochsConstraintShouldWorkWhenEpochsExeed()
        {
            var maxEpochs = rnd.Next(1000) + 1;
            var elapsedEpochs = maxEpochs + 1 + rnd.Next(1000);
            var manager = new ComputationTerminationManager(0, maxEpochs, 0);

            Assert.That(manager.Terminated(100, elapsedEpochs, 100), Is.True);
        }

        [Test]
        [Repeat(1000)]
        public void MaxEpochsConstraintShouldWorkWhenEpochsDoNotExeed()
        {
            var elapsedEpochs = rnd.Next(1000) + 1;
            var maxEpochs = elapsedEpochs + 1 + rnd.Next(1000);
            var manager = new ComputationTerminationManager(0, maxEpochs, 0);

            Assert.That(manager.Terminated(100, elapsedEpochs, 100), Is.False);
        }

        [Test]
        [Repeat(1000)]
        public void MaxEpochsWithoutFitnessImprovementConstraintShouldWorkWhenEpochsExeed()
        {
            var maxEpochsWFO = rnd.Next(1000) + 1;
            var elapsedEpochsWFO = maxEpochsWFO + 1 + rnd.Next(1000);
            var manager = new ComputationTerminationManager(0, 0, maxEpochsWFO);

            Assert.That(manager.Terminated(100, 100, elapsedEpochsWFO), Is.True);
        }

        [Test]
        [Repeat(1000)]
        public void MaxEpochsWithoutFitnessImprovementConstraintShouldWorkWhenEpochsDoNotExeed()
        {
            var elapsedEpochsWFO = rnd.Next(1000) + 1;
            var maxEpochsWFO = elapsedEpochsWFO + 1 + rnd.Next(1000);
            var manager = new ComputationTerminationManager(0, 0, maxEpochsWFO);

            Assert.That(manager.Terminated(100, 100, elapsedEpochsWFO), Is.False);
        }

        [Test]
        [Repeat(1000)]
        public void ShouldRiseExceptionWhenAllContraintsAreZero()
        {
            Assert.That(() => new ComputationTerminationManager(0, 0, 0), Throws.ArgumentException);
        }
    }
}