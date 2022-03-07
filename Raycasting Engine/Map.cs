using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
	}
}
