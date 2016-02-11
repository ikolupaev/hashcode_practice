using hashcode_practice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Drones
{
    internal class Commander
    {
        private DronesData data;
        private List<Drone> drones;

        public Commander(DronesData data)
        {
            this.data = data;
            this.drones = Enumerable.Repeat<Drone>(new Drone(), data.NumberOfDrones).ToList();
        }

        internal void PlanWork()
        {
            foreach (var order in data.Orders)
            {
                foreach (var product in order.Products)
                {
                    var warehouse = data.Warehouses.FirstOrDefault(w => w.Products.Any(p => p.ProductType == product.ProductType));
                    var drone = GetBestDrone();
                    var warehouseProduct = warehouse.Products[product.ProductType];
                    var amount = Math.Min(product.Amount, warehouseProduct.Amount);

                    drone.Commands.Add(new LoadCommand(warehouse.Index, product.ProductType, amount));
                    drone.Commands.Add(new DeliverCommand(order.Index, product.ProductType, amount));
                }
            }
        }

        private Drone GetBestDrone()
        {
            return drones.First();
        }
    }
}