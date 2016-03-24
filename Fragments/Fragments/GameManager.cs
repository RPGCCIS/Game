﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Fragments
{
    class GameManager
    {
        public enum GameState
        {
            Menu,
            Town,
            Battle,
            Map,
            Pause
        }

        //Member Variables
        private static GameManager instance;
        private Map overworld;
        private GameState gameState;
        private GameState prevState;
        private Player player;
        private Map currentMap;
        private Dictionary<string, bool> keyEvents;
        private List<Vector2> townLocations = new List<Vector2>();
        //Singleton property
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                    
                }
                return instance;
            }
        }

        //Properties
        public GameState State
        {
            get { return gameState; }
            set {
                prevState = gameState;
                gameState = value;
            }
        }
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
        public Map CurrentMap
        {
            get { return currentMap; }
            set { currentMap = value; }
        }
        public GameState PrevState
        {
            get { return prevState; }
        }

        //Constructor
        private GameManager()
        {
            //EMPTY
            townLocations.Add(new Vector2(9, 12));
            townLocations.Add(new Vector2(8, 2));
            townLocations.Add(new Vector2(9, 12));
            townLocations.Add(new Vector2(9, 12));
            townLocations.Add(new Vector2(9, 12));
        }

        //Method
        public GameState RunMenu()
        {
            return GameState.Town;
        }

        //Update and check for switches between game states
        public void Update(TextList menuOptions, TextList messageOptions, TextList battleOptions, KeyboardState kbState, KeyboardState oldKbState)
        {
            switch (GameManager.Instance.State)
            {
                case GameManager.GameState.Menu:
                    if (IsKeyPressed(kbState, oldKbState, Keys.W))
                    {
                        menuOptions.Previous();
                        
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.S))
                    {
                        menuOptions.Next();
                    }

                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        switch (menuOptions.Selected)
                        {
                            //Option 1
                            case 0:
                                GameManager.Instance.State = GameManager.GameState.Battle;
                                break;

                            //Option 2
                            //Loading the overworld map
                            case 1:
                                overworld = new Map("overworld");
                                GameManager.Instance.currentMap = overworld;
                                GameManager.Instance.State = GameManager.GameState.Map;
                                break;

                            //Play Game
                            case 2:
                                MapManager.Instance.LoadMap("test");
                                break;
                        }
                    }
                    break;

                case GameManager.GameState.Town:
                    //State changes for testing
                    if (IsKeyPressed(kbState, oldKbState, Keys.A))
                    {
                        GameManager.Instance.State = GameManager.GameState.Menu;
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.D))
                    {
                        GameManager.Instance.State = GameManager.GameState.Map;
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.G))
                    {
                        GameManager.Instance.State = GameManager.GameState.Pause;
                    }

                    //Interactable
                    else if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        //We don't care if it returns true or not
                        GameManager.Instance.Player.IsColliding(
                            GameManager.Instance.CurrentMap.ParallaxLayer,
                            TypeOfObject.Interactable);
                    }

                    //Player movement
                    GameManager.Instance.Player.Move(Keyboard.GetState());

                    //Layer movement
                    if (GameManager.Instance.Player.MS == Player.MovementState.WalkingRight)
                    {
                        GameManager.Instance.CurrentMap.MoveLayers();

                        if (GameManager.Instance.Player.IsColliding(
                            GameManager.Instance.CurrentMap.ParallaxLayer,
                            TypeOfObject.Solid))
                        {
                            GameManager.Instance.CurrentMap.MoveLayers(false);
                        }
                    }
                    else if (GameManager.Instance.Player.MS == Player.MovementState.WalkingLeft)
                    {
                        GameManager.Instance.CurrentMap.MoveLayers(false);

                        if (GameManager.Instance.Player.IsColliding(
                            GameManager.Instance.CurrentMap.ParallaxLayer,
                            TypeOfObject.Solid))
                        {
                            GameManager.Instance.CurrentMap.MoveLayers();
                        }
                    }

                    break;

                case GameManager.GameState.Map:
                    if (IsKeyPressed(kbState, oldKbState, Keys.T))
                    {
                        GameManager.Instance.State = GameManager.GameState.Menu;
                    }
                    
                    //overworld movement
                    //Checks the movement flags
                    if (IsKeyPressed(kbState, oldKbState, Keys.Up))
                    {
                        if (CurrentMap.Tiles[(int)player.mapX, (int)player.mapY].Flags.HasFlag(MovementFlags.UP))
                        {
                            player.mapY--;
                        }
                        
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.Down))
                    {
                        if (CurrentMap.Tiles[(int)player.mapX, (int)player.mapY].Flags.HasFlag(MovementFlags.DOWN))
                        {
                            player.mapY++;
                        }
                        
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.Left))
                    {
                        if (CurrentMap.Tiles[(int)player.mapX, (int)player.mapY].Flags.HasFlag(MovementFlags.LEFT))
                        {
                            player.mapX--;
                        }
                        
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.Right))
                    {
                        if (CurrentMap.Tiles[(int)player.mapX, (int)player.mapY].Flags.HasFlag(MovementFlags.RIGHT))
                        {
                            player.mapX++;
                        }
                        
                    }

                    //If the location of the player when they press enter is within the townLoactions list
                    //it will load the map at that location
                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        if (townLocations.Contains(player.MapPos))
                        {
                            if(player.MapPos == townLocations[0])
                            {
                                MapManager.Instance.LoadMap("test");
                            }
                        }
                    }
                    break;

                case GameManager.GameState.Battle:
                    //BattleManager.Instance.Player = player;
                    if (IsKeyPressed(kbState, oldKbState, Keys.D))
                    {
                        GameManager.Instance.State = GameManager.GameState.Map;
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.G))
                    {
                        GameManager.Instance.State = GameManager.GameState.Pause;
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.W))
                    {
                        battleOptions.Previous();
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.S))
                    {
                        battleOptions.Next();
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        switch (battleOptions.Selected)
                        {
                            //Option 1
                            case 0:
                                GameManager.Instance.State = GameManager.GameState.Menu;
                                break;

                            //Option 2
                            case 1:
                                GameManager.Instance.State = GameManager.GameState.Town;
                                break;
                        }
                    }
                    break;
                case GameManager.GameState.Pause:
                    if (IsKeyPressed(kbState, oldKbState, Keys.G))
                    {
                        GameManager.Instance.State = GameManager.Instance.PrevState;
                    }
                    break;
            }

            oldKbState = kbState;
        }

        public bool IsKeyPressed(KeyboardState current, KeyboardState old, Keys k)
        {
            return (current.IsKeyDown(k) && old.IsKeyUp(k));
        }

        //Drawing
        public void Draw(SpriteBatch spriteBatch, TextList menuOptions, Message battle, GraphicsDevice graphics, Message m)
        {
            switch (GameManager.Instance.State)
            {
                case GameManager.GameState.Menu:
                    graphics.Clear(Color.White);
                    menuOptions.DrawText(spriteBatch);
                    break;

                case GameManager.GameState.Town:
                    graphics.Clear(Color.Green);
                    GameManager.Instance.CurrentMap.Draw(spriteBatch);
                    GameManager.Instance.Player.Draw(spriteBatch);
                    break;

                case GameManager.GameState.Map:
                    graphics.Clear(Color.Brown);
                    GameManager.Instance.CurrentMap = overworld;
                    GameManager.Instance.CurrentMap.DrawOverworld(spriteBatch);
                    GameManager.Instance.Player.DrawOverworld(spriteBatch);
                    //m.Draw(spriteBatch);
                    break;

                case GameManager.GameState.Battle:
                    graphics.Clear(Color.Red);
                    battle.Draw(spriteBatch);
                    break;
            }
        }
    }
}
