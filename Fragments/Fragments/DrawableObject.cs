using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fragments
{
    abstract class DrawableObject
    {
        private Rectangle rec;
        private Texture2D texture;
        public DrawableObject(int x, int y, int width, int height)
        {
            rec = new Rectangle(x, y, width, height);
        }
        //Properties for variables
        public Rectangle Rec { get { return rec; } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public int X { get { return rec.X; } set { rec.X = value; } }
        public int Y { get { return rec.Y; } set { rec.Y = value; } }
        //Overrideable method for draw if we want to change how things are drawn
        abstract public void Draw();
        
    }
}
