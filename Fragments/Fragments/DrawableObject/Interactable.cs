using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace Fragments
{
    class InteractableObject : DrawableObject
    {
        private string destinationMap;

        //Properties
        public string Destination
        {
            get { return destinationMap; }
        }

        //Constructor
        public InteractableObject(int x, int y, int width, int height, Texture2D texture, string dest)
            : base (x, y, width, height, texture, TypeOfObject.Interactable)
        {
            destinationMap = dest;
        }
    }
}
