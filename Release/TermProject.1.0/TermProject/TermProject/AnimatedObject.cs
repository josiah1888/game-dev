using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TermProject
{
    public class AnimatedObject : GameObject
    {
        public float Depth;
        public bool Repeat = true;
        public bool IsPaused = false;

        protected int FrameCount;
        private float _TimePerFrame;
        protected float TimePerFrame
        {
            get
            {
                return this.Velocity.X == 0 ? _TimePerFrame : _TimePerFrame / Math.Abs(this.Velocity.X);
            }
            set
            {
                _TimePerFrame = value;
            }
        }

        private float MaxSpeed;
        private int Frame;
        private Timer Timer = new Timer();
        private SpriteEffects _Effects;
        private SpriteEffects Effects
        {
            get
            {
                if (this.Velocity.X > 0)
                {
                    _Effects = SpriteEffects.None;
                }
                else if (this.Velocity.X < 0)
                {
                    _Effects = SpriteEffects.FlipHorizontally;
                }

                return _Effects;
            }
        }

        public AnimatedObject(Texture2D loadedTexture, Vector2 position, float rotation, float scale, float depth, int frameCount, float timePerFrame, float maxSpeed = 5)
            : base(loadedTexture, position)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
            this.FrameCount = frameCount;
            this.Frame = 0;
            this.TimePerFrame = timePerFrame;
            this.MaxSpeed = maxSpeed;
        }

        public virtual void Update(List<GameObject> levelObjects, double elapsed)
        {
            if (Alive && !IsPaused && Timer.IsTimeYet(elapsed, TimePerFrame))
            {
                Frame = Repeat ? (Frame + 1) % FrameCount : Math.Min(FrameCount - 1, Frame + 1);
            }

            base.Update();
        }

        public void Restart()
        {
            Reset();
            Play();
        }

        public void Reset()
        {
            Frame = 0;
            this.Timer = new Timer();
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Play()
        {
            IsPaused = false;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public override void Draw(SpriteBatch batch, Vector2 position, SpriteEffects spriteEffects, Rectangle? spriteFrame = null)
        {
            int frameWidth = Sprite.Width / FrameCount;
            spriteFrame = new Rectangle(frameWidth * this.Frame, 0, frameWidth, this.Rectangle.Height);
            base.Draw(batch, position, Effects, spriteFrame);
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
