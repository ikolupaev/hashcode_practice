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
            var drones = InitializeDrones(data.DronesAmount);

            for (int i = 0; i < data.MaxTurns; i++)
            {
                foreach (var drone in drones)
                {
                    
                }    
            }
        }

        private static List<Drone> InitializeDrones(int amount)
        {
            var drones = new List<Drone>(amount);

            for (int i = 0; i < amount; i++)
            {
                drones.Add(new Drone());
            }

            return drones;
        } 
    }
}
