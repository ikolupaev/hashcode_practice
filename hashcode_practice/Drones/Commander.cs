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

        int currentDroneIndex = 0;

        public Commander(DronesData data)
        {
            this.data = data;
            this.drones = new List<Drone>(data.NumberOfDrones);

            for (int i = 0; i < data.NumberOfDrones; i++)
            {
                drones.Add(new Drone { FreeLocation = data.Warehouses[0].Location, WillBeFreeAtStep = 0 });
            }
        }

        internal void PlanWork()
        {
            while (drones.Any(x => x.WillBeFreeAtStep < data.MaxTurns))
            {
                var orders = data.Orders.Where(x => x.TotalQuantity > 0);

                if (!orders.Any()) return;

                foreach (var order in orders)
                {
                    foreach (var product in order.Products.Where(x => x.Quantity > 0))
                    {
                        var warehouse = data.Warehouses.FirstOrDefault(w => w.Products.Any(p => p.ProductType == product.ProductType && p.Quantity > 0));

                        var drone = GetBestDrone(warehouse, order);

                        var warehouseProduct = warehouse.Products[product.ProductType];
                        var amount = Math.Min(product.Quantity, warehouseProduct.Quantity);

                        SendDrone(drone, warehouse, order, product, amount);

                        warehouseProduct.Quantity -= amount;
                        product.Quantity -= amount;
                    }
                }
            }
        }

        void SendDrone(Drone drone, Warehouse warehouse, Order order, Product product, int amount)
        {
            drone.Commands.Add(new LoadCommand(warehouse.Index, product.ProductType, amount));
            drone.Commands.Add(new DeliverCommand(order.Index, product.ProductType, amount));

            drone.WillBeFreeAtStep += GetStepsToDoneOrder(drone, warehouse, order);

            drone.FreeLocation = order.Location;
        }

        int GetStepsToDoneOrder( Drone drone, Warehouse warehouse, Order order )
        {
            return GetStepsToGo(drone.FreeLocation, warehouse.Location) + 1 +
                   GetStepsToGo(warehouse.Location, order.Location) + 1;
        }

        private int GetStepsToGo(Coordinate l1, Coordinate l2)
        {
            var r = Math.Abs(l1.Row - l2.Row);
            var c = Math.Abs(l1.Column - l2.Column);

            if (r == 0 && c == 0) return 0;

            return (int)Math.Sqrt(r * r + c * c) + 1;
        }

        private Drone GetBestDrone(Warehouse warehouse, Order order)
        {
            return drones.Where( x=> x.WillBeFreeAtStep + GetStepsToDoneOrder(x, warehouse, order ) < data.MaxTurns )
                         .OrderBy(x => GetStepsToGo(x.FreeLocation, warehouse.Location)).First();
        }
    }
}