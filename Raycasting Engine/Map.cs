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
		int maxL;

		int mapX;
		int mapY;
		int mapS;

		GameObject[] lmap;

		Player player;

		public int MaxL { get => maxL; set => maxL = value; }
		public int MapX { get => mapX; set => mapX = value; }
		public int MapY { get => mapY; set => mapY = value; }
		public int MapS { get => mapS; set => mapS = value; }
		public GameObject[] map { get => lmap; set => lmap = value; }
		public Player Player { get => player; set => player = value; }

		public Map(int maxL, int mapX, int mapY, int mapS, GameObject[] map, Player player)
		{
			MaxL = maxL;
			this.mapX = mapX;
			this.mapY = mapY;
			this.mapS = mapS;
			this.lmap = map;

			this.player = player;
		}

		/// <summary>
		/// Default map betöltése
		/// </summary>
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

			maxL = 16;

			player = new Player(5, 5, mapS);
		}
	}
}
