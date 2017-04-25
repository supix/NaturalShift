//-----------------------------------------------------------------------
// <copyright file="TestAllocationState.cs" company="supix">
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
using System;
using Moq;
using NaturalShift.SolvingEnvironment.ItemSelectors;
using NaturalShift.SolvingEnvironment.Matrix;
using NUnit.Framework;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class Test_AllocationState
    {
        [Test]
        public void ShouldConstructorInitializeDaySlotAndAptitudes()
        {
            var itemSelector = new RouletteWheel();
            var allState = new AllocationState(10, 15, new Single[] { 1.5F, 2.5F, 3.5F, 4.5F, 5.5F }, itemSelector);
            Assert.That(allState.Day, Is.EqualTo(10));
            Assert.That(allState.Slot, Is.EqualTo(15));
            Assert.That(allState.InitialAptitudes.Length, Is.EqualTo(allState.CurrentAptitudes.Length));
        }

        [Test]
        public void ShouldResetCorrectlyInitializeState()
        {
            var itemSelector = new RouletteWheel();
            var allState = new AllocationState(10, 15, new Single[] { 1.5F, 2.5F, 3.5F, 4.5F, 5.5F }, itemSelector);
            allState.Reset();
            Assert.That(allState.Processed, Is.False);
            Assert.That(allState.Forced, Is.False);
            Assert.That(allState.ChosenItem, Is.Null);
            Assert.That(allState.InitialAptitudes, Is.EquivalentTo(allState.CurrentAptitudes));
        }

        [Test]
        public void TestForce()
        {
            var itemSelector = new RouletteWheel();
            var allState = new AllocationState(10, 15, new Single[] { 1.5F, 2.5F, 3.5F, 4.5F, 5.5F }, itemSelector);
            allState.Reset();
            allState.Force(2);
            Assert.That(allState.Processed, Is.True);
            Assert.That(allState.Forced, Is.True);
            Assert.That(allState.ChosenItem, Is.EqualTo(2));
        }

        [Test]
        public void TestForceAndResetIfNotForced()
        {
            var itemSelector = new RouletteWheel();
            var allState = new AllocationState(10, 15, new Single[] { 1.5F, 2.5F, 3.5F, 4.5F, 5.5F }, itemSelector);
            allState.Reset();
            allState.Force(2);
            //Overwrites currentaptitudes
            for (int i = 0; i < allState.InitialAptitudes.Length; i++)
                allState.CurrentAptitudes[i] = 0;

            allState.ResetIfNotForced();
            Assert.That(allState.Processed, Is.True);
            Assert.That(allState.Forced, Is.True);
            Assert.That(allState.ChosenItem, Is.EqualTo(2));
        }

        [Test]
        public void TestForceAndResetForced()
        {
            var itemSelector = new RouletteWheel();
            var allState = new AllocationState(10, 15, new Single[] { 1.5F, 2.5F, 3.5F, 4.5F, 5.5F }, itemSelector);
            allState.Reset();
            allState.Force(2);
            //Overwrites currentaptitudes
            for (int i = 0; i < allState.InitialAptitudes.Length; i++)
                allState.CurrentAptitudes[i] = 0;

            allState.Reset();
            Assert.That(allState.Processed, Is.False);
            Assert.That(allState.Forced, Is.False);
            Assert.That(allState.ChosenItem, Is.Null);
        }

        [Test]
        public void TestProcessNotNull()
        {
            var expResult = 1;
            var array = new Single[] { 1.5F, 2.5F, 3.5F, 4.5F, 5.5F };
            var rndValue = .3F;

            var itemSelectorMock = new Mock<IItemSelector>();
            itemSelectorMock.Setup(x => x.SelectItem(array, rndValue)).Returns(expResult);
            var itemSelector = itemSelectorMock.Object;

            var allState = new AllocationState(10, 15, array, itemSelector);
            allState.Reset();

            allState.Process(rndValue);

            Assert.That(allState.Processed, Is.True);
            Assert.That(allState.Forced, Is.False);
            Assert.That(allState.ChosenItem, Is.EqualTo(expResult));
            itemSelectorMock.Verify(x => x.SelectItem(array, rndValue), Times.Once);
        }

        [Test]
        public void TestProcessNull()
        {
            int? expResult = null;
            var array = new Single[] { 1.5F, 2.5F, 3.5F, 4.5F, 5.5F };
            var rndValue = .3F;

            var itemSelectorMock = new Mock<IItemSelector>();
            itemSelectorMock.Setup(x => x.SelectItem(array, rndValue)).Returns(expResult);
            var itemSelector = itemSelectorMock.Object;

            var allState = new AllocationState(10, 15, array, itemSelector);
            allState.Reset();

            allState.Process(rndValue);

            Assert.That(allState.Processed, Is.True);
            Assert.That(allState.Forced, Is.False);
            Assert.That(allState.ChosenItem, Is.Null);
            itemSelectorMock.Verify(x => x.SelectItem(array, rndValue), Times.Once);
        }
    }
}