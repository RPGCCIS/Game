using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        //Keyboard
        KeyboardState kbState;
        KeyboardState oldKbState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Singleton initialization
            GameManager.Instance.State = GameManager.GameState.Menu;

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

            // TODO: use this.Content to load your game content here
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

            switch(GameManager.Instance.State)
            {
                case GameManager.GameState.Menu:
                    if(IsKeyPressed(kbState, oldKbState, Keys.S))
                    {
                        GameManager.Instance.State = GameManager.GameState.Town;
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
                    else if (IsKeyPressed(kbState, oldKbState, Keys.F))
                    {
                        GameManager.Instance.State = GameManager.GameState.Pause;
                    }
                    break;

                case GameManager.GameState.Map:
                    if (IsKeyPressed(kbState, oldKbState, Keys.S))
                    {
                        GameManager.Instance.State = GameManager.GameState.Town;
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.F))
                    {
                        GameManager.Instance.State = GameManager.GameState.Battle;
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.F))
                    {
                        GameManager.Instance.State = GameManager.GameState.Pause;
                    }
                    break;

                case GameManager.GameState.Battle:
                    if (IsKeyPressed(kbState, oldKbState, Keys.D))
                    {
                        GameManager.Instance.State = GameManager.GameState.Map;
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.F))
                    {
                        GameManager.Instance.State = GameManager.GameState.Pause;
                    }
                    break;
                case GameManager.GameState.Pause:
                    if (IsKeyPressed(kbState, oldKbState, Keys.F))
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

            switch (GameManager.Instance.State)
            {
                case GameManager.GameState.Menu:
                    GraphicsDevice.Clear(Color.White);
                    break;

                case GameManager.GameState.Town:
                    GraphicsDevice.Clear(Color.Green);
                    break;

                case GameManager.GameState.Map:
                    GraphicsDevice.Clear(Color.Brown);
                    break;

                case GameManager.GameState.Battle:
                    GraphicsDevice.Clear(Color.Red);
                    break;
            }
            base.Draw(gameTime);
        }
    }
}
