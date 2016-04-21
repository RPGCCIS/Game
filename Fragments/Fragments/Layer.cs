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
        private List<TextObject> txtObjects;

        private float layerOffset;
        private string name;

        //House constants
        private const int houseHeight = 205;
        private const int signHeight = 100;
        private const int gateHeight = 210;
        private const int npcHeight = 565;

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
            txtObjects = new List<TextObject>();
        }

        public string Name { get { return name; } }

        public float Multiplier
        {
            get { return multiplier; }
        }

        //Specific objects
        //A house that is only a visual
        public void AddHouse(int position, bool hasSign = false, string signTxt = null)
        {
            //Get the textures you need
            Texture2D house = GameManager.Instance.CurrentMap.GetTexture("house");
            Texture2D door = GameManager.Instance.CurrentMap.GetTexture("door");
            Texture2D sign = GameManager.Instance.CurrentMap.GetTexture("sign");

            //Add house
            objects.Add(new DrawableObject(
                position,
                houseHeight,
                house.Width,
                house.Height,
                house,
                TypeOfObject.Normal));

            //Add door
            objects.Add(new DrawableObject(
                position + (house.Width / 2) - (door.Width / 2),
                houseHeight + (house.Height) - (door.Height),
                door.Width,
                door.Height,
                door,
                TypeOfObject.Normal));

            //Add sign
            if (hasSign)
            {
                //Sign
                objects.Add(new DrawableObject(
                    position + (house.Width / 2) - (sign.Width / 2),
                    houseHeight + signHeight,
                    sign.Width,
                    sign.Height,
                    sign,
                    TypeOfObject.Normal));

                //Text
                Vector2 txtPosition = new Vector2();
                SpriteFont txtFont = GameManager.Instance.CurrentMap.GetFont("Georgia_32");
                txtPosition.X = position + (house.Width / 2) - (txtFont.MeasureString(signTxt).X / 2);
                txtPosition.Y = houseHeight + signHeight + (sign.Height / 2) - (txtFont.MeasureString(signTxt).Y / 2);

                txtObjects.Add(new TextObject(
                    txtFont,
                    signTxt,
                    txtPosition));
            }
        }

        //A house that leads somewhere
        public void AddHouse(int position, string destination, bool hasSign = false, string signTxt = null)
        {
            //Get the textures you need
            Texture2D house = GameManager.Instance.CurrentMap.GetTexture("house");
            Texture2D door = GameManager.Instance.CurrentMap.GetTexture("door");
            Texture2D sign = GameManager.Instance.CurrentMap.GetTexture("sign");

            //Add house
            objects.Add(new DrawableObject(
                position,
                houseHeight,
                house.Width,
                house.Height,
                house, 
                TypeOfObject.Normal));

            //Add door
            objects.Add(new InteractableObject(
                position + (house.Width / 2) - (door.Width / 2),
                houseHeight + (house.Height) - (door.Height),
                door.Width,
                door.Height,
                door,
                destination));

            //Add sign
            if (hasSign)
            {
                //Sign
                objects.Add(new DrawableObject(
                    position + (house.Width / 2) - (sign.Width / 2),
                    houseHeight + signHeight,
                    sign.Width,
                    sign.Height,
                    sign,
                    TypeOfObject.Normal));

                //Text
                Vector2 txtPosition = new Vector2();
                SpriteFont txtFont = GameManager.Instance.CurrentMap.GetFont("Georgia_32");
                txtPosition.X = position + (house.Width / 2) - (txtFont.MeasureString(signTxt).X / 2);
                txtPosition.Y = houseHeight + signHeight + (sign.Height / 2) - (txtFont.MeasureString(signTxt).Y / 2);

                txtObjects.Add(new TextObject(
                    txtFont,
                    signTxt,
                    txtPosition));
            }
        }

        //Gate
        public void AddGate(int position)
        {
            Texture2D gate = GameManager.Instance.CurrentMap.GetTexture("gate");

            objects.Add(new DrawableObject(
                position,
                gateHeight,
                gate.Width,
                gate.Height,
                gate,
                TypeOfObject.Gate));
        }

        //NPC
        public void AddNPC(int position, ConversationTree dialogue)
        {
            Texture2D npc = GameManager.Instance.CurrentMap.GetTexture("npc");

            objects.Add(new NPC(
                position,
                npcHeight,
                (int)(GameManager.Instance.Player.PlayerWidth * 1.5),
                (int)(GameManager.Instance.Player.PlayerHeight * 1.2),
                npc));
           if(objects[objects.Count-1].Type == TypeOfObject.NPC)
            {
                NPC n = (NPC)(objects[objects.Count - 1]);
                n.Conversation = dialogue;
            }
        }

        //General objects
        public void AddObject(DrawableObject obj)
        {
            objects.Add(obj);
        }

        public void AddObject(int index, DrawableObject obj)
        {
            objects.Insert(index, obj);
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
        public void Draw(SpriteBatch s, Color col)
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
                    col);
            }

            DrawTxt(s);
        }

        public void DrawTxt(SpriteBatch s)
        {
            //Key = texture, value = position
            foreach (TextObject txt in txtObjects)
            {
                txt.DrawText(s, (int)layerOffset);
            }
        }

        public void Clear()
        {
            objects.Clear();
        }
    }
}
