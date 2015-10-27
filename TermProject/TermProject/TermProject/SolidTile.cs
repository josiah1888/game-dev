using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    class SolidTile : Tile
    {
        public SolidTile()
            : base()
        {
            this.Type = TileType.Solid;
        }
    }
}
