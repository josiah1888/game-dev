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

        Rectangle ViewPort { get; set; }
        MapMaker MapMaker;
        Background Background;

        ExplosionParticleSystem Explosion;
        ExplosionSmokeParticleSystem Smoke;

        List<GameObject> LevelObjects;

        Player Player
        {
            get
            {
                return (Player)this.LevelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));
            }
        }

        enum GameStates
        {
            Playing,
            Transition
        }

        GameStates GameState;

        Queue<Action> _WinActions;
        Queue<Action> WinActions
        {
            get
            {
                if (_WinActions == null)
                {
                    _WinActions = new Queue<Action>();
                    _WinActions.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level1--intro");
                        UpdateViewport(0);
                        this.GameState = GameStates.Transition;
                    });
                    _WinActions.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level1");
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door));
                        door.WinAction = WinActions.Dequeue();
                        UpdateViewport(0);
                    });
                    _WinActions.Enqueue(() =>
                    {
                        UpdateViewport(0);
                        LevelObjects = MapMaker.ReadMap("maps/level-selection");
                        LevelObjects
                            .Where(i => i.Designator == 1)
                            .ToList()
                            .ForEach(i => i.Alive = false);
                        // optionally, add kaboom after a delay
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 2);
                        door.WinAction = WinActions.Dequeue();
                    });
                    _WinActions.Enqueue(() =>
                    {
                        // Level 2
                    });
                }
                return _WinActions;
            }
        }

        private object TransitionDrawLock = new object();
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
            LevelObjects = MapMaker.ReadMap("maps/level-selection");
            Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 1);
            door.WinAction = WinActions.Dequeue();
            Background = new TermProject.Background(this.Content);
            UpdateViewport(0);
        }

        private Action<GameObject> GetDeathAction()
        {
            return (GameObject gameObject) =>
            {
                Smoke.AddParticles(gameObject.Position.GetDrawablePosition(this.ViewPort));
                Explosion.AddParticles(gameObject.Position.GetDrawablePosition(this.ViewPort));
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            double elapsed = gameTime.TotalGameTime.TotalMilliseconds;

            if (LevelObjects.Any())
            {
                Update_AnimatedObjects(elapsed);
                Update_Enemies();
                Update_Player(elapsed);
                Update_Positions();
                Update_Camera();
            }

            Background.Update();
            base.Update(gameTime);
        }

        #region Update
        private void Update_Enemies()
        {
            this.LevelObjects
                .Where(i => i.Alive && i is Enemy)
                .Select(i => (Enemy)i).ToList()
                .ForEach(i => i.Update(LevelObjects));
        }

        private void Update_AnimatedObjects(double elapsed)
        {
            LevelObjects
                .Where(i => i.Alive && i is AnimatedObject && !(i is Player))
                .Select(i => (AnimatedObject)i).ToList()
                .ForEach(i => i.Update(LevelObjects, elapsed));
        }

        private void Update_Player(double elapsed)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Player.Update(LevelObjects, keyboardState.GetPressedKeys(), this.ViewPort, elapsed);
        }

        private void Update_Positions()
        {
            LevelObjects.ForEach(i => i.Update());
        }

        private void Update_Camera()
        {
            float distancePlayerIsAhead = Player.Position.X + Player.Center.X - this.ViewPort.X;
            if (distancePlayerIsAhead > this.ViewPort.Width * (3.0 / 5.0))
            {
                UpdateViewport(this.ViewPort.X + distancePlayerIsAhead / 150f);
            }
        }

        private void UpdateViewport(double x)
        {
            this.ViewPort = new Rectangle((int)x, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
        }
        #endregion

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin();

            LevelObjects
                .Union(Background.Clouds)
                .Where(i => i.Rectangle.Intersects(this.ViewPort))
                .OrderBy(i => i is Player)
                .ThenBy(i => i is Enemy)
                .ThenBy(i => i is Tile)
                .ThenBy(i => i is Cloud)
                .ToList().ForEach(i =>
            {
                i.Draw(SpriteBatch, i.Position.GetDrawablePosition(this.ViewPort), SpriteEffects.None);
            });

            if (this.GameState == GameStates.Transition && Timer.IsTimeYet(TransitionDrawLock, gameTime.TotalGameTime.TotalMilliseconds, TRANSITION_DELAY_TIME))
            {
                this.GameState = GameStates.Playing;
                this.WinActions.Dequeue()();
            }

            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
