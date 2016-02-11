using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drones;

namespace hashcode_practice
{
    public enum State
    {
        InWarehouse,
        AtCustomer,
        Fly
    }

    public class Drone
    {
        public int Count { get; set; }
        public State CurrentState { get; set; }
        public int MaxPayload => DronesData.MaxPayLoad;
        public int CurrentPayload { get; set; }

        public List<Product> Products { get; } = new List<Product>();

        public List<Command> Commands { get; } = new List<Command>(); 

        //public void AddProduct(Product product)
        //{
        //    if(CurrentPayload + product.Weight > MaxPayload)
        //        throw new InvalidOperationException("Out of limit by maxpayload");

        //    Products.Add(product);
        //    Commands.Add(new LoadCommand());
        //    CurrentPayload += product.Weight;
        //}

        //public void AddOrder(Order order)
        //{
        //    if (CurrentPayload + order.Weight > MaxPayload)
        //        throw new InvalidOperationException("Out of limit by maxpayload");

        //    Products.AddRange(order.Products);
        //    CurrentPayload += order.Weight;
        //}

        public void Deliver()
        {
            
        }

        public void Wait(int number)
        {
            
        }
    }
}
