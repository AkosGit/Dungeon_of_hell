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
		string mapname;
		MapObject[] lmap;
		EntityObject[] lEntityMap;
		Player player;
		public string MapName { get => mapname; set => mapname = value; }
		public int MaxL { get => maxL; set => maxL = value; }
		public int MapX { get => mapX; set => mapX = value; }
		public int MapY { get => mapY; set => mapY = value; }
		public int MapS { get => mapS; set => mapS = value; }
		public MapObject[] map { get => lmap; set => lmap = value; }
		public EntityObject[] EntityMap { get => lEntityMap; set => lEntityMap = value; }
		public Player Player { get => player; set => player = value; }

		public Map(int maxL, int mapX, int mapY, int mapS, MapObject[] map,EntityObject[] entitymap, Player player,string mapname)
		{
			this.mapname = MapName;
			MaxL = maxL;
			this.mapX = mapX;
			this.mapY = mapY;
			this.mapS = mapS;
			this.lmap = map;
			this.player = player;
			this.lEntityMap = entitymap;
		}
		public Map()
        {

        }
	}
}
