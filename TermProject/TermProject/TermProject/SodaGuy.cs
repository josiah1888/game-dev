using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class SodaGuy : Enemy
    {
        public SodaGuy(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/place-holder"), position, 1, 1, SodaGuy.Ai)
        {

        }

        public static Action<Enemy> Ai
        {
            get
            {
                return (Enemy SodaGuy) =>
                {
                    // do ai logic using this instance of SodaGuy

                    //if (SodaGuy.getState() == Enemy.EnemyState.Idle)
                    //{
                    //    SodaGuy.velocity.Y = -10;
                    //    //SodaGuy.velocity.X = SodaGuy.MAX_SPEED * SodaGuy.
                    //}
                };
            }
        }
    }
}
