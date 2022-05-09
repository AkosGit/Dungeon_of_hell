using HUD;
using Raycasting_Engine.GameObject_types;
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
	public class RenderGame
    {
		Canvas canvas;
		UI HUD;
		Action<bool> Isready;
		public RenderGame(Canvas canvas, UI HUD, Dictionary<GameObject, List<RenderObject>> renderlist, Action<bool> Isready)
        {
			this.canvas = canvas;
			this.HUD = HUD;
			this.Isready = Isready;
			DoRender(renderlist);
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
		async void DoRender(Dictionary<GameObject, List<RenderObject>> renderingList)
		{
			Color shadow = Color.FromArgb(50, 0, 0, 0);
			List<Task<System.Drawing.Bitmap>> tasks = new List<Task<System.Drawing.Bitmap>>();
			List<PointCollection> points = new List<PointCollection>();
			List<Brush> shadows = new List<Brush>();
			List<GameObject> objs = new List<GameObject>();
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
						RenderResult r = RenderSide(item.Key, SideA, Side.horizontal, texture);
						tasks.Add(r.task);
						points.Add(r.p);
						shadows.Add(Brushes.Transparent);
						objs.Add(item.Key);
					}
					else if (item.Key is EntityObject)
					{
						RenderResult r = RenderSide(item.Key, SideA, Side.horizontal, ((EntityObject)item.Key).textures[((EntityObject)item.Key).actualTexture]);
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
						RenderResult r = (RenderSide(item.Key, SideB, Side.vertical, texture));
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
			Isready?.Invoke(true);
		}
		System.Drawing.Bitmap MakeImage(string texture, double percentVisible, PointCollection myPointCollection, List<RenderObject> render, bool IsWall)
		{
			Bitmap s = new Bitmap(texture);
            if (!IsWall)
            {
				return s;
            }
			int with = (int)(s.Width * percentVisible);
			System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle();
			cropRect.Width = with;
			cropRect.Height = s.Height;
			Bitmap bit = new Bitmap(with, s.Height);
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
			//a new bitmap is required to flip the image back
			Bitmap bit2 = new Bitmap(bit.Width, bit.Height);
			using (Graphics gdi = Graphics.FromImage(bit2))
			{
				if (render.First().ScreenP1.X == 0 && percentVisible < 0.8)
				{
					float centerX = bit.Width / 2F;
					float centerY = bit.Height / 2F;
					gdi.TranslateTransform(centerX, centerY);
					gdi.RotateTransform(180.0F);
					gdi.TranslateTransform(-centerX, -centerY);
					gdi.DrawImage(bit, 0, 0);
				}
				else
				{
					gdi.DrawImage(bit, 0, 0);
				}

			}
			bit.Dispose();
			//Transform by 4 corners
			Rendering.FreeTransform transform = new Rendering.FreeTransform();
			transform.Bitmap = bit2;
			bit2.Dispose();
			transform.FourCorners = RUtils.PointsToPointF(myPointCollection);
			return transform.Bitmap;
		}
		RenderResult RenderSide(GameObject obj,List<RenderObject> render, Side side, string texture)
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

			PointCollection myPointCollection = new PointCollection();
			myPointCollection.Add(Point1);
			myPointCollection.Add(Point2);
			myPointCollection.Add(Point3);
			myPointCollection.Add(Point4);
			Task<System.Drawing.Bitmap> task = Task.Run(() => { return MakeImage((string)RUtils.DeepCopy(texture), percentVisible, (PointCollection)RUtils.DeepCopy(myPointCollection), render.Select(x => (RenderObject)x.Clone()).ToList(),(obj is MapObject)); });
			return new RenderResult() { p = myPointCollection, task = task };
		}
    }
}
