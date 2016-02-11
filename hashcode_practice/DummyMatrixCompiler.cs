using System;
using System.Collections.Generic;
using System.IO;

namespace hashcode_practice
{
    class DummyMatrixCompiler: IMatrixCompiler
    {
        ICollection<string> commands = new List<string>();

        public void Compile(Matrix matrix)
        {
            for (var x = 0; x < matrix.Rows; x++)
            {
                for (var y = 0; y < matrix.Columns; y++)
                {
                    if (matrix.Data[x, y])
                    {
                        commands.Add($"PAINT_SQUARE {x} {y} 0");
                    }
                }
            }
        }

        public void Save(string outFile)
        {
            using (var writer = File.CreateText(outFile))
            {
                writer.WriteLine(commands.Count);

                foreach(var x in commands)
                {
                    writer.WriteLine(x);
                }
            }
        }
    }
}