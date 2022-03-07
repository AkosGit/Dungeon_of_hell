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
		protected Canvas canvas;
		int gridX;
		int gridY;
		double x;
		double y;

		public int GridX { get => gridX; set => gridX = value; }
		public int GridY { get => gridY; set => gridY = value; }
		public double X { get => x; set => x = value; }
		public double Y { get => y; set => y = value; }

		public EntityObject(int gridX, int gridY, Canvas canvas, int mapS, bool isSolid = false)
			: base(isSolid)
		{
			this.gridX = gridX;
			this.gridY = gridY;
		}
	}
}
