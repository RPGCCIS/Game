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
        Map test2;

        //Message testing
        Message m;
        TextObject message;
        TextList messageOptions;
        SpriteFont messageFont;
        TextObject messageB;
        TextList battleOptions;
        Message battle;

        Player p;
        Texture2D playerText;

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
            graphics.PreferredBackBufferWidth = 900;  // width of the window
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

            // Load the player
            playerText = Content.Load<Texture2D>("player");
            p = new Player(400, 605, 200, 300, playerText);
            GameManager.Instance.Player = p;
            GameManager.Instance.Player.SpriteSheet = Content.Load<Texture2D>("rpg_sprite_walk");
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
            oldKbState = kbState;
            kbState = Keyboard.GetState();

            GameManager.Instance.Update(menuOptions, messageOptions, battleOptions, kbState, oldKbState,gameTime);

            base.Update(gameTime);
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
            GameManager.Instance.Draw(spriteBatch, menuOptions, battle, this.GraphicsDevice, m);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
