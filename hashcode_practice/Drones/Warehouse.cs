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

    public class Warehouse : ILocated
    {
        public int Index { get; set; }
        public Coordinate Location { get; set; }
        public List<Product> Products { get; } = new List<Product>();
    }

    [DebuggerDisplay("type: {ProductType} - qty: {Quantity} - w: {Weight}")]
    public class Product
    {
        public Product()
        {
        }

        public Product(Product p)
        {
            this.ProductType = p.ProductType;
            this.Quantity = p.Quantity;
        }

        public int ProductType { get; set; }
        public int Quantity { get; set; }
        public int Weight => DronesData.ProductWeights[ProductType];
    }

    public class Order: ILocated
    {
        public int Index { get; set; }
        public Coordinate Location { get; set; }
        public List<Product> Products { get; } = new List<Product>();
        public int GetProductQuantity(int productType)
        {
            var p = Products.FirstOrDefault(x => x.ProductType == productType);

            if (p == null) return 0;

            return p.Quantity;
        }
    }
}
