using Microsoft.Xna.Framework;
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
        public enum GameState
        {
            Menu,
            Town,
            Battle,
            Map
        }

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
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            test = new Map("test");
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

            foreach (Layer l in test.GetLayers())
            {
                l.Texture = Content.Load<Texture2D>(l.Name);
            }

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
                                GameManager.Instance.State = GameManager.GameState.Town;
                                break;
                        }
                    }
                    break;

                case GameManager.GameState.Town:
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
                    p.Move(Keyboard.GetState(), GameManager.Instance);

                    if(p.MS == Player.MovementState.WalkingRight && p.X > 150)
                        test.GetLayers()[1].PosX -= p.Movement * test.GetLayers()[1].MM;
                    if (p.MS == Player.MovementState.WalkingLeft&& p.X < 700)
                        test.GetLayers()[1].PosX += p.Movement * test.GetLayers()[1].MM;

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
                    test.Draw(spriteBatch);
                    p.Draw(spriteBatch);
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
