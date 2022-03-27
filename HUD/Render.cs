using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace HUD
{
    public static class Render
    {

		public static  void DrawRectangle(Canvas c,double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, Brush contents, Brush outline, double thickness = 0)
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

		[Obsolete]
		public  static Drawing DrawMyText(string textString)
		{
			// Create a new DrawingGroup of the control.
			DrawingGroup drawingGroup = new DrawingGroup();

			// Open the DrawingGroup in order to access the DrawingContext.
			using (DrawingContext drawingContext = drawingGroup.Open())
			{
				// Create the formatted text based on the properties set.
				FormattedText formattedText = new FormattedText(
					textString,
					CultureInfo.GetCultureInfo("en-us"),
					FlowDirection.LeftToRight,
					new Typeface("Comic Sans MS Bold"),
					48,
					System.Windows.Media.Brushes.Black // This brush does not matter since we use the geometry of the text.
					);

				// Build the geometry object that represents the text.
				Geometry textGeometry = formattedText.BuildGeometry(new System.Windows.Point(20, 0));

				// Draw a rounded rectangle under the text that is slightly larger than the text.
				drawingContext.DrawRoundedRectangle(System.Windows.Media.Brushes.Transparent, null, new Rect(new System.Windows.Size(formattedText.Width + 50, formattedText.Height + 5)), 5.0, 5.0);

				// Draw the outline based on the properties that are set.
				drawingContext.DrawGeometry(System.Windows.Media.Brushes.Gold, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Maroon, 1.5), textGeometry);

				// Return the updated DrawingGroup content to be used by the control.
				return drawingGroup;
			}
		}
	}
}
