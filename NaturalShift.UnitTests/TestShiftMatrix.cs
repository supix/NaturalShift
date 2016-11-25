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