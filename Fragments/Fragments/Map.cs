﻿using System;
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
        Map overworld;
        //Multipliers
        private const float backgroundMultiplier = .3f;
        private const float parallaxMultiplier = 7;
        private const float superForegroundMultiplier = 0;

        private Tile[,] tiles;

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

            //Load(name);
        }
        public Map()
        {
            mapName = "overworld";
            layers = new List<Layer>();
            Load("overworld");
        }

        //Fonts
        public SpriteFont GetFont(string name)
        {
            if (!MapManager.Instance.Fonts.ContainsKey(name))
            {
                MapManager.Instance.Fonts.Add(
                   name,
                   MapManager.Instance.Content.Load<SpriteFont>(name));
            }

            return MapManager.Instance.Fonts[name];
        }

        // Get/add Texture
        public Texture2D GetTexture(string name)
        {
            if(!MapManager.Instance.Textures.ContainsKey(name))
            {
                MapManager.Instance.Textures.Add(
                   name,
                   MapManager.Instance.Content.Load<Texture2D>(name)); 
            }

            return MapManager.Instance.Textures[name];
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
            //layers.Add(superForegroundLayer);
        }

        public void ClearMap()
        {
            foreach(Layer l in layers)
            {
                l.Clear();
            }
            MapManager.Instance.Textures.Clear();
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
                //if (l.Name == "background" || l.Name == "ocean")
                //{
                    if (l.Objects.Count == 1)
                    {
                        DrawableObject centerBg = l.Objects[0];
                        
                        DrawableObject leftBg = new DrawableObject(
                            l.Objects[0].X,
                            l.Objects[0].Y,
                            l.Objects[0].Rec.Width,
                            l.Objects[0].Rec.Height, 
                            l.Objects[0].Texture);

                        leftBg.X = -(centerBg.Rec.Width);

                        DrawableObject rightBg = new DrawableObject(
                            l.Objects[0].X,
                            l.Objects[0].Y,
                            l.Objects[0].Rec.Width,
                            l.Objects[0].Rec.Height,
                            l.Objects[0].Texture);

                        rightBg.X = centerBg.Rec.Width;

                        l.AddObject(0, leftBg);
                        l.AddObject(rightBg);

                        l.Objects[0].Type = TypeOfObject.BGL;
                        l.Objects[1].Type = TypeOfObject.BGC;
                        l.Objects[2].Type = TypeOfObject.BGR;
                    }
                    else
                    {
                        //He should be in the middle
                        //If colliding with left
                        if (GameManager.Instance.Player.IsColliding(l, TypeOfObject.BGL))
                        {
                            DrawableObject movedRightBg = l.Objects[2];

                            movedRightBg.X -= 3 * l.Objects[1].Rec.Width;
                            l.AddObject(0, movedRightBg);
                            l.Objects.RemoveAt(3);

                            l.Objects[0].Type = TypeOfObject.BGL;
                            l.Objects[1].Type = TypeOfObject.BGC;
                            l.Objects[2].Type = TypeOfObject.BGR;
                        }

                        //If colliding with right
                        if (GameManager.Instance.Player.IsColliding(l, TypeOfObject.BGR))
                        {
                            DrawableObject movedLeftBg = l.Objects[0];

                            movedLeftBg.X += 3 * l.Objects[1].Rec.Width;
                            l.AddObject(movedLeftBg);
                            l.Objects.RemoveAt(0);

                            l.Objects[0].Type = TypeOfObject.BGL;
                            l.Objects[1].Type = TypeOfObject.BGC;
                            l.Objects[2].Type = TypeOfObject.BGR;
                        }
                    }

                    //Console.WriteLine(l.Objects.Count);
                //}

                l.Draw(s, col);
            }
        }
        //For drawing overworld tiles
        public void DrawOverworld(SpriteBatch s, Color col)
        {
            
            for(int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    if(tiles[i,j].Filename == "purple")
                    {
                        s.Draw(
                        GetTexture("road"),
                        new Rectangle(900 / 14 * i, 750 / 14 * j, 900 / 14, 750 / 14),
                        col);
                    }
                    else if (tiles[i, j].Filename == "green")
                    {
                        s.Draw(
                        GetTexture("grass"),
                        new Rectangle(900 / 14 * i, 750 / 14 * j, 900 / 14, 750 / 14),
                        col);
                    }
                    else if (tiles[i, j].Filename == "blue")
                    {
                        s.Draw(
                        GetTexture("water"),
                        new Rectangle(900 / 14 * i, 750 / 14 * j, 900 / 14, 750 / 14),
                        col);
                    }
                    else if (tiles[i, j].Filename == "brown")
                    {
                        s.Draw(
                        GetTexture("rock"),
                        new Rectangle(900 / 14 * i, 750 / 14 * j, 900 / 14, 750 / 14),
                        col);
                    }
                    else
                    {
                        if(GameManager.Instance.TownLocations[1] == new Vector2(i,j) && !Progress.Instance.Flags.HasFlag(ProgressFlags.TalkedWithElder))
                        {
                            Blocked(s,i,j, col);
                        }
                        else if(GameManager.Instance.TownLocations[2] == new Vector2(i, j) && !Progress.Instance.Flags.HasFlag(ProgressFlags.SecondFragment))
                        {
                            Blocked(s, i, j, col);
                        }
                        else if (GameManager.Instance.TownLocations[3] == new Vector2(i, j) && !Progress.Instance.Flags.HasFlag(ProgressFlags.ThirdFragment))
                        {
                            Blocked(s, i, j, col);
                        }
                        else if (GameManager.Instance.TownLocations[4] == new Vector2(i, j) && !Progress.Instance.Flags.HasFlag(ProgressFlags.FourthFragment))
                        {
                            Blocked(s, i, j, col);
                        }
                        else if (GameManager.Instance.TownLocations[5] == new Vector2(i, j) && !Progress.Instance.Flags.HasFlag(ProgressFlags.FifthFragment))
                        {
                            Blocked(s, i, j, col);
                        }
                        else
                        {
                            s.Draw(
                                GetTexture("orange"),
                                new Rectangle(900 / 14 * i, 750 / 14 * j, 900 / 14, 750 / 14),
                                col);
                        }
                        
                    }
                  
                    
                }
            }

            GameManager.Instance.Player.DrawOverworld(s);
        }
        public void Blocked(SpriteBatch s, int i, int j, Color col)
        {
                s.Draw(
                                    GetTexture(tiles[i, j].Filename),
                                    new Rectangle(900 / 14 * i, 750 / 14 * j, 900 / 14, 750 / 14),
                                    new Color(Color.Red.ToVector3()*col.ToVector3()));
        }
    }
}
