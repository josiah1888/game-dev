using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TermProject
{
    public class Hill : GameObject
    {
    public Hill(ContentManager content)
            : base(content.Load<Texture2D>("sprites/lazyhill"), new Vector2(0, 40))
        {
            this.Velocity.X = 0f;
            this.Velocity.Y = 0f;
            this.ObeysGravity = false;
        }

        

    }
}
