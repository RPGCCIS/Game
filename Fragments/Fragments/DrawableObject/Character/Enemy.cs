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
        private EnemyType type;

        //gets the instance's type
        public EnemyType Type
        {
            get { return type; }
            set { type = value; }
        }

        //initial constructor for the enemy class
        public Enemy(EnemyType type, int x, int y, int w, int h, Texture2D text) : base(x, y, w, h, text)
        {
            this.type = type;
        }

        public void Act(Message m)
        {
            int rawAtk = this.Atk - BattleManager.Instance.Player.Def;
            if (rawAtk < 0)
            {
                rawAtk = 0;
            }
            int dealtAtk = Attack(BattleManager.Instance.Player, rawAtk);
            m.Name = "The Enemy jumped at you! It dealt " + dealtAtk + " damage!";
        }
    }
}
