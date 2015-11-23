﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class Door : AnimatedObject
    {
        public Action WinAction = () => { };

        private Texture2D SwingingSprite;
        private bool IsOpen = false;
        private const double DOOR_DELAY_TIME = 1500;

        public Door(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/door"), position, 0f, 1f, 1f, 1, .8f)
        {
            this.SwingingSprite = content.Load<Texture2D>("sprites/door-swing");
            this.ObeysGravity = false;
        }

        public override void Update(List<GameObject> levelObjects, double elapsed)
        {
            Player player = (Player)levelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));

            if (player != null && this.Rectangle.Intersects(player.Rectangle) && !this.IsOpen)
            {
                player.Stop();
                OpenDoor();
            }
            else if (this.IsOpen && Timer.IsTimeYet(this, elapsed, DOOR_DELAY_TIME))
            {
                this.WinAction();
            }

            base.Update(levelObjects, elapsed);
        }

        private void OpenDoor()
        {
            this.Reset();
            this.Repeat = false;
            this.TimePerFrame = 100f;
            this.FrameCount = 5;
            this.Sprite = this.SwingingSprite;
            this.IsOpen = true;
        }
    }
}
