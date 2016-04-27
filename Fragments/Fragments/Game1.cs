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
        SpriteFont messageFont;
        TextObject messageB;
        TextList battleOptions;
        Message battle;

        Player p;
        Texture2D playerText;
        Texture2D enemyText;

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

            test = new Map("test");
            test2 = new Map("test2");

            // Singleton initialization
            GameManager.Instance.State = GameManager.GameState.Menu;

            //Messages
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
            ShopManager.Instance.Font = Content.Load<SpriteFont>("LCALLIG_14");
            ShopManager.Instance.Scroll = Content.Load<Texture2D>("scroll");
            
            
            // Load the player
            //playerText = Content.Load<Texture2D>("player");
            p = new Player(400, 605, 200, 300, playerText);
            p.Atk = 5;
            p.Def = 4;
            p.MaxHp = 55;
            p.MaxSp = 20;
            p.Spd = 6;
            p.Sp = p.MaxSp;
            p.Hp = p.MaxHp;
            
            GameManager.Instance.Player = p;
            GameManager.Instance.Player.SpriteSheet = Content.Load<Texture2D>("rpg_sprite_walk");
            font = Content.Load<SpriteFont>("Georgia_32");

            //Temporary EnemyContent
            enemyText = Content.Load<Texture2D>("Goomba");
            Enemy e = new Enemy(EnemyType.grunt, 300, 300, 100, 100, enemyText);
            e.Atk = 100;
            e.Def = 1;
            e.MaxHp = 5;
            e.MaxSp = 20;
            e.Spd = 6;
            e.Sp = e.MaxSp;
            e.Hp = e.MaxHp;
            BattleManager.Instance.Enemy = e;


            //For Battle
            messageFont = Content.Load<SpriteFont>("LCALLIG_14");
            messageB = new TextObject(messageFont, null, new Vector2(battle.RectX + 150, battle.RectY + 50));
            battleOptions = new TextList(messageFont, new Vector2(battle.RectX + 175, battle.RectY + 125 ));
            battle.Texture = Content.Load<Texture2D>(battle.TextureName);
            battle = new Message(messageB, battleOptions, battle.Texture,battle.Rect);

            BattleManager.Instance.Title = battle;
            BattleManager.Instance.Player = p;

            //Apply loaded Content
            GameManager.Instance.PauseMenu = new TextList(font, new Vector2(350, 250));
            GameManager.Instance.PauseMenu.Add("Resume");
            GameManager.Instance.PauseMenu.Add("Load");

            //GameManager.Instance.PauseMenu.DefaultColor = Color.Wheat;
            GameManager.Instance.ScrollTexture = Content.Load<Texture2D>("scroll");
            GameManager.Instance.Font = font;
            GameManager.Instance.MFont = messageFont;
            //Menu
            menuOptions.Font = font;
            menuOptions.Add("Option 1");
            menuOptions.Add("Load");
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

            GameManager.Instance.Update(menuOptions, kbState, oldKbState,gameTime,spriteBatch);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //ShopManager.Instance.SpriteBatch = spriteBatch;
            
            GameManager.Instance.Draw(spriteBatch, menuOptions, battle, this.GraphicsDevice);
            
            //GameManager.Instance.Draw(spriteBatch, menuOptions, battle, this.GraphicsDevice, m);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
