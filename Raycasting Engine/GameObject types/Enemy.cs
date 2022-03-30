using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Raycasting_Engine.GameObject_types
{
    class Enemy : MovableEntityObject
    {
        public Brush[] Textures { get; set; }
        public Enemy(int gridX, int gridY, int mapS,Brush[] textures, bool isSolid = false, int a = 0) : base(gridX, gridY, mapS, isSolid, a)
        {
            Textures = textures;
        }
    }
}
