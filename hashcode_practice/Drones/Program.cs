using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = Directory.GetFiles(@"../../data/", "*.in");

            foreach (var f in files)
            {
                ParseFile(f, f.Replace(".in", ".out"));
            }

            Console.WriteLine("Complete. Please press any key...");
            Console.ReadLine();
        }

        private static void ParseFile(string inFile, string outFile)
        {
            var data = new DronesData();
            data.Load(inFile);

            SimulateWork(data);
        }

        private static void SimulateWork(DronesData data)
        {
            for (int i = 0; i < data.MaxTurns; i++)
            {
                
            }
        }
    }
}
