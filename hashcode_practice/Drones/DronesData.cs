using hashcode_practice;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public int Rows { get; private set; }

        public int NumberOfDrones { get; private set; }

        public static int MaxPayLoad { get; private set; }
        public int ProductTypes { get; private set; }
        public int MaxTurns { get; private set; }

        public static int[] ProductWeights { get; private set; }

        public int NumberOfWarehouse { get; private set; }
        public Warehouse[] Warehouses { get; private set; }

        public int NumberOfOrders { get; private set; }
        public Order[] Orders { get; private set; }

        TextReader reader;

        internal void Load(string inFile)
        {
            using (reader = File.OpenText(inFile))
            {
                SetParameters(reader.ReadLine());

                this.ProductTypes = int.Parse(reader.ReadLine());
                ProductWeights = ReadLineAsArrayOfInts();
                this.NumberOfWarehouse = int.Parse(reader.ReadLine());

                LoadWareHouses();

                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            this.NumberOfOrders = ReadLineAsArrayOfInts().First();

            this.Orders = new Order[this.NumberOfOrders];

            for(int i = 0; i< NumberOfOrders; i++)
            {
                this.Orders[i] = new Order { Index = i};
                LoadOrder(this.Orders[i]);
            }
        }

        private void LoadOrder(Order order)
        {
            var a = ReadLineAsArrayOfInts();
            order.Location = new Coordinate(a[0], a[1]);
            var numberOfItems = ReadLineAsArrayOfInts().First();
            LoadOrderProducts(order.Products);

            Debug.Assert(numberOfItems == order.Products.Count);
        }

        private void LoadWareHouses()
        {
            this.Warehouses = new Warehouse[NumberOfWarehouse];
            for( int i = 0; i < NumberOfWarehouse; i++ )
            {
                Warehouses[i] = new Warehouse {Index = i};
                LoadWareHouse(Warehouses[i]);
            }
        }

        private void LoadWareHouse(Warehouse warehouse)
        {
            var a = ReadLineAsArrayOfInts();
            warehouse.Location = new Coordinate(a[0], a[1]);

            LoadWarehouseProducts(warehouse.Products);
        }

        void LoadOrderProducts( List<Product> list )
        {
            var productsQuantity = ReadLineAsArrayOfInts();
            list.AddRange(productsQuantity.Select((index) => new Product { ProductType = index, Amount = 1 }));
        }

        void LoadWarehouseProducts(List<Product> list)
        {
            var productsQuantity = ReadLineAsArrayOfInts();
            list.AddRange(productsQuantity.Select((q, index) => new Product { ProductType = index, Amount = q }));
        }

        private int[] ReadLineAsArrayOfInts()
        {
            return reader.ReadLine().Split(' ').Select(int.Parse).ToArray();
        }

        private void SetParameters(string v)
        {
            var s = v.Split(' ');
            this.Rows = int.Parse(s[0]);
            this.Columns = int.Parse(s[1]);
            this.NumberOfDrones = int.Parse(s[2]);
            this.MaxTurns = int.Parse(s[3]);
            MaxPayLoad = int.Parse(s[4]);
        }
    }
}