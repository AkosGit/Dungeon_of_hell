using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Raycasting_Engine
{
    public class Player : MovableEntityObject,IPlayer
    {
        public Player(int gridX, int gridY, int mapS, bool isSolid = false, int a = 0) :base(gridX, gridY, mapS, isSolid,a)
        {

        }

    }
}
