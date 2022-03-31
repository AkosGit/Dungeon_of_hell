﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Raycasting_Engine
{
	public enum Side
	{
		vertical, horizontal
	}
	public class RenderObject
	{
		double flatX;
		double flatY;

		Side side;

		Point screenP1;
		Point screenP2;
		Point screenP3;
		Point screenP4;


		public double FlatX { get => flatX; set => flatX = value; }
		public double FlatY { get => flatY; set => flatY = value; }
		public Side Side { get => side; set => side = value; }
		public Point ScreenP1 { get => screenP1; set => screenP1 = value; }
		public Point ScreenP2 { get => screenP2; set => screenP2 = value; }
		public Point ScreenP3 { get => screenP3; set => screenP3 = value; }
		public Point ScreenP4 { get => screenP4; set => screenP4 = value; }

		public double Height { get { return screenP2.Y - screenP3.Y; } }

		public RenderObject(double flatX, double flatY, Side side, Point p1, Point p2, Point p3, Point p4)
		{
			this.FlatX = flatX;
			this.FlatY = flatY;
			this.Side = side;
			this.ScreenP1 = p1;
			this.ScreenP2 = p2;
			this.ScreenP3 = p3;
			this.ScreenP4 = p4;
		}

	}
}
