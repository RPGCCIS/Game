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
    class House:DrawableObject
    {
        InteractableObject door;
        public House(int x, int y,int width,int height,Texture2D texture, int xOff, int yOff) 
            : base(x,y,width,height,texture)
        {
            door = new InteractableObject(width + xOff, height + yOff, 100, 200,null,"test1");
        }
    }
}
