using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fragments
{
    /// <summary>
    /// Player and Enemies
    /// </summary>
    class Character : DrawableObject
    {
        //Stats
        private string name;
        private int maxHp;
        private int hp;
        private int maxSp;
        private int sp;
        private int atk;
        private int def;
        private int spd;
        private bool defending = false;

        //Properties for stats
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int MaxHp
        {
            get { return maxHp; }
            set { maxHp = value; }
        }
        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }
        public int MaxSp
        {
            get { return maxSp; }
            set { maxSp = value; }
        }
        public int Sp
        {
            get { return sp; }
            set { sp = value; }
        }
        public int Atk
        {
            get { return atk; }
            set { atk = value; }
        }
        public int Def
        {
            get { return def; }
            set { def = value; }
        }
        public int Spd
        {
            get { return spd; }
            set { spd = value; }
        }

        public bool Defending
        {
            get { return defending; }
            set { defending = value; }
        }

        //Constructor

        public Character(int x, int y, int w, int h, Texture2D texture)
            : base(x, y, w, h, texture)
        {
            //EMPTY
        }

        //BATTLE FUNCTIONS

        /// <summary>
        /// Returns true if alive
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return hp > 0;
        }

        public void Attack(Character foe)
        {
            if (!foe.Defending)
            {
                foe.Hp -= this.atk;
            }
            else
            {
                foe.Hp -= (int)((0.75) * this.atk);
            }
        }

        public void Magic(Character foe)
        {
            foe.Hp = (foe.hp - this.atk);
        }

        //DRAWING FUNCTIONS

        public void Draw(SpriteBatch spriteBatch,Color c)
        {
            spriteBatch.Draw(
                texture,
                rec,
                c);
        }
    }
}
