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
    class TextObject
    {
        //Methods
        private string text;
        private SpriteFont font;
        private Color color;

        private Vector2 position;

        //Properties
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


        //Constructor
        public TextObject(SpriteFont font, string text, Vector2 position)
        {
            this.font = font;
            this.text = text;
            this.position = position;
            color = Color.Black;
        }

        //Drawing
        public void DrawText(SpriteBatch spriteBatch)
        {
            if (text == null)
            {
                text = "";
            }
            spriteBatch.DrawString(
                font,
                text,
                position,
                color
                );
        }

        public void DrawText(SpriteBatch spriteBatch, int offset)
        {
            Vector2 adjustedPosition = position;
            adjustedPosition.X += offset;

            spriteBatch.DrawString(
                font,
                text,
                adjustedPosition,
                color
                );
        }

        public void DrawText(SpriteBatch spriteBatch, Vector2 pos, Color c)
        {
            spriteBatch.DrawString(
                font,
                text,
                pos,
                c
                );
        }
    }
}
