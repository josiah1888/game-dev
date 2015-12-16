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
            : base(content.Load<Texture2D>("sprites/sun"), new Vector2(SUN_POSITION_X, SUN_POSITION_Y))
        {
            this.Velocity = Vector2.Zero;
            this.ObeysGravity = false;
            this.AlwaysDraw = true;
        }

        public override void Draw(SpriteBatch batch, Vector2 position, SpriteEffects spriteEffects, Rectangle? spriteFrame = null)
        {
            batch.Draw(this.Sprite, new Vector2(SUN_POSITION_X, SUN_POSITION_Y), spriteFrame, this.Color, this.Rotation, Vector2.Zero, 1.0f, spriteEffects, 0);
        }
    }
}
