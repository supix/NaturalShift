using Moq;
using NaturalShift.Model.ProblemModel;
using NaturalShift.SolvingEnvironment;
using NUnit.Framework;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class TestSimpleSolvingEnvironment
    {
        [Test]
        public void ConstructorRaisesExceptionWithNullProblem()
        {
            var environmentConfig = new Mock<EnvironmentConfig>().Object;
            Assert.That(() => new SimpleSolvingEnvironment(null, environmentConfig), Throws.ArgumentNullException);
        }

        [Test]
        public void ConstructorRaisesExceptionWithNullEnvironmentConfig()
        {
            var problem = new Mock<Problem>().Object;
            Assert.That(() => new SimpleSolvingEnvironment(problem, null), Throws.ArgumentNullException);
        }
    }
}