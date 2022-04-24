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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Rendering
{
	public class RUtils
	{
		[Obsolete]
		public static Drawing DrawMyText(string textString)
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
				Geometry textGeometry = formattedText.BuildGeometry(new System.Windows.Point(15, 0));

				// Draw a rounded rectangle under the text that is slightly larger than the text.
				drawingContext.DrawRoundedRectangle(System.Windows.Media.Brushes.Transparent, null, new Rect(new System.Windows.Size(formattedText.Width + 50, formattedText.Height + 5)), 5.0, 5.0);

				// Draw the outline based on the properties that are set.
				drawingContext.DrawGeometry(System.Windows.Media.Brushes.Gold, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Maroon, 1.5), textGeometry);

				// Return the updated DrawingGroup content to be used by the control.
				return drawingGroup;
			}
		}
		[DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject([In] IntPtr hObject);

		public static ImageSource ImageSourceFromBitmap(Bitmap bmp)
		{
			var handle = bmp.GetHbitmap();
			try
			{
				return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}
			finally { DeleteObject(handle); }
		}
		public static PointF[] PointsToPointF(PointCollection pc)
		{
			PointF[] points = new PointF[pc.Count];
			for (int i = 0; i < pc.Count; i++)
			{
				points[i] = new PointF((float)pc[i].X, (float)pc[i].Y);
			}
			return points;
		}
		public static T Clone<T>(T source)
		{
			if (!typeof(T).IsSerializable)
			{
				throw new ArgumentException("The type must be serializable.", "source");
			}

			// Don't serialize a null object, simply return the default for that object
			if (Object.ReferenceEquals(source, null))
			{
				return default(T);
			}

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new MemoryStream();
			using (stream)
			{
				formatter.Serialize(stream, source);
				stream.Seek(0, SeekOrigin.Begin);
				return (T)formatter.Deserialize(stream);
			}
		}
	}
}
