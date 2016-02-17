using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hashcode_practice;

namespace Drones
{
    public interface ICommand { }

    public class LoadCommand : ICommand
    {
        private int amount;
        private int productType;
        private int warehouseIndex;

        protected virtual string GetCommandLetter()
        {
            return "L";
        }

        public LoadCommand(int warehouseIndex, int productType, int amount)
        {
            this.warehouseIndex = warehouseIndex;
            this.productType = productType;
            this.amount = amount;
        }

        public override string ToString()
        {
            return $"{GetCommandLetter()} {this.warehouseIndex} {this.productType} {this.amount}";
        }
    }

    public class UnloadCommand: LoadCommand
    {
        public UnloadCommand(int warehouseIndex, int productType, int amount) : base(warehouseIndex, productType, amount) { }

        protected override string GetCommandLetter()
        {
            return "U";
        }
    }

    public class DeliverCommand : ICommand
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

    public class WaitCommand : ICommand { }
}
