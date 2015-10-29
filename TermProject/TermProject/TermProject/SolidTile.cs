using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TermProject
{
    class SolidTile : Tile
    {
        public SolidTile()
            : base()
        { }


        public SolidTile(ContentManager content)
            : base(content.Load<Texture2D>("Sprites/solid-tile"))
        {
            this.Type = TileType.Solid;
        }
    }
}
