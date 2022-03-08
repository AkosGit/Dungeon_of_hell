using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Raycasting_Engine
{
	public class Map
	{
		Canvas canvas;

		public int MaxL;

		public int mapX;
		public int mapY;
		public int mapS;

		public GameObject[] map;

		public Map(int maxL, int mapX, int mapY, int mapS, GameObject[] map)
		{
			MaxL = maxL;
			this.mapX = mapX;
			this.mapY = mapY;
			this.mapS = mapS;
			this.map = map;
		}

		public Map()
		{
			GameObject Wall;
			GameObject Wall2;
			GameObject Air;

			Wall = new SolidObject(0, 0, Color.FromArgb(255, 130, 160, 255), true);
			Wall2 = new SolidObject(0, 0, Color.FromArgb(255, 226, 107, 139), true);
			Air = new GameObject(false);

			map = new GameObject[]
			{
				Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Wall, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Wall2, Air, Wall, Air, Wall, Air, Air, Air, Wall, Air, Wall2, Air, Wall,
				Wall, Air, Wall, Wall, Air, Air, Air, Wall, Air, Air, Wall, Wall, Air, Air, Air, Wall,
				Wall, Air, Wall, Air, Air, Air, Air, Wall, Air, Air, Wall, Air, Air, Air, Air, Wall,
				Wall, Air, Wall, Air, Air, Air, Air, Wall, Air, Air, Wall, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Wall, Air, Wall, Wall, Wall, Wall, Wall, Air, Wall, Wall, Wall, Wall, Wall, Wall, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Wall, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Wall, Air, Air, Air, Air, Air, Air, Air, Wall2,
				Wall, Air, Air, Wall, Air, Wall, Air, Wall, Air, Air, Air, Wall, Air, Wall, Air, Wall2,
				Wall, Air, Wall, Wall, Air, Air, Air, Wall, Air, Air, Wall, Wall, Air, Air, Air, Wall2,
				Wall, Air, Wall, Air, Air, Air, Air, Wall, Air, Air, Wall, Air, Air, Air, Air, Wall2,
				Wall, Air, Wall, Air, Air, Air, Air, Wall, Air, Air, Wall, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall,
			};
			mapX = 16;
			mapY = 16;
			mapS = 64;

			MaxL = 16;
		}
	}
}
