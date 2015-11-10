using Microsoft.Xna.Framework;
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
        double WaitTimer = 0;
        public Action WinAction = () => { };

        public Door(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/door"), position, 0f, 1f, 1f, 1, .8f)
        {
        }

        public override void Update(List<GameObject> levelObjects, double elapsed)
        {
            Player player = (Player)levelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));

            if (this.Rectangle.Intersects(player.Rectangle) && this.Repeat)
            {
                player.Pause();
                OpenDoor();
                this.WaitTimer = elapsed + 1500;
            }
            else if (!this.Repeat)
            {
                if (elapsed > WaitTimer)
                {
                    this.WinAction();
                }
            }

            base.Update(levelObjects, elapsed);
        }

        private void OpenDoor()
        {
            this.Reset();
            this.Repeat = false;
            this.FrameCount = 1; // todo make/implement door opening sprite
        }
    }
}
