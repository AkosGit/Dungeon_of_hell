using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Drawing;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;
using Point = System.Windows.Point;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace Rendering
{
    public static class RGeometry
	{
		//koordináták sorrendje: bal lent,bal fent,jobb fent,jobb lent
		public static void DrawRectangleNoShadow(Canvas c, double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, Brush contents, Brush outline, double thickness = 0)
		{
			Polygon myPolygon = new Polygon();
			myPolygon.Stroke = outline;
			myPolygon.Fill = contents;
			myPolygon.StrokeThickness = thickness;
			myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
			myPolygon.VerticalAlignment = VerticalAlignment.Center;

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

			c.Children.Add(myPolygon);
		}
		public static void DrawRectangle(Canvas canvas,int height, int width, int x, int y, System.Windows.Media.Brush brush)
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
		//koordináták sorrendje: bal lent,bal fent,jobb fent,jobb lent
		public static void DrawRectangle(Canvas canvas,double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, Brush color, Brush shadow, double thickness = 0)
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
		}
		public static void DrawLineFromPlayer(Canvas canvas,double PlayerX,double PlayerY,double x, double y, Color color, double thickness)
		{
			Point p1 = new Point(PlayerX, PlayerY);
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
		public static void DrawLine(Canvas canvas,double x1, double y1, double x2, double y2, Color color, double thickness)
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
