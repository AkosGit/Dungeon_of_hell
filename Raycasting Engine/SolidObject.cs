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
		Brush textureA;
		public Brush TextureA { get => textureA; set => textureA = value; }




		public SolidObject(int gridX, int gridY, Color A, Canvas canvas, int Type = 0) 
			: base(gridX, gridY, canvas, Type)
		{
			textureA = new SolidColorBrush(A);
		}

	}
}
