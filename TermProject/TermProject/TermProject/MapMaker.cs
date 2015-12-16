using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TermProject.Particles;

namespace TermProject
{
    public class Map
    {
        public Map(List<GameObject> levelObjects, Rectangle maxViewPort)
        {
            this.LevelObjects = levelObjects;
            this.MaxViewPort = maxViewPort;
        }

        public List<GameObject> LevelObjects;
        public Rectangle MaxViewPort;
    }

    public class MapMaker
    {

        private Dictionary<char, GameObjectType> _Legend;
        private Dictionary<char, GameObjectType> Legend
        {
            get
            {
                if (_Legend == null)
                {
                    _Legend = new Dictionary<char, GameObjectType>();
                    _Legend.Add('*', GameObjectType.SolidTile);
                    _Legend.Add('_', GameObjectType.SemiSolidTile);
                    _Legend.Add('p', GameObjectType.Player);
                    _Legend.Add('f', GameObjectType.Frog);
                    _Legend.Add('e', GameObjectType.Emu);
                    _Legend.Add('s', GameObjectType.SodaGuy);
                    _Legend.Add('d', GameObjectType.Door);
                }
                return _Legend;
            }
        }

        private ContentManager Content;
        private Action<GameObject> DeathAction;

        private enum GameObjectType
        {
            SolidTile,
            SemiSolidTile,
            Player,
            Frog,
            Emu,
            SodaGuy,
            Door,
            Hill,
            Sun
        }

        public MapMaker(ContentManager content, Action<GameObject> deathAction)
        {
            this.Content = content;
            this.DeathAction = deathAction;
        }

        public Map ReadMap(string asset)
        {
            List<GameObject> levelObjects = new List<GameObject>();
            List<string> lines = new List<string>(System.IO.File.ReadAllLines("../../../" + asset)); // asset not getting copied to bin folder
            for (int height = 0; height < lines.Count; height++)
            {
                for (int width = 0; width < lines.Min(i => i.Length); width++)
                {
                    char mapCode = lines[height][width];

                    if (this.Legend.ContainsKey(mapCode))
                    {
                        GameObjectType gameObjectType = this.Legend[char.ToLower(mapCode)];
                        List<GameObject> gameObjects = new List<GameObject>();
                        switch (gameObjectType)
                        {
                            default:
                            case GameObjectType.SolidTile:
                                gameObjects.Add(new SolidTile(this.Content, GetPosition(height, width)));
                                break;
                            case GameObjectType.SemiSolidTile:
                                gameObjects.Add(new SemiSolidTile(this.Content, GetPosition(height, width)));
                                break;
                            case GameObjectType.Player:
                                gameObjects.Add(new Player(this.Content, GetPosition(height, width)));
                                break;
                            case GameObjectType.Frog:
                                gameObjects.Add(new Frog(this.Content, GetPosition(height, width)));
                                break;
                            case GameObjectType.Emu:
                                gameObjects.Add(new Emu(this.Content, GetPosition(height, width)));
                                break;
                            case GameObjectType.SodaGuy:
                                gameObjects.AddRange(SodaGuy.CreateSodaGuysWithCans(this.Content, GetPosition(height, width)));
                                break;
                            case GameObjectType.Door:
                                gameObjects.Add(new Door(this.Content, GetPosition(height, width)));
                                break;
                            case GameObjectType.Hill:
                                gameObjects.Add(new Hill(this.Content));
                                break;
                            case GameObjectType.Sun:
                                gameObjects.Add(new Sun(this.Content));
                                break;
                        }

                        foreach (GameObject gameObject in gameObjects)
                        {
                            if (lines[height].Length > width + 1)
                            {
                                string designator = lines[height][width + 1].ToString();
                                int.TryParse(designator, out gameObject.Designator);
                            }
                            levelObjects.AddRange(gameObjects);
                        }
                    }
                }
            }

            levelObjects.ForEach(i =>
            {
                i.LevelObjects = levelObjects;
                i.DeathAction = this.DeathAction;
            });

            return new Map(levelObjects, new Rectangle(0, 0, lines.Min(i => i.Length) * Tile.SIZE, lines.Count * Tile.SIZE));
        }

        public List<GameObject> MakeSplashScreen(string asset)
        {
            return new List<GameObject>()
            { 
                new GameObject(this.Content.Load<Texture2D>(asset), Vector2.Zero) ,
                new Player(this.Content, new Vector2(0, 9999), new List<GameObject>())
            };
        }

        private Vector2 GetPosition(int x, int y)
        {
            return new Vector2(Tile.SIZE * y, Tile.SIZE * x);
        }
    }
}
