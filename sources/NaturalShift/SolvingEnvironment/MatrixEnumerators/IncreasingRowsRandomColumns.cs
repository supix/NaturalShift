using System;
using System.Collections.Generic;
using System.Linq;

namespace NaturalShift.SolvingEnvironment.MatrixEnumerators
{
    internal class IncreasingRowsRandomColumns : IMatrixEnumerator
    {
        private readonly int rows;
        private readonly int cols;
        private readonly Random random = new Random();
        private int curRow;
        private int curCol;
        private int[,] matrix;

        public IncreasingRowsRandomColumns(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            matrix = new int[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                var list = new List<KeyValuePair<int, int>>();

                for (int col = 0; col < cols; col++)
                    list.Add(new KeyValuePair<int, int>(random.Next(), col));

                var shuffledColIndexes =
                    from kvp in list
                    orderby kvp.Key
                    select kvp.Value;

                int c = 0;

                foreach (var i in shuffledColIndexes)
                    matrix[row, c++] = i;
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
            var thereIsANext = (curRow < rows);

            if (thereIsANext)
            {
                row = curRow;
                col = matrix[curRow, curCol];

                curCol++;
                if (curCol == cols)
                {
                    curCol = 0;
                    curRow++;
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