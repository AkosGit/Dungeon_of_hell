using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Raycasting_Engine
{
	class RenderEntity : RenderObject
	{
		double originalWallHeight;

		public RenderEntity(double flatX, double flatY, Side side, Point p1, Point p2, Point p3, Point p4, Brush brush, double wallHeight) 
			:base(flatX, flatY, side, p1, p2, p3, p4, brush)
		{
			originalWallHeight = wallHeight;
		}

		public override int CompareTo(object obj)
		{
			if(obj is RenderEntity)
			{
				return (obj as RenderEntity).originalWallHeight.CompareTo(this.originalWallHeight);
			}
			return (obj as RenderObject).Height.CompareTo(this.originalWallHeight);
		}
	}
}
