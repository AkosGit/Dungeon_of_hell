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
		public bool IsReady;
		public Game(Canvas canvas, Canvas hud, int Inventoryslots, Item defitem, Map mainmap = null)
		{

			MapManager = new MapManager();
			HUD = new UI(hud, Inventoryslots, defitem);
			this.canvas = canvas;
			if (mainmap == null) mainmap = MapManager.GetMap("Main");

			LoadMapToInGameMap(mainmap);

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
			EntityObject test = new EntityObject(2, 2, mapS, "Józsi", 360, 240);
			test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\entity.png");
			entities.Add(test);
			EntityObject test2 = new EntityObject(1, 1, mapS, "Béla", 360, 240);
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
			IsReady = false;
			//drawMap2D();
			//DrawPayer();
			//Canvas.Width = 722;
			//Canvas.Height = 500;
			//canvas.Children.Clear();
			//RGeometry.DrawRectangle(canvas, 0, 250, 722, 250, 722, 500, 0, 500, Brushes.Aqua, Brushes.Transparent);
			drawRays3D();
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
			Rendering.Vector startVector = new Rendering.Vector();
			Rendering.Vector endVector = new Rendering.Vector();

			List<EntityObject> tmpEntities = new List<EntityObject>();
			ra = player.A - DR * 40; if (ra < 0) { ra += 2 * PI; }
			if (ra > 2 * PI) { ra -= 2 * PI; }
			for (r = 0; r < 80; r++)
			{
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
				if (r == 0) startVector = new Rendering.Vector(new PointF((float)player.X, (float)player.Y), new PointF((float)rx, (float)ry));
				if (r == 79) endVector = new Rendering.Vector(new PointF((float)player.X, (float)player.Y), new PointF((float)rx, (float)ry));

				while (dof < MaxL)
				{
					mx = (int)(rx) >> 6; my = (int)(ry) >> 6; mp = my * mapX + mx;
					if (mp > 0 && mp < mapX * mapY && entities.Where(x => x.IsHere(mx, my)).Count() > 0)
					{
						foreach (EntityObject entity in entities.Where(x => x.IsHere(mx, my)))
						{
							if (!tmpEntities.Contains(entity)) tmpEntities.Add(entity);
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
			foreach (EntityObject entity in tmpEntities)
			{

				Rendering.Vector entityVector = new Rendering.Vector(new PointF((float)player.X, (float)player.Y), new PointF((float)entity.X, (float)entity.Y));
				double Angle = Math.Acos(entityVector.DotProduct(startVector) / (startVector.Magnitude * entityVector.Magnitude)) / DR;


				double PlaceOnScreenPercent = Angle / 80;
				double PlaceOnScreenX = canvas.ActualWidth * PlaceOnScreenPercent;

				//double ca = player.A - Angle * DR; if (ca < 0) { ca += 2 * PI; }
				//if (ca > 2 * PI) { ca -= 2 * PI; }
				double disE = Distance(player.X, player.Y, entity.X, entity.Y, 0);
				double entityH = mapS * 500 / disE; if (entityH > 500) { entityH = 500; }
				double entityO = 250 - entityH / 2;

				if (PointInTriangle(new PointF((float)entity.X, (float)entity.Y), new PointF((float)player.X, (float)player.Y), new PointF((float)(player.X + startVector.X * 20), (float)(player.Y + startVector.Y * 20)), new PointF((float)(player.X + endVector.X * 20), (float)(player.Y + endVector.Y*20))))
				{
					if (!renderingList.Keys.Contains(entity))
					{
						renderingList.Add(entity, new List<RenderObject>());
					}
					renderingList[entity].Add(new RenderEntity(entity.X, entity.Y, Side.horizontal, new Point(PlaceOnScreenX - (entity.Width / 2) / (500 / entityH), (entityH + entityO) - entity.Height / (500 / entityH)), new Point(PlaceOnScreenX + (entity.Width / 2) / (500 / entityH), (entityH + entityO) - entity.Height / (500 / entityH)), new Point(PlaceOnScreenX + (entity.Width / 2) / (500 / entityH), entityH + entityO), new Point(PlaceOnScreenX - (entity.Width / 2) / (500 / entityH), entityH + entityO), Brushes.Green, entityH));
				}

				//visibleEntities.Add(entity);
				//double disE = Distance(player.X, player.Y, entity.X, entity.Y, ca);
				//disE = disE * Math.Cos(ca);
				//double entityH = mapS * 500 / disE; if (entityH > 500) { entityH = 500; }
				//double entityO = 250 - entityH / 2;
				//renderingList.Add(entity, new List<RenderObject>());
				//renderingList[entity].Add(new RenderEntity(entity.X, entity.Y, Side.horizontal, new Point(r * 9 + MoveRight - (entity.Width / 2), lineH + lineO - entity.Height), new Point(r * 9 + MoveRight + (entity.Width / 2), lineH + lineO - entity.Height), new Point(r * 9 + MoveRight + (entity.Width / 2), lineH + lineO), new Point(r * 9 + MoveRight - (entity.Width / 2), lineH + lineO), Brushes.Green, entityH));

			}
			//sorting items in order of height: back to fron rendering of objects
			renderingList = renderingList.OrderBy(x => x.Value.Min(z => { if (z is RenderEntity) return (z as RenderEntity).originalWallHeight; else return z.Height; })).ToDictionary(z => z.Key, y => y.Value);
			RenderGame render = new RenderGame(canvas, HUD, renderingList);
			IsReady = true;
		}
		float sign(PointF p1, PointF p2, PointF p3)
		{
			return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
		}

		bool PointInTriangle(PointF pt, PointF v1, PointF v2, PointF v3)
		{
			float d1, d2, d3;
			bool has_neg, has_pos;

			d1 = sign(pt, v1, v2);
			d2 = sign(pt, v2, v3);
			d3 = sign(pt, v3, v1);

			has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
			has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

			return !(has_neg && has_pos);
		}
		private double Distance(double ax, double ay, double bx, double by, double ang)
		{
			return Math.Sqrt(Math.Pow(bx - ax, 2) + Math.Pow(by - ay, 2));
		}
        #endregion
    }
}


