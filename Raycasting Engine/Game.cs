using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Raycasting_Engine
{
	public class Game
	{
		const double PI = 3.1415926535;
		const double P2 = PI / 2;
		const double P3 = 3 * PI / 2;
		const double DR = 0.0174533;

		const int MoveRight = 5;
		protected Canvas canvas;

		Player player;
		public GameObject[] map;
		public int mapX;
		public int mapY;
		public int mapS;
		int MaxL;
		Color shadow;

		public Player Player { get => player; set => player = value; }

		public Game(Canvas canvas, Map mainmap = null)
		{
			shadow = Color.FromArgb(50, 0, 0, 0);
			this.canvas = canvas;
			if (mainmap == null) mainmap = new Map();

			MapGameMaptogame(mainmap);
			SetDefaultThing();
			
		}

		private void MapGameMaptogame(Map mainmap)
		{
			map = mainmap.map;
			MaxL = mainmap.MaxL;
			mapX = mainmap.mapX;
			mapY = mainmap.mapY;
			mapS = mainmap.mapS;
		}

		private void SetDefaultThing()
		{
			player = new Player(5, 5, canvas, mapS);
			//player.DrawPayer();
		}

		public void DrawTurn()
		{
			canvas.Children.Clear();
			//drawMap2D();
			drawRays3D();
			//player.DrawPayer
		}

		private void drawMap2D()
		{
			double xo, yo;
			for (int y = 0; y < mapY; y++)
			{
				for (int x = 0; x < mapX; x++)
				{
					Brush color;
					if (map[y * mapY + x].IsSolid) color = Brushes.White; else color = Brushes.Black;
					xo = x * mapS; yo = y * mapS;
					DrawRectangle(xo + 1, yo + 1, xo + 1, yo + mapS - 1, xo + mapS - 1, yo + mapS - 1, xo + mapS - 1, yo + 1, color, new SolidColorBrush(Colors.Transparent), 0);
				}
			}
		}

		void drawRays3D()
		{
			int r, mx, my, mp, dof, mpH, mpV; double rx, ry, ra, xo, yo, disT;
			bool typeH, typeV;
			yo = 0;
			xo = 0;
			rx = Player.X;
			ry = player.Y;
			mp = 0;
			mpH = 0;
			mpV = 0;
			disT = 0;
			typeH = false;
			typeV = false;
			int me;

			ra = player.A - DR * 40; if (ra < 0) { ra += 2 * PI; }
			if (ra > 2 * PI) { ra -= 2 * PI; }
			for (r = 0; r < 80; r++)
			{
				//---Check Horizontal Lines---
				dof = 0;
				double disH = 1000000000;
				double hx = player.X;
				double hy = player.Y;

				double aTan = -1 / Math.Tan(ra);
				if (ra > PI) { ry = (((int)player.Y >> 6) << 6) - 0.0001; rx = (player.Y - ry) * aTan + player.X; yo = -64; xo = -yo * aTan; } //looking up
				if (ra < PI) { ry = (((int)player.Y >> 6) << 6) + 64; rx = (player.Y - ry) * aTan + player.X; yo = 64; xo = -yo * aTan; } //looking down
				if (ra == 0 || ra == PI) { rx = Player.X; ry = player.Y; dof = MaxL; }
				while (dof < MaxL)
				{
					mx = (int)(rx) >> 6; my = (int)(ry) >> 6; mp = my * mapX + mx;
					if (mp > 0 && mp < mapX * mapY && map[mp].IsSolid)
					{
						if (map[mp].IsSolid) { hx = rx; hy = ry; disH = Distance(player.X, player.Y, hx, hy, ra); typeH = map[mp].IsSolid; mpH = mp; dof = MaxL; }
						else {me = mp; }
					}
					else { rx += xo; ry += yo; dof += 1; }
				}
				//DrawLineFromPlayer(rx, ry, Colors.Green, 6);

				//---Check Vertical Lines---
				dof = 0;
				double disV = 1000000000;
				double vx = player.X;
				double vy = player.Y;

				double nTan = -Math.Tan(ra);
				if (ra > P2 && ra < P3) { rx = (((int)player.X >> 6) << 6) - 0.0001; ry = (player.X - rx) * nTan + player.Y; xo = -64; yo = -xo * nTan; } //looking left
				if (ra < P2 || ra > P3) { rx = (((int)player.X >> 6) << 6) + 64; ry = (player.X - rx) * nTan + player.Y; xo = 64; yo = -xo * nTan; } //looking right
				if (ra == 0 || ra == PI) { rx = Player.X; ry = player.Y; dof = MaxL; }
				while (dof < MaxL)
				{
					mx = (int)(rx) >> 6; my = (int)(ry) >> 6; mp = my * mapX + mx;
					if (mp > 0 && mp < mapX * mapY && map[mp].IsSolid) { vx = rx; vy = ry; disV = Distance(player.X, player.Y, vx, vy, ra); typeV = map[mp].IsSolid; mpV = mp; dof = MaxL; }
					else { rx += xo; ry += yo; dof += 1; }
				}
				Color color = Colors.Transparent;
				Brush brush = Brushes.Transparent;
				Brush addedShadow = Brushes.Transparent;
				if (disV < disH) { rx = vx; ry = vy; disT = disV; color = Colors.Blue; brush = (map[mpV] as SolidObject).TextureA; addedShadow = new SolidColorBrush(shadow); }
				if (disV > disH) { rx = hx; ry = hy; disT = disH; color = Colors.CornflowerBlue; brush = (map[mpH] as SolidObject).TextureA; }
				//DrawLineFromPlayer(rx, ry, color, 2);
				ra += DR; if (ra < 0) { ra += 2 * PI; }
				if (ra > 2 * PI) { ra -= 2 * PI; }

				//---Draw 3D Walls---
				double ca = player.A - ra; if (ca < 0) { ca += 2 * PI; }
				if (ca > 2 * PI) { ca -= 2 * PI; }
				disT = disT * Math.Cos(ca);
				double lineH = (mapS * 450) / disT; if (lineH > 450) { lineH = 450; }
				double lineO = 250 - lineH / 2;
				//DrawLine(r * 8 + MoveRight, lineO, r * 8 + MoveRight, lineH + lineO, color, 8);
				DrawRectangle(r * 9 + MoveRight - 5, lineO, r * 9 + MoveRight + 5, lineO, r * 9 + MoveRight + 5, lineH + lineO, r * 9 + MoveRight - 5, lineH + lineO, brush, addedShadow, 0);


			}
		}

		private double Distance(double ax, double ay, double bx, double by, double ang)
		{
			return Math.Sqrt(Math.Pow(bx - ax, 2) + Math.Pow(by - ay, 2));
		}

		// ToDO SA: Kitenni a rajzolást külön osztályba
		public void DrawRectangle(int height, int width, int x, int y, Brush brush)
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
		public void DrawRectangle(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, Brush color, Brush shadow, double thickness = 0)
		{
			Polygon myPolygon = new Polygon();
			myPolygon.Stroke = color;
			myPolygon.Fill = color;
			myPolygon.StrokeThickness = thickness;
			myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
			myPolygon.VerticalAlignment = VerticalAlignment.Center;

			Polygon myPolygon2 = new Polygon();
			myPolygon2.Stroke = shadow;
			myPolygon2.Fill = shadow;
			myPolygon2.StrokeThickness = thickness;
			myPolygon2.HorizontalAlignment = HorizontalAlignment.Left;
			myPolygon2.VerticalAlignment = VerticalAlignment.Center;

			Point Point1 = new Point(x1, y1);
			Point Point2 = new Point(x2, y2);
			Point Point3 = new Point(x3, y3);
			Point Point4 = new Point(x4, y4);

			PointCollection myPointCollection = new PointCollection();
			myPointCollection.Add(Point1);
			myPointCollection.Add(Point2);
			myPointCollection.Add(Point3);
			myPointCollection.Add(Point4);

			myPolygon.Points = myPointCollection;
			myPolygon2.Points = myPointCollection;

			canvas.Children.Add(myPolygon);
			canvas.Children.Add(myPolygon2);
		}
		public void DrawLineFromPlayer(double x, double y, Color color, double thickness)
		{
			Point p1 = new Point(player.X, player.Y);
			Point p2 = new Point(x, y);
			Line l = new Line();
			l.Stroke = new SolidColorBrush(color);
			l.StrokeThickness = thickness;
			l.X1 = p1.X;
			l.X2 = p2.X;
			l.Y1 = p1.Y;
			l.Y2 = p2.Y;
			canvas.Children.Add(l);
		}
		public void DrawLine(double x1, double y1, double x2, double y2, Color color, double thickness)
		{
			Point p1 = new Point(x1, y1);
			Point p2 = new Point(x2, y2);
			Line l = new Line();
			l.Stroke = new SolidColorBrush(color);
			l.StrokeThickness = thickness;
			l.Fill = Brushes.Red;
			l.X1 = p1.X;
			l.X2 = p2.X;
			l.Y1 = p1.Y;
			l.Y2 = p2.Y;
			canvas.Children.Add(l);
		}
	}
}

