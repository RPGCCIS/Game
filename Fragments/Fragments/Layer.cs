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
        private float movementMulitplier =10.6f;
        private float updateable;

        private List<KeyValuePair<Texture2D, Vector2>> objects;
        private float layerOffset;
        private string name;

        //Properties
        public float X
        {
            get { return layerOffset; }
            set { layerOffset = value; }
        }

        public List<KeyValuePair<Texture2D, Vector2>> Objects
        {
            get { return objects; }
        }

        //Constructor
        public Layer(string name)
        {
            this.name = name;
            layerOffset = 0;
            //pos = new Vector2(0, 0); // Initial anchor point

            objects = new List<KeyValuePair<Texture2D, Vector2>>();
        }

    public string Name { get { return name; } }
        //public float PosX { get { return pos.X; } set { pos.X = value; } }
        //public Vector2 Pos { get { return pos; } }
        public float MM
        {
            get { return movementMulitplier; }
            set { movementMulitplier = value; }
        }

        public void AddObject (Texture2D text, Vector2 pos)
        {
            objects.Add(new KeyValuePair<Texture2D, Vector2>(text, pos));
        }

        //Drawing
        public void Draw(SpriteBatch s)
        {
            //Key = texture, value = position
            foreach(KeyValuePair<Texture2D, Vector2> t in objects)
            {
                s.Draw(t.Key, t.Value + new Vector2(layerOffset, 0), Color.White);
            }
        }
        public void Clear()
        {
            objects.Clear();
        }
    }
}
