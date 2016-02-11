using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Coordinate Location { get; set; }
        public List<Product> Products { get; } = new List<Product>(); 
    }

    public class Product
    {
        public int ProductType { get; set; }
        public int Amount { get; set; }
    }

    public class Order
    {
        public Coordinate Location { get; set; }

        List<Product> Products { get; } = new List<Product>();
    }
}
