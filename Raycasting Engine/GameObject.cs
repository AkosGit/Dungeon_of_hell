using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Raycasting_Engine
{
	public class GameObject
	{
		protected Canvas canvas;
		protected int mapS;
		int gridX;
		int gridY;
		double x;
		double y;

		int Type;

		public int GridX { get => gridX; set => gridX = value; }
		public int GridY { get => gridY; set => gridY = value; }
		public double X { get => x; set => x = value; }
		public double Y { get => y; set => y = value; }

		public GameObject(int gridX, int gridY, Canvas canvas, int Type = 0)
		{
			this.gridX = gridX;
			this.gridY = gridY;
			this.Type = Type;
			this.canvas = canvas;
		}
		public void DrawRectangle(int height, int width, double x, double y, Brush brush, double a = 0, double rX = 0, double rY = 0)
		{
			System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle
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
