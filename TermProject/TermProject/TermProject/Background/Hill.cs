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
            : base(content.Load<Texture2D>("sprites/lazy-hill"), GetRandomHillPosition())
        {
            this.Velocity = Vector2.Zero;
            this.ObeysGravity = false;
            this.Color = Color.White * 0.75f;
        }

        private static Vector2 GetRandomHillPosition()
        {
            return new Vector2(Rand.Next(-100, 0), Rand.Next(75, 150));
        }
    }
}
