using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TermProject
{
    public class AnimatedObject : GameObject
    {
        public float Depth;

        protected int FrameCount;
        private float _TimePerFrame;
        protected float TimePerFrame
        {
            get
            {
                return this.Velocity.X == 0 ? 1 : _TimePerFrame / Math.Abs(this.Velocity.X);
            }
            set
            {
                _TimePerFrame = value;
            }
        }

        private float MaxSpeed;
        private int Frame;
        private double Elapsed;
        private bool Paused;
        public bool Repeat = true;

        public AnimatedObject(Texture2D loadedTexture, Vector2 position, float rotation, float scale, float depth, int frameCount, float timePerFrame, float maxSpeed = 5)
            : base(loadedTexture, position)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
            this.FrameCount = frameCount;
            this.Paused = false;
            this.Elapsed = 0;
            this.Frame = 0;
            this.TimePerFrame = timePerFrame;
            this.MaxSpeed = maxSpeed;
        }

        public virtual void Update(List<GameObject> levelObjects, double elapsed)
        {
            if (!Paused && TimePerFrame > 0)
            {
                if (elapsed - Elapsed > TimePerFrame)
                {
                    Frame = Repeat ? (Frame + 1) % FrameCount : Math.Min(FrameCount - 1, Frame + 1);
                    Elapsed = elapsed;
                }
            }
        }

        public bool IsPaused
        {
            get { return Paused; }
        }

        public void Reset()
        {
            Frame = 0;
            Elapsed = 0f;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Play()
        {
            Paused = false;
        }

        public void Pause()
        {
            Paused = true;
        }

        public override void Draw(SpriteBatch batch, Rectangle viewPort, SpriteEffects spriteEffects, Rectangle? spriteFrame = null)
        {
            int frameWidth = Sprite.Width / FrameCount;
            spriteFrame = new Rectangle(frameWidth * this.Frame, 0, frameWidth, this.Rectangle.Height);
            spriteEffects = this.Velocity.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            base.Draw(batch, viewPort, spriteEffects, spriteFrame);
        }

        public override Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(this.Sprite.Width * Scale / this.FrameCount), (int)(this.Sprite.Height * Scale));
            }
        }
    }
}
