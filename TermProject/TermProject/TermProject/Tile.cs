using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TermProject
{
    public class Tile : GameObject
    {
        public const int SIZE = 25;
        public enum TileType
        {
            SemiSolid,
            Solid
        }

        public TileType Type;

        public Tile()
            : base()
        {
        }

        public Tile(Texture2D loadedTexture, Vector2 position, TileType type)
            : base(loadedTexture, position)
        {
            this.Type = type;
        }

        public new Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)this.position.X, (int)this.position.Y, SIZE, SIZE);
            }
        }
    }
}
