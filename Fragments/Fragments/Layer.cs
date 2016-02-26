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
        //LAyer Class
        private float movementMulitplier;
        private float updateable;
        private List<DrawableObject> textures;
        //Needs to bring in drawable objects and there coordinates
        public Layer(List<DrawableObject> d)
        {
            textures = d;
        }
    }
}
