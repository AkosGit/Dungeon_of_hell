using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Raycasting_Engine
{
	public class MapObject : GameObject
	{
		string name;
		public string Name { get => name; set => name = value; }

		bool canOpen;
		public bool CanOpen { get => canOpen; set => canOpen = value; }

		Brush textureA;
		public string image { get; set; }
		public Brush TextureA { get => textureA; set => textureA = value; }

		public MapObject(int gridX, int gridY, Color A, bool isSolid = false, bool canOpen = false)
			: base(isSolid)
		{
			textureA = new SolidColorBrush(A);
			this.canOpen = canOpen;
		}
		public MapObject(bool isSolid):base(isSolid)
        {

        }
		public MapObject()
        {

        }

		public void Open()
		{
			this.IsSolid = false;
		}
		public void Close()
		{
			this.IsSolid = true;
		}
	}
}
