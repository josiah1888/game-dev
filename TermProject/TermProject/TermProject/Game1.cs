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
        Enemy Bird;

        Enemy.EnemyState idle = Enemy.EnemyState.Idle;
        Enemy.EnemyState attack = Enemy.EnemyState.Attack;
        Enemy.Direction left = Enemy.Direction.Left;
        Enemy.Direction right = Enemy.Direction.Right;

        public Action<Enemy> GetFrogAI()
        {
            return (Enemy frog) =>
            {
                if (frog.DistanceFrom(Player) > Enemy.threshold)
                {
                    frog.setState(idle);
                }

                else
                {
                    frog.setState(attack);
                }    


                if (frog.getState() == Enemy.EnemyState.Idle)
                {
                    if(frog.IsOnGround(levelObjects))
                    {
                        frog.velocity.X = 0;
                        frog.velocity.Y = -10;
                    }

                    if(!frog.IsOnGround(levelObjects))
                    {
                        if (frog.getDirection() == Enemy.Direction.Left)
                            frog.velocity.X = (Enemy.MAX_SPEED / 2) * -1;
                        else if (frog.getDirection() == Enemy.Direction.Right)
                            frog.velocity.X = (Enemy.MAX_SPEED / 2);
                        else
                            frog.velocity.X = 0;
                    }
                }

                else if (frog.getState() == Enemy.EnemyState.Attack)
                {
                    Vector2 closingVelocity = Player.velocity - frog.velocity;
                    Vector2 closingRange = Player.position - frog.position;
                    Vector2 closingTime = new Vector2(Math.Abs(closingVelocity.X) / Math.Abs(closingRange.X), Math.Abs(closingVelocity.Y) / Math.Abs(closingRange.Y));

                    Vector2 frogFuturePosition = Player.position + (Player.velocity * closingTime);
                    float angle = frog.GetAngle(frog.position, Player.position);

                    if (frog.IsOnGround(levelObjects))
                    {
                        frog.velocity.X = 0;
                        frog.velocity.Y = (float)Math.Cos(angle) * 10.0f;
                    }

                    if (!frog.IsOnGround(levelObjects))
                        frog.velocity.X = (float)Math.Sin(angle) * Enemy.MAX_SPEED;
                }
            };
        }

        public Action<Enemy> GetBirdAI()
        {
            return (Enemy bird) =>
            {

                if (bird.DistanceFrom(Player) > Enemy.threshold)
                {
                    bird.setState(idle);
                }

                else
                {
                    bird.setState(attack);
                }
    
                if (bird.getState() == Enemy.EnemyState.Idle)
                {
                    if (bird.getDirection() == Enemy.Direction.Left)
                        bird.velocity.X = (Enemy.MAX_SPEED / 2) * -1;
                    else if (bird.getDirection() == Enemy.Direction.Right)
                        bird.velocity.X = (Enemy.MAX_SPEED / 2);
                    else
                        bird.velocity.X = 0;
                }

                else if (bird.getState() == Enemy.EnemyState.Attack)
                {
                    if (bird.getDirection() == Enemy.Direction.Left)
                        bird.velocity.X = Enemy.MAX_SPEED * -1;
                    else if (bird.getDirection() == Enemy.Direction.Right)
                        bird.velocity.X = Enemy.MAX_SPEED;
                    else
                        bird.velocity.X = 0;
                }

                if (!bird.IsOnGround(levelObjects) && bird.getDirection() == Enemy.Direction.Left)
                {
                    bird.setDirection(right);
                }

                if (!bird.IsOnGround(levelObjects) && bird.getDirection() == Enemy.Direction.Right)
                {
                    bird.setDirection(left);
                }
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

            for (int i = 0; i < levelObjects.Count; i++)
            {
                if (!(levelObjects[i] is Tile))
                    levelObjects[i].obeysGravity = true;
            }

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
            base.Update(gameTime);
        }

        private void Update_Player()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Player.Update(levelObjects, keyboardState.GetPressedKeys());
        }

        private void Update_Positions()
        {
            levelObjects.ForEach(i =>
            {
                i.position.X += i.velocity.X;
                i.position.Y += i.velocity.Y;
            });
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            levelObjects.ForEach(i =>
            {
                spriteBatch.Draw(i.sprite, i.position, null, Color.White, i.rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            });

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
