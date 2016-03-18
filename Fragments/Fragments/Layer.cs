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

        private List<DrawableObject> objects;
        private float layerOffset;
        private string name;

        //Properties
        public float X
        {
            get { return layerOffset; }
            set { layerOffset = value; }
        }

        public List<DrawableObject> Objects
        {
            get { return objects; }
        }

        //Constructor
        public Layer(string name)
        {
            this.name = name;
            layerOffset = 0;
            //pos = new Vector2(0, 0); // Initial anchor point

            objects = new List<DrawableObject>();
        }

        public string Name { get { return name; } }

        public float MM
        {
            get { return movementMulitplier; }
            set { movementMulitplier = value; }
        }

        public void AddObject (Texture2D text, Vector2 pos, TypeOfObject type = TypeOfObject.Normal)
        {
            objects.Add(new DrawableObject(
                (int)pos.X, 
                (int)pos.Y, 
                text.Width, 
                text.Height, 
                text, type));
        }

        public void AddObject(Texture2D text, Vector2 pos, string destination)
        {
            objects.Add(new InteractableObject(
                (int)pos.X,
                (int)pos.Y,
                text.Width,
                text.Height,
                text, destination));
        }

        public void AddObject(Texture2D text, Vector2 pos, Vector2 dimensions, TypeOfObject type = TypeOfObject.Normal)
        {
            objects.Add(new DrawableObject(
                (int)pos.X,
                (int)pos.Y,
                (int)dimensions.X,
                (int)dimensions.Y,
                text, type));
        }

        public void AddObject(Texture2D text, Vector2 pos, Vector2 dimensions, string dest)
        {
            objects.Add(new InteractableObject(
                (int)pos.X,
                (int)pos.Y,
                (int)dimensions.X,
                (int)dimensions.Y,
                text, dest));
        }

        //Drawing
        public void Draw(SpriteBatch s)
        {
            //Key = texture, value = position
            foreach(DrawableObject obj in objects)
            {
                s.Draw(
                    obj.Texture, 
                    new Rectangle(
                        obj.Rec.Location.X + (int)layerOffset,
                        obj.Rec.Location.Y,
                        obj.Rec.Width,
                        obj.Rec.Height), 
                    Color.White);
            }
        }
        public void Clear()
        {
            objects.Clear();
        }
    }
}
