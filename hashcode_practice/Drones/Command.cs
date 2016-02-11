using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drones
{
    public abstract class Command { }

    public class LoadCommand : Command { }

    public class DeliverCommand : Command { }

    public class WaitCommand : Command { }
}
