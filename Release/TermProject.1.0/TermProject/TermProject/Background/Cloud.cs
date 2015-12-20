using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TermProject
{
    public class Cloud : GameObject
    {
        private static float MAX_CLOUD_SPEED = .2f;

        public Cloud(ContentManager content)
            : base(content.Load<Texture2D>("sprites/cloud"), GetRandomCloudPosition())
        {
            this.Velocity = GetRandomCloudVelocity();
            this.ObeysGravity = false;
        }

        private static Vector2 GetRandomCloudPosition()
        {
            return new Vector2(Rand.Next(0, 4000), Rand.Next(15, 120));
        }

        private static Vector2 GetRandomCloudVelocity()
        {
            double randomSpeed = (Rand.NextDouble() + 1) * MAX_CLOUD_SPEED;
            return new Vector2((float)(randomSpeed * Math.Sin(10 * randomSpeed)), 0f);
        }
    }
}
