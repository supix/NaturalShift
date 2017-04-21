//-----------------------------------------------------------------------
// <copyright file="TestIntRange.cs" company="supix">
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