namespace NaturalShift.SolvingEnvironment.MatrixEnumerators
{
    internal interface IMatrixEnumerator
    {
        void Reset();

        bool GetNext(out int col, out int row);
    }
}