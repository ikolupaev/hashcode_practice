namespace hashcode_practice
{
    interface IMatrixCompiler
    {
        void Compile(Matrix matrix);
        void Save(string outFile);
    }
}