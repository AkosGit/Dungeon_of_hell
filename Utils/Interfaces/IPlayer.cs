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
        public bool IsMoving { get; set; }
        public bool IsHurting { get; set; }
        public bool IsSpeaking { get; set; }
        public int armor { get; set; }
        public double A { get; set; }
        public double Dx { get; set; }
        public double Dy { get; set; }
    }
}
