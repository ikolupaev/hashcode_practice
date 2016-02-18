using hashcode_practice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Drones
{
    static class ListProductsExtentions
    {
        public static bool ContainsAllOf(this List<Product> allProducts, List<Product> subset)
        {
            foreach( var sp in subset )
            {
                if( !allProducts.Any(x=> x.ProductType==sp.ProductType && x.Quantity>= sp.Quantity) )
                {
                    return false;
                }
            }

            return true;
        }
    }

    internal class Commander
    {
        private DronesData data;
        public List<Drone> drones;

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
                    WillBeFreeAtStep = 0,
                    FreeAtOrder = null,
                    FreeAtWarehouse = data.Warehouses[0]
                });
            }
        }

        internal void PlanWork()
        {
            while (true)
            {
                var orders = data.Orders.Where(x => x.TotalQuantity > 0);
                var freeDrones = drones.Where(x => x.WillBeFreeAtStep < data.MaxTurns).ToList();
                var freeDronesCount = freeDrones.Count();

                Console.Write($"orders left: {orders.Count()} drones left: {freeDronesCount} {new string(' ', 10)}\r");

                if (!orders.Any() || freeDronesCount == 0) break;

                var drone = GetBestDrone();

                if (drone.LoadedProducts.Any())
                {
                    throw new NotImplementedException();
                }

                var warehouse = GetNearestWarehouse(drone);
                var order = FindNearestOrderWithAllItems( warehouse.Location, warehouse.Products);
                if (order != null)
                {
                    DeliverOrderFromOneWarehouse(drone, order);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            Console.WriteLine();
        }

        private Order FindNearestOrderWithAllItems(Coordinate location, List<Product> products)
        {
            return data.Orders.Where(o => o.TotalQuantity > 0 && products.ContainsAllOf(o.Products)).OrderBy(o => GetStepsToGo(location, o.Location)).First();
        }

        private Warehouse GetNearestWarehouse(Drone drone)
        {
            if (drone.FreeAtWarehouse != null)
            {
                return drone.FreeAtWarehouse;
            }
            else
            {
                return data.Warehouses.OrderBy(x => GetStepsToGo(drone.FreeLocation, x.Location)).First();
            }
        }

        private void DeliverOrderFromOneWarehouse(Drone drone, Order order)
        {
            var warehouse = drone.FreeAtWarehouse;
            var products = new List<Product>(order.Products.Select( x=> new Product { ProductType = x.ProductType, Quantity = x.Quantity } ));

            while (products.Count > 0 && drone != null)
            {
                LoadDrone(drone, warehouse, products);
                UnloadDrone(drone, order, drone.LoadedProducts);

                if (order.Products.Count > 0)
                {
                    drone = FindNearestDrone(warehouse);
                }
            }

            if (drone == null)
            {
                throw new NotImplementedException();
            }
        }

        private Drone FindNearestDrone(Warehouse warehouse)
        {
            var drone = drones.Where(x => x.FreeAtWarehouse == warehouse).OrderBy(x => x.WillBeFreeAtStep).FirstOrDefault();

            if (drone == null)
            {
                drone = drones.Where(x => x.FreeAtWarehouse == warehouse).OrderBy(x => GetStepsToGo(warehouse.Location, x.FreeLocation)).First();
            }

            return drone;
        }

        private void UnloadDrone(Drone drone, Order order, List<Product> products)
        {
            foreach (var p in products)
            {
                UnloadProduct(drone, order, p.ProductType, p.Quantity);
            }

            RemoveEmptyProducts(order.Products);
            RemoveEmptyProducts(drone.LoadedProducts);
        }

        private void LoadDrone(Drone drone, Warehouse warehouse, List<Product> products)
        {
            var atLeastOneLoaded = true;
            while (atLeastOneLoaded)
            {
                atLeastOneLoaded = false;
                foreach (var p in products)
                {
                    var maxQuantity = (DronesData.MaxPayload - drone.LoadedWeight) / p.Weight;

                    if (p.Quantity > 0 && maxQuantity > 0)
                    {
                        var q = Math.Min(p.Quantity, maxQuantity);
                        LoadProduct(drone, warehouse, p.ProductType, q);
                        p.Quantity -= q;
                        atLeastOneLoaded = true;
                    }
                }

                RemoveEmptyProducts(products);
            }
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

        void LoadProduct(Drone drone, Warehouse warehouse, int productType, int quantity)
        {
            drone.Commands.Add(new LoadCommand(warehouse.Index, productType, quantity));
            drone.WillBeFreeAtStep += GetStepsToGo(drone.FreeLocation, warehouse.Location) + 1;
            drone.FreeLocation = warehouse.Location;
            drone.LoadedProducts.Add(new Product { ProductType = productType, Quantity = quantity });

            warehouse.Products[productType].Quantity -= quantity;
        }

        void UnloadProduct(Drone drone, Order order, int productType, int quantity)
        {
            drone.Commands.Add(new DeliverCommand(order.Index, productType, quantity));
            drone.WillBeFreeAtStep += GetStepsToGo(drone.FreeLocation, order.Location) + 1;
            drone.FreeLocation = order.Location;
            drone.LoadedProducts.Find(x => x.ProductType == productType).Quantity -= quantity;

            order.Products.Find(x => x.ProductType == productType).Quantity -= quantity;
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