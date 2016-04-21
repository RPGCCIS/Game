using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Fragments
{
    public enum TypeOfObject
    {
        Normal,
        BGL, //Background Left
        BGC, //Background Center
        BGR, //Background Right
        Solid,
        Interactable,
        NPC,
        Gate
    }

    class DrawableObject
    {
        protected Rectangle rec;
        protected Texture2D texture;
        protected TypeOfObject type;

        public DrawableObject(int x, int y, int width, int height, Texture2D texture, 
            TypeOfObject type = TypeOfObject.Normal)
        {
            rec = new Rectangle(x, y, width, height);
            this.texture = texture;
            this.type = type;
        }

        //Properties for variables
        public Rectangle Rec { get { return rec; } }
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public TypeOfObject Type { get { return type; } set { type = value; } }
        public int X { get { return rec.X; } set { rec.X = value; } }
        public int Y { get { return rec.Y; } set { rec.Y = value; } }


        //Overrideable method for draw if we want to change how things are drawn
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
