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
            Transition
        }

        public GameStates GameState;
        public Rectangle ViewPort;
        public static Vector2 vpCoords;
        public string reserveMap;
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
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level-selection");
                        reserveMap = "maps/level-selection";
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 1);
                        door.WinAction = LevelCreators.Peek();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level1--intro");
                        reserveMap = "maps/level1--intro";
                        this.GameState = GameStates.Transition;
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level1");
                        reserveMap = "maps/level1";
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door));
                        door.WinAction = LevelCreators.Peek();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level-selection");
                        LevelObjects
                            .Where(i => i.Designator == 1)
                            .ToList()
                            .ForEach(i => i.Alive = false);
                        reserveMap = "maps/level-selection";
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 2);
                        door.WinAction = LevelCreators.Peek();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level2--intro");
                        reserveMap = "maps/level2--intro";
                        this.GameState = GameStates.Transition;
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level2");
                        reserveMap = "maps/level2";
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door));
                        door.WinAction = LevelCreators.Peek();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level-selection");
                        reserveMap = "maps/level-selection";
                        LevelObjects
                            .Where(i => i.Designator > 0 && i.Designator < 3)
                            .ToList()
                            .ForEach(i => i.Alive = false);
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 3);
                        door.WinAction = LevelCreators.Peek();
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level3--intro");
                        reserveMap = "maps/level3--intro";
                        this.GameState = GameStates.Transition;
                        UpdateViewport(0);
                    });
                    _LevelCreators.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level3");
                        reserveMap = "maps/level3";
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door));
                        door.WinAction = LevelCreators.Peek();
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
            LevelCreators.Dequeue()();
        }

        public void Update(double elapsed)
        {
            UpdateCamera();

            for (int i = 0; i < LevelObjects.Count; i++)
            {
                if (LevelObjects[i] is Door)
                    if (((Door)LevelObjects[i]).IsOpen && Timer.IsTimeYet(this, elapsed, Door.DOOR_DELAY_TIME))
                        LevelCreators.Dequeue();
            }

            Player player = (Player)this.LevelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));

            if (this.GameState == GameStates.Transition && Timer.IsTimeYet(this, elapsed, TRANSITION_DELAY_TIME))
            {
                this.GameState = GameStates.Playing;
                LevelCreators.Dequeue()();
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
                    LevelObjects = MapMaker.ReadMap(reserveMap);
                    Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door));
                    door.WinAction = LevelCreators.Peek();

                    player.Health = Player.MAX_HEALTH;
                    player.Alive = true;
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
                UpdateViewport(this.ViewPort.X + distancePlayerIsAhead / CAMERA_SCROLL_SMOOTHNESS);

                for (int i = 0; i < Player.MAX_HEALTH; i++)
                {
                    player.HealthIcons[i].Position.X = this.ViewPort.X + (32 + 32 * i);
                }
            }
            else if (distancePlayerIsAhead < this.ViewPort.Width * (1.0 / 5.0))
            {
                UpdateViewport(this.ViewPort.X - CAMERA_SCROLL_SMOOTHNESS / distancePlayerIsAhead);

                for (int i = 0; i < Player.MAX_HEALTH; i++)
                {
                    player.HealthIcons[i].Position.X = this.ViewPort.X + (32 + 32 * i);
                }
            }
            vpCoords.X = this.ViewPort.X;
            vpCoords.Y = this.ViewPort.Y;
        }

        private void UpdateViewport(double x)
        {
            this.ViewPort = new Rectangle((int)Math.Max(x, 0), 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
        }
    }
}
