// Copyright (c) 2016 - esposito.marce@gmail.com
// This file is part of NaturalShift.
// 
// NaturalShift is free software: you can redistribute it and/or modify
// it under the terms of the Affero GNU General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// Foobar is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;

namespace NaturalShift.SolvingEnvironment.MatrixEnumerators
{
    internal class IncreasingColumnsRandomRows : IMatrixEnumerator
    {
        private readonly int rows;
        private readonly int cols;
        private readonly Random random = new Random();
        private int curRow;
        private int curCol;
        private int[,] matrix;

        public IncreasingColumnsRandomRows(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            matrix = new int[rows, cols];

            for (int col = 0; col < cols; col++)
            {
                var list = new List<KeyValuePair<int, int>>();

                for (int row = 0; row < rows; row++)
                    list.Add(new KeyValuePair<int, int>(random.Next(), row));

                var shuffledRowIndexes =
                    from kvp in list
                    orderby kvp.Key
                    select kvp.Value;

                int r = 0;
                foreach (var i in shuffledRowIndexes)
                    matrix[r++, col] = i;
            }

            Reset();
        }

        public void Reset()
        {
            curRow = 0;
            curCol = 0;
        }

        public bool GetNext(out int row, out int col)
        {
            var thereIsANext = (curCol < cols);

            if (thereIsANext)
            {
                col = curCol;
                row = matrix[curRow, curCol];

                curRow++;
                if (curRow == rows)
                {
                    curRow = 0;
                    curCol++;
                }
            }
            else
            {
                row = -1;
                col = -1;
            }

            return thereIsANext;
        }
    }
}