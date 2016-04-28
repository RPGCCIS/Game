using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fragments
{
    class Item
    {
        private string name;
        private int level;
        private int dam;
        private int def;
        private int hp;
        private int sp;
        private int spd;
        private int cost;

        public string Name { get { return name; } }
        public int Dam { get { return dam; } }
        public int Level { get { return level; } }
        public int Def { get { return def; } }
        public int Hp { get { return hp; } }
        public int Sp { get { return sp; } }
        public int Spd { get { return spd; } }
        public int Cost { get { return cost; } }
        public Item(string name, int level, int dam,int def, int hp,int sp,int spd,int cost)
        {
            this.name = name;
            this.level = level;
            this.dam = dam;
            this.def = def;
            this.hp = hp;
            this.sp = sp;
            this.spd = spd;
            this.cost = cost;
        }


    }
}
