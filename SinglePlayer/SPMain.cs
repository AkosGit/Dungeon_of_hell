using HUD;
using Raycasting_Engine;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SinglePlayer
{
	public class SPMain : Game
	{
		string[] maps;
		int mapcount;
		public SPMain(Canvas canvas, Canvas hud, int Inventoryslots, Item defitem, Map map = null)
			: base(canvas, hud, Inventoryslots, defitem, map)
		{
			maps = new string[] { "map1", "map2", "map3" };
			mapcount = 0;
			LoadNextMapEvent += LoadNextMap;
		}

		public void ChangeMap(Map map)
		{
			LoadMapToInGameMap(map);
		}

		public void LoadNextMap()
		{
			ChangeMap(MapManager.GetMap(maps[mapcount]));
			mapcount++;
		}
	}
}
