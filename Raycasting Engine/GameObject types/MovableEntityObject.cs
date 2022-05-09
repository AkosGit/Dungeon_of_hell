using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Utils;

namespace Raycasting_Engine
{
	public class MovableEntityObject : EntityObject
	{
		protected const double PI = 3.1415926535;
		protected double dx;
		protected double dy;
		protected double a;
		//for playing walking sound
		public bool IsMoving { get; set; }
		public bool IsHurting { get; set; }
		public bool IsSpeaking { get; set; }

		public double A { get => a; set => a = value; }
		[JsonIgnore]
		public Point Pxy { get => new Point(X, Y); }
		public double Dx { get => dx; set => dx = value; }
		public double Dy { get => dy; set => dy = value; }

		public MovableEntityObject(int gridX, int gridY, int mapS, string name, double he = 0, double wi = 0, bool isSolid = false, int a = 0, Dictionary<Audio_player.EnitySound, List<string>> Sounds = null)
			: base(gridX, gridY, mapS, name, he, wi, isSolid, Sounds)
		{

		}
		public bool CanMoveForward(MapObject[] map, int mapY)
		{
			int xo = 0; if (dx < 0) { xo = -20; } else xo = 20;
			int yo = 0; if (dy < 0) { yo = -20; } else yo = 20;
			int ipx = (int)X / 64; int ipx_P_xo = (int)(X + xo) / 64;
			int ipy = (int)Y / 64; int ipy_P_yo = (int)(Y + yo) / 64;
			if (!map[ipy * mapY + ipx_P_xo].IsSolid && !map[ipy_P_yo * mapY + ipx].IsSolid)
			{
				return true;
			}
			return false;
		}
		public bool CanMoveBackwards(MapObject[] map, int mapY)
		{
			int xo = 0; if (dx < 0) { xo = -20; } else xo = 20;
			int yo = 0; if (dy < 0) { yo = -20; } else yo = 20;
			int ipx = (int)X / 64; int ipx_M_xo = (int)(X - xo) / 64;
			int ipy = (int)Y / 64; int ipy_M_yo = (int)(Y - yo) / 64;
			if (!map[ipy * mapY + ipx_M_xo].IsSolid && !map[ipy_M_yo * mapY + ipx].IsSolid)
			{
				return true;
			}
			return false;
		}

		public void Move(Key k, MapObject[] map, int mapX, int mapY, EntityActions action)
		{
			int xo = 0; if (dx < 0) { xo = -20; } else xo = 20;
			int yo = 0; if (dy < 0) { yo = -20; } else yo = 20;
			int ipx = (int)X / 64; int ipx_P_xo = (int)(X + xo) / 64; int ipx_M_xo = (int)(X - xo) / 64;
			int ipy = (int)Y / 64; int ipy_P_yo = (int)(Y + yo) / 64; int ipy_M_yo = (int)(Y - yo) / 64;

			GridX = (int)X / 64;
			GridY = (int)Y / 64;
			switch (action)
			{
				case EntityActions.Forward:

					if (!map[ipy * mapY + ipx_P_xo].IsSolid) { X += dx; IsMoving = true; }
					if (!map[ipy_P_yo * mapY + ipx].IsSolid) Y += dy;
					if (map[ipy * mapY + ipx_M_xo].IsSolid && map[ipy * mapY + ipx_M_xo].CanOpen && GridX != ipx_M_xo) map[ipy * mapY + ipx_M_xo].Close();
					if (map[ipy_M_yo * mapY + ipx].IsSolid && map[ipy_M_yo * mapY + ipx].CanOpen && GridY != ipy_M_yo) map[ipy_M_yo * mapY + ipx].Close();
					return;
				case EntityActions.Left:
					a -= 0.1; if (a < 0) a += 2 * PI;
					dx = Math.Cos(a) * 5;
					dy = Math.Sin(a) * 5;
					return;
				case EntityActions.Backwards:
					if (!map[ipy * mapY + ipx_M_xo].IsSolid) { X -= dx; IsMoving = true; }
					if (!map[ipy_M_yo * mapY + ipx].IsSolid) Y -= dy;
					return;
				case EntityActions.Right:
					a += 0.1; if (a > 2 * PI) a -= 2 * PI;
					dx = Math.Cos(a) * 5;
					dy = Math.Sin(a) * 5;
					return;
				case EntityActions.Use:
					if (map[ipy * mapY + ipx_P_xo].IsSolid && map[ipy * mapY + ipx_P_xo].CanOpen) map[ipy * mapY + ipx_P_xo].Open();
					if (map[ipy_P_yo * mapY + ipx].IsSolid && map[ipy_P_yo * mapY + ipx].CanOpen) map[ipy_P_yo * mapY + ipx].Open();
					return;
				default:
					return;
			}

		}
	}
}
