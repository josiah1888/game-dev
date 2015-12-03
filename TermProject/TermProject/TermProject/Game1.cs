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
using TermProject.Particles;
using System.Timers;

namespace TermProject
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        MapMaker MapMaker;
        Background Background;
        ExplosionParticleSystem Explosion;
        ExplosionSmokeParticleSystem Smoke;
        SoundEffect ExplosionSound;
        GamePlay GamePlay;

        Song BackgroundMusic;


        Player Player
        {
            get
            {
                return (Player)GamePlay.LevelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));
            }
        }

        private const double TRANSITION_DELAY_TIME = 3000;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Explosion = new ExplosionParticleSystem(this, 1, "sprites/explosion");
            Components.Add(Explosion);

            Smoke = new ExplosionSmokeParticleSystem(this, 2, "sprites/smoke");
            Components.Add(Smoke);
        }

        protected override void Initialize()
        {
            Graphics.PreferredBackBufferHeight = 608; // 19 Tiles
            Graphics.PreferredBackBufferWidth = 1024; // 32 Tiles
            Graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            MapMaker = new MapMaker(this.Content, GetDeathAction());
            Background = new Background(this.Content);
            GamePlay = new GamePlay(MapMaker, Window);
            ExplosionSound = Content.Load<SoundEffect>("sounds/explosion");
            BackgroundMusic = Content.Load<Song>("sounds/groove");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(BackgroundMusic);
        }

        private Action<GameObject> GetDeathAction()
        {
            return (GameObject gameObject) =>
            {
                Smoke.AddParticles(gameObject.Position.GetDrawablePosition(GamePlay.ViewPort));
                Explosion.AddParticles(gameObject.Position.GetDrawablePosition(GamePlay.ViewPort));
                ExplosionSound.Play();
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            double elapsed = gameTime.TotalGameTime.TotalMilliseconds;

            if (GamePlay.LevelObjects.Any())
            {
                Update_AnimatedObjects(elapsed);
                Update_Player(elapsed);
            }

            Background.Update();
            GamePlay.Update(elapsed);
            base.Update(gameTime);
        }

        #region Update
        private void Update_AnimatedObjects(double elapsed)
        {
            GamePlay.LevelObjects
                .Where(i => i.Alive && i is AnimatedObject && !(i is Player))
                .Select(i => (AnimatedObject)i).ToList()
                .ForEach(i => i.Update(GamePlay.LevelObjects, elapsed));
        }

        private void Update_Player(double elapsed)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Player.Update(GamePlay.LevelObjects, keyboardState.GetPressedKeys(), elapsed);
        }
        #endregion

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin();

            IEnumerable<GameObject> drawableObjects = GamePlay.GameState == TermProject.GamePlay.GameStates.Playing
                ? GamePlay.LevelObjects.Union(Background.BackgroundObjects)
                : GamePlay.LevelObjects;

            drawableObjects
                .Where(i => i.Rectangle.Intersects(GamePlay.ViewPort))
                .OrderBy(i => i is Player)
                .ThenBy(i => i is Enemy)
                .ThenBy(i => i is Door)
                .ThenBy(i => !(i is Player || i is Enemy || i is Door || i is Tile || i is Cloud || i is Hill || i is Sun))
                    /*
                     * Player life icons 
                     * todo: refactor into its own class
                     */
                .ThenBy(i => i is Tile)
                .ThenBy(i => i is Hill)
                .ThenBy(i => i is Cloud)
                .ThenBy(i => i is Sun)
                .ToList().ForEach(i =>
            {
                i.Draw(SpriteBatch, i.Position.GetDrawablePosition(GamePlay.ViewPort), SpriteEffects.None);
            });

            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
