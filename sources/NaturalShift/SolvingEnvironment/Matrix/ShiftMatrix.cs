using NaturalShift.SolvingEnvironment.ItemSelectors;
using System;
using System.Text;

namespace NaturalShift.SolvingEnvironment.Matrix
{
    internal class ShiftMatrix
    {
        private readonly AllocationState[,] matrix;
        private readonly int days;
        private readonly int slots;
        private readonly int items;

        public ShiftMatrix(int days, int slots, int items, Single defaultAptitude)
        {
            this.days = days;
            this.slots = slots;
            this.items = items;
            var itemSelector = new RouletteWheel();
            matrix = new AllocationState[Days, Slots];
            for (int day = 0; day < matrix.GetLength(0); day++)
                for (int slot = 0; slot < matrix.GetLength(1); slot++)
                {
                    var apts = new Single[Items];
                    for (int i = 0; i < apts.Length; i++)
                        apts[i] = defaultAptitude;
                    matrix[day, slot] = new AllocationState(day, slot, apts, itemSelector);
                }

            Reset();
        }

        public AllocationState[,] Matrix { get { return matrix; } }
        public AllocationState this[int day, int slot] { get { return matrix[day, slot]; } }
        public int Days { get { return days; } }
        public int Slots { get { return slots; } }
        public int Items { get { return items; } }

        public void Reset()
        {
            foreach (var allState in matrix)
                allState.Reset();
        }

        public void ResetUnforcedAllocationStates()
        {
            foreach (var allState in matrix)
                allState.ResetIfNotForced();
        }

        public int GetNumberOfSlots()
        {
            return days * slots;
        }

        public int GetNumberOfUnforcedSlots()
        {
            int k = 0;
            foreach (var allState in matrix)
                if (!allState.Forced)
                    k++;

            return k;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int day = 0; day < matrix.GetLength(0); day++)
            {
                for (int slot = 0; slot < matrix.GetLength(1); slot++)
                {
                    sb.Append(matrix[day, slot].ChosenItem.HasValue ? matrix[day, slot].ChosenItem.Value.ToString() : " ");
                    sb.Append(',');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}