using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TermProject
{
    public class Sun : GameObject
    {
        public Sun(ContentManager content)
            : base(content.Load<Texture2D>("sprites/tempsun"), new Vector2(500, 700))
        {
            this.Velocity.X = 0f;
            this.Velocity.Y = 0f;
            this.ObeysGravity = false;
        }

        public void UpdateSun()
        {
            this.Position.X = GamePlay.vpCoords.X - 500;
            this.Position.Y = GamePlay.vpCoords.Y - 700;
        }

    }
}
