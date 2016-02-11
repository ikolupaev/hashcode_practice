using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hashcode_practice;

namespace Drones
{
    public abstract class Command { }

    public class LoadCommand : Command
    {
        private int amount;
        private int productType;
        private int warehouseIndex;

        public LoadCommand(int warehouseIndex, int productType, int amount)
        {
            this.warehouseIndex = warehouseIndex;
            this.productType = productType;
            this.amount = amount;
        }

        public override string ToString()
        {
            return $"L {this.warehouseIndex} {this.productType} {this.amount}";
        }
    }

    public class DeliverCommand : Command
    {
        private int amount;
        private int orderIndex;
        private int productType;

        public DeliverCommand(int orderIndex, int productType, int amount)
        {
            this.orderIndex = orderIndex;
            this.productType = productType;
            this.amount = amount;
        }

        public override string ToString()
        {
            return $"D {this.orderIndex} {this.productType} {this.amount}";
        }
    }

    public class WaitCommand : Command { }
}
