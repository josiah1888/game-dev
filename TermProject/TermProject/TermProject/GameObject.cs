using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TermProject
{
    public class GameObject
    {
        public Texture2D Sprite;
        public Vector2 Position;
        public float Rotation;
        public Vector2 Center;
        public Vector2 Velocity;
        public bool Alive;
        public int Health;
        public float Scale = 1.0f;

        protected bool ObeysGravity;
        protected const int STANDARD_HEALTH = 1000;

        private const float VISION_FIELD = .02f;
        private const int VISION_LENGTH = 350;
        private const int MAX_GRAVITY = 3;


        public GameObject()
        {

        }

        public GameObject(Texture2D loadedTexture)
        {
            Setup(loadedTexture, Vector2.Zero, STANDARD_HEALTH);
        }

        public GameObject(Texture2D loadedTexture, Vector2 position, int health = STANDARD_HEALTH)
        {
            Setup(loadedTexture, position, health);
        }

        public void Setup(Texture2D loadedTexture, Vector2 position, int health)
        {
            this.Rotation = 0.0f;
            this.Position = position;
            this.Sprite = loadedTexture;
            this.Center = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            this.Velocity = Vector2.Zero;
            this.Alive = false;
            this.ObeysGravity = true;
            this.Health = health;
        }

        public void Rotate(float rotation = .05f)
        {
            this.Rotation += rotation;
        }

        public void Move(Keys[] keys)
        {
            if (keys.Contains(Keys.Up) && keys.Contains(Keys.Right))
            {
                this.Position.Y -= this.Velocity.Y / 2;
                this.Position.X += this.Velocity.X / 2;
            }
            else if (keys.Contains(Keys.Right) && keys.Contains(Keys.Down))
            {
                this.Position.Y += this.Velocity.Y / 2;
                this.Position.X += this.Velocity.X / 2;
            }
            else if (keys.Contains(Keys.Down) && keys.Contains(Keys.Left))
            {
                this.Position.Y += this.Velocity.Y / 2;
                this.Position.X -= this.Velocity.X / 2;
            }
            else if (keys.Contains(Keys.Left) && keys.Contains(Keys.Up))
            {
                this.Position.Y -= this.Velocity.Y / 2;
                this.Position.X -= this.Velocity.X / 2;
            }
            else if (keys.Contains(Keys.Left))
            {
                this.Position.X -= this.Velocity.X;
            }
            else if (keys.Contains(Keys.Up))
            {
                this.Position.Y -= this.Velocity.Y;
            }
            else if (keys.Contains(Keys.Down))
            {
                this.Position.Y += this.Velocity.Y;
            }
            else if (keys.Contains(Keys.Right))
            {
                this.Position.X += this.Velocity.X;
            }
        }

        protected void ApplyGravity(List<GameObject> levelObjects)
        {
            if (this.ObeysGravity)
            {
                if (!IsOnGround(levelObjects))
                {
                    this.Velocity.Y = Math.Min(MAX_GRAVITY, this.Velocity.Y + 1);
                }
                else if (this.Velocity.Y > 0)
                {
                    this.Velocity.Y = 0;
                }
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(this.Sprite.Width * Scale), (int)(this.Sprite.Height * Scale));
            }
        }

        public Rectangle TopRectangle
        {
            get
            {
                return new Rectangle(this.Rectangle.X, this.Rectangle.Y, this.Rectangle.Width, 4);
            }
        }

        public Rectangle BottomRectangle
        {
            get
            {
                return new Rectangle(this.Rectangle.X, this.Rectangle.Y + this.Rectangle.Height, this.Rectangle.Width, 4);
            }
        }

        public bool IsOnGround(List<GameObject> levelObjects)
        {
            return this.Velocity.Y >= 0 && levelObjects.Where(i => i is SemiSolidTile || i is SolidTile).Any(i => i.TopRectangle.Intersects(this.BottomRectangle));
        }

        #region Legacy GameObject Methods
        [Obsolete("Legacy code from a past game.")]
        public double DistanceFrom(GameObject otherGuy)
        {
            double deltaX = otherGuy.Position.X - this.Position.X;
            deltaX *= deltaX;
            double deltaY = otherGuy.Position.Y - this.Position.Y;
            deltaY *= deltaY;

            return Math.Sqrt(deltaX + deltaY);
        }

        [Obsolete("Legacy code from a past game.")]
        public void MoveTowards(GameObject otherGuy)
        {
            Vector2 direction = GetDirection(otherGuy);
            this.Position.X += direction.X * this.Velocity.X;
            this.Position.Y += direction.Y * this.Velocity.Y;

            this.Rotation = GetAngle(direction, Vector2.UnitY);
        }

        [Obsolete("Legacy code from a past game.")]
        private Vector2 GetDirection(GameObject otherGuy)
        {
            Vector2 direction = new Vector2(otherGuy.Position.X - this.Position.X, otherGuy.Position.Y - this.Position.Y);
            direction.Normalize();
            return direction;
        }

        [Obsolete("Legacy code from a past game.")]
        private float GetAngle(Vector2 a, Vector2 b)
        {
            double sin = -a.X * b.Y - b.X * a.Y;
            double cos = a.X * b.X + a.Y * b.Y;

            return (float)(Math.Atan2(sin, cos) + Math.PI);
        }

        [Obsolete("Legacy code from a past game.")]
        public bool CanSee(GameObject otherGuy)
        {
            return GetAngle(GetDirection(otherGuy), Vector2.UnitY) - Rotation < VISION_FIELD
                && DistanceFrom(otherGuy) < VISION_LENGTH;
        }
        #endregion
    }
}
