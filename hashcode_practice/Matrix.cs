using System;
using System.IO;

namespace hashcode_practice
{
    internal class Matrix
    {
        public int Rows;
        public int Columns;

        public bool[,] Data;

        public Matrix()
        {
        }

        internal void Load(string inFile)
        {
            using (var reader = File.OpenText(inFile))
            {
                SetDimentions(reader.ReadLine());

                var line = reader.ReadLine();
                var row = 0;
                while( line != null )
                {
                    for(var i = 0; i< line.Length; i++)
                    {
                        Data[row, i] = line[i] == '#';
                    }
                    row++;
                    line = reader.ReadLine();
                }
            }
        }

        private void SetDimentions(string line)
        {
            var s = line.Split(' ');
            Rows = int.Parse(s[0]);
            Columns = int.Parse(s[1]);

            Data = new bool[Rows,Columns];
        }

        public override string ToString()
        {
            return Rows + " x " + Columns;
        }
    }
}