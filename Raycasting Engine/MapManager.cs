using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Utils;
using Color = System.Windows.Media.Color;

namespace Raycasting_Engine
{
    public class MapManager
    {
        List<Map> Maps;
        public MapManager()
        {
            if (!GlobalSettings.Settings.DisableSaving)
            {
                LoadMaps();
            }
            Maps = new List<Map>();
            Map main = new Map()
            {
                map = new MapObject[]
    {
                    new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Air(), new Brick(), new Air(), new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Brick(), new Air(), new Wood(),
                    new Wood(), new Air(), new Wood(), new Wood(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Wood(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Wood(), new Air(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Air(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                    new Wood(), new Air(), new Air(), new Wood(), new Air(), new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Wood(), new Air(), new Brick(),
                    new Wood(), new Air(), new Wood(), new Wood(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Wood(), new Air(), new Air(), new Air(), new Brick(),
                    new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                    new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(),
    },
                EntityMap = new EntityObject[16],
                MapName = "Main",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(2, 2, 64)
            };
            AddMap(main);
            Map test = new Map()
            {
                map = new MapObject[]
            {
                new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Door(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(),
            },
                MapName = "Test",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(2, 2, 64)
            };
            AddMap(test);
        }
        void LoadMaps()
        {
                foreach (string file in Directory.GetFiles(GlobalSettings.Settings.AssetsPath + "map"))
                {
                    Maps.Add((Map)ObjectManager.Read(file, typeof(Map)));
                }
        }
        public Map GetMap(string mapname)
        {
            return Maps.Where(n => n.MapName == mapname).First();
        }
        public void AddMap(Map map)
        {
            Maps.Add(map);
            if (!GlobalSettings.Settings.DisableSaving)
            {
                ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "map\\" + map.MapName + ".json", map);
            }
        }

    }
    //hell walls
    public class Rock : MapObject
    {

        public Rock() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 1.jpg";
        }
    }
    //between map
    //between 1-4 when multiple material texture +lava
    public class Between1 : MapObject
    {

        public Between1() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 3.jpg";
        }
    }
    public class Between2 : MapObject
    {

        public Between2() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 4.jpg";
        }
    }
    public class Between3 : MapObject
    {

        public Between3() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 5 .jpg";
        }
    }
    public class Between4 : MapObject
    {

        public Between4() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 8.jpg";
        }
    }
    public class BrickBetween : MapObject
    {

        public BrickBetween() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 8.jpg";
        }
    }
    public class WoodBetween : MapObject
    {

        public WoodBetween() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 8.jpg";
        }
    }
    //uni walls
    //brick + glass + wood
    public class InBetweenUni : MapObject
    {

        public InBetweenUni() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 2.jpg";
        }
    }
    public class Window : MapObject
    {

        public Window() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Window Wall .jpg";
        }
    }
    public class Wood : MapObject
    {

        public Wood() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wood Wall.jpg";
        }
    }
    public class Brick : MapObject
    {
        public Brick() : base(0, 0, Color.FromArgb(255, 226, 107, 139), true) 
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Brick Wall.jpg";
        }
    }
    public class Air : MapObject
    {
        public Air() : base(false) { }
    }
    public class Door : MapObject
    {
        public Door() : base(0, 0, Color.FromArgb(255, 123, 70, 23), true, true) {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\test.jpg";
        }

    }
}
