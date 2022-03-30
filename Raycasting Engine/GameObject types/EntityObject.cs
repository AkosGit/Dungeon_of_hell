using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Raycasting_Engine
{
	public class EntityObject : GameObject
	{
		protected int gridX;
		protected int gridY;
		protected double x;
		protected double y;
		protected double z;

		protected bool visible;
		protected int health;


		public int GridX { get => gridX; set => gridX = value; }
		public int GridY { get => gridY; set => gridY = value; }
		public double X { get => x; set => x = value; }
		public double Y { get => y; set => y = value; }
		public double Z { get => z; set => z = value; }

		protected bool Visible { get => visible; set => visible = value; }
		protected int Health { get => health; }

		public EntityObject(int gridX, int gridY, int mapS, bool isSolid = false)
			: base(isSolid)
		{
			this.gridX = gridX;
			this.gridY = gridY;
		}
	}
}
