using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fragments
{
    class NPC : DrawableObject
    {
        private ConversationTree conversation;

        //Property
        public ConversationTree Conversation
        {
            get { return conversation; }
        }

        //Constructor
        public NPC(int x, int y, int width, int height, Texture2D texture)
            : base(x, y, width, height, texture, TypeOfObject.NPC)
        {
            //EMPTY
        }

        //Methods
    }
}
