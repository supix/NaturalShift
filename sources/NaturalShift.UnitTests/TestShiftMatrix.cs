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

using NaturalShift.SolvingEnvironment.Matrix;
using NUnit.Framework;
using System;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class Test_ShiftMatrix
    {
        private Single defApt;
        private int days;
        private int slots;
        private int items;
        private ShiftMatrix m;

        [OneTimeSetUp]
        public void TestFixtureSetUp()
        {
            defApt = 1.33F;
            days = 10;
            slots = 11;
            items = 12;
        }

        [SetUp]
        public void SetUp()
        {
            m = new ShiftMatrix(days, slots, items, defApt);
        }

        [Test]
        public void TestInstantiation()
        {
            Assert.AreEqual(days, m.Days);
            Assert.AreEqual(slots, m.Slots);
            Assert.AreEqual(items, m.Items);
            Assert.AreEqual(days * slots, m.GetNumberOfUnforcedSlots());
            Assert.AreEqual(days * slots, m.GetNumberOfSlots());

            for (int day = 0; day < m.Days; day++)
                for (int slot = 0; slot < m.Slots; slot++)
                    for (int item = 0; item < m.Items; item++)
                        Assert.AreEqual(defApt, m[day, slot].InitialAptitudes[item]);
        }

        [Test]
        public void TestForce()
        {
            m[1, 2].Force(3);
            Assert.AreEqual(days * slots - 1, m.GetNumberOfUnforcedSlots());
            Assert.AreEqual(days * slots, m.GetNumberOfSlots());
            Assert.That(m[1, 2].ChosenItem, Is.EqualTo(3));
        }

        [Test]
        public void TestToString()
        {
            var output = m.ToString();
            Console.WriteLine(output);
            Assert.That(output, Is.Not.Null.Or.Empty);
        }
    }
}