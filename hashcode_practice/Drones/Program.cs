﻿using System;
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
            var files = Directory.GetFiles(@"../../data/", "mother_of_all_warehouses.in");

            foreach (var f in files)
            {
                ParseFile(f, f.Replace(".in", ".out"));
            }
        }

        private static void ParseFile(string inFile, string outFile)
        {
            var data = new DronesData();
            data.Load(inFile);

        }
    }
}
