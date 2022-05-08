using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface IPlayer
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int Health { get; set; }
        public int Credit { get; set; }
    }
}
