using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class Emu : Enemy
    {
        public Emu(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/place-holder"), position, 1, 1, Emu.Ai)
        {

        }

        private static Action<Enemy> Ai
        {
            get
            {
                return (Enemy Emu) =>
                {
                    // do ai logic using this instance of Emu

                    //if (Emu.getState() == Enemy.EnemyState.Idle)
                    //{
                    //    Emu.velocity.Y = -10;
                    //    //Emu.velocity.X = Emu.MAX_SPEED * Emu.
                    //}
                };
            }
        }
    }
}
