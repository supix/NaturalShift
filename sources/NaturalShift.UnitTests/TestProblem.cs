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