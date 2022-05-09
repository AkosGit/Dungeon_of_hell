using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Interfaces
{
    public interface IFirearm
    {
        public int Ammo { get; set; }
        public int Damage { get; set; }
        public int Rounds { get; set; }
        public int maxrounds { get; set; }
        public bool shotIsOngoing { get; set; }
        public bool IsShooting { get; set; }
        public bool IsReloading { get; set; }
    }
}
