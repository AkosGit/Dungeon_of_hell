using HUD;
using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Utils;

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
		Point finishZone;
		Item key;

		public string MapName { get => mapname; set => mapname = value; }
		public int MaxL { get => maxL; set => maxL = value; }
		public int MapX { get => mapX; set => mapX = value; }
		public int MapY { get => mapY; set => mapY = value; }
		public int MapS { get => mapS; set => mapS = value; }
		public MapObject[] map { get => lmap; set => lmap = value; }
		public EntityObject[] EntityMap { get => lEntityMap; set => lEntityMap = value; }
		public Player Player { get => player; set => player = value; }
		public Point FinishZone { get => finishZone; }
		public Item Key { get => key; set => key = value; }

		public Map(int maxL, int mapX, int mapY, int mapS, MapObject[] map,Enemy[] entitymap, Player player,string mapname)
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
		public void SetDefaults()
		{
			finishZone = new Point(player.GridX, player.GridY);
			key = new Item(lEntityMap.FirstOrDefault(x => { if (x is Props && (x as Props).Type == PropType.key) return true; else return false; }).Name);
			key.Icon = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\keyCard.png")));
			key.Holding = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\keyCard.png")));
		}
		public Map()
        {

        }
	}
}
