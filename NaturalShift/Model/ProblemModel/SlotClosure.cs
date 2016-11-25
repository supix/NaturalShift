namespace NaturalShift.Model.ProblemModel
{
    public class SlotClosure
    {
        internal SlotClosure()
        {
            this.Days = new IntRange();
            this.Slots = new IntRange();
        }

        public IntRange Days { get; set; }
        public IntRange Slots { get; set; }
    }
}