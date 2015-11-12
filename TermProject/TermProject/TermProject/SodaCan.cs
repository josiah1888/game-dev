using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class SodaCan : Enemy
    {
        public SodaCan(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/place-holder"), position, 1, 1, SodaCan.Ai)
        {
            this.AttackSprite = this.IdleSprite = content.Load<Texture2D>("sprites/place-holder");
        }

        private static Action<Enemy> Ai
        {
            get
            {
                return (Enemy SodaCan) =>
                {
                    if (SodaCan.Direction == EnemyDirection.Left)
                        SodaCan.Velocity.X = MAX_SPEED * -1;
                    else if (SodaCan.Direction == EnemyDirection.Right)
                        SodaCan.Velocity.X = MAX_SPEED;
                    else
                        SodaCan.Velocity.X = 0;
                };
            }
        }
    }
}
