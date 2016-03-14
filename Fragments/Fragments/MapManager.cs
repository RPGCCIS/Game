using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fragments
{
    class MapManager
    {
        private static MapManager instance;

        public static MapManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapManager();
                }
                return instance;
            }
        }
    }
}
