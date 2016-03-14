﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace Fragments
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Default Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map test;
        Map test2;

        //Message testing
        Message m;
        TextObject message;
        TextList messageOptions;
        SpriteFont messageFont;
        TextObject messageB;
        TextList battleOptions;
        Message battle;
        Player p = new Player(400, 550, 150, 200);

        /// <summary>
        /// TODO: This should be held in the GameManager, 
        /// which should have global access(?)
        /// </summary>

        //Keyboard
        KeyboardState kbState;
        KeyboardState oldKbState;

        //Texture2D

        //Fonts
        SpriteFont font;

        //Menu variables
        TextList menuOptions;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;  // width of the window
            graphics.PreferredBackBufferHeight = 750;   // height of the window
            graphics.ApplyChanges();
            GameManager.Instance.Player = p;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;

            // TODO: Add your initialization logic here
            test = new Map("test");
            test2 = new Map("test2");
            // Singleton initialization
            GameManager.Instance.State = GameManager.GameState.Menu;

            //Messages
            m = new Message("scroll", false);
            battle = new Message("scroll", true);

            //Set up Menu
            menuOptions = new TextList(
                null,
                new Vector2(100, 100));

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameManager.Instance.CurrentMap = test;
            MapManager.Instance.Content = Content;
            
            

            // TODO: use this.Content to load your game content here
            p.Texture = Content.Load<Texture2D>("player");
            font = Content.Load<SpriteFont>("Georgia_32");

            //For Map
            messageFont = Content.Load<SpriteFont>("LCALLIG_14");
            message = new TextObject(messageFont, "Your adventure will be coming soon.", new Vector2(m.RectX + 150, m.RectY + 25));
            messageOptions = new TextList(null, new Vector2(m.RectX + 175, m.RectY + 75));
            messageOptions.Font = messageFont;
            messageOptions.Add("Ok");
            messageOptions.Add("Go Faster!");
            m.Texture = Content.Load<Texture2D>(m.TextureName);
            m = new Message(message, messageOptions,m.Texture,m.Rect);

            //For Battle
            messageB = new TextObject(messageFont, "In Battle.", new Vector2(battle.RectX + 150, battle.RectY + 50));
            battleOptions = new TextList(null, new Vector2(battle.RectX + 175, battle.RectY + 125 ));
            battleOptions.Font = messageFont;
            battleOptions.Add("Fight");
            battleOptions.Add("Run");
            battle.Texture = Content.Load<Texture2D>(battle.TextureName);
            battle = new Message(messageB, battleOptions, battle.Texture,battle.Rect);

            //Apply loaded Content

            //Menu
            menuOptions.Font = font;
            menuOptions.Add("Option 1");
            menuOptions.Add("Option 2");
            menuOptions.Add("Play Game");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();

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
                                MapManager.Instance.LoadMap(test);
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

                        if(GameManager.Instance.Player.IsColliding(
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
                    if (IsKeyPressed(kbState, oldKbState, Keys.V))
                    {
                        MapManager.Instance.LoadMap(test2);
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

            base.Update(gameTime);
        }

        public bool IsKeyPressed(KeyboardState current, KeyboardState old, Keys k)
        {
            return (current.IsKeyDown(k) && old.IsKeyUp(k));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (GameManager.Instance.State)
            {
                case GameManager.GameState.Menu:
                    GraphicsDevice.Clear(Color.White);
                    menuOptions.DrawText(spriteBatch);
                    break;

                case GameManager.GameState.Town:
                    GraphicsDevice.Clear(Color.Green);
                    GameManager.Instance.CurrentMap.Draw(spriteBatch);
                    GameManager.Instance.Player.Draw(spriteBatch);
                    break;

                case GameManager.GameState.Map:
                    GraphicsDevice.Clear(Color.Brown);
                    m.Draw(spriteBatch);
                    break;

                case GameManager.GameState.Battle:
                    GraphicsDevice.Clear(Color.Red);
                    battle.Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
