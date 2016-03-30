using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fragments
{
    class BattleManager
    {
        //instance of battle manager
        private static BattleManager instance;

        //creates the battle manager
        public static BattleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BattleManager();
                }
                return instance;
            }
        }

        //fields holding the player and an enemy
        Player p;
        Enemy e;

        //whose turn it is
        private Turn turn;

        //properties to get or set the above variables
        public Player Player
        {
            get { return p; }
            set { p = value; }
        }

        public Enemy Enemy
        {
            get { return e; }
            set { e = value; }
        }

        //enum for whose turn it is in battle
        private enum Turn
        {
            self,
            foe,
            victory,
        }

        public void Update()
        {
            //decides who goes first based on speed
            if(p.Spd > e.Spd)
            {
                turn = Turn.self;
            }
            else
            {
                turn = Turn.foe;
            }
            //alternates turns
            switch (turn)
            {
                case Turn.self:
                    //take player input and react accordingly
                    break;
                case Turn.foe:
                    EnemyManager.Instance.Update();
                    break;
                case Turn.victory:
                    //some type of victory screen
                    break;
            }
        }
    }
}
