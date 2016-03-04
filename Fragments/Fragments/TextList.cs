using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Fragments
{
    class TextList
    {
        private List<TextObject> textObjects;
        private SpriteFont font;
        private Vector2 position;
        private Color defaultColor;
        private Color selectedColor;

        private float lineHeight;
        private const float spacing = 20;

        private int selected;

        //Properties
        public SpriteFont Font
        {
            set
            {
                font = value;
                lineHeight = font.MeasureString("Test").Y;
            }
        }
        public int Selected
        {
            get { return selected; }
        }

        //Constructor
        public TextList(SpriteFont font, Vector2 position)
        {
            textObjects = new List<TextObject>();
            this.font = font;
            this.position = position;
            defaultColor = Color.Black;
            selectedColor = Color.Blue;

            selected = 0;
        }

        public TextList(SpriteFont font, Vector2 position, Color color)
        {
            textObjects = new List<TextObject>();
            this.font = font;
            this.position = position;

            defaultColor = color;
            selectedColor = Color.Blue;

            selected = 0;
        }

        //Methods
        public void Add(string text)
        {
            textObjects.Add(new TextObject(font, text, position));
        }

        public void Next()
        {
            selected++;
            if (selected >= textObjects.Count)
            {
                selected -= textObjects.Count;
            }
        }

        public void Previous()
        {
            selected--;
            if (selected < 0)
            {
                selected += textObjects.Count;
            }
        }

        //Drawing
        public void DrawText(SpriteBatch spriteBatch)
        {
            Color drawingColor = defaultColor;
            for (int i = 0; i < textObjects.Count; i++)
            {
                if(i == selected)
                {
                    drawingColor = selectedColor;
                }
                else
                {
                    drawingColor = defaultColor;
                }

                textObjects[i].DrawText(spriteBatch,
                    new Vector2(position.X, position.Y + (i * (lineHeight + spacing))),
                    drawingColor);
            }
        }
    }
}
