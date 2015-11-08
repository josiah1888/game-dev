using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TermProject
{
    public class Enemy : AnimatedObject
    {
        private Action<Enemy> Ai;
        public enum EnemyState { Idle, Attack }
        public EnemyState state;
        public const int threshold = 50;

        public const int MAX_SPEED = 4;
        private const int MAX_GRAVITY = 3;

        public enum Direction
        {
            Left = -1,
            Right = 1
        }

        public Enemy(Texture2D loadedTexture, Vector2 position, int frameCount, int framesPerSec, Action<Enemy> ai)
            : base(loadedTexture, position, 0f, 1f, 1f, frameCount, framesPerSec)
        {
            this.Ai = ai;
        }

        public EnemyState getState()
        {
            return state;
        }

        public void setState(EnemyState newState)
        {
            state = newState;
        }

        public void Update(List<GameObject> levelObjects)
        {
            if (ObeysGravity)
                ApplyGravity(levelObjects);

            this.Ai(this);
        }

        public bool IsOnGround(List<GameObject> levelObjects)
        {
            return this.velocity.Y >= 0 && levelObjects.Where(i => i is SemiSolidTile || i is SolidTile).Any(i => i.TopRectangle.Intersects(this.BottomRectangle));
        }

        private void ApplyGravity(List<GameObject> levelObjects)
        {
            if (!IsOnGround(levelObjects))
            {
                this.velocity.Y = Math.Min(MAX_GRAVITY, this.velocity.Y + 1);
            }
            else if (this.velocity.Y > 0)
            {
                this.velocity.Y = 0;
            }
        }
    }
}
