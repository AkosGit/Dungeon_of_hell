using System;
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

		public Player(int gridX, int gridY, Canvas canvas, int mapS, bool isSolid = false)
			: base(gridX, gridY, canvas, mapS, isSolid)
		{
			X = gridX * mapS;
			Y = gridY * mapS;
			a = 0;
			dx = Math.Cos((PI / 180) * a);
			dy = Math.Sin((PI / 180) * a);
		}

		public void DrawPayer()
		{
			DrawRectangle(10, 10, X - 5, Y - 5, Brushes.Blue);

			Point p1 = new Point(X, Y);
			Point p2 = new Point(X + dx * 5, Y + dy * 5);
			Line l = new Line();
			l.Stroke = new SolidColorBrush(Colors.Blue);
			l.StrokeThickness = 2.0;
			l.X1 = p1.X;
			l.X2 = p2.X;
			l.Y1 = p1.Y;
			l.Y2 = p2.Y;
			canvas.Children.Add(l);
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

		public void DrawRectangle(int height, int width, double x, double y, Brush brush, double a = 0, double rX = 0, double rY = 0)
		{
			Rectangle rect = new Rectangle
			{
				Stroke = brush,
				StrokeThickness = 2,
				Fill = brush,
				Height = height,
				Width = width
			};

			Canvas.SetLeft(rect, x);
			Canvas.SetTop(rect, y);
			canvas.Children.Add(rect);
		}
	}
}
