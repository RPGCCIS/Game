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

        private float multiplier;

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
        public Layer(string name, float multiplier)
        {
            this.name = name;
            this.multiplier = multiplier;

            layerOffset = 0;

            objects = new List<DrawableObject>();
        }

        public string Name { get { return name; } }

        public float Multiplier
        {
            get { return multiplier; }
        }

        //Specific objects
        public void AddHouse(int position)
        {
            //Get the textures you need
            Texture2D house = GameManager.Instance.CurrentMap.GetTexture("house");
            Texture2D door = GameManager.Instance.CurrentMap.GetTexture("door");

            //Add house
            objects.Add(new DrawableObject(
                position,
                205,
                house.Width,
                house.Height,
                house,
                TypeOfObject.Normal));

            //Add door
            objects.Add(new DrawableObject(
                position + (house.Width / 2) - (door.Width / 2),
                205 + (house.Height) - (door.Height),
                door.Width,
                door.Height,
                door,
                TypeOfObject.Normal));
        }

        public void AddHouse(int position, string destination)
        {
            //Get the textures you need
            Texture2D house = GameManager.Instance.CurrentMap.GetTexture("house");
            Texture2D door = GameManager.Instance.CurrentMap.GetTexture("door");

            //Add house
            objects.Add(new DrawableObject(
                position,
                205,
                house.Width,
                house.Height,
                house, 
                TypeOfObject.Normal));

            //Add door
            objects.Add(new InteractableObject(
                position + (house.Width / 2) - (door.Width / 2),
                205 + (house.Height) - (door.Height),
                door.Width,
                door.Height,
                door,
                destination));
        }

        //General objects
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
