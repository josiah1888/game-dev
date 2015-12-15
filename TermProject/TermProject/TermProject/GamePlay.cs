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
            SplashScreens,
            Exit
        }

        public GameStates GameState;
        public Rectangle ViewPort;
        bool isGameOver = false;

        public List<GameObject> LevelObjects;
        private MapMaker MapMaker;
        private GameWindow Window;
        private Timer Timer = new Timer();
        private Action CurrentLevel;

        private const double TRANSITION_DELAY_TIME = 1800;
        private const double SPLASH_DELAY_TIME = 2500;
        private const float CAMERA_SCROLL_SMOOTHNESS = 120f;

        private Queue<Action> _LevelCreators;
        private Queue<Action> LevelCreators
        {
            get
            {
                if (_LevelCreators == null)
                {
                    _LevelCreators = new Queue<Action>();
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level7");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartSplashScreen("splash-screens/logo");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartSplashScreen("splash-screens/contributors");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartSplashScreen("splash-screens/splash-screen");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartSplashScreen("splash-screens/menu");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level-selection", 1);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/level1--intro");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level1");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level-selection", 2, GetKillLevelSelectionObjects(2));
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/level2--intro");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level2");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level-selection", 3, GetKillLevelSelectionObjects(3));
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/level3--intro");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level3");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level-selection", 4, GetKillLevelSelectionObjects(4));
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/level4--intro");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level4");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level-selection", 5, GetKillLevelSelectionObjects(5));
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/level5--intro");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level5");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level-selection", 6, GetKillLevelSelectionObjects(6));
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/level6--intro");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level6");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level-selection", 7, GetKillLevelSelectionObjects(7));
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/level7--intro");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level7");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level-selection", 8, GetKillLevelSelectionObjects(8));
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/level8--intro");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level8");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level-selection", 9, GetKillLevelSelectionObjects(9));
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/level9--intro");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartLevel("maps/level9");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        StartTransition("maps/you-win");
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        this.GameState = GameStates.Exit;
                    });
                }
                return _LevelCreators;
            }
        }

        private Action GetKillLevelSelectionObjects(int maxDesignator)
        {
            return () => LevelObjects.Where(i => i.Designator > 0 && i.Designator < maxDesignator).ToList().ForEach(i => i.Alive = false);
        }

        private void StartSplashScreen(string splashScreen)
        {
            this.GameState = GameStates.SplashScreens;
            this.LevelObjects = MapMaker.MakeSplashScreen(splashScreen);
            this.LevelCreators.Dequeue();
        }

        private void StartLevel(string map, int doorDesignator = 0, Action levelSetup = null)
        {
            this.LevelCreators.Dequeue();

            this.CurrentLevel = () =>
            {
                this.GameState = GameStates.Playing;
                this.LevelObjects = MapMaker.ReadMap(map);
                Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == doorDesignator);
                door.WinAction = this.LevelCreators.Peek();
                UpdateViewport(0);

                if (levelSetup != null)
                {
                    levelSetup();
                }
            };

            this.CurrentLevel();
        }

        private void StartTransition(string map)
        {
            this.GameState = GameStates.Transition;
            this.LevelObjects = MapMaker.ReadMap(map);
            this.LevelCreators.Dequeue();
            UpdateViewport(0);
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
            switch (this.GameState)
            {
                case GameStates.Playing:
                    Update_Playing(elapsed);
                    break;
                case GameStates.Transition:
                    Update_Transition(elapsed);
                    break;
                case GameStates.SplashScreens:
                    Update_SplashScreens(elapsed);
                    break;
            }
        }

        private void Update_Playing(double elapsed)
        {
            Player player = (Player)this.LevelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));

            if (!player.Alive || isGameOver)
            {
                if (!isGameOver)
                {
                    isGameOver = true;
                    LevelObjects = MapMaker.ReadMap("maps/game-over");
                    UpdateViewport(0);
                }

                if (this.Timer.IsTimeYet(elapsed, TRANSITION_DELAY_TIME))
                {
                    this.CurrentLevel();
                    isGameOver = false;
                }
            }
        }

        private void Update_Transition(double elapsed)
        {
            PlayerLife lifeIcon;

            for (int i = 0; i < Player.MAX_HEALTH; i++)
            {
                lifeIcon = (PlayerLife)this.LevelObjects.FirstOrDefault(j => j.GetType() == typeof(PlayerLife) && ((PlayerLife)j).HealthDesignator == i);
                lifeIcon.Alive = false;
            }

            if (this.Timer.IsTimeYet(elapsed, TRANSITION_DELAY_TIME))
            {
                this.GameState = GameStates.Playing;
                this.LevelCreators.Peek()();
            }
        }

        private void Update_SplashScreens(double elapsed)
        {
            PlayerLife lifeIcon;

            for (int i = 0; i < Player.MAX_HEALTH; i++)
            {
                lifeIcon = (PlayerLife)this.LevelObjects.FirstOrDefault(j => j.GetType() == typeof(PlayerLife) && ((PlayerLife)j).HealthDesignator == i);
                lifeIcon.Alive = false;
            }

            if (this.Timer.IsTimeYet(elapsed, SPLASH_DELAY_TIME))
            {
                this.LevelCreators.Peek()();
            }
        }

        private void UpdateCamera()
        {
            Player player = (Player)this.LevelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));
            PlayerLife lifeIcon;

            float distancePlayerIsAhead = player.Position.X + player.Center.X - this.ViewPort.X;
            if (distancePlayerIsAhead > this.ViewPort.Width * (3.0 / 5.0))
            {
                UpdateViewport(this.ViewPort.X + (distancePlayerIsAhead / CAMERA_SCROLL_SMOOTHNESS));

                for (int i = 0; i < Player.MAX_HEALTH; i++)
                {
                    lifeIcon = (PlayerLife)this.LevelObjects.FirstOrDefault(j => j.GetType() == typeof(PlayerLife) && ((PlayerLife)j).HealthDesignator == i);
                    lifeIcon.Position.X = this.ViewPort.X + (32 + 32 * lifeIcon.HealthDesignator);
                }
            }
            else if (distancePlayerIsAhead < this.ViewPort.Width * (2.0 / 5.0))
            {
                UpdateViewport(this.ViewPort.X - ((this.ViewPort.Width - distancePlayerIsAhead) / CAMERA_SCROLL_SMOOTHNESS));

                for (int i = 0; i < Player.MAX_HEALTH; i++)
                {
                    lifeIcon = (PlayerLife)this.LevelObjects.FirstOrDefault(j => j.GetType() == typeof(PlayerLife) && ((PlayerLife)j).HealthDesignator == i);
                    lifeIcon.Position.X = this.ViewPort.X + (32 + 32 * lifeIcon.HealthDesignator);
                }
            }
        }

        private void UpdateViewport(double x)
        {
            this.ViewPort = new Rectangle((int)Math.Max(x, 0), 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
        }
    }
}
