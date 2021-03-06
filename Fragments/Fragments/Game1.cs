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

        //universals
        public static Texture2D whiteSquareText;
        public static SpriteFont universalFont;

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
			graphics.PreferredBackBufferWidth = 890;  // width of the window
			graphics.PreferredBackBufferHeight = 740;   // height of the window
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

			test = new Map("airedale");

			// Singleton initialization
			GameManager.Instance.State = GameManager.GameState.Menu;

			//Messages
			battle = new Message("scroll", true);

			//Set up Menu
			menuOptions = new TextList(
				null,
				new Vector2(100, 550));

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
			SoundManager.Instance.Content = Content;

			ShopManager.Instance.Font = Content.Load<SpriteFont>("LCALLIG_14");
			ShopManager.Instance.Scroll = Content.Load<Texture2D>("scroll");
            
            
			// Load the player
			//playerText = Content.Load<Texture2D>("player");
			p = new Player(400, 605, 200, 300, playerText);
			p.Atk = 2;
			p.Def = 0;
			p.MaxHp = 25;
			p.MaxSp = 10;
			p.Spd = 6;
			p.Sp = p.MaxSp;
			p.Hp = p.MaxHp;
            
			GameManager.Instance.Player = p;
			GameManager.Instance.Player.SpriteSheet = Content.Load<Texture2D>("rpg_sprite_walk");
			font = Content.Load<SpriteFont>("Georgia_32");

            //Temporary EnemyContent
            
            BattleManager.Instance.PlayerTexture = Content.Load<Texture2D>("player");
            BattleManager.Instance.EnemyTexture = Content.Load<Texture2D>("enemy");
            //MapManager.Instance.Textures.Add("Goomba", enemyText);
            BattleManager.Instance.Content = Content;

            //exclusively to get healthbars working
            whiteSquareText = Content.Load<Texture2D>("whitesquare");
            universalFont = Content.Load<SpriteFont>("TimesNewRoman_12");

			//For Battle
			messageFont = Content.Load<SpriteFont>("LCALLIG_14");
			messageB = new TextObject(messageFont, null, new Vector2(battle.RectX + 150, battle.RectY + 50));
			battleOptions = new TextList(messageFont, new Vector2(battle.RectX + 175, battle.RectY + 125));
			battle.Texture = Content.Load<Texture2D>(battle.TextureName);
			battle = new Message(messageB, battleOptions, battle.Texture, battle.Rect);

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
			GameManager.Instance.MenuTexture = Content.Load<Texture2D>("menu");
			menuOptions.Font = font;
			//menuOptions.Add("Option 1");
			menuOptions.Add("Load");
			menuOptions.Add("Play Game");

            MapManager.Instance.boat = Content.Load<Texture2D>("boat");
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

			GameManager.Instance.Update(menuOptions, kbState, oldKbState, gameTime, spriteBatch);

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
