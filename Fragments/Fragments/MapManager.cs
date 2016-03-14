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
        private Map currentMap;

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
            set { content = value; }
        }


        public void LoadMap(Map m)
        {
            if(currentMap != null)
                currentMap.ClearMap();
            foreach (Layer l in GameManager.Instance.CurrentMap.Layers)
            {
                l.AddObject(content.Load<Texture2D>(l.Name), new Vector2(0));
            }
            switch (m.MapName)
            {
                case "test":
                    GameManager.Instance.CurrentMap.AddTexture("wall", content.Load<Texture2D>("wall"));

                    //Draw wall
                    GameManager.Instance.CurrentMap.ParallaxLayer.AddObject(
                         GameManager.Instance.CurrentMap.Textures["wall"],
                         new Vector2(-2000, 0));
                    break;
            }
            currentMap = m;
            GameManager.Instance.State = GameManager.GameState.Town;
            GameManager.Instance.CurrentMap = m;
            
        }
    }
}
