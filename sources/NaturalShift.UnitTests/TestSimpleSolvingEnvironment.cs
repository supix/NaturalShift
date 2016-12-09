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