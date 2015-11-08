using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class Frog : Enemy
    {
        public Frog(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/place-holder"), position, 1, 1, Frog.Ai)
        {

        }

        private static Action<Enemy> Ai
        {
            get
            {
                return (Enemy frog) =>
                {
                    // do ai logic using this instance of frog

                    //if (frog.getState() == Enemy.EnemyState.Idle)
                    //{
                    //    frog.velocity.Y = -10;
                    //    //frog.velocity.X = frog.MAX_SPEED * frog.
                    //}
                };
            }
        }
    }
}
