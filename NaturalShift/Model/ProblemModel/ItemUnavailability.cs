namespace NaturalShift.Model.ProblemModel
{
    public class ItemUnavailability
    {
        internal ItemUnavailability()
        {
            this.Days = new IntRange();
            this.Slots = new IntRange();
            this.Items = new IntRange();
        }

        public IntRange Days { get; set; }
        public IntRange Slots { get; set; }
        public IntRange Items { get; set; }
    }
}