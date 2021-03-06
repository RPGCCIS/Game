﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace Fragments
{
    class Message
    {
        private Texture2D texture;
        private TextList dialog;
        private TextObject message;
        private Rectangle rect;
        private string textureName;
        public TextList Options
        {
            get { return dialog; }
            set
            {
                dialog = value;
            }
        }
        public TextObject Mess
        {
            get { return message; }
            set
            {
                message = value;
            }
        }
        public Message(string textureName, bool battle)
        {
            this.textureName = textureName;
            if (battle)
            {
                rect = new Rectangle(0, 450, 900, 300);

            }
            else
            {
                rect = new Rectangle(0, 0, 900, 200);
            }
        }
        public Message(TextObject message, TextList dialog,Texture2D texture,Rectangle rect)
        {
            this.dialog = dialog;
            this.message = message;
            this.texture = texture;
            this.rect = rect;
            
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rect, Color.White);
            message.DrawText(sb);
            dialog.DrawText(sb);
        }
        public float RectX { get { return rect.X; } }
        public float RectY { get { return rect.Y; } }
        public Rectangle Rect { get { return rect; }
            set { rect = value; }}
        public TextList Dialogue
        {
            get { return dialog; }
            set { dialog = value; }
        }
        public string Name
        {
            get { return message.Text; }
            set { message.Text = value; }
        }
        public string TextureName { get { return textureName; } }
    }
}
