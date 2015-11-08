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
        private float MaxSpeed;
        private float _TimePerFrame;
        protected float TimePerFrame
        {
            get
            {
                return _TimePerFrame * Math.Abs(this.Velocity.X);
            }
            set
            {
                _TimePerFrame = value;
            }
        }
        private int Frame;
        private double TotalElapsed;
        private bool Paused;

        public AnimatedObject(Texture2D loadedTexture, Vector2 position, float rotation, float scale, float depth, int frameCount, int timePerFrame, float maxSpeed = 5)
            : base(loadedTexture, position)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
            this.FrameCount = frameCount;
            this.Paused = false;
            this.TotalElapsed = 0;
            this.Frame = 0;
            this.TimePerFrame = timePerFrame;
            this.MaxSpeed = maxSpeed;
        }

        public void Update(double elapsed)
        {
            if (!Paused)
            {
                TotalElapsed += elapsed;
                if (TotalElapsed > TimePerFrame)
                {
                    Frame++;
                    Frame = Frame % FrameCount;
                    TotalElapsed = 0;
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
            TotalElapsed = 0f;
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
