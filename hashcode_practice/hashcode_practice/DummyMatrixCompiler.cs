using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.IO;

namespace hashcode_practice
{

    public struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class DummyMatrixCompiler: IMatrixCompiler
    {
        public ICollection<string> Commands {get;} = new List<string>();

        private Matrix m_Matrix;

        public void Compile(Matrix matrix)
        {
            m_Matrix = matrix;
            for (var squareSize = 0; squareSize >= 0; squareSize--)
            {
                FillWithSquares(squareSize);    
            }
        }

        public void Save(string outFile)
        {
            using (var writer = File.CreateText(outFile))
            {
                writer.WriteLine(Commands.Count);

                foreach(var x in Commands)
                {
                    writer.WriteLine(x);
                }
            }
        }

        private void FillWithSquares(int squareSize)
        {
            //Console.WriteLine($"Fill with square size: {squareSize}");

            for (int x = squareSize; x + squareSize < m_Matrix.Width; x++)
                for (int y = squareSize; y + squareSize < m_Matrix.Height; y++)
                    AnalyzePrint(x, y, squareSize);
        }

        private void AnalyzePrint(int centerX, int centerY, int size)
        {
            List<Point> list = new List<Point>();

            for (int x = centerX - size; x <= centerX + size; x++)
            {
                for (int y = centerY - size; y <= centerY + size; y++)
                {
                    if (!m_Matrix.Data[x][y])
                    {
                        list.Add(new Point(x, y));
                    }
                }
            }

            if (Math.Pow(size + 1, 2) > list.Count && list.Count == 0)
            {
                PrintSquare(centerX, centerY, size);
                //EraseCell(list);
            }
        }

        private void PrintSquare(int centerX, int centerY, int size)
        {
            Commands.Add($"PAINT_SQUARE {centerY} {centerX} {size}");
            for (int x = centerX - size; x < centerX + size; x++)
            {
                for (int y = centerY - size; y < centerY + size; y++)
                {
                    m_Matrix.Data[x][y] = false;
                }
            }
                
        }

        private void EraseCell(IEnumerable<Point> list)
        {
            foreach (var point in list)
            {
                Commands.Add($"ERASE_CELL {point.y} {point.x}");    
            }
        }
    }
}