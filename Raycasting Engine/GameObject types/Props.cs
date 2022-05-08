using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycasting_Engine
{
	public enum PropType
	{
		heal, ammo, key, prop
	}
	class Props : EntityObject
	{
		PropType type;

		public PropType Type { get => type; }
		public Props(int gridX, int gridY, int mapS, string name, double he = 0, double wi = 0, bool isSolid = false, PropType type = PropType.prop, bool visible = true) 
			: base(gridX, gridY, mapS,name, he, wi, isSolid)
		{
			this.visible = visible;
			this.type = type;
		}
	}
}
