using hashcode_practice;
using System;
using System.IO;
using System.Linq;

namespace Drones
{
    internal class DronesData
    {
        public DronesData()
        {
        }

        public int Columns { get; private set; }
        public int Drones { get; private set; }
        public static int MaxPayLoad { get; private set; }
        public int ProductTypes { get; private set; }
        public int Rows { get; private set; }
        public int MaxTurns { get; private set; }
        public static int[] ProductWeights { get; private set; }
        public int NumberOfWarehouse { get; private set; }
        public Warehouse[] Warehouses { get; private set; }

        TextReader reader;

        internal void Load(string inFile)
        {
            using (reader = File.OpenText(inFile))
            {
                SetParameters(reader.ReadLine());

                this.ProductTypes = int.Parse(reader.ReadLine());
                ProductWeights = StringToArrayOfInts(reader.ReadLine());
                this.NumberOfWarehouse = int.Parse(reader.ReadLine());

                LoadWareHouses();
            }
        }

        private void LoadWareHouses()
        {
            this.Warehouses = new Warehouse[NumberOfWarehouse];
            for( int i = 0; i < NumberOfWarehouse; i++ )
            {
                Warehouses[i] = new Warehouse();
                LoadWareHouse(Warehouses[i]);
            }
        }

        private void LoadWareHouse(Warehouse warehouse)
        { var a = StringToArrayOfInts(reader.ReadLine());
            warehouse.Location = new Coordinate(a[0], a[1]);
        }

        private int[] StringToArrayOfInts(string v)
        {
            return v.Split(' ').Select(int.Parse).ToArray();
        }

        private void SetParameters(string v)
        {
            var s = v.Split(' ');
            this.Rows = int.Parse(s[0]);
            this.Columns = int.Parse(s[1]);
            this.Drones = int.Parse(s[2]);
            this.MaxTurns = int.Parse(s[3]);
            this.MaxPayLoad = int.Parse(s[4]);
        }
    }
}