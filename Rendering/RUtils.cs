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
using System.Reflection;
using Point = System.Windows.Point;

namespace Rendering
{
	public class RUtils
	{
		public static VisualBrush DrawText(string text, System.Windows.Media.Brush textcolor, System.Windows.Media.Brush back)
		{
			VisualBrush myVisualBrush = new VisualBrush();
			StackPanel myStackPanel = new StackPanel();
			myStackPanel.Background = back;
			TextBlock someText = new TextBlock();
			someText.Foreground = textcolor;
			FontSizeConverter myFontSizeConverter = new FontSizeConverter();
			someText.FontSize = (double)myFontSizeConverter.ConvertFrom("10pt");
			someText.Text = text;
			someText.Margin = new Thickness(2);
			myStackPanel.Children.Add(someText);
			myVisualBrush.Visual = myStackPanel;
			return myVisualBrush;
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
		public static Point CenterOfCanvas(Canvas c)
        {
			Point p = new Point();
			p.X = (int)c.ActualWidth / 2;
			p.Y = (int)c.ActualHeight / 2;
			return p;

		}
		public static object DeepCopy(object obj)
		{
			if (obj == null)
				return null;
			Type type = obj.GetType();

			if (type.IsValueType || type == typeof(string))
			{
				return obj;
			}
			else if (type.IsArray)
			{
				Type elementType = Type.GetType(
					 type.FullName.Replace("[]", string.Empty));
				var array = obj as Array;
				Array copied = Array.CreateInstance(elementType, array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					copied.SetValue(DeepCopy(array.GetValue(i)), i);
				}
				return Convert.ChangeType(copied, obj.GetType());
			}
			else if (type.IsClass)
			{

				object toret = Activator.CreateInstance(obj.GetType());
				FieldInfo[] fields = type.GetFields(BindingFlags.Public |
							BindingFlags.NonPublic | BindingFlags.Instance);
				foreach (FieldInfo field in fields)
				{
					object fieldValue = field.GetValue(obj);
					if (fieldValue == null)
						continue;
					field.SetValue(toret, DeepCopy(fieldValue));
				}
				return toret;
			}
			else
				throw new ArgumentException("Unknown type");
		}
	}
}
