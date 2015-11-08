using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TermProject
{
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

        private enum GameObjectType
        {
            SolidTile,
            SemiSolidTile,
            Player,
            Frog,
            Emu,
            SodaGuy,
            Door
        }

        public MapMaker(ContentManager content)
        {
            this.Content = content;
        }

        public List<GameObject> ReadMap(string asset)
        {
            List<GameObject> mapObjects = new List<GameObject>();
            try
            {
                List<string> lines = new List<string>(System.IO.File.ReadAllLines("../../../" + asset)); // asset not getting copied to bin folder
                for (int x = 0; x < lines.Count; x++)
                {
                    for (int y = 0; y < lines.Min(i => i.Length); y++)
                    {
                        if (this.Legend.ContainsKey(lines[x][y]))
                        {
                            GameObjectType gameObjectType = this.Legend[lines[x].ToLower()[y]];
                            GameObject gameObject = null;
                            switch(gameObjectType)
                            {
                                default:
                                case GameObjectType.SolidTile:
                                    gameObject = new SolidTile(this.Content, GetPosition(x, y));
                                    break;
                                case GameObjectType.SemiSolidTile:
                                    gameObject = new SemiSolidTile(this.Content, GetPosition(x, y));
                                    break;
                                case GameObjectType.Player:
                                    gameObject = new Player(this.Content, GetPosition(x, y));
                                    break;
                                case GameObjectType.Frog:
                                    gameObject = new Frog(this.Content, GetPosition(x, y));
                                    break;
                                case GameObjectType.Emu:
                                    gameObject = new Emu(this.Content, GetPosition(x, y));
                                    break;
                                case GameObjectType.SodaGuy:
                                    gameObject = new SodaGuy(this.Content, GetPosition(x, y));
                                    break;
                                case GameObjectType.Door:
                                    gameObject = new Door(this.Content, GetPosition(x, y));
                                    break;
                            }

                            if (gameObject != null) mapObjects.Add(gameObject);
                        }
                        else
                        {
                            //throw new Exception("Invalid character found in map");
                            // for final testing?
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Maybe try to load a different level? reload? Maybe this will just never happen
                throw e;
            }

            return mapObjects;
        }

        private Vector2 GetPosition(int x, int y)
        {
            return new Vector2(Tile.SIZE * y, Tile.SIZE * x);
        }
    }
}
