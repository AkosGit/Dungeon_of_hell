using HUD;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using Utils;
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
using Rendering;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

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
		protected Player player;
		public MapObject[] map;
		public int mapX;
		public int mapY;
		public int mapS;
		protected int MaxL;
		System.Windows.Media.Color shadow;
		public MapManager MapManager;
		public UI HUD;
		public Player Player { get => player; set => player = value; }
		public List<EntityObject> entities;

		public Game(Canvas canvas, Canvas hud, int Inventoryslots, Item defitem, Map mainmap = null)
		{

			MapManager = new MapManager();
			HUD = new UI(hud, Inventoryslots, defitem);
			shadow = Color.FromArgb(50, 0, 0, 0);
			this.canvas = canvas;
			if (mainmap == null) mainmap = MapManager.GetMap("Main");

			LoadMapToInGameMap(mainmap);

		}
		//render Item in hand
		public void RenderItem()
		{
			Brush Selected = HUD.Inventory.SelectedItem.Holding;
			if(HUD.Inventory.SelectedItem is FireArm)
            {
				if (((FireArm)HUD.Inventory.SelectedItem).IsShooting)
				{
					Selected = HUD.Inventory.SelectedItem.InUse;
					((FireArm)HUD.Inventory.SelectedItem).IsShooting = false;
				}
			}
			double pos = canvas.Width / 7 * 6;
			double itemh = 30;
			double itemw = 50;
			RGeometry.DrawRectangle(canvas,pos, canvas.ActualHeight, pos, canvas.Height - itemh, pos + itemw, canvas.Height - itemh, pos + itemw, canvas.Height, Selected, Brushes.Transparent);

		}
		protected void LoadMapToInGameMap(Map map)
		{
			this.map = map.map;
			MaxL = map.MaxL;
			mapX = map.MapX;
			mapY = map.MapY;
			mapS = map.MapS;

			this.player = map.Player;

			entities = new List<EntityObject>();
			EntityObject test = new EntityObject(2, 2, 100,"Józsi", 40, mapS);
			test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\entity.png");
			EntityObject test2 = new EntityObject(2, 2, 400, "nem józsi", 50, mapS);
			test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\entity.png");
			entities.Add(test2);
		}
		void PlaySounds(EntityObject obj)
		{
			Random r = new Random();
			const double MAXDISTFROMPLAYER = 600;
			void UpdateDistanceFromPlayer(string name)
			{
				double dist = Distance(Player.X, Player.Y, obj.X, obj.Y, Player.A);
				if (dist <= MAXDISTFROMPLAYER)
				{
					Audio_player.UpdateDistance(name, (float)(10 * (dist / MAXDISTFROMPLAYER)));

				}
			}
			void PlayandUpdate(EntityObject obj, Audio_player.EnitySound key, bool playSound)
			{
				//check if one of the sound is playing and update distance based on player pos
				bool isPlaying = false;
				foreach (string name in obj.Sounds[key])
				{
					if (Audio_player.IsPlaying(name))
					{
						isPlaying = true;
						UpdateDistanceFromPlayer(name);
					}
				}
				if (!isPlaying && playSound)
				{
					//play random sound if non is playing
					string soundname = obj.Sounds[key][r.Next(0, obj.Sounds[key].Count)];
					Audio_player.Play(soundname);
					UpdateDistanceFromPlayer(soundname);
				}
			}
			if (obj is MovableEntityObject)
			{
				foreach (Audio_player.EnitySound key in obj.Sounds.Keys)
				{
					bool playSound = false;
					if (key == Audio_player.EnitySound.walking)
					{
						if (((MovableEntityObject)obj).IsMoving)
						{
							//stop movement to avoid multiple sounds playing
							playSound = true;
							((MovableEntityObject)obj).IsMoving = false;
						}
						PlayandUpdate(obj, key, playSound);
					}
					if (key == Audio_player.EnitySound.hurting)
					{
						if (((MovableEntityObject)obj).IsHurting)
						{
							playSound = true;
							((MovableEntityObject)obj).IsHurting = false;
						}
						PlayandUpdate(obj, key, playSound);
					}
					if (key == Audio_player.EnitySound.speaking)
					{
						if (((MovableEntityObject)obj).IsSpeaking)
						{
							playSound = true;
							((MovableEntityObject)obj).IsSpeaking = false;
						}
						PlayandUpdate(obj, key, playSound);
					}
				}
			}
		}
		public void DrawTurn()
		{
			canvas.Children.Clear();
			//drawMap2D();
			//DrawPayer();
			//Canvas.Width = 722;
			//Canvas.Height = 500;
			RGeometry.DrawRectangle(canvas,0, 250, 722, 250, 722, 500, 0, 500, Brushes.Aqua, Brushes.Transparent);
			drawRays3D();
			RenderItem();
			PlaySounds(Player);
			foreach (EntityObject ent in entities)
			{
				PlaySounds(ent);
			}
		}

		#region 2D
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
					RGeometry.DrawRectangle(canvas,xo + 1, yo + 1, xo + 1, yo + mapS - 1, xo + mapS - 1, yo + mapS - 1, xo + mapS - 1, yo + 1, color, new SolidColorBrush(Colors.Transparent), 0);
				}
			}
		}
		public void DrawPayer()
		{
			Rectangle rect = new Rectangle
			{
				Stroke = Brushes.Blue,
				StrokeThickness = 2,
				Fill = Brushes.Blue,
				Height = 10,
				Width = 10
			};

			Canvas.SetLeft(rect, player.Pxy.X - 5);
			Canvas.SetTop(rect, player.Pxy.Y - 5);
			canvas.Children.Add(rect);

			Point p1 = new Point(player.Pxy.X, player.Pxy.Y);
			Point p2 = new Point(player.Pxy.X + player.Dx * 5, player.Pxy.Y + player.Dy * 5);
			Line l = new Line();
			l.Stroke = new SolidColorBrush(Colors.Blue);
			l.StrokeThickness = 2.0;
			l.X1 = p1.X;
			l.X2 = p2.X;
			l.Y1 = p1.Y;
			l.Y2 = p2.Y;
			canvas.Children.Add(l);
		}
		#endregion

		#region 3D
		void drawRays3D()
		{
			int r, mx, my, mp, dof, mpH, mpV; double rx, ry, ra, xo, yo, disT;
			bool typeH, typeV;
			yo = 0;
			xo = 0;
			rx = player.X;
			ry = player.Y;
			mp = 0;
			mpH = 0;
			mpV = 0;
			disT = 0;
			typeH = false;
			typeV = false;
			int me;
			Dictionary<GameObject, List<RenderObject>> renderingList = new Dictionary<GameObject, List<RenderObject>>();
			List<EntityObject> visibleEntities = new List<EntityObject>();

			ra = player.A - DR * 40; if (ra < 0) { ra += 2 * PI; }
			if (ra > 2 * PI) { ra -= 2 * PI; }
			for (r = 0; r < 80; r++)
			{
				List<EntityObject> tmpEntities = new List<EntityObject>(); //Used for tempolary saving in ray enemies.
				List<double> tmpEnitiesDistances = new List<double>();
				GameObject toBeRendered = null;
				//Check Horizontals
				dof = 0;
				double disH = 1000000000;
				double hx = player.X;
				double hy = player.Y;

				double aTan = -1 / Math.Tan(ra);
				if (ra > PI) { ry = (((int)player.Y >> 6) << 6) - 0.0001; rx = (player.Y - ry) * aTan + player.X; yo = -64; xo = -yo * aTan; } //looking up
				if (ra < PI) { ry = (((int)player.Y >> 6) << 6) + 64; rx = (player.Y - ry) * aTan + player.X; yo = 64; xo = -yo * aTan; } //looking down
				if (ra == 0 || ra == PI) { rx = player.X; ry = player.Y; dof = MaxL; }
				while (dof < MaxL)
				{
					mx = (int)(rx) >> 6; my = (int)(ry) >> 6; mp = my * mapX + mx;
					if (mp > 0 && mp < mapX * mapY && entities.Where(x => x.IsHere(mx, my)).Count() > 0)
					{
						foreach (EntityObject entity in entities.Where(x => x.IsHere(mx, my))) 
						{
							if(!visibleEntities.Contains(entity)) tmpEntities.Add(entity); tmpEnitiesDistances.Add(Distance(player.X, player.Y, entity.X, entity.Y, ra));
						}
					}
					if (mp > 0 && mp < mapX * mapY && map[mp].IsSolid)
					{
						if (map[mp].IsSolid) { hx = rx; hy = ry; disH = Distance(player.X, player.Y, hx, hy, ra); typeH = map[mp].IsSolid; mpH = mp; dof = MaxL; }
						else { me = mp; }
					}
					else { rx += xo; ry += yo; dof += 1; }
				}
				//DrawLineFromPlayer(rx, ry, Colors.Green, 6); //on 2D map

				//Check Verticals
				dof = 0;
				double disV = 1000000000;
				double vx = player.X;
				double vy = player.Y;

				double nTan = -Math.Tan(ra);
				if (ra > P2 && ra < P3) { rx = (((int)player.X >> 6) << 6) - 0.0001; ry = (player.X - rx) * nTan + player.Y; xo = -64; yo = -xo * nTan; } //looking left
				if (ra < P2 || ra > P3) { rx = (((int)player.X >> 6) << 6) + 64; ry = (player.X - rx) * nTan + player.Y; xo = 64; yo = -xo * nTan; } //looking right
				if (ra == 0 || ra == PI) { rx = player.X; ry = player.Y; dof = MaxL; }
				while (dof < MaxL)
				{
					mx = (int)(rx) >> 6; my = (int)(ry) >> 6; mp = my * mapX + mx;
					if (mp > 0 && mp < mapX * mapY && map[mp].IsSolid) { vx = rx; vy = ry; disV = Distance(player.X, player.Y, vx, vy, ra); typeV = map[mp].IsSolid; mpV = mp; dof = MaxL; }
					else { rx += xo; ry += yo; dof += 1; }
				}
				Color color = Colors.Transparent;
				Brush brush = Brushes.Transparent;
				Brush addedShadow = Brushes.Transparent;
				if (disV < disH) { rx = vx; ry = vy; disT = disV; color = Colors.Blue; brush = map[mpV].TextureA; addedShadow = new SolidColorBrush(shadow); toBeRendered = map[mpV]; }
				if (disV > disH) { rx = hx; ry = hy; disT = disH; color = Colors.CornflowerBlue; brush = map[mpH].TextureA; toBeRendered = map[mpH]; }
				//DrawLineFromPlayer(rx, ry, color, 2); //on 2D map
				ra += DR; if (ra < 0) { ra += 2 * PI; }
				if (ra > 2 * PI) { ra -= 2 * PI; }

				//---Draw 3D Walls---
				double ca = player.A - ra; if (ca < 0) { ca += 2 * PI; }
				if (ca > 2 * PI) { ca -= 2 * PI; }
				disT = disT * Math.Cos(ca);
				double lineH = mapS * 500 / disT;
				double lineO = 250 - lineH / 2;
				//DrawLine(r * 8 + MoveRight, lineO, r * 8 + MoveRight, lineH + lineO, color, 8);
				//DrawRectangle(r * 9 + MoveRight - 5, lineO, r * 9 + MoveRight + 5, lineO, r * 9 + MoveRight + 5, lineH + lineO, r * 9 + MoveRight - 5, lineH + lineO, brush, addedShadow, 0);
				foreach (EntityObject entity in tmpEntities)
				{
					visibleEntities.Add(entity);
					double disE = Distance(player.X, player.Y, entity.X, entity.Y, ca);
					disE = disE * Math.Cos(ca);
					double entityH = mapS * 500 / disE; if (entityH > 500) { entityH = 500; }
					double entityO = 250 - entityH / 2;
					renderingList.Add(entity, new List<RenderObject>());
					renderingList[entity].Add(new RenderEntity(entity.X, entity.Y, Side.horizontal, new Point(r * 9 + MoveRight - (entity.Width/2), lineH + lineO - entity.Height), new Point(r * 9 + MoveRight + (entity.Width / 2), lineH + lineO - entity.Height), new Point(r * 9 + MoveRight + (entity.Width / 2), lineH + lineO), new Point(r * 9 + MoveRight - (entity.Width / 2), lineH + lineO), Brushes.Green, entityH));
					
				}
				//RGeometry.DrawRectangle(canvas,r * 9 + MoveRight - 5, lineO, r * 9 + MoveRight + 5, lineO, r * 9 + MoveRight + 5, lineH + lineO, r * 9 + MoveRight - 5, lineH + lineO, brush, addedShadow, 0);
				
				Side side;
				if (addedShadow != Brushes.Transparent) side = Side.vertical;
				else side = Side.horizontal;
				if (!renderingList.Keys.Contains(toBeRendered))
				{
					renderingList.Add(toBeRendered, new List<RenderObject>());
				}
				renderingList[toBeRendered].Add(new RenderObject(rx, ry, side, new Point(r * 9 + MoveRight - 5, lineO), new Point(r * 9 + MoveRight + 5, lineO), new Point(r * 9 + MoveRight + 5, lineH + lineO), new Point(r * 9 + MoveRight - 5, lineH + lineO), brush));

			}
			//sorting items in order of height: back to fron rendering of objects
			renderingList = renderingList.OrderByDescending(x => x.Value.Min(z => z.Height)).ToDictionary(z => z.Key, y => y.Value);
			foreach (var item in renderingList)
			{
				//Seperate each visible side of obj
				List<RenderObject> SideA = item.Value.Where(y => y.Side == Side.horizontal).ToList();
				List<RenderObject> SideB = item.Value.Where(y => y.Side == Side.vertical).ToList();
				if (SideA.Count != 0)
				{
					if (item.Key is MapObject)
					{
						List<string> textures = new List<string>();
						textures.Add(((MapObject)item.Key).image);
						RenderSide(SideA, Side.horizontal,textures);
					}
					else if(item.Key is EntityObject)
					{
						RenderSide(SideA, Side.horizontal, ((EntityObject)item.Key).textures);

					}
				}
				if (SideB.Count != 0)
				{
					if (item.Key is MapObject)
					{
						List<string> textures = new List<string>();
						textures.Add(((MapObject)item.Key).image);
						RenderSide(SideB, Side.vertical, textures);
					}
				}
			}
		}
		void RenderSideParalel(List<RenderObject> render, Side side, List<string> textures)
		{
			Bitmap s = new Bitmap(textures[0]);
			var uiContext = SynchronizationContext.Current;
			Task.Run(() => { renderTask(render.ConvertAll(x => (RenderObject)x.Clone()), side,new Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\test.jpg"),uiContext); });

			void renderTask(List<RenderObject> render, Side side,Bitmap original, SynchronizationContext uiContext)
			{
				const bool ENABLETEXURING = false;
				Rendering.FreeTransform transform = new Rendering.FreeTransform();
				double percentVisible;
				Brush sideShadow = Brushes.Transparent;
				Brush imgbrush = Brushes.AliceBlue;
				Point Point1 = new Point(render.First().ScreenP1.X, render.First().ScreenP1.Y);
				Point Point2 = new Point(render.Last().ScreenP2.X, render.Last().ScreenP2.Y);
				Point Point3 = new Point(render.Last().ScreenP3.X, render.Last().ScreenP3.Y);
				Point Point4 = new Point(render.First().ScreenP4.X, render.First().ScreenP4.Y);

				PointCollection myPointCollection = new PointCollection();
				myPointCollection.Add(Point1);
				myPointCollection.Add(Point2);
				myPointCollection.Add(Point3);
				myPointCollection.Add(Point4);
				if (ENABLETEXURING)
                {
					if (render.Count == 1)
					{
						if (render[0] is RenderEntity) { percentVisible = 1; }
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
					int with = (int)(original.Width * percentVisible);
					System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle();
					cropRect.Width = with;
					cropRect.Height = original.Height;
					Bitmap bit = new Bitmap(with, original.Height);
					using (Graphics gdi = Graphics.FromImage(bit))
					{
						//if left side is not visible so texturing dosent start from the left side
						if (render.First().ScreenP1.X < 1 && percentVisible < 0.8)
						{
							//rotate from center to cut from right end
							float centerX = original.Width / 2F;
							float centerY = original.Height / 2F;
							gdi.TranslateTransform(centerX, centerY);
							gdi.RotateTransform(180.0F);
							gdi.TranslateTransform(-centerX, -centerY);
							//cropping to rect
							gdi.DrawImage(original, -cropRect.X, -cropRect.Y);
						}
						else
						{
							gdi.DrawImage(original, -cropRect.X, -cropRect.Y);
						}
					}
					//a new bitmap is required to flip the image back
					Bitmap bit2 = new Bitmap(bit.Width, bit.Height);
					using (Graphics gdi = Graphics.FromImage(bit2))
					{
						if (render.First().ScreenP1.X < 1 && percentVisible < 0.8)
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
					//Transform by 4 corners
					imgbrush = new ImageBrush();
					transform.Bitmap = bit2;
					transform.FourCorners = RUtils.PointsToPointF(myPointCollection);
					((ImageBrush)imgbrush).Stretch = Stretch.UniformToFill;
					((ImageBrush)imgbrush).ImageSource = RUtils.ImageSourceFromBitmap(transform.Bitmap);
				}
				uiContext.Post(new SendOrPostCallback((o) => {
				
					Polygon myPolygon = new Polygon();
					myPolygon.Stroke = imgbrush;
					myPolygon.Fill = imgbrush;
					myPolygon.StrokeThickness = 0;
					myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
					myPolygon.VerticalAlignment = VerticalAlignment.Center;

					Polygon myPolygon2 = new Polygon();
					myPolygon2.Stroke = sideShadow;
					myPolygon2.Fill = sideShadow;
					myPolygon2.StrokeThickness = 0;
					myPolygon2.HorizontalAlignment = HorizontalAlignment.Left;
					myPolygon2.VerticalAlignment = VerticalAlignment.Center;
					myPolygon.Points = myPointCollection;
					myPolygon2.Points = myPointCollection;

					canvas.Children.Add(myPolygon);
					canvas.Children.Add(myPolygon2);
				}), null);
				
			}
			//RGeometry.DrawRectangle(canvas,render.First().ScreenP1.X, render.First().ScreenP1.Y, render.Last().ScreenP2.X, render.Last().ScreenP2.Y, render.Last().ScreenP3.X, render.Last().ScreenP3.Y, render.First().ScreenP4.X, render.First().ScreenP4.Y, imgbrush, sideShadow);
		}
		void RenderSide(List<RenderObject> render, Side side, List<string> textures)
		{
			const bool ENABLE_TEXTURES = true;
			Bitmap s = new Bitmap(textures[0]);
			double percentVisible;
			Brush sideShadow = Brushes.Transparent;
			Brush imgbrush = Brushes.AliceBlue;
			//find visible portion
			if (render.Count == 1)
			{
				if (render[0] is RenderEntity) { percentVisible = 1; }
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

			if (ENABLE_TEXTURES)
			{
				//crop to image percent visible
				int with = (int)(s.Width * percentVisible);
				System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle();
				cropRect.Width = with;
				cropRect.Height = s.Height;
				Bitmap bit = new Bitmap(with, s.Height);
				using (Graphics gdi = Graphics.FromImage(bit))
				{
					//if left side is not visible so texturing dosent start from the left side
					if (render.First().ScreenP1.X < 1 && percentVisible < 0.8)
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
					if (render.First().ScreenP1.X < 1 && percentVisible < 0.8)
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
				//Transform by 4 corners
				Rendering.FreeTransform transform = new Rendering.FreeTransform();
				transform.Bitmap = bit2;
				transform.FourCorners = RUtils.PointsToPointF(myPointCollection);
				//apply image to brush
				imgbrush = new ImageBrush();
				((ImageBrush)imgbrush).Stretch = Stretch.UniformToFill;
				((ImageBrush)imgbrush).ImageSource = RUtils.ImageSourceFromBitmap(transform.Bitmap);
				//((ImageBrush)imgbrush).ImageSource = RUtils.ImageSourceFromBitmap(bit);
				bit.Dispose();

			}
			//draw polygon
			Polygon myPolygon = new Polygon();
			myPolygon.Stroke = imgbrush;
			myPolygon.Fill = imgbrush;
			myPolygon.StrokeThickness = 0;
			myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
			myPolygon.VerticalAlignment = VerticalAlignment.Center;

			Polygon myPolygon2 = new Polygon();
			myPolygon2.Stroke = sideShadow;
			myPolygon2.Fill = sideShadow;
			myPolygon2.StrokeThickness = 0;
			myPolygon2.HorizontalAlignment = HorizontalAlignment.Left;
			myPolygon2.VerticalAlignment = VerticalAlignment.Center;
			myPolygon.Points = myPointCollection;
			myPolygon2.Points = myPointCollection;

			canvas.Children.Add(myPolygon);
			canvas.Children.Add(myPolygon2);
			//RGeometry.DrawRectangle(canvas,render.First().ScreenP1.X, render.First().ScreenP1.Y, render.Last().ScreenP2.X, render.Last().ScreenP2.Y, render.Last().ScreenP3.X, render.Last().ScreenP3.Y, render.First().ScreenP4.X, render.First().ScreenP4.Y, imgbrush, sideShadow);
		}
		private double Distance(double ax, double ay, double bx, double by, double ang)
		{
			return Math.Sqrt(Math.Pow(bx - ax, 2) + Math.Pow(by - ay, 2));
		}
		#endregion

	}
}

