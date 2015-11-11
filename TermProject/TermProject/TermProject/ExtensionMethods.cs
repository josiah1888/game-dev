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
    }
}
