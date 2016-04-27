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
        public Enemy(EnemyType type, int x, int y, int w, int h, Texture2D text) : base(x, y, w, h, text)
        {
            level = type;
        }

        public void Act(Message m)
        {
            if (!IsAlive())
            {
                BattleManager.Instance.State = BattleState.Win;
            }
            else
            {
                int rawAtk = this.Atk - BattleManager.Instance.Player.Def;
                int dealtAtk = Attack(BattleManager.Instance.Player, rawAtk);
                m.Name = "The Enemy jumped at you! It dealt " + dealtAtk + " damage!";
            }
        }
    }
}
