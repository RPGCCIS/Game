using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fragments
{
    class Enemy : Character
    {
        //enum that defines three different enemy levels
        public enum Type
        {
            grunt,
            boss,
            final
        }

        //holds an instance's type
        private Type level;

        //gets the instance's type
        public Type Level
        {
            get { return level; }
            set { level = value; }
        }

        //initial constructor for the enemy class
        public Enemy(Type type, int x, int y, int w, int h) : base(x, y, w, h)
        {
            level = type;
        }

        //a temporary draw method
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
