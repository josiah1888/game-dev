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

        private int FrameCount;
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;

        public AnimatedObject(Texture2D loadedTexture, Vector2 position, float rotation, float scale, float depth, int frameCount, int framesPerSec, int health = STANDARD_HEALTH)
            : base(loadedTexture, position, health)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
            this.FrameCount = frameCount;
            this.Paused = false;
            this.TotalElapsed = 0;
            this.Frame = 0;
            this.TimePerFrame = 1.0f / framesPerSec;
        }

        public void UpdateFrame(float elapsed)
        {
            if (!Paused)
            {
                TotalElapsed += elapsed;
                if (TotalElapsed > TimePerFrame)
                {
                    Frame++;
                    Frame = Frame % FrameCount;
                    TotalElapsed -= TimePerFrame;
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

        private void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, Frame, screenPos);
        }

        private void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            int FrameWidth = Sprite.Width / FrameCount;
            Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0, FrameWidth, Sprite.Height);
            batch.Draw(Sprite, screenPos, sourcerect, Color.White, Rotation, Position, Scale, SpriteEffects.None, Depth);
        }

        public new Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(this.Sprite.Width * Scale / this.FrameCount), (int)(this.Sprite.Height * Scale));
            }
        }
    }
}
