using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Fragments
{
    class Timer
    {
        private double counter;
        private double max;

        //Constructor
        public Timer(double max)
        {
            counter = 0;
            this.max = max;
        }

        //Methods
        //Returns true if finished
        public bool Update (GameTime gameTime)
        {
            counter += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (counter > max)
            {
                counter = 0;
                return true;
            }

            return false;
        }
    }
}
