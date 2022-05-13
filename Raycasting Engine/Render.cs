using HUD;
using Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace Raycasting_Engine
{
	public class RenderResult
	{
		public Task<System.Drawing.Bitmap> task;
		public PointCollection p;

	}
	//overlay can be added to two queues with duration
	public class Overlay
    {
		public int Duration;
		public UIElement Element;
		//left up
		public Point Pos;
		public bool IsFront;
    }
	public class RenderGame
    {
		Canvas canvas;
		UI HUD;
		Action<bool> Isready;
		Dictionary<string, Bitmap> images;
		Overlay currentOverlayFront;
		Overlay currentOverlayBack;
		Queue<Overlay> frontOverlay;
		Queue<Overlay> backOverlay;
		const int WIDTH = 722;
		const int HEIGHT = 500;
		public RenderGame(Canvas canvas, UI HUD, Action<bool> Isready)
        {
			this.canvas = canvas;
			this.HUD = HUD;
			this.Isready = Isready;
			images = new Dictionary<string, Bitmap>();
			frontOverlay = new Queue<Overlay>();
			backOverlay = new Queue<Overlay>();
			currentOverlayBack = new Overlay { Duration = 0 };
			currentOverlayFront = new Overlay { Duration = 0 };
		}
		public void AddOverlay(Overlay overlay)
		{
            if (overlay.IsFront)
            {
				frontOverlay.Enqueue(overlay);
            }
            else
            {
				backOverlay.Enqueue(overlay);
            }
		}
		public void AddSubtitles(string text)
        {
			StackPanel stackOuter = new StackPanel();
			StackPanel stackInner = new StackPanel();
			stackOuter.Width = WIDTH;
			//stackInner.Background= new SolidColorBrush(Color.FromArgb((byte)0.47, 94, 0, 12));
			stackInner.Background = new SolidColorBrush(Colors.Blue);
			TextBlock textBlock2 = new TextBlock();
			textBlock2.Margin = new Thickness(5);
			textBlock2.Text = text;
			textBlock2.FontSize = 25;
			textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock2.VerticalAlignment = VerticalAlignment.Center;
			stackInner.VerticalAlignment = VerticalAlignment.Center;
			stackInner.HorizontalAlignment = HorizontalAlignment.Center;
			textBlock2.Foreground = new SolidColorBrush(Colors.Black);
			stackInner.Children.Add(textBlock2);
			stackOuter.Children.Add(stackInner);
			Point p = new Point();
			p.X = 0;
			p.Y = HEIGHT - 100;
			AddOverlay(new Overlay {IsFront=false, Duration=200, Element=stackOuter, Pos=p });
		}
		void displayOverlays()
        {
            if (currentOverlayBack.Duration == 0 && backOverlay.Count!=0)
            {
				currentOverlayBack = backOverlay.Dequeue();

			}
			if (currentOverlayFront.Duration == 0 && frontOverlay.Count != 0)
			{
				currentOverlayFront = frontOverlay.Dequeue();

			}
			//inprogress
			if (currentOverlayBack.Duration != 0) {
				currentOverlayBack.Duration--;
				canvas.Children.Add(currentOverlayBack.Element);
				Canvas.SetLeft(currentOverlayBack.Element, currentOverlayBack.Pos.X);
				Canvas.SetTop(currentOverlayBack.Element, currentOverlayBack.Pos.Y);
			}
			//inprogress
			if (currentOverlayFront.Duration != 0)
			{
				currentOverlayFront.Duration--;
				canvas.Children.Add(currentOverlayFront.Element);
				Canvas.SetLeft(currentOverlayFront.Element, currentOverlayFront.Pos.X);
				Canvas.SetTop(currentOverlayFront.Element, currentOverlayFront.Pos.Y);
			}
		}
		public void RenderItem()
		{
			Brush Selected = HUD.Inventory.SelectedItem.Holding;
			if (HUD.Inventory.SelectedItem is FireArm)
			{
				if (((FireArm)HUD.Inventory.SelectedItem).IsShooting)
				{
					Selected = HUD.Inventory.SelectedItem.InUse;
					((FireArm)HUD.Inventory.SelectedItem).IsShooting = false;
				}
				((FireArm)HUD.Inventory.SelectedItem).Tick();
				double pos = canvas.Width / 10 * 5 - 25;
				double itemh = 128;
				double itemw = 128;
				if (((FireArm)HUD.Inventory.SelectedItem).IsReloading)
				{
					//when reloading put part of the gun out of frame
					RGeometry.DrawRectangle(canvas, pos, canvas.ActualHeight + 30, pos, canvas.Height - itemh, pos + itemw, canvas.Height - itemh, pos + itemw, canvas.Height + 30, Selected, Brushes.Transparent);

				}
				else
				{
					RGeometry.DrawRectangle(canvas, pos, canvas.ActualHeight, pos, canvas.Height - itemh, pos + itemw, canvas.Height - itemh, pos + itemw, canvas.Height, Selected, Brushes.Transparent);

				}
			}
			else
			{
				double pos = canvas.Width / 7 * 6;
				double itemh = 30;
				double itemw = 50;
				RGeometry.DrawRectangle(canvas, pos, canvas.ActualHeight, pos, canvas.Height - itemh, pos + itemw, canvas.Height - itemh, pos + itemw, canvas.Height, Selected, Brushes.Transparent);
			}
		}
		public async void DoRender(Dictionary<GameObject, List<RenderObject>> renderingList)
		{
			Color shadow = Color.FromArgb(50, 0, 0, 0);
			List<Task<System.Drawing.Bitmap>> tasks = new List<Task<System.Drawing.Bitmap>>();
			List<PointCollection> points = new List<PointCollection>();
			List<Brush> shadows = new List<Brush>();
			List<GameObject> objs = new List<GameObject>();
			//if not presenet in images dict add it
			var mapPaths = renderingList.Where(z => (z.Key is MapObject) && !images.ContainsKey(((MapObject)z.Key).image)).Select(z => ((MapObject)z.Key).image).Distinct();
			var enitityPaths = renderingList.Where(z => (z.Key is EntityObject) && !images.ContainsKey(((EntityObject)z.Key).textures[((EntityObject)z.Key).actualTexture])).Select(z => ((EntityObject)z.Key).textures[((EntityObject)z.Key).actualTexture]).Distinct();
			foreach (var item in mapPaths)
			{
				images.Add(item, new Bitmap(item));
			}
			foreach (var item in enitityPaths)
			{
				images.Add(item, new Bitmap(item));
			}
			foreach (var item in renderingList)
			{
				//Seperate each visible side of obj
				List<RenderObject> SideA = item.Value.Where(y => y.Side == Side.horizontal).ToList();
				List<RenderObject> SideB = item.Value.Where(y => y.Side == Side.vertical).ToList();
				if (SideA.Count != 0)
				{
					if (item.Key is MapObject)
					{
						string texture = ((MapObject)item.Key).image;
						RenderResult r = RenderSide(item.Key, SideA, Side.horizontal, images[texture].Clone(new System.Drawing.Rectangle(0, 0, images[texture].Width, images[texture].Height), images[texture].PixelFormat));
						tasks.Add(r.task);
						points.Add(r.p);
						shadows.Add(Brushes.Transparent);
						objs.Add(item.Key);
					}
					else if (item.Key is EntityObject)
					{
						var texture = ((EntityObject)item.Key).textures[((EntityObject)item.Key).actualTexture];
						RenderResult r = RenderSide(item.Key, SideA, Side.horizontal, images[texture].Clone(new System.Drawing.Rectangle(0, 0, images[texture].Width, images[texture].Height), images[texture].PixelFormat));
						tasks.Add(r.task);
						points.Add(r.p);
						shadows.Add(Brushes.Transparent);
						objs.Add(item.Key);
					}
				}
				if (SideB.Count != 0)
				{
					if (item.Key is MapObject)
					{
						string texture = ((MapObject)item.Key).image;
						RenderResult r = (RenderSide(item.Key, SideB, Side.vertical, images[texture].Clone(new System.Drawing.Rectangle(0, 0, images[texture].Width, images[texture].Height), images[texture].PixelFormat)));
						tasks.Add(r.task);
						points.Add(r.p);
						shadows.Add(new SolidColorBrush(shadow));
						objs.Add(item.Key);
					}
				}
			}
			await Task.WhenAll(tasks.ToArray());
			canvas.Children.Clear();
			RGeometry.DrawRectangle(canvas, 0, 250, 722, 250, 722, 500, 0, 500, new SolidColorBrush(Color.FromArgb(255, (byte)79, (byte)65, (byte)52)), Brushes.Transparent);
			for (int i = 0; i < tasks.Count; i++)
			{
				//apply image to brush
				ImageBrush imgbrush = new ImageBrush();
				((ImageBrush)imgbrush).Stretch = Stretch.Fill;
				((ImageBrush)imgbrush).ImageSource = RUtils.ImageSourceFromBitmap(tasks[i].Result);
				//draw polygon
				Polygon myPolygon = new Polygon();
				myPolygon.Stroke = imgbrush;
				myPolygon.Fill = imgbrush;
				myPolygon.StrokeThickness = 0;
				myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
				myPolygon.VerticalAlignment = VerticalAlignment.Center;

				Polygon myPolygon2 = new Polygon();
				myPolygon2.Stroke = shadows[i];
				myPolygon2.Fill = shadows[i];
				myPolygon2.StrokeThickness = 0;
				myPolygon2.HorizontalAlignment = HorizontalAlignment.Left;
				myPolygon2.VerticalAlignment = VerticalAlignment.Center;
				myPolygon2.Points = points[i];
				myPolygon.Points = points[i];
				Point c = RUtils.CenterOfCanvas(canvas);
				GameObject obj = objs[i];
				if (HUD.Inventory.SelectedItem is FireArm && obj is Enemy) 
				{
					//if enemy has been hit
					if (points[i][0].X <= c.X && points[i][0].Y <= c.Y && points[i][2].X >= c.X && points[i][2].Y >= c.Y && ((FireArm)HUD.Inventory.SelectedItem).IsShooting)
					{
						((Enemy)obj).Health -= ((FireArm)HUD.Inventory.SelectedItem).Damage;
						((Enemy)obj).IsHurting = true;
					}
				}
				canvas.Children.Add(myPolygon);
				canvas.Children.Add(myPolygon2);

				tasks[i].Result.Dispose();				
			}
			RenderItem();
			displayOverlays();
			Isready?.Invoke(true);
			tasks.Clear();
			points.Clear();
			objs.Clear();
			images.Clear();
			shadows.Clear();
		}
		System.Drawing.Bitmap MakeImage(Bitmap s, double percentVisible, PointCollection myPointCollection, List<RenderObject> render, bool IsWall)
		{
            if (!IsWall)
            {
				return s;
            }
			int with = (int)(s.Width * percentVisible);
			System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle();
			cropRect.Width = with;
			cropRect.Height = s.Height;
			Bitmap bit = new Bitmap(with, s.Height);
			//if left side is not visible so texturing dosent start from the left side
			using (Graphics gdi = Graphics.FromImage(bit))
			{
				//if left side is not visible so texturing dosent start from the left side
				if (render.First().ScreenP1.X == 0 && percentVisible < 0.8)
				{
					//rotate from center to cut from right end
					float centerX = s.Width / 2F;
					float centerY = s.Height / 2F;
					gdi.TranslateTransform(centerX, centerY);
					gdi.RotateTransform(180.0F);
					gdi.TranslateTransform(-centerX, -centerY);
					//cropping to rect
					gdi.DrawImage(s, -cropRect.X, -cropRect.Y);
				}
				else
				{
					gdi.DrawImage(s, -cropRect.X, -cropRect.Y);
				}
			}
			//Transform by 4 corners
			if (render.First().ScreenP1.X == 0 && percentVisible < 0.8)
			{
				bit.RotateFlip(RotateFlipType.Rotate180FlipNone);
			}
			Rendering.FreeTransform transform = new Rendering.FreeTransform();
			transform.Bitmap = bit;
			bit.Dispose();
			transform.FourCorners = RUtils.PointsToPointF(myPointCollection);
			return transform.Bitmap;
		}
		RenderResult RenderSide(GameObject obj,List<RenderObject> render, Side side, Bitmap texture)
		{
			Color shadow = Color.FromArgb(50, 0, 0, 0);
			double percentVisible;
			Brush sideShadow = Brushes.Transparent;
			Brush imgbrush = Brushes.AliceBlue;
			//find visible portion
			if (render.Count == 1)
			{
				if (render[0] is RenderEntity) { 
					percentVisible = 1; 
				}
				else { percentVisible = 0.1; }
			}
			else
			{
				if (side == Side.vertical)
				{
					percentVisible = (Math.Abs((render.Last().FlatY) - (render.First().FlatY))) / 62;
					sideShadow = new SolidColorBrush(shadow);
				}
				else { percentVisible = (Math.Abs((render.Last().FlatX) - (render.First().FlatX))) / 62; }
			}
			//avoid visible percentages rounded to 0;
			if (percentVisible < 0.1) { percentVisible = 0.1; }

			Point Point1 = new Point(render.First().ScreenP1.X, render.First().ScreenP1.Y);
			Point Point2 = new Point(render.Last().ScreenP2.X, render.Last().ScreenP2.Y);
			Point Point3 = new Point(render.Last().ScreenP3.X, render.Last().ScreenP3.Y);
			Point Point4 = new Point(render.First().ScreenP4.X, render.First().ScreenP4.Y);
			//StackPanel stack = new StackPanel();
			//stack.Width = 100;
			//stack.Height = 20;
			//TextBlock textBlock2 = new TextBlock();
			//textBlock2.Text = "dfdffffffffffff";
			//textBlock2.FontSize = 25;
			//textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
			//textBlock2.VerticalAlignment = VerticalAlignment.Center;
			//textBlock2.Foreground = new SolidColorBrush(Colors.Black);
			//stack.Children.Add(textBlock2);
			//canvas.Children.Add(stack);
			//Canvas.SetLeft(stack, Point2.X);
			//Canvas.SetTop(stack, Point2.Y);
			PointCollection myPointCollection = new PointCollection();
			myPointCollection.Add(Point1);
			myPointCollection.Add(Point2);
			myPointCollection.Add(Point3);
			myPointCollection.Add(Point4);
			var RenderCopy=render.Select(x => (RenderObject)x.Clone()).ToList();
			render.Clear();
			Task<System.Drawing.Bitmap> task = Task.Run(() => { return MakeImage(texture, percentVisible, (PointCollection)RUtils.DeepCopy(myPointCollection), RenderCopy, (obj is MapObject)); });
			
			return new RenderResult() { p = myPointCollection, task = task };
		}
    }
}
