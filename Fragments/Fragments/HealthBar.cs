using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fragments
{
    class HealthBar
    {
        #region Fields and Properties
        //fields
        private int totalHealth;
        private int health;
        private Rectangle max;
        private Rectangle cur;
        private TextObject textObj;

        //properties
        public int TotalHealth
        {
            set
            {
                totalHealth = value;
                health = value;
                textObj.Text = health + "/" + totalHealth;
            }
        }

        public int Health
        {
            set
            {
                health = value;
                cur.Width = (health / totalHealth) * max.Width;
                textObj.Text = health + "/" + totalHealth;
            }
        }
        #endregion

        #region Constructor and Methods
        public HealthBar(int hp, int maxhp, int xpos, int ypos, SpriteFont font)
        {
            max = new Rectangle(xpos, ypos, 150, 30);
            cur = new Rectangle(xpos, ypos, 150, 30);
            totalHealth = maxhp;
            health = hp;
            textObj = new TextObject(font, hp + "/" + maxhp, new Vector2(xpos, ypos + 35));
        }

        public void Draw(SpriteBatch sb, Texture2D text)
        {
            sb.Draw(text, max, Color.Red);
            sb.Draw(text, cur, Color.Green);
        }
        #endregion
    }
}
