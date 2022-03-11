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

		public SPMain(Canvas canvas,Map map = null)
			:base(canvas,map)
		{
		}

		public void ChangeMap(Map map)
		{
			LoadMapToInGameMap(map);
		}

		public void LoadNextMap()
		{
			TestMapLoad();

		}
		private void TestMapLoad()
		{
			ChangeMap(MapManager.GetMap("Test"));
		}
	}
}
