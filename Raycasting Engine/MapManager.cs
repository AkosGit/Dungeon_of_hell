
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
                EntityMap = new Enemy[16],
                MapName = "Main",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(2, 2, 64)
            };

            List<EntityObject> entities = new List<EntityObject>();
            Enemy test = new Enemy(1, 1, main.MapS, "Józsi", 360, 240);
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(test);
            Enemy test2 = new Enemy(4, 4, main.MapS, "Béla", 360, 240);
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(test2);

            Props health1 = new Props(2, 2, main.MapS, "Medkit", 120, 230, PropType.heal);
            health1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\medkit.png");
            entities.Add(health1);

            Props key = new Props(4, 4, main.MapS, "BlueKey", 100, 150, PropType.key);
            key.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\keyCard.png");
            entities.Add(key);

            main.EntityMap = entities.ToArray();
            main.SetDefaults();
            AddMap(main);

			#region map1
			Map map1 = new Map()
            {
                map = new MapObject[]
            {
                new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Rock(),  new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Rock(), new Door(), new Rock(), new Rock(), new Air(), new Air(), new Rock(),  new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Air(), new Rock(), new Brick(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(),
            },
                MapName = "map1",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(1, 14, 64)
            };
            entities = new List<EntityObject>();

            Enemy enemy1 = new Enemy(14, 1, map1.MapS, "Analízis", 360, 240);
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy1);

            Enemy enemy2 = new Enemy(6, 5, map1.MapS, "Dimat", 360, 240);
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy2);

            Enemy enemy3 = new Enemy(1, 5, map1.MapS, "Bev Infó", 360, 240);
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy3);

            Enemy enemy4 = new Enemy(7, 10, map1.MapS, "Villanytan", 360, 240);
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy4);

            Enemy enemy5 = new Enemy(12, 5, map1.MapS, "Makró", 360, 240);
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy5);

            Enemy enemy6 = new Enemy(14, 10, map1.MapS, "Szoftver fejlesztés", 360, 240);
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy6);

            Props health2 = new Props(7, 2, main.MapS, "Medkit", 120, 230, PropType.heal);
            health2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\medkit.png");
            entities.Add(health2);

            Props redKey = new Props(9, 14, main.MapS, "RedKey", 100, 150, PropType.key);
            redKey.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\keyCard.png");
            entities.Add(redKey);

            map1.EntityMap = entities.ToArray();
            map1.SetDefaults();
            AddMap(map1);
			#endregion
		}
		public Map GetMap(string mapname)
        {
            return Maps.Where(n => n.MapName == mapname).First();
        }
        public void AddMap(Map map)
        {
            Maps.Add(map);
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
