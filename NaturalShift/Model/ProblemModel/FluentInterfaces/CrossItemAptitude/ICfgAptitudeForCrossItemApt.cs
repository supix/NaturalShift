using System;

namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgAptitudeForCrossItemApt
    {
        ICfgSecondItemForCrossItemApt AptitudeIsMultipliedBy(Single multiplier);
    }
}