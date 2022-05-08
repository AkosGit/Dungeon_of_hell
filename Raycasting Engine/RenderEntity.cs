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
		public double originalWallHeight;

		public RenderEntity(double flatX, double flatY, Side side, Point p1, Point p2, Point p3, Point p4, Brush brush, double wallHeight) 
			:base(flatX, flatY, side, p1, p2, p3, p4, brush)
		{
			this.originalWallHeight = wallHeight;
		}

		public override int CompareTo(object obj)
		{
			if(obj is RenderEntity)
			{
				return originalWallHeight.CompareTo((obj as RenderEntity).originalWallHeight);
			}
			return originalWallHeight.CompareTo((obj as RenderObject).Height);
		}
	}
}
