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
        public Texture2D sprite;
        public Vector2 position;
        public float rotation;
        public Vector2 center;
        public Vector2 velocity;
        public bool alive;
        public bool obeysGravity;
        public bool isOnGround;
        public int health;
        protected const int STANDARD_HEALTH = 1000;
        private const float VISION_FIELD = .02f;
        private const int VISION_LENGTH = 350;
        public float Scale = 1.0f;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)this.position.X, (int)this.position.Y, (int)(this.sprite.Width * Scale), (int)(this.sprite.Height * Scale));
            }
        }

        public void setup(Texture2D loadedTexture, Vector2 position, int health)
        {
            this.rotation = 0.0f;
            this.position = position;
            this.sprite = loadedTexture;
            this.center = new Vector2(sprite.Width / 2, sprite.Height / 2);
            this.velocity = Vector2.Zero;
            this.alive = false;
            this.obeysGravity = false;
            this.isOnGround = true; // ensures vertical velocity 
            this.health = health;
        }

        public GameObject()
        {

        }

        public GameObject(Texture2D loadedTexture)
        {
            setup(loadedTexture, Vector2.Zero, STANDARD_HEALTH);
        }

        public GameObject(Texture2D loadedTexture, Vector2 position, int health = STANDARD_HEALTH)
        {
            setup(loadedTexture, position, health);
        }

        public double DistanceFrom(GameObject otherGuy)
        {
            double deltaX = otherGuy.position.X - this.position.X;
            deltaX *= deltaX;
            double deltaY = otherGuy.position.Y - this.position.Y;
            deltaY *= deltaY;

            return Math.Sqrt(deltaX + deltaY);
        }

        public void MoveTowards(GameObject otherGuy)
        {
            Vector2 direction = GetDirection(otherGuy);
            this.position.X += direction.X * this.velocity.X;
            this.position.Y += direction.Y * this.velocity.Y;

            this.rotation = GetAngle(direction, Vector2.UnitY);
        }

        private Vector2 GetDirection(GameObject otherGuy)
        {
            Vector2 direction = new Vector2(otherGuy.position.X - this.position.X, otherGuy.position.Y - this.position.Y);
            direction.Normalize();
            return direction;
        }

        public float GetAngle(Vector2 a, Vector2 b)
        {
            double sin = -a.X * b.Y - b.X * a.Y;
            double cos = a.X * b.X + a.Y * b.Y;

            return (float) (Math.Atan2(sin, cos) + Math.PI);
        }

        public void Rotate(float rotation = .05f)
        {
            this.rotation += rotation;
        }

        public void Move(Keys[] keys)
        {
            if (keys.Contains(Keys.Up) && keys.Contains(Keys.Right))
            {
                this.position.Y -= this.velocity.Y / 2;
                this.position.X += this.velocity.X / 2;
            }
            else if (keys.Contains(Keys.Right) && keys.Contains(Keys.Down))
            {
                this.position.Y += this.velocity.Y / 2;
                this.position.X += this.velocity.X / 2;
            }
            else if (keys.Contains(Keys.Down) && keys.Contains(Keys.Left))
            {
                this.position.Y += this.velocity.Y / 2;
                this.position.X -= this.velocity.X / 2;
            }
            else if (keys.Contains(Keys.Left) && keys.Contains(Keys.Up))
            {
                this.position.Y -= this.velocity.Y / 2;
                this.position.X -= this.velocity.X / 2;
            }
            else if (keys.Contains(Keys.Left))
            {
                this.position.X -= this.velocity.X;
            }
            else if (keys.Contains(Keys.Up))
            {
                this.position.Y -= this.velocity.Y;
            }
            else if (keys.Contains(Keys.Down))
            {
                this.position.Y += this.velocity.Y;
            }
            else if (keys.Contains(Keys.Right))
            {
                this.position.X += this.velocity.X;
            }
        }

        public bool CanSee(GameObject otherGuy)
        {
            return GetAngle(GetDirection(otherGuy), Vector2.UnitY) - rotation < VISION_FIELD
                && DistanceFrom(otherGuy) < VISION_LENGTH;
        }

        public void ApplyGravity()
        {
            if (this.obeysGravity && !this.isOnGround)
                this.velocity.Y += 1;

            if (this.isOnGround)
                this.velocity.Y = 0;
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
    }
}
