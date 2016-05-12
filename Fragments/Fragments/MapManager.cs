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
        private ConversationTree shopKeeperDialogue;
        private Texture2D boatTexture;
        public Texture2D boat { set { boatTexture = value; } }
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

            //------Shopkeeper
            shopKeeperDialogue = new ConversationTree("Buy something, will ya?", new String[] { "Sure", "Nah" }, "Shop Keeper");

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
        }

        // Loads the map as a whole
        // i.e. a town
        public void LoadMap(string m)
        {
            switch (m)
            {
                case "airedale":
                    LoadMapFromFile("airedale");
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


                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-2000, 0), 
                         TypeOfObject.Solid);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                        GameManager.Instance.CurrentMap.GetTexture("wall"),
                        new Vector2(3500, 0),
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

                    

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(1500, shopKeeperDialogue,"shopkeeper", 1.4,1.2);

                    ConversationTree elderDialogue = new ConversationTree("Oh... your awake", new String[] { "..." }, "Elder");
                    elderDialogue.AddNode("Well anyway... its nice to see that you are up and about!", new String[] { "Where am I?" });
                    elderDialogue.AddNode("You're in Airedale, we found you blacked out on the city limits.",new int[] { 0 }, new String[] { "I can't remeber a thing" });
                    elderDialogue.AddNode("Well... all we know about you is that you were found \n with this helmet fragment...   [Recieves helmet fragment].", new int[] { 0,0 }, new String[] { "What should I do with this?" });
                    elderDialogue.AddNode("I assume you would want to find the rest of the fragemnts, \n it won't hurt to check nearby towns", new int[] { 0, 0, 0 }, new String[] { "Okay, thanks for the help." });
                    elderDialogue.AddCapNode("A", "You can come back anytime.", new int[] { 0, 0,0,0 }, new String[] { "Leave" });
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(750, elderDialogue,"elder",1.2,1.2);

                    break;

                case "tardide":
                    LoadMapFromFile("tardide");
                   
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-4000, 0),
                         TypeOfObject.Solid);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(1500, 0),
                         TypeOfObject.Solid);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        800,
                        "Inn",
                        true,
                        "Inn");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                       -800,
                        "Shop",
                        true,
                        "Shop");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                       -1500,
                        "Champion's House1",
                        true,
                        "Champion's \n      House");


                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(-800, shopKeeperDialogue, "shopkeeper", 1.4, 1.2);
                    ConversationTree elderDialogue2 = new ConversationTree("Ah.. you've finally arrived to our town of Tardide.", new String[] { "I was told of a helmet fragment." }, "Elder2");
                    elderDialogue2.AddNode("Ahh of course, the champion of our town holds it.", new String[] { "Where is he?" });
                    elderDialogue2.AddNode("He's is at his house near the edge of town", new int[] { 0 }, new String[] { "Thank you I will head there now." });                   
                    elderDialogue2.AddCapNode("A", "Careful he is very strong...", new int[] { 0, 0 }, new String[] { "Thanks again" });
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(700, elderDialogue2, "elder", 1.2, 1.2);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddGate(0);
                    break;
                case "kineallen":
                    LoadMapFromFile("kineallen");

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-4500, 0),
                         TypeOfObject.Normal);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("boat"),
                         new Vector2(-4150, 400),
                         TypeOfObject.Normal);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-6500, 0),
                         TypeOfObject.Solid);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(800, 0),
                         TypeOfObject.Solid);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        -800,
                        "Inn",
                        true,
                        "Inn");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                       -1500,
                        "Shop",
                        true,
                        "Shop");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                       -2200,
                        "Champion's House2",
                        true,
                        "Champion's \n      House");

                    ConversationTree chrisDialogue = new ConversationTree("Oh.... you found me. Hi my name is Chris.", new String[] { "What are you doing out here?" }, "Chris");
                    chrisDialogue.AddNode("I'm what you would call an easter egg!", new String[] { "Huh?" });
                    chrisDialogue.AddNode("Ah it doesn't matter, do you like boats?", new int[] { 0 }, new String[] { "Uh... no." });
                    chrisDialogue.AddCapNode("A", "Oh!... Well I like boats!", new int[] { 0,0 }, new String[] { "Ok... goodbye." });
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(-3950, chrisDialogue);


                    ConversationTree elderDialogue3 = new ConversationTree("Welcome to Kineallen.", new String[] { "Do you know of a helmet fragment?" }, "Elder3");
                    elderDialogue3.AddNode("Ahh of course, the champion of our town holds it.", new String[] { "Where is he?" });
                    elderDialogue3.AddNode("He's is at his house.", new int[] { 0 }, new String[] { "I will head there now." });
                    elderDialogue3.AddCapNode("A", "Good luck with your battle.", new int[] { 0, 0 }, new String[] { "Thanks." });
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(700, elderDialogue3, "elder", 1.2, 1.2);


                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(-1500, shopKeeperDialogue, "shopkeeper", 1.4, 1.2);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddGate(0);
                    break;
                case "whitebridge":
                    LoadMapFromFile("whitebridge");

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-4000, 0),
                         TypeOfObject.Solid);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(1500, 0),
                         TypeOfObject.Solid);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        800,
                        "Inn",
                        true,
                        "Inn");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                       -800,
                        "Shop",
                        true,
                        "Shop");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                       -1500,
                        "Champion's House3",
                        true,
                        "Champion's \n      House");

                    ConversationTree elderDialogue4 = new ConversationTree("Welcome to Whitebridge.", new String[] { "I have come for a hemet fragment" }, "Elder4");
                    elderDialogue4.AddNode("Of course, I do believe you know where it will be.", new String[] { "The champion's house?" });
                    elderDialogue4.AddCapNode("A", "Correct, good luck with your battle.", new int[] { 0 }, new String[] { "Thanks." });
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(700, elderDialogue4, "elder", 1.2, 1.2);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(-800, shopKeeperDialogue, "shopkeeper", 1.4, 1.2);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddGate(0);
                    break;
                case "ironhaven":
                    LoadMapFromFile("ironhaven");

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-3000, 0),
                         TypeOfObject.Solid);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(2200, 0),
                         TypeOfObject.Solid);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        -800,
                        "Inn",
                        true,
                        "Inn");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        1500,
                        "Shop",
                        true,
                        "Shop");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        800,
                        "Champion's House4",
                        true,
                        "Champion's \n      House");
                    ConversationTree elderDialogue5 = new ConversationTree("Welcome to Ironhaven.", new String[] { "I have come for the final helmet fragment." }, "Elder5");
                    elderDialogue5.AddNode("You have come a long way since you lost your memory.", new String[] { "Yes, but I need to remeber who I am." });
                    elderDialogue5.AddCapNode("A", "Remembering isn't always necessary to know who you truly are.", new int[] { 0 }, new String[] { "..." });
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(700, elderDialogue5, "elder", 1.2, 1.2);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(1500, shopKeeperDialogue, "shopkeeper", 1.4, 1.2);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddGate(0);
                    break;
                case "solaris":
                    LoadMapFromFile("solaris");

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-4000, 0),
                         TypeOfObject.Solid);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(1500, 0),
                         TypeOfObject.Solid);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        800,
                        "Inn",
                        true,
                        "Inn");
                    //GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                    //    -800,
                    //    "Evil Mansion",
                    //    true,
                    //    "Evil Mansion");
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                       -1500,
                        "Shop",
                        true,
                        "Shop");

                    ConversationTree emperorDialogue = new ConversationTree("Welcome to Solaris.", new String[] { "You tryed to destroy me." }, "Emperor");
                    emperorDialogue.AddNode("I did something you never could.", new String[] { "What, betray a friend?" });
                    emperorDialogue.AddNode("No, I gained true power.", new int[] { 0 }, new String[] { "You've gained nothing." });
                    emperorDialogue.AddCapNode("A", "Maybe, but you lost everything!", new int[] { 0,0 }, new String[] { "Have I?" });
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(-400, emperorDialogue, "enemy", 1.2, 1.2);
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(-1500, shopKeeperDialogue, "shopkeeper", 1.4, 1.2);

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
