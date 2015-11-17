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
        private static Random Rand = new Random();
        private static float MAX_CLOUD_SPEED = .4f;

        public Cloud(ContentManager content)
            : base(content.Load<Texture2D>("sprites/cloud"), GetRandomCloudPosition())
        {
            this.Velocity = GetRandomCloudVelocity();
        }

        private static Vector2 GetRandomCloudPosition()
        {
            return new Vector2(Rand.Next(0, 4000), Rand.Next(15, 120));
        }

        private static Vector2 GetRandomCloudVelocity()
        {
            double randomDouble = Rand.NextDouble();
            return new Vector2((float)(MAX_CLOUD_SPEED * randomDouble * (Rand.Next(2) - 1)), 0f);
        }
    }
}
