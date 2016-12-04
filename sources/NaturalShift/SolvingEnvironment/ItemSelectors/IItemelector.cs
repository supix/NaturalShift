using System;

namespace NaturalShift.SolvingEnvironment.ItemSelectors
{
    internal interface IItemSelector
    {
        /// <summary>
        /// Select an item among candidates
        /// </summary>
        /// <param name="candidates">Candidates weights</param>
        /// <param name="x">The value selection is based on</param>
        /// <returns>The selected item</returns>
        int? SelectItem(Single[] candidates, Single x);
    }
}