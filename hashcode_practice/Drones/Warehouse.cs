using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drones;
using System.Diagnostics;

namespace hashcode_practice
{
    public struct Coordinate
    {
        public int Row;
        public int Column;

        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }

    public class Warehouse
    {
        public int Index { get; set; }
        public Coordinate Location { get; set; }
        public List<Product> Products { get; } = new List<Product>();
    }

    [DebuggerDisplay("{ProductType} - {Quantity} - {Weight}")]
    public class Product
    {
        public Product()
        {
        }
        public Product( int productType, int quantity )
        {
            this.ProductType = productType;
            this.Quantity = quantity;
        }

        public int ProductType { get; set; }
        public int Quantity { get; set; }
        public int Weight => DronesData.ProductWeights[ProductType];
    }

    public class Order
    {
        public int Index { get; set; }
        public Coordinate Location { get; set; }

        public List<Product> Products { get; } = new List<Product>();

        public int TotalWeight => Products.Sum(product => product.Weight);

        public int TotalQuantity => Products.Sum(product => product.Quantity);
    }
}
