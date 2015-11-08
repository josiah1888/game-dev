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

namespace TermProject
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle ViewPort { get; set; }

        private Dictionary<char, Type> _MapLegend;
        public Dictionary<char, Type> MapLegend
        {
            get
            {
                if (_MapLegend == null)
                {
                    _MapLegend = new Dictionary<char, Type>();
                    _MapLegend.Add('*', (new SolidTile()).GetType());
                    _MapLegend.Add('_', (new SemiSolidTile()).GetType());
                }
                return _MapLegend;
            }
        }

        List<GameObject> levelObjects;

        Player Player;
        Enemy Frog;

        public Action<Enemy> GetFrogAI()
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

        public Action<Enemy> GetBirdAI()
        {
            return (Enemy bird) =>
            {
                // logic in here
            };
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MapMaker mapMaker = new MapMaker(Content);
            mapMaker.Legend = MapLegend;
            levelObjects = mapMaker.ReadMap("maps/level1");
            Player = new Player(Content.Load<Texture2D>("Sprites/playerIdle"), new Vector2(35, 50), 1, 1);
            Frog = new Enemy(Content.Load<Texture2D>("Sprites/place-holder"), new Vector2(100, 50), 1, 1, GetFrogAI());
            levelObjects.Add(Player);
            UpdateViewport(0);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Update_Player();
            Update_Positions();
            Update_Camera();
            base.Update(gameTime);
        }

        private void Update_Player()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Player.Update(levelObjects, keyboardState.GetPressedKeys(), this.ViewPort);
        }

        private void Update_Positions()
        {
            levelObjects.ForEach(i =>
            {
                i.position.X += i.velocity.X;
                i.position.Y += i.velocity.Y;
            });
        }

        private void Update_Camera()
        {
            float distancePlayerIsAhead = Player.position.X + Player.center.X - this.ViewPort.X;
            if (distancePlayerIsAhead > this.ViewPort.Width * (3.0 / 5.0))
            {
                UpdateViewport(this.ViewPort.X + distancePlayerIsAhead / 150f);
            }
        }

        private void UpdateViewport(double x)
        {
            this.ViewPort = new Rectangle((int)x, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            levelObjects.Where(i => i.Rectangle.Intersects(this.ViewPort)).ToList().ForEach(i =>
            {
                spriteBatch.Draw(i.sprite, new Vector2(i.position.X - this.ViewPort.X, i.position.Y), null, Color.White, i.rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            });

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
