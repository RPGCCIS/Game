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

                    //GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                    //    100,
                    //    "test1",
                    //    true, 
                    //    "Test Land");

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

                    //Shopkeeper
                    ConversationTree shopKeeperDialogue = new ConversationTree("Buy something, will ya?");

                    //---Root

                    //Sure
                    shopKeeperDialogue.AddNode("Good choice, what do you need");
                    //Nah
                    shopKeeperDialogue.AddCapNode("A", "Well that's too bad"); //End
                    

                    //---Nah

                    //Items
                    shopKeeperDialogue.AddCapNode("B", "Best to be prepared!", new int[] { 0 }); //End
                    //Gear
                    shopKeeperDialogue.AddCapNode("C", "I'll make you tough!", new int[] { 0 }); //End

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddNPC(2200, shopKeeperDialogue);

                    break;

                case "test1":
                    Console.WriteLine("TEST!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
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
