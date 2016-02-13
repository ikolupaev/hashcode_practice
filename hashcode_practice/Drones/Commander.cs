﻿using hashcode_practice;
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
                    var matchedItems = data.Warehouses.Select(w => new { WarehouseIndex = w.Index, Items = GetMatchedItems(w, order) }).OrderByDescending(x => x.Items.Count());

                    if (!matchedItems.Any()) continue;

                    Drone orderDrone = null;

                    foreach (var item in matchedItems)
                    {
                        foreach (var wi in item.Items)
                        {
                            var orderProduct = order.Products.SingleOrDefault(x => x.ProductType == wi.ProductType);

                            var warehouse = data.Warehouses[item.WarehouseIndex];

                            if (orderDrone == null)
                            {
                                var availableDrones = from drone in drones
                                                      where drone.WillBeFreeAtStep +
                                                            + GetStepsToGo(drone.FreeLocation, warehouse.Location)
                                                            + LoadTime
                                                            + (drone.LoadedProducts.Count + 1) * UnloadTime
                                                            < data.MaxTurns
                                                      select drone;

                                if (!availableDrones.Any()) continue;

                                orderDrone = availableDrones.OrderBy(x => x.WillBeFreeAtStep).First();
                            }

                            if ( orderDrone != null 
                                 && orderProduct.Quantity > 0
                                 && orderDrone.LoadedWeight + wi.Weight <= DronesData.MaxPayload )
                            {
                                LoadItem(orderDrone, warehouse, wi.ProductType, wi.Quantity);
                                orderProduct.Quantity -= wi.Quantity;
                            }
                        }
                    }

                    if (orderDrone != null)
                    {
                        foreach (var item in orderDrone.LoadedProducts)
                        {
                            UnoadItem(orderDrone, order, item.ProductType, item.Quantity);
                        }
                    }

                    RemoveEmptyProducts(order.Products);
                }
            }
        }

        private IEnumerable<Product> GetMatchedItems(Warehouse warehouse, Order order)
        {
            foreach( var warehouseProduct in warehouse.Products.Where(x=> x.Quantity > 0 ) )
            {
                var p = order.Products.FirstOrDefault(x => x.ProductType == warehouseProduct.ProductType);

                if( p != null )
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

        void UnoadItem(Drone drone, Order order, int productType, int quantity)
        {
            drone.Commands.Add(new DeliverCommand(order.Index, productType, quantity));
            drone.WillBeFreeAtStep += GetStepsToGo(drone.FreeLocation, order.Location) + 1;
            drone.FreeLocation = order.Location;

            //items are removed before to dont overload the order
            //order.Products.Single(x => x.ProductType == productType).Quantity -= quantity;
            //RemoveEmptyProducts(drone.LoadedProducts);

            drone.LoadedProducts.Single(x => x.ProductType == productType).Quantity -= quantity;
            RemoveEmptyProducts(order.Products);
        }

        private void RemoveEmptyProducts(List<Product> products)
        {
            products.RemoveAll(x=> x.Quantity == 0);
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