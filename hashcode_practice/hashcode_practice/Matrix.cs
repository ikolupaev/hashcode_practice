using System;
using System.IO;

namespace hashcode_practice
{
    internal class Matrix
    {
        public int Height;
        public int Width;

        public bool[][] Data;

        public Matrix()
        {
        }

        internal void Load(string inFile)
        {
            using (var reader = File.OpenText(inFile))
            {
                SetDimentions(reader.ReadLine());

                var line = reader.ReadLine();
                var y = 0;
                while( line != null )
                {
                    for (var x = 0; x < line.Length; x++)
                    {
                        Data[x][y] = line[x] == '#';
                    }
                    y++;
                    line = reader.ReadLine();
                }
            }
        }

        private void SetDimentions(string line)
        {
            var s = line.Split(' ');
            Height = int.Parse(s[0]);
            Width = int.Parse(s[1]);

            Data = new bool[Width][];
            for(int x = 0; x < Width; x++ )
                Data[x] = new bool[Height];
        }

        public override string ToString()
        {
            return Height + " x " + Width;
        }
    }
}