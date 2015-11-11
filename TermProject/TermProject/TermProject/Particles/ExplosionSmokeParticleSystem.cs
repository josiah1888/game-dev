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
        public class ExplosionSmokeParticleSystem : ParticleSystem
        {
            public ExplosionSmokeParticleSystem(Game1 game, int howManyEffects, string textureFileName)
                : base(game, howManyEffects, textureFileName)
            {
            }

            protected override void InitializeConstants()
            {
                minInitialSpeed = 20;
                maxInitialSpeed = 200;

                minAcceleration = -10;
                maxAcceleration = -50;

                minLifetime = .5f;
                maxLifetime = 1.5f;

                minScale = .3f;
                maxScale = .8f;

                minNumParticles = 4;
                maxNumParticles = 9;

                minRotationSpeed = -MathHelper.PiOver4;
                maxRotationSpeed = MathHelper.PiOver4;

                spriteBlendMode = BlendState.AlphaBlend;

                DrawOrder = AlphaBlendDrawOrder;
            }
        }
    }
}
