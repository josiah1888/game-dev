using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class GamePlay
    {
        public enum GameStates
        {
            Playing,
            Transition,
            SplashScreens
        }

        public GameStates GameState;
        public Rectangle ViewPort;
        bool isGameOver = false;

        public List<GameObject> LevelObjects;
        private MapMaker MapMaker;
        private GameWindow Window;

        private const double TRANSITION_DELAY_TIME = 1800;
        private const float CAMERA_SCROLL_SMOOTHNESS = 120f;

        private Queue<Action> _LevelCreators;
        private Queue<Action> LevelCreators
        {
            get
            {
                if (_LevelCreators == null)
                {
                    _LevelCreators = new Queue<Action>();
                    //_LevelCreators.Enqueue(() =>
                    //{
                    //    // todo: add timer event for GameStates.SplashScreen

                    //    LevelObjects = MapMaker.MakeSplashScreen("splash-screens/logo");
                    //    /*
                    //     * includes team name/logo, and team member names(Game designers and model designers)
                    //    */
                    //});
                    //_LevelCreators.Enqueue(() =>
                    //{
                    //    LevelObjects = MapMaker.MakeSplashScreen("splash-screens/contributors");
                    //    /*
                    //     * includes pictures of each team member and his/her contributions such as team leader, game designer, model designer, etc
                    //    */
                    //});
                    //_LevelCreators.Enqueue(() =>
                    //{
                    //    LevelObjects = MapMaker.MakeSplashScreen("splash-screens/splash-screen");
                    //    /*
                    //     * includes a title screen for the game and audio introduction for the game background
                    //    */
                    //});
                    //_LevelCreators.Enqueue(() =>
                    //{
                    //    LevelObjects = MapMaker.MakeSplashScreen("splash-screens/menu");
                    //    /*
                    //     * includes a menu for how to play the game
                    //    */
                    //});
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level-selection");
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 1);
                        door.WinAction = LevelCreators.Skip(1).First();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelCreators.Dequeue();
                        LevelObjects = MapMaker.ReadMap("maps/level1--intro");
                        this.GameState = GameStates.Transition;
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelCreators.Dequeue();
                        LevelObjects = MapMaker.ReadMap("maps/level1");
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door));
                        door.WinAction = LevelCreators.Skip(1).First();
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelCreators.Dequeue();
                        LevelObjects = MapMaker.ReadMap("maps/level-selection");
                        LevelObjects
                            .Where(i => i.Designator == 1)
                            .ToList()
                            .ForEach(i => i.Alive = false);
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 2);
                        door.WinAction = LevelCreators.Skip(1).First();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelCreators.Dequeue();
                        LevelObjects = MapMaker.ReadMap("maps/level2--intro");
                        this.GameState = GameStates.Transition;
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelCreators.Dequeue();
                        LevelObjects = MapMaker.ReadMap("maps/level2");
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door));
                        door.WinAction = LevelCreators.Skip(1).First();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelCreators.Dequeue();
                        LevelObjects = MapMaker.ReadMap("maps/level-selection");
                        LevelObjects
                            .Where(i => i.Designator > 0 && i.Designator < 3)
                            .ToList()
                            .ForEach(i => i.Alive = false);
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 3);
                        door.WinAction = LevelCreators.Skip(1).First();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelCreators.Dequeue();
                        LevelObjects = MapMaker.ReadMap("maps/level3--intro");
                        this.GameState = GameStates.Transition;
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelCreators.Dequeue();
                        LevelObjects = MapMaker.ReadMap("maps/level3");
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door));
                        door.WinAction = LevelCreators.Skip(1).First();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {

                    });
                }
                return _LevelCreators;
            }
        }

        public GamePlay(MapMaker mapMaker, GameWindow window)
        {
            this.MapMaker = mapMaker;
            this.LevelObjects = new List<GameObject>();
            this.Window = window;
            LevelCreators.Peek()();
        }

        public void Update(double elapsed)
        {
            UpdateCamera();

            Player player = (Player)this.LevelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));

            if (this.GameState == GameStates.Transition && Timer.IsTimeYet(this, elapsed, TRANSITION_DELAY_TIME))
            {
                this.GameState = GameStates.Playing;
                LevelCreators.Skip(1).First()();
            }

            if (!player.Alive || isGameOver)
            {
                if (!isGameOver)
                {
                    isGameOver = true;
                    LevelObjects = MapMaker.ReadMap("maps/game-over");
                    UpdateViewport(0);
                }

                if (Timer.IsTimeYet(this, elapsed, TRANSITION_DELAY_TIME))
                {
                    LevelCreators.Peek()();
                    isGameOver = false;
                }
            }
        }

        private void UpdateCamera()
        {
            Player player = (Player)this.LevelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));

            float distancePlayerIsAhead = player.Position.X + player.Center.X - this.ViewPort.X;
            if (distancePlayerIsAhead > this.ViewPort.Width * (3.0 / 5.0))
            {
                UpdateViewport(this.ViewPort.X + (distancePlayerIsAhead / CAMERA_SCROLL_SMOOTHNESS));

                for (int i = 0; i < Player.MAX_HEALTH; i++)
                {
                    // todo refactor into PlayerLife class
                    player.HealthIcons[i].Position.X = this.ViewPort.X + (32 + 32 * i);
                }
            }
            else if (distancePlayerIsAhead < this.ViewPort.Width * (2.0 / 5.0))
            {
                UpdateViewport(this.ViewPort.X - ((this.ViewPort.Width - distancePlayerIsAhead) / CAMERA_SCROLL_SMOOTHNESS));

                for (int i = 0; i < Player.MAX_HEALTH; i++)
                {
                    // todo refactor into PlayerLife class
                    player.HealthIcons[i].Position.X = this.ViewPort.X + (32 + 32 * i);
                }
            }
        }

        private void UpdateViewport(double x)
        {
            this.ViewPort = new Rectangle((int)Math.Max(x, 0), 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
        }
    }
}
