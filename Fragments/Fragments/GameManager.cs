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

        private GameState gameState;
        private GameState prevState;
        private Player player;
        private Map currentMap;
        private Dictionary<string, bool> keyEvents;

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
                            case 1:
                                GameManager.Instance.State = GameManager.GameState.Map;
                                break;

                            //Play Game
                            case 2:
                                GameManager.Instance.State = GameManager.GameState.Town;
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
                    GameManager.Instance.Player.Move(Keyboard.GetState(), GameManager.Instance);

                    //Layer movement
                    if (GameManager.Instance.Player.MS == Player.MovementState.WalkingRight)
                    {
                        GameManager.Instance.CurrentMap.ParallaxLayer.X -=
                            GameManager.Instance.Player.Movement * GameManager.Instance.CurrentMap.ParallaxLayer.MM;

                        if (GameManager.Instance.Player.IsColliding(
                            GameManager.Instance.CurrentMap.ParallaxLayer))
                        {
                            GameManager.Instance.CurrentMap.ParallaxLayer.X +=
                                GameManager.Instance.Player.Movement * GameManager.Instance.CurrentMap.ParallaxLayer.MM;
                        }
                    }
                    else if (GameManager.Instance.Player.MS == Player.MovementState.WalkingLeft)
                    {
                        GameManager.Instance.CurrentMap.ParallaxLayer.X +=
                            GameManager.Instance.Player.Movement * GameManager.Instance.CurrentMap.ParallaxLayer.MM;

                        if (GameManager.Instance.Player.IsColliding(
                            GameManager.Instance.CurrentMap.ParallaxLayer))
                        {
                            GameManager.Instance.CurrentMap.ParallaxLayer.X -=
                                GameManager.Instance.Player.Movement * GameManager.Instance.CurrentMap.ParallaxLayer.MM;
                        }
                    }


                    break;

                case GameManager.GameState.Map:
                    if (IsKeyPressed(kbState, oldKbState, Keys.T))
                    {
                        GameManager.Instance.State = GameManager.GameState.Town;
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.F))
                    {
                        GameManager.Instance.State = GameManager.GameState.Battle;
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.G))
                    {
                        GameManager.Instance.State = GameManager.GameState.Pause;
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.W))
                    {
                        messageOptions.Previous();
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.S))
                    {
                        messageOptions.Next();
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        switch (messageOptions.Selected)
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

                case GameManager.GameState.Battle:
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
                    m.Draw(spriteBatch);
                    break;

                case GameManager.GameState.Battle:
                    graphics.Clear(Color.Red);
                    battle.Draw(spriteBatch);
                    break;
            }
        }
    }
}
