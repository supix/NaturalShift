using System;

namespace NaturalShift.Model.ProblemModel
{
    public class ItemAptitude
    {
        internal ItemAptitude()
        {
            this.Days = new IntRange();
            this.Slots = new IntRange();
            this.Items = new IntRange();
        }

        public IntRange Days { get; set; }
        public IntRange Slots { get; set; }
        public IntRange Items { get; set; }
        public Single Aptitude { get; set; }
    }
}