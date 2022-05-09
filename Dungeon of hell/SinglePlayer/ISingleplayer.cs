using HUD;
using System.Collections.Generic;
using Utils.Interfaces;

namespace Utils
{
	public interface ISingleplayer
	{
		public IPlayer Player { get; set; }
		public string Mapname { get; set; }
		public List<Item> InventoryItems { get; set; }
	}
}