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
        private float movementMulitplier =.6f;
        private float updateable;

        private Texture2D texture;
        private string name;
        private Vector2 pos;
        public Layer(string name)
        {
            this.name = name;
            pos = new Vector2(0, 0);
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public string Name { get { return name; } }
        public float PosX { get { return pos.X; } set { pos.X = value; } }
        public Vector2 Pos { get { return pos; } }
        public float MM
        {
            get { return movementMulitplier; }
            set { movementMulitplier = value; }
        }
    }
}
