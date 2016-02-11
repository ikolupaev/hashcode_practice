using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hashcode_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"../../data/", "*.in");

            foreach( var f in files )
            {
                ParseFile(f, f.Replace(".in", ".out"));
            }
        }

        private static void ParseFile(string inFile, string outFile)
        {
            IMatrixCompiler parser = new DummyMatrixCompiler();

            Console.WriteLine(inFile);
            var matrix = new Matrix();

            Console.WriteLine("Loading...");
            matrix.Load(inFile);
            Console.WriteLine(matrix);


            Console.WriteLine("Compiling...");
            parser.Compile(matrix);

            Console.WriteLine("Saving...");
            parser.Save(outFile);
        }
    }
}
