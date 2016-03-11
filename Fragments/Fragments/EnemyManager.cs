using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fragments
{
    class EnemyManager
    {
        /*
        //fields
        private static EnemyManager instance;
        private Enemy foe;

        //instance for gamemanager
        public static EnemyManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new EnemyManager();
                }
                return instance;
            }
        }

        //properties
        public Enemy Foe
        {
            get
            {
                return foe;
            }
            set
            {
                foe = value;
            }
        }

        //two instances of this method exist. This one is used at the start of battle to create an enemy 
        public void Update(EnemyType type)
        {
            foe = new Enemy(type, 10, 10, 10, 10);
        }

        //this one updates the enemy in battle, should only be called during the player's turn
        public void Update()
        {
            if (foe.IsAlive())
            {
                if (foe.GetType().Equals(EnemyType.grunt))
                {
                    //grunt level AI
                }
                else if (foe.GetType().Equals(EnemyType.boss))
                {
                    //boss level AI
                }
                else
                {
                    //Final boss level AI
                }
            }
            else
            {
                foe.Death();
            }
        }
        */
    }
    
}
