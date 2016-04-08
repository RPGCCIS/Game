using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace Fragments
{
    class Map
    {
        private string mapName;
        private List<Layer> layers;
        public uint version;
        private int width;
        private int height;

        private String superForeground;
        private String parallax;
        private String background;

        //Multipliers
        private const float backgroundMultiplier = 0.2f;
        private const float parallaxMultiplier = 40;
        private const float superForegroundMultiplier = 0;

        private Tile[,] tiles;
        private Dictionary<string, Texture2D> textures;

        Layer backgroundLayer;
        Layer parallaxLayer;
        Layer foregroundLayer;
        Layer superForegroundLayer;

        //Properties
        public List<Layer> Layers
        {
            get { return layers; }
        }

        public Layer BackgroundLayer
        {
            get { return backgroundLayer; }
        }

        public Layer ParallaxLayer
        {
            get { return parallaxLayer; }
        }

        public Layer Foreground
        {
            get { return foregroundLayer; }
        }

        public Layer SuperForeGround
        {
            get { return superForegroundLayer; }
        }

        public Dictionary<string, Texture2D> Textures
        {
            get { return textures; }
        }
        public string MapName
        {
            get { return mapName; }
        }
        public Tile[,] Tiles
        {
            get { return tiles; }
        }

        //Hold for when we actually get a class for entities
        //List<DrawableObject> entities;
        public Map(string name)
        {
            mapName = name;

            layers = new List<Layer>();
            textures = new Dictionary<string, Texture2D>();

            Load(name);
        }

        //Add
        public void AddTexture(string name, Texture2D texture)
        {
            Textures.Add(name, texture);
        }

        //Loading map from file
        public void Load(string name)
        {
            try
            {
                using (Stream inStream = File.OpenRead("../../../Maps/" + name + ".map"))
                using (BinaryReader input = new BinaryReader(inStream))
                {
                    version = input.ReadUInt32();
                    width = input.ReadInt32();
                    height = input.ReadInt32();
                    tiles = new Tile[width, height];

                    for (int i = 0; i < width; ++i)
                    {
                        for (int j = 0; j < height; ++j)
                        {
                            tiles[i, j] = new Tile((MovementFlags)input.ReadInt32(), input.ReadString());
                        }
                    }

                    background = input.ReadString();
                    parallax = input.ReadString();
                    superForeground = input.ReadString();

                    while (input.ReadBoolean())
                    {
                        //Will not currently load entities
                        input.ReadInt32();
                        input.ReadInt32();
                        input.ReadString();
                    }
                    input.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("File error:" + e.Message);
            }

            backgroundLayer = new Layer(background, backgroundMultiplier);
            parallaxLayer = new Layer(parallax, parallaxMultiplier);
            superForegroundLayer = new Layer(superForeground, superForegroundMultiplier);

            layers.Clear();

            layers.Add(backgroundLayer);
            layers.Add(parallaxLayer);
            layers.Add(superForegroundLayer);
        }
        /*
        /// <summary>
        /// Get layers
        /// </summary>
        /// <returns>list of layers</returns>
        public List<Layer> GetLayers()
        {
            return layers;
        }
        */
        public void ClearMap()
        {
            foreach(Layer l in layers)
            {
                l.Clear();
            }
            textures.Clear();
        }

        public void MoveLayers(bool isMovingRight = true)
        {
            if (isMovingRight)
            {
                foreach (Layer l in layers)
                {
                    l.X -= l.Multiplier;
                }
            }
            else
            {
                foreach (Layer l in layers)
                {
                    l.X += l.Multiplier;
                }
            }

        }

        public void Draw(SpriteBatch s, Color col)
        {
            foreach (Layer l in layers)
            {
                //everything is just currently drawn to 0,0

                l.Draw(s, col);
                                
            }
        }
        //For drawing overworld tiles
        public void DrawOverworld(SpriteBatch s, Color col)
        {
            for(int i = 0; i < 14; i++)
            {
                for(int j = 0; j < 14; j++)
                {
                    s.Draw(MapManager.Instance.Content.Load<Texture2D>(tiles[i,j].Filename), new Rectangle(900/14* i, 750/14 * j, 900/14, 750/14), col);
                }
            }
        }
    }
}
