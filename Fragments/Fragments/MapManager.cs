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
    class MapManager
    {
        private static MapManager instance;

        private ContentManager content;
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, SpriteFont> fonts;

        public static MapManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapManager();
                }
                return instance;
            }
        }

        public ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }

        public Dictionary<string, SpriteFont> Fonts
        {
            get { return fonts; }
        }

        public Dictionary<string, Texture2D> Textures
        {
            get { return textures; }
        }

        //Constructor
        public MapManager()
        {
            textures = new Dictionary<string, Texture2D>();

            //Fonts
            fonts = new Dictionary<string, SpriteFont>();
        }

        // Loads the map as a whole
        // i.e. a town
        public void LoadMap(string m)
        {
            switch (m)
            {
                case "test":
                    LoadMapFromFile("test");
                    //GameManager.Instance.CurrentMap = new Map("test");
                    //Draw everything

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddGate(100);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        800,
                        "Inn",
                        true,
                        "Inn");

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        1500,
                        "Shop",
                        true,
                        "Shop");

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        2200,
                        true,
                        "Fight Club");
                    

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-2000, 0), 
                         TypeOfObject.Solid);

                    //ADD NPCS

                    //------Gatekeeper
                    ConversationTree gateKeeperDialogue = new ConversationTree("You can use these gates to reach the road.", 
                        new String[] { "Continue" }, "Gate Keeper");

                    //---Root
                    //Continue
                    gateKeeperDialogue.AddCapNode("A", "Oh, it's locked? Let me get that for you... \n" 
                        + "[The gate keeper unlocks the gate]", new String[] { "Continue..." }); //End

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(150, gateKeeperDialogue, "gatekeeper", 1.2,1.2);

                    //------Shopkeeper
                    ConversationTree shopKeeperDialogue = new ConversationTree("Buy something, will ya?", new String[] {"Sure","Nah"}, "Shop Keeper");
                    
                    //---Root
                    //Sure
                    shopKeeperDialogue.AddNode("Good choice, what do you need?", new String[] { "Items", "Gear" });
                    //Nah
                    shopKeeperDialogue.AddNode("Are ya sure?", new String[] { "Yes", "No" });

                    //---Sure
                    //Items
                    shopKeeperDialogue.AddCapNode("B", "I don't currently have any items", new int[] { 0 }, new String[] { "Ok" }); // End
                    //Gear
                    shopKeeperDialogue.AddCapNode("C", "I'll make you tough!", new int[] { 0 }, new String[] { "Enter" }); //End

                    //---Nah
                    //Yes
                    shopKeeperDialogue.AddCapNode("D", "Bye now!", new int[] { 1 }, new String[] { "Ok" }); // End
                    //No
                    shopKeeperDialogue.AddCapNode("E", "Well, what do you want?", new int[] { 1 }, new String[] { "Let me see your wares" });

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(1500, shopKeeperDialogue,"shopkeeper", 1.4,1.2);

                    ConversationTree elderDialogue = new ConversationTree("Oh... your awake", new String[] { "..." }, "Elder");
                    elderDialogue.AddNode("Well anyway... its nice to see that you are up and about!", new String[] { "Where am I?" });
                    elderDialogue.AddNode("You're in ________, we found you blacked out on the city limits.",new int[] { 0 }, new String[] { "I can't remeber a thing" });
                    elderDialogue.AddNode("Well... all we know about you is that you were found \n with this helmet fragment...   [Recieves helmet fragment].", new int[] { 0,0 }, new String[] { "What should I do with this?" });
                    elderDialogue.AddNode("I assume you would want to find the rest of the fragemnts, \n it won't hurt to check nearby towns", new int[] { 0, 0, 0 }, new String[] { "Okay, thanks for the help." });
                    elderDialogue.AddCapNode("A", "You can come back anytime.", new int[] { 0, 0,0,0 }, new String[] { "Leave" });
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(750, elderDialogue,"elder",1.2,1.2);

                    break;

                case "test1":
                    LoadMapFromFile("test1");
                   
                    //GameManager.Instance.CurrentMap = new Map("test2");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-4000, 0),
                         TypeOfObject.Solid);

                    //GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                    //    300,
                    //    "test");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        800,
                        "Inn",
                        true,
                        "Inn");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddGate(0);
                    break;
                case "test2":
                    LoadMapFromFile("test2");

                    //GameManager.Instance.CurrentMap = new Map("test2");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-4000, 0),
                         TypeOfObject.Solid);

                    //GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                    //    300,
                    //    "test");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        800,
                        "Inn",
                        true,
                        "Inn");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddGate(0);
                    break;
            }

            GameManager.Instance.State = GameManager.GameState.Town;
        }

        // Loads just the map data
        // that exists in the .map file
        public void LoadMapFromFile(string file)
        {
            if (GameManager.Instance.CurrentMap != null)
            {
                GameManager.Instance.CurrentMap.ClearMap();
                GameManager.Instance.CurrentMap = new Map(file);
                GameManager.Instance.CurrentMap.Load(file);
                
            }

            else
            {
                GameManager.Instance.CurrentMap = new Map(file);
                GameManager.Instance.CurrentMap.Load(file);
            }

            foreach (Layer l in GameManager.Instance.CurrentMap.Layers)
            {
                l.AddObject(
                    GameManager.Instance.CurrentMap.GetTexture(l.Name), 
                    new Vector2(0));
            }

            GameManager.Instance.CurrentMap.ParallaxLayer.Clear();
        }

        public void ClearMap()
        {
            GameManager.Instance.CurrentMap.ClearMap();
        }
        
    }
}
