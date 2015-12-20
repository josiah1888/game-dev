using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public static class ExtensionMethods
    {
        public static Vector2 GetDrawablePosition(this Vector2 position, Rectangle viewport)
        {
            return new Vector2(position.X - viewport.X, position.Y);
        }

        public static int GetLateralDirectionSign(this GameObject.Directions direction)
        {
            int lateralDirection = 0;
            if (direction.HasFlag(GameObject.Directions.Right))
            {
                lateralDirection++;
            }
            if (direction.HasFlag(GameObject.Directions.Left))
            {
                lateralDirection--;
            }
            return lateralDirection;
        }
    }
}
