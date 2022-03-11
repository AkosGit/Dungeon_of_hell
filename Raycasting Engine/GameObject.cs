using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Raycasting_Engine
{
	public class GameObject
	{
		bool isSolid;
		public bool IsSolid { get => isSolid; set => isSolid = value; }

		public GameObject(bool isSolid)
		{
			this.isSolid = isSolid;
		}
		public GameObject()
        {

        }
	}
}
