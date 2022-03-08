﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Raycasting_Engine
{
	public class Player : EntityObject
	{
		const double PI = 3.1415926535;
		double dx;
		double dy;
		double a;

		public double A { get => a; set => a = value; }
		public Point Pxy { get => new Point(X, Y); }
		public double Dx { get => dx; set => dx = value; }
		public double Dy { get => dy; set => dy = value; }

		public Player(int gridX, int gridY,  int mapS, bool isSolid = false, int a = 0)
			: base(gridX, gridY, mapS, isSolid)
		{
			X = gridX * mapS;
			Y = gridY * mapS;
			this.a = a;
			dx = Math.Cos((PI / 180) * this.a);
			dy = Math.Sin((PI / 180) * this.a);
		}

		public void Move(Key k, GameObject[] map, int mapX, int mapY)
		{
			int xo = 0; if (dx < 0) { xo = -20; } else xo = 20;
			int yo = 0; if (dy < 0) { yo = -20; } else yo = 20;
			int ipx = (int)X / 64; int ipx_P_xo = (int)(X + xo) / 64; int ipx_M_xo = (int)(X - xo) / 64;
			int ipy = (int)Y / 64; int ipy_P_yo = (int)(Y + yo) / 64; int ipy_M_yo = (int)(Y - yo) / 64;
			switch (k)
			{
				case Key.W:
					if (!map[ipy * mapY + ipx_P_xo].IsSolid) X += dx;
					if (!map[ipy_P_yo * mapY + ipx].IsSolid) Y += dy;
					return;
				case Key.A:
					a -= 0.1; if (a < 0) a += 2 * PI;
					dx = Math.Cos(a) * 5;
					dy = Math.Sin(a) * 5;
					return;
				case Key.S:
					if (!map[ipy * mapY + ipx_M_xo].IsSolid) X -= dx;
					if (!map[ipy_M_yo * mapY + ipx].IsSolid) Y -= dy;
					return;
				case Key.D:
					a += 0.1; if (a > 2 * PI) a -= 2 * PI;
					dx = Math.Cos(a) * 5;
					dy = Math.Sin(a) * 5;
					return;
				default:
					return;
			}

		}
	}
}
