using System;

namespace NaturalShift.Model.ProblemModel
{
    public class IntRange
    {
        public IntRange() : this(0)
        {
        }

        public IntRange(int from, int to)
        {
            if (from > to)
                throw new ArgumentException("from must be less or equal than to");

            this.From = from;
            this.To = to;
        }

        public IntRange(int fromto) : this(fromto, fromto)
        {
        }

        public int From { get; set; }
        public int To { get; set; }
    }
}