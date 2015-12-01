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
        private const float SUN_POSITION_X = 150;
        private const float SUN_POSITION_Y = 100;

        public Sun(ContentManager content)
            : base(content.Load<Texture2D>("sprites/tempsun"), new Vector2(SUN_POSITION_X, SUN_POSITION_Y))
        {
            this.Velocity = Vector2.Zero;
            this.ObeysGravity = false;
        }

        public override void Update()
        {
            this.Position.X = GamePlay.vpCoords.X + SUN_POSITION_X;
            this.Position.Y = GamePlay.vpCoords.Y + SUN_POSITION_Y;

            base.Update();
        }
    }
}
