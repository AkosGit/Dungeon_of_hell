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
		bool isWin;
		public SPMain(Canvas canvas, Canvas hud, int Inventoryslots, Item defitem, string mapname,Player p=null, List<EntityObject> entities = null)
			: base(canvas, hud, Inventoryslots, defitem, mapname,p,entities)
		{
			maps = new string[] { "map1", "map2", "map3" };
			mapcount = 0;
			LoadNextMapEvent += LoadNextMap;
			isWin = false;
		}

		public bool IsWin { get => isWin; set => isWin = value; }

		public void ChangeMap(Map map)
		{
			LoadMapToInGameMap(map);
		}

		public void LoadNextMap()
		{
			mapcount++;
			if (mapcount == 3)
			{
				isWin = true;
				return;
			}
			ChangeMap(MapManager.GetMap(maps[mapcount]));
			
		}
	}
}
