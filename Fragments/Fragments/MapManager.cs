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

        public Dictionary<string, Texture2D> Textures
        {
            get { return textures; }
        }

        //Constructor
        public MapManager()
        {
            textures = new Dictionary<string, Texture2D>();
        }

        // Loads the map as a whole
        // i.e. a town
        public void LoadMap(string m)
        {
            switch (m)
            {
                case "test":
                    LoadMapFromFile("test");

                    //Draw everything

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        100, 
                        "test1");

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-2000, 0),
                         TypeOfObject.Solid);

                    break;

                case "test1":
                    LoadMapFromFile("test");

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.GetTexture("wall"),
                         new Vector2(-4000, 0),
                         TypeOfObject.Solid);

                    GameManager.Instance.CurrentMap.ParallaxLayer.AddHouse(
                        300,
                        "test");
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
                GameManager.Instance.CurrentMap.Load(file);
            }

            else
            {
                GameManager.Instance.CurrentMap = new Map(file);
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
