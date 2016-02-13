using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drones;

namespace hashcode_practice
{
    public class Drone
    {
        public List<Command> Commands { get; } = new List<Command>();

        public int WillBeFreeAtStep { get; set; }

        public Coordinate FreeLocation { get; set; }
    }
}
