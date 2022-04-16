using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Raycasting_Engine
{
	public enum Side
	{
		vertical, horizontal
	}
	public class RenderObject : IComparable
	{
		double flatX;
		double flatY;

		Side side;

		Point screenP1;
		Point screenP2;
		Point screenP3;
		Point screenP4;

		Brush brush;

		protected double height;

		public double FlatX { get => flatX; set => flatX = value; }
		public double FlatY { get => flatY; set => flatY = value; }
		public Side Side { get => side; set => side = value; }
		public Point ScreenP1 { get => screenP1; set => screenP1 = value; }
		public Point ScreenP2 { get => screenP2; set => screenP2 = value; }
		public Point ScreenP3 { get => screenP3; set => screenP3 = value; }
		public Point ScreenP4 { get => screenP4; set => screenP4 = value; }

		public double Height { get => height; }

		public Brush Brush { get => brush; set => brush = value; }

		public RenderObject(double flatX, double flatY, Side side, Point p1, Point p2, Point p3, Point p4, Brush brush)
		{
			this.FlatX = flatX;
			this.FlatY = flatY;
			this.Side = side;
			this.ScreenP1 = p1;
			this.ScreenP2 = p2;
			this.ScreenP3 = p3;
			this.ScreenP4 = p4;
			this.brush = brush;

			height = screenP3.Y - screenP2.Y;
		}

		public virtual int CompareTo(object obj)
		{
			return (obj as RenderObject).Height.CompareTo(this.Height);
		}
	}
}
