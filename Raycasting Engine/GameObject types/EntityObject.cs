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

		private double height;
		private double width;

		protected bool visible;
		protected int health;
		protected string sound;
		protected string name;

		public string Name { get => name; set => name = value; }
		public string Sound { get => sound; set => sound = value; }
		public int GridX { get => gridX; set => gridX = value; }
		public int GridY { get => gridY; set => gridY = value; }
		public double X { get => x; set => x = value; }
		public double Y { get => y; set => y = value; }
		public double Z { get => z; set => z = value; }

		public System.Drawing.Bitmap image { get; set; }

		protected bool Visible { get => visible; set => visible = value; }
		public int Health { get => health; set => health = value; }

		public double Height { get => height; set => height = value; }
		public double Width { get => width; set => width = value; }

		public EntityObject(int gridX, int gridY, int mapS,string name, double he = 0, double wi = 0, bool isSolid = false,string sound="none")
			: base(isSolid)
		{
			this.gridX = gridX;
			this.gridY = gridY;
			health = 100;
			this.name = name;
			height = he;
			width = wi;
		}
		public bool IsHere(int mapX, int mapY)
		{
			if (mapX == gridX && mapY == gridY) return true;
			return false;
		}
	}
}
