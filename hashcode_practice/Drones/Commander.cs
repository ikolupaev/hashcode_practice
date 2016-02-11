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
            this.drones = Enumerable.Repeat<Drone>(new Drone(), data.NumberOfDrones).ToList();
        }

        internal void PlanWork()
        {
            foreach( var order in data.Orders )
            {

            }
        }
    }
}