using Raycasting_Engine;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace SinglePlayer
{
	public class SPMain : Game
	{
		public SPMain(Canvas canvas, Map map = null)
			:base(canvas, map)
		{
		}

		public void ChangeMap(Map map)
		{
			LoadMapToInGameMap(map);
		}

		public void LoadNextMap()
		{
			GameObject Wall;
			GameObject Wall2;
			GameObject Air;

			Wall = new SolidObject(0, 0, Color.FromArgb(255, 130, 160, 255), true);
			Wall2 = new SolidObject(0, 0, Color.FromArgb(255, 226, 107, 139), true);
			Air = new GameObject(false);

			GameObject[] gamemap = new GameObject[]
			{
				Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall2,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall2,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall2,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall2,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Air, Wall,
				Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall, Wall,
			};
			Player p = new Player(2, 2, 64);
			Map map = new Map(16,16,16,64,gamemap, p);
			ChangeMap(map);
		}
	}
}
