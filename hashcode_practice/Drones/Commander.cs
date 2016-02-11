using hashcode_practice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Drones
{
    internal class Commander
    {
        private DronesData data;
        public List<Drone> drones;

        public Commander(DronesData data)
        {
            this.data = data;
            this.drones = new List<Drone>(data.NumberOfDrones);

            for (int i = 0; i < data.NumberOfDrones; i++)
            {
                drones.Add(new Drone());
            }
        }

        internal void PlanWork()
        {
            foreach (var order in data.Orders)
            {
                foreach (var product in order.Products)
                {
                    var warehouse = data.Warehouses.FirstOrDefault(w => w.Products.Any(p => p.ProductType == product.ProductType && p.Amount > 0));
                    //if (warehouse == null)
                    //    continue;
                    var drone = GetBestDrone();
                    if (drone == null)
                        return;
                    var warehouseProduct = warehouse.Products[product.ProductType];
                    var amount = Math.Min(product.Amount, warehouseProduct.Amount);

                    drone.Commands.Add(new LoadCommand(warehouse.Index, product.ProductType, amount));
                    drone.Commands.Add(new DeliverCommand(order.Index, product.ProductType, amount));
                    drone.Count++;
                }
            }
        }



        private Drone GetBestDrone()
        {
            return drones.FirstOrDefault(d => d.Count < 170);
        }
    }
}