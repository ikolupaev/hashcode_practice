using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drones;

namespace hashcode_practice
{
    public class Drone
    {
        public int Index { get; set; }
        public List<ICommand> Commands { get; } = new List<ICommand>();

        public int WillBeFreeAtStep { get; set; }

        public Coordinate FreeLocation { get; set; }

        public int LoadedWeight => LoadedProducts.Sum(x => x.Weight * x.Quantity);

        public List<Product> LoadedProducts { get; } = new List<Product>();

        public int GetProductQuantity( int productType )
        {
            var p = LoadedProducts.FirstOrDefault(x => x.ProductType == productType);

            if (p == null) return 0;

            return p.Quantity;
        }
    }
}
