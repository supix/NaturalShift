﻿//-----------------------------------------------------------------------
// <copyright file="IncreasingRowsRandomColumns.cs" company="supix">
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