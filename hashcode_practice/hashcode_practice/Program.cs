using System;
using System.IO;
using System.Threading;

namespace hashcode_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"../../data/", "*.in");

            foreach( var f in files )
            {
                new Thread(() => ParseFile(f, f.Replace(".in", ".out"))).Start();      
            }

            Console.ReadLine();
        }

        private static void ParseFile(string inFile, string outFile)
        {
            IMatrixCompiler parser = new DummyMatrixCompiler();

            Console.WriteLine(inFile);
            var matrix = new Matrix();

            Console.WriteLine($"Loading {inFile}...");
            matrix.Load(inFile);
            Console.WriteLine(matrix);


            Console.WriteLine($"Compiling {inFile}...");
            parser.Compile(matrix);

            parser.Save(outFile);
            Console.WriteLine($"Saved {outFile}: {parser.Commands.Count}...");
        }
    }
}
