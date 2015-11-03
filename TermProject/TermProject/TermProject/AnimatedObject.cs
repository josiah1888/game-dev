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
        private int FrameCount;
        private float TimePerFrame;
        private int Frame;
        private float TotalElapsed;
        private bool Paused;

        public float Rotation, Depth;
        public Vector2 Position;
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
            this.TimePerFrame = (float)1.0 / framesPerSec;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % FrameCount;
                TotalElapsed -= TimePerFrame;
            }
        }

        // class AnimatedTexture
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, Frame, screenPos);
        }
        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            int FrameWidth = sprite.Width / FrameCount;
            Rectangle sourcerect = new Rectangle(FrameWidth * frame, 0,
                FrameWidth, sprite.Height);
            batch.Draw(sprite, screenPos, sourcerect, Color.White,
                Rotation, Position, Scale, SpriteEffects.None, Depth);
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

        public new Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)this.position.X, (int)this.position.Y, (int)(this.sprite.Width * Scale / this.FrameCount), (int)(this.sprite.Height * Scale));
            }
        }
    }
}
