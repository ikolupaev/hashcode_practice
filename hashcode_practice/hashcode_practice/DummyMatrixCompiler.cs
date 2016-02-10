using System;
using System.Collections.Generic;
using System.IO;

namespace hashcode_practice
{
    class DummyMatrixCompiler: IMatrixCompiler
    {
        ICollection<string> commands = new List<string>();

        private Matrix m_Matrix;

        public void Compile(Matrix matrix)
        {
            m_Matrix = matrix;
            for (var squareSize = Math.Min(matrix.Rows, matrix.Columns); squareSize >= 0; squareSize--)
            {
                FillWithSquares(squareSize);    
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

        private void FillWithSquares(int squareSize)
        {
            Console.WriteLine($"Fill with square size: {squareSize}");

            for (int x = squareSize; x + squareSize < m_Matrix.Rows; x++)
                for(int y = squareSize; y + squareSize < m_Matrix.Columns; y++)
                    if (IsSquareFit(x, y, squareSize)) PrintSquare(x, y, squareSize);
        }

        private bool IsSquareFit(int x, int y, int size)
        {
            for (int row = x - size; row <= x + size; row++)
                for(int column = y - size; column <= y + size; column++)
                    if (!m_Matrix.Data[row, column]) return false;

            return true;
        }

        private void PrintSquare(int x, int y, int size)
        {
            commands.Add($"PAINT_SQUARE {x} {y} {size}");
            for(int row = x - size; row < x + size; row++)
                for (int column = y - size; column < y + size; column++)
                    m_Matrix.Data[row, column] = false;
        }
    }
}