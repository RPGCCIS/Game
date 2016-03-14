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
        //holds an instance's type
        private EnemyType level;

        //gets the instance's type
        public EnemyType Level
        {
            get { return level; }
            set { level = value; }
        }

        //initial constructor for the enemy class
        public Enemy(EnemyType type, int x, int y, int w, int h) : base(x, y, w, h)
        {
            level = type;
        }
        /*
        //a temporary draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        //what to do when enemy dies
        public void Death()
        {
            //what to do when an enemy is finished.
        }
        */
    }
}
