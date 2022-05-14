using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Raycasting_Engine
{
	public enum PropType
	{
		heal, ammo, key, kredit, prop
	}
	public class Props : EntityObject
	{
		PropType type;
		int credit;

		public int Credit { get => credit; }

		public PropType Type { get => type; }
		public Props(int gridX, int gridY, int mapS, string name, double he = 0, double wi = 0, PropType type = PropType.prop, int credit = 0, bool isSolid = false, bool visible = true)
			: base(gridX, gridY, mapS, name, he, wi, isSolid)
		{
			this.visible = visible;
			this.type = type;
			this.credit = credit;
		}

		public static Props MakeMedkit(int x, int y, int maps = 64)
		{
			Props medkit = new Props(x, y, maps, $"Medkit({x};{y})", 150, 230, PropType.heal);
			medkit.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\medkit_1.png");
			return medkit;
		}

		public static Props MakeAmmoBox(int x, int y, int maps = 64)
		{

			Props ammoBox = new Props(x, y, maps, $"AmmoBox({x};{y})", 150, 230, PropType.ammo);
			ammoBox.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\AmmoBox.png");
			return ammoBox;
		}

		public static Props MakeKredit(int x, int y, int maps = 64)
		{

			Props kredit = new Props(x, y, maps, $"Kredit({x};{y})", 200, 200, PropType.kredit, 2);
			kredit.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Coin.png");
			return kredit;
		}

		public static Props MakeProp(int x, int y, string textureName, int h = 120, int w = 120, int maps = 64)
		{

			Props prop = new Props(x, y, maps, $"{textureName}({x};{y})", 120, 120, PropType.prop);
			prop.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\{textureName}.png");
			return prop;
		}
	}

}

