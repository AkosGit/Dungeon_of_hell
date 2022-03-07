using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Raycasting_Engine
{
	class SolidObject : GameObject
	{
		string name;
		public string Name { get => name; set => name = value; }

		Brush textureA;
		public Brush TextureA { get => textureA; set => textureA = value; }

		public SolidObject(int gridX, int gridY, Color A, bool isSolid = false) 
			: base(isSolid)
		{
			textureA = new SolidColorBrush(A);
		}

	}
}
