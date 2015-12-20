using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class PlayerLife : GameObject
    {
        public ContentManager Content;
        private Player Player;
        public int HealthDesignator;

        public override List<GameObject> LevelObjects
        {
            get
            {
                return this.Player.LevelObjects;
            }
        }

        public PlayerLife(Player player, int healthDesignator)
            : base(player.HealthIconSprite)
        {
            this.HealthDesignator = healthDesignator;
            this.Position = new Vector2(32 + 32 * HealthDesignator, 32);
            this.Player = player;
            this.Alive = true;
        }

        public void UpdateHealthIcons()
        {
            if (Player.Health <= HealthDesignator)
                this.Alive = false;
        }
    }
}
