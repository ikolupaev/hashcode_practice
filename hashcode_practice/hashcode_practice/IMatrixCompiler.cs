using System.Collections.Generic;

namespace hashcode_practice
{
    interface IMatrixCompiler
    {
        ICollection<string> Commands { get; }

        void Compile(Matrix matrix);
        void Save(string outFile);
    }
}