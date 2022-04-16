using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raycasting_Engine
{
	class Props : EntityObject
	{

		public Props(int gridX, int gridY, int mapS, int z, bool visible, int x = 0, int y = 0, bool isSolid = false) : base(gridX, gridY, mapS, 0, 0, isSolid)
		{
			this.x = gridX * mapS + x;
			this.y = gridY * mapS + y;
			this.z = z;

			this.visible = visible;
		}
	}
}
