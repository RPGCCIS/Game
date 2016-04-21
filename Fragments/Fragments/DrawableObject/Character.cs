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

        /// <summary>
        /// Returns true if alive
        /// </summary>
        /// <returns></returns>
        public void Attack(Character foe)
        {
            foe.Hp = (foe.hp - this.atk);
        }

        //DRAWING FUNCTIONS

<<<<<<< HEAD
        public override void Draw(SpriteBatch spriteBatch)
=======
        public void Draw(SpriteBatch spriteBatch,Color c)
>>>>>>> 4623577893e511f67a735e2486b92107aba17a24
        {
            spriteBatch.Draw(
                texture,
                rec,
                c);
        }
    }
}
