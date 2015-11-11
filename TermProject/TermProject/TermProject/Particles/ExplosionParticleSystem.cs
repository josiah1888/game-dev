using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TermProject
{
    namespace Particles
    {
        public class ExplosionParticleSystem : ParticleSystem
        {
            public ExplosionParticleSystem(Game1 game, int howManyEffects, string textureFileName)
                : base(game, howManyEffects, textureFileName)
            {
            }

            protected override void InitializeConstants()
            {
                minInitialSpeed = 10;
                maxInitialSpeed = 80;

                minAcceleration = 0;
                maxAcceleration = 0;

                minLifetime = .3f;
                maxLifetime = .6f;

                minScale = .2f;
                maxScale = .6f;

                minNumParticles = 5;
                maxNumParticles = 10;

                minRotationSpeed = -MathHelper.PiOver4;
                maxRotationSpeed = MathHelper.PiOver4;

                spriteBlendMode = BlendState.Additive;

                DrawOrder = AdditiveDrawOrder;
            }

            protected override void InitializeParticle(Particle p, Vector2 where)
            {
                base.InitializeParticle(p, where);
                p.acceleration = -p.velocity / p.lifetime;
            }
        }
    }
}
