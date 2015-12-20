using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TermProject
{
    class SolidTile : Tile
    {
        public SolidTile(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/solid-tile"), position, TileType.Solid)
        {
        }
    }
}
