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
        public float speed = MAX_SPEED;
        public float jumpHeight = -10;

        const float JUMP_HEIGHT_INCREASE = .05f;
        const float FRICTION = .02f;
        const float BOUNCE = 1.3f;

        public SodaCan(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/can"), position, 1, 1, SodaCan.Ai)
        {
            this.AttackSprite = this.IdleSprite = content.Load<Texture2D>("sprites/can");
        }

        private static Action<Enemy> Ai
        {
            get
            {
                return (Enemy sodaCan) =>
                {
                    if (sodaCan.IsOnGround())
                    {
                        if (((SodaCan)sodaCan).jumpHeight < 0)
                        {
                            ((SodaCan)sodaCan).jumpHeight = (int)Math.Ceiling(((SodaCan)sodaCan).jumpHeight / BOUNCE);
                            sodaCan.Velocity.Y = ((SodaCan)sodaCan).jumpHeight;
                        }

                        if (sodaCan.Velocity.Y == Enemy.MAX_GRAVITY && ((SodaCan)sodaCan).jumpHeight > -10)
                        {
                            ((SodaCan)sodaCan).jumpHeight -= JUMP_HEIGHT_INCREASE;
                        }
                    }

                    if (((SodaCan)sodaCan).speed <= 0)
                        sodaCan.Alive = false;

                    if (sodaCan.Alive == true)
                    {
                        if (sodaCan.Direction == EnemyDirection.Left)
                            sodaCan.Velocity.X = ((SodaCan)sodaCan).speed * -1;
                        else if (sodaCan.Direction == EnemyDirection.Right)
                            sodaCan.Velocity.X = ((SodaCan)sodaCan).speed;
                        else
                            sodaCan.Velocity.X = 0;
                    }

                    if (sodaCan.IsOnGround())
                        ((SodaCan)sodaCan).speed -= FRICTION;
                };
            }
        }
    }
}