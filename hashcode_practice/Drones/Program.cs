using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hashcode_practice;

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

            Console.WriteLine($"Planing {inFile}...");

            var commander = new Commander(data);
            commander.PlanWork();

            SaveWork(commander.drones, outFile);
        }

        private static void SaveWork(List<Drone> drones, string fileName)
        {
            using (var writer = File.CreateText(fileName))
            {
                writer.WriteLine(drones.Sum(d => d.Commands.Count));

                int i = 0;
                foreach (var drone in drones)
                {
                    foreach (var command in drone.Commands)
                    {
                        writer.WriteLine($"{i} {command.ToString()}");
                    }
                    i++;
                }
            }

            Console.WriteLine($"{fileName} is saved");
        }
    }
}
