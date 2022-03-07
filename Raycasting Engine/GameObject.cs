using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Raycasting_Engine
{
	public class GameObject
	{
		protected int mapS;
		int gridX;
		int gridY;
		double x;
		double y;

		int type;

		public int GridX { get => gridX; set => gridX = value; }
		public int GridY { get => gridY; set => gridY = value; }
		public double X { get => x; set => x = value; }
		public double Y { get => y; set => y = value; }
		public int Type { get => type; set => type = value; }

		public GameObject(int gridX, int gridY, int Type = 0)
		{
			this.gridX = gridX;
			this.gridY = gridY;
			this.type = Type;
		}
	}
}
