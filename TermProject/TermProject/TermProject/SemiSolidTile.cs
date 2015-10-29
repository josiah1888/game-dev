using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TermProject
{
    class SemiSolidTile : Tile
    {
          public SemiSolidTile()
            : base()
        { }

        public SemiSolidTile(ContentManager content)
            : base(content.Load<Texture2D>("Sprites/semi-solid-tile"))
        {
            this.Type = TileType.SemiSolid;
        }
    }
}
