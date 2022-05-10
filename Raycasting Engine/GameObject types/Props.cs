using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
