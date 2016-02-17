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
        const int LoadTime = 1;
        const int UnloadTime = LoadTime;

        public Commander(DronesData data)
        {
            this.data = data;
            this.drones = new List<Drone>(data.NumberOfDrones);

            for (int i = 0; i < data.NumberOfDrones; i++)
            {
                drones.Add(new Drone
                {
                    Index = i,
                    FreeLocation = data.Warehouses[0].Location,
                    WillBeFreeAtStep = 0
                });
            }
        }

        internal void PlanWork()
        {
            while (drones.Any(x => x.WillBeFreeAtStep < data.MaxTurns))
            {
                var drone = GetBestDrone();

                //loop loading until drone will be out of capacity 

                var warehouseLocation = GetNearest(drone.FreeLocation, data.Warehouses);

                var items = GetBestItemsAccordingToCapacity(data.Warehouses[warehouseLocation.Object.Index].Products).ToList();

                if (!items.Any()) break;

                foreach (var item in items)
                {
                    LoadItem(drone, data.Warehouses[warehouseLocation.Object.Index], item.ProductType, item.Quantity);
                }

                while (drone.LoadedProducts.Any())
                {
                    var orders = GetOrdersOfItems(drone.LoadedProducts);
                    if (!orders.Any()) break;

                    var nearestOrderDestination = GetNearest(drone.FreeLocation, orders);

                    if (!HasTimeToRun(drone, nearestOrderDestination))
                    {
                        break;
                    }

                    var order = data.Orders[nearestOrderDestination.Object.Index];
                    foreach (var p in order.Products)
                    {
                        var qd = drone.GetProductQuantity(p.ProductType);
                        var qo = order.GetProductQuantity(p.ProductType);
                        if (qd > 0 && qo > 0)
                        {
                            var q = Math.Min(qd, qo);
                            UnloadItem(drone, order, p.ProductType, q);
                        }
                    }

                    RemoveEmptyProducts(order.Products);
                    RemoveEmptyProducts(drone.LoadedProducts);
                }

                UnloadAllProductsOnNearestWarehouse(drone);
            }
        }

        private void UnloadAllProductsOnNearestWarehouse(Drone drone)
        {
            var nearest = GetNearest(drone.FreeLocation, data.Warehouses);

            if (!HasTimeToRun(drone, nearest)) return;

            foreach (var x in drone.LoadedProducts)
            {
                UnloadItem(drone, data.Warehouses[nearest.Object.Index], x.ProductType, x.Quantity);
            }
        }

        private bool HasTimeToRun(Drone drone, Destination destination)
        {
            return drone.WillBeFreeAtStep + destination.Distance + UnloadTime <= data.MaxTurns;
        }

        private IEnumerable<Order> GetOrdersOfItems(IEnumerable<Product> items)
        {
            foreach (var o in data.Orders)
            {
                if (o.Products.Any(x => x.Quantity > 0 && items.Any(i => i.ProductType == x.ProductType)))
                {
                    yield return o;
                }
            }
        }

        private Destination GetNearest(Coordinate from, IEnumerable<ILocated> possibleDestinations)
        {
            var d = possibleDestinations.OrderBy(x => GetStepsToGo(from, x.Location)).First();

            return new Destination { Object = d, Distance = GetStepsToGo(from, d.Location) };
        }

        private IEnumerable<Product> GetBestItemsAccordingToCapacity(List<Product> products)
        {
            var orderedProducts = data.Orders.SelectMany(x => x.Products).Distinct();
            var selectedProducts = products.Where(x => x.Quantity > 0 && orderedProducts.Any(p => p.ProductType == x.ProductType)).Select(x => new Product(x)).ToList();

            var capacity = DronesData.MaxPayload;

            var items = new List<int>();

            while (true)
            {
                var p = selectedProducts.FirstOrDefault(x => x.Quantity > 0 && x.Weight <= capacity);

                if (p == null)
                {
                    break;
                }

                items.Add(p.ProductType);
                p.Quantity -= 1;
                capacity -= p.Weight;
            }

            return items.GroupBy(x => x).Select(x => new Product { ProductType = x.Key, Quantity = x.Count() });
        }

        private IEnumerable<Product> GetMatchedItems(Warehouse warehouse, Order order)
        {
            foreach (var warehouseProduct in warehouse.Products.Where(x => x.Quantity > 0))
            {
                var p = order.Products.FirstOrDefault(x => x.ProductType == warehouseProduct.ProductType);

                if (p != null)
                {
                    yield return new Product
                    {
                        ProductType = p.ProductType,
                        Quantity = Math.Min(p.Quantity, warehouseProduct.Quantity)
                    };
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

        void LoadItem(Drone drone, Warehouse warehouse, int productType, int quantity)
        {
            drone.Commands.Add(new LoadCommand(warehouse.Index, productType, quantity));
            drone.WillBeFreeAtStep += GetStepsToGo(drone.FreeLocation, warehouse.Location) + 1;
            drone.FreeLocation = warehouse.Location;
            drone.LoadedProducts.Add(new Product { ProductType = productType, Quantity = quantity });

            warehouse.Products[productType].Quantity -= quantity;
        }

        void UnloadItem(Drone drone, Order order, int productType, int quantity)
        {
            drone.Commands.Add(new DeliverCommand(order.Index, productType, quantity));
            drone.WillBeFreeAtStep += GetStepsToGo(drone.FreeLocation, order.Location) + 1;
            drone.FreeLocation = order.Location;
            drone.LoadedProducts.Find(x => x.ProductType == productType).Quantity -= quantity;

            order.Products.Find(x => x.ProductType == productType).Quantity -= quantity;
        }

        void UnloadItem(Drone drone, Warehouse warehouse, int productType, int quantity)
        {
            drone.Commands.Add(new UnloadCommand(warehouse.Index, productType, quantity));
            drone.WillBeFreeAtStep += GetStepsToGo(drone.FreeLocation, warehouse.Location) + 1;
            drone.FreeLocation = warehouse.Location;
            drone.LoadedProducts.Find(x => x.ProductType == productType).Quantity -= quantity;

            warehouse.Products[productType].Quantity += quantity;
        }

        private void RemoveEmptyProducts(List<Product> products)
        {
            products.RemoveAll(x => x.Quantity == 0);
        }

        int GetStepsToDoneOrder(Drone drone, Warehouse warehouse, Order order)
        {
            return GetStepsToGo(drone.FreeLocation, warehouse.Location) + drone.LoadedProducts.Count() + 1 +
                   GetStepsToGo(warehouse.Location, order.Location) + 1;
        }

        private int GetStepsToGo(Coordinate l1, Coordinate l2)
        {
            var r = Math.Abs(l1.Row - l2.Row);
            var c = Math.Abs(l1.Column - l2.Column);

            if (r == 0 && c == 0) return 0;

            return (int)Math.Sqrt(r * r + c * c) + 1;
        }

        private Drone GetBestDrone()
        {
            return drones.Where(x => x.WillBeFreeAtStep < data.MaxTurns).OrderBy(x => x.WillBeFreeAtStep).First();
        }
    }
}