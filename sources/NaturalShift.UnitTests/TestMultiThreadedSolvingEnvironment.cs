using Moq;
using NaturalShift.Model.ProblemModel;
using NaturalShift.SolvingEnvironment;
using NUnit.Framework;
using System;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class TestSolvingEnvironment
    {
        [Test]
        public void ConstructorRaisesExceptionWithNullEnvironmentConfig()
        {
            var problem = new Mock<Problem>().Object;
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MultiThreadedSolvingEnvironment(problem, null);
            });
        }

        [Test]
        public void ConstructorRaisesExceptionWithNullProblem()
        {
            var environmentConfig = new Mock<EnvironmentConfig>().Object;
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MultiThreadedSolvingEnvironment(null, environmentConfig);
            });
        }

        [Test]
        public void ConstructorRaisesExceptionWithOneThread()
        {
            var problem = new Mock<Problem>().Object;
            var environmentConfig = new Mock<EnvironmentConfig>().Object;
            Assert.Throws<ArgumentException>(() =>
            {
                new MultiThreadedSolvingEnvironment(problem, environmentConfig, 1);
            });
        }

        [Test]
        public void ZeroThreadsDefaultsToProcessorCount()
        {
            var problem = new Mock<Problem>().Object;
            var environmentConfig = new Mock<EnvironmentConfig>().Object;

            var solvingEnvironment = new MultiThreadedSolvingEnvironment(problem, environmentConfig, 0);
            Assert.That(solvingEnvironment.NumberOfThreads, Is.EqualTo(System.Environment.ProcessorCount));
        }
    }
}