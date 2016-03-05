using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fragments
{
    public class Tile
    {
        private MovementFlags flags;
        private String texturefilename;

        public MovementFlags Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }

        public String Filename
        {
            get
            {
                return texturefilename;
            }
            set
            {
                texturefilename = value;
            }
        }

        public Tile(MovementFlags flags, String texturefilename)
        {
            Flags = flags;
            Filename = texturefilename;
        }
    }

}