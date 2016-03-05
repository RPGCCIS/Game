using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Fragments
{
    class Layer
    {
        //Layer Class

        //Currently nt implemented
        private float movementMulitplier;
        private float updateable;

        private Texture2D texture;
        private string name;

        public Layer(string name)
        {
            this.name = name;
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public string Name { get { return name; } }
    }
}
