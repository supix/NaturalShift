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

using NaturalShift.SolvingEnvironment.MatrixEnumerators;
using NUnit.Framework;

namespace NaturalShift.UnitTests
{
    [TestFixture]
    public class TestMatrixEnumerator
    {
        [Test]
        public void Test_RRIC()
        {
            const int cols = 20;
            const int rows = 15;
            var me = new IncreasingColumnsRandomRows(rows, cols);
            var m = new bool[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    m[r, c] = false;

            int row, col;
            int k = 0;
            while (me.GetNext(out row, out col))
            {
                Assert.IsFalse(m[row, col]);
                Assert.AreEqual(k++ / rows, col);
                m[row, col] = true;
            }
            Assert.AreEqual(cols * rows, k);

            foreach (var b in m)
                Assert.IsTrue(b);
        }

        [Test]
        public void Test_RRIC_ZeroMatrix()
        {
            const int cols = 0;
            const int rows = 0;
            var me = new IncreasingColumnsRandomRows(rows, cols);
            int r, c;
            Assert.IsFalse(me.GetNext(out r, out c));
        }

        [Test]
        public void Test_IRRC()
        {
            const int cols = 20;
            const int rows = 15;
            var me = new IncreasingRowsRandomColumns(rows, cols);
            var m = new bool[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    m[r, c] = false;

            int row, col;
            int k = 0;
            while (me.GetNext(out row, out col))
            {
                Assert.IsFalse(m[row, col]);
                Assert.AreEqual(k++ / cols, row);
                m[row, col] = true;
            }
            Assert.AreEqual(cols * rows, k);

            foreach (var b in m)
                Assert.IsTrue(b);
        }

        [Test]
        public void Test_IRRC_ZeroMatrix()
        {
            const int cols = 0;
            const int rows = 0;
            var me = new IncreasingRowsRandomColumns(rows, cols);
            int r, c;
            Assert.IsFalse(me.GetNext(out r, out c));
        }
    }
}