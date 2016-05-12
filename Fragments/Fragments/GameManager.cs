using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System.IO;

namespace Fragments
{
    
    class GameManager
	{
        public enum GameState
        {
            Off,
            Menu,
            Town,
            Battle,
            Map,
            Shop,
            Over
        }

        //Member Variables
        private static GameManager instance;

		private Map currentMap;
		private Map overworld;

		private GameState gameState;
		private GameState prevState;

		private Player player;
		private Dictionary<string, bool> keyEvents;
		private List<Vector2> townLocations = new List<Vector2>();
		private TextList pauseMenu;
		private Texture2D scroll;
		private Texture2D menu;
		private SpriteFont font;
		private SpriteFont mFont;
		private bool paused = false;
		private bool conversation = false;
		private Message dialog;
		private ConversationTree ct;
		private Random gen = new Random();
		//Singleton property
		public static GameManager Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new GameManager();
				}

				return instance;
			}
		}

		#region Properties

		//Properties
        public List<Vector2> TownLocations { get { return townLocations; } }
		public GameState State
		{
			get
			{
				return gameState;
			}
			set
			{
				prevState = gameState;
				gameState = value;
			}
		}

		public Player Player
		{
			get
			{
				return player;
			}
			set
			{
				player = value;
			}
		}

		public Map CurrentMap
		{
			get
			{
				return currentMap;
			}
			set
			{
				currentMap = value;
			}
		}

		public GameState PrevState
		{
			get
			{
				return prevState;
			}
		}

		public TextList PauseMenu
		{

			get
			{
				return pauseMenu;
			}
			set
			{
				pauseMenu = value;
			}
		}

		public Texture2D ScrollTexture
		{
			set
			{
				scroll = value;
			}
		}

		public SpriteFont Font
		{
			set
			{
				font = value;
			}
		}

		public SpriteFont MFont
		{
			set
			{
				mFont = value;
			}
		}

		public ConversationTree CT
		{
			set
			{
				ct = value;
			}
		}

		public bool Conversation
		{
			set
			{
				conversation = value;
			}
		}

		public Texture2D MenuTexture
		{
			set
			{
				menu = value;
			}
		}

		#endregion

		//Constructor
		private GameManager()
		{

			townLocations.Add(new Vector2(9, 12));
			townLocations.Add(new Vector2(8, 2));
			townLocations.Add(new Vector2(1, 9));
			townLocations.Add(new Vector2(5, 9));
			townLocations.Add(new Vector2(3, 6));
			townLocations.Add(new Vector2(12, 5));

			pauseMenu = new TextList(
				null,
				Vector2.Zero);

			//Overworld
			overworld = new Map();

			prevState = GameState.Off;
		}

		//Methods

		public GameState RunMenu()
		{
			return GameState.Town;
		}

		public void ConversationEndTrigger()
		{
			if(ct.Name == "Shop Keeper")
			{
				if(ct.CapNodes["C"] == ct.Current)
				{
					ct.Previous = ct.Current;
					ShopManager.Instance.Current = new Shop(GameManager.Instance.CurrentMap.MapName);
					ShopManager.Instance.UpdateShop();
					GameManager.Instance.State = GameState.Shop;
                    SoundManager.Instance.PlaySoundEffect("DoorOpened");
                    ct.Current = ct.Root;
					conversation = false;
				}
				else if(ct.CapNodes["B"] == ct.Current)
				{
					ct.Current = ct.Previous;
				}
				else if(ct.CapNodes["D"] == ct.Current)
				{
					ct.Current = ct.Root;
					conversation = false;
				}
				else if(ct.CapNodes["E"] == ct.Current)
				{
					ct.GoToNode(new int[] { 0 });
				}

			}

			if(ct.Name == "Gate Keeper")
			{
				if(ct.CapNodes["A"] == ct.Current)
				{
					ct.Previous = ct.Current;
					ct.Current = ct.Root;
					Progress.Instance.SetProgress(ProgressFlags.FirstGateUnlocked);
					conversation = false;
				}
			}
			if(ct.Name == "Elder")
			{
				if(ct.CapNodes["A"] == ct.Current)
				{
					ct.Previous = ct.Current;
					ct.Current = ct.Root;
					Progress.Instance.SetProgress(ProgressFlags.TalkedWithElder);
					conversation = false;

                    if (Progress.Instance.Fragments == 0)
                    {
                        Progress.Instance.Fragments = Progress.Instance.Fragments + 1;

                    }

				}
			}
            if (ct.Name == "Elder2")
            {
                if (ct.CapNodes["A"] == ct.Current)
                {
                    ct.Current = ct.Root;
                    conversation = false;
                }
            }
            if (ct.Name == "Elder3")
            {
                if (ct.CapNodes["A"] == ct.Current)
                {
                    ct.Current = ct.Root;
                    conversation = false;
                }
            }
            if (ct.Name == "Elder4")
            {
                if (ct.CapNodes["A"] == ct.Current)
                {
                    ct.Current = ct.Root;
                    conversation = false;
                }
            }
            if (ct.Name == "Chris")
            {
                if(ct.CapNodes["A"] == ct.Current)
                {
                    
                    ct.Current = ct.Root;
                    conversation = false;
                    BattleManager.Instance.Initialize(EnemyType.chris);
                }
            }
            if (ct.Name == "Emperor")
            {
                if (ct.CapNodes["A"] == ct.Current)
                {

                    ct.Current = ct.Root;
                    conversation = false;
                    BattleManager.Instance.Initialize(EnemyType.final);
                }
            }
        }

		public void RandomEncounter()
		{
			if(gen.Next(0, 20) == 0)
			{
                BattleManager.Instance.Initialize(EnemyType.grunt);
			}
		}

		#region Update

		//Update and check for switches between game states
		public void Update(TextList menuOptions,
		                   KeyboardState kbState,
		                   KeyboardState oldKbState,
		                   GameTime gameTime,
		                   SpriteBatch sb)
		{
			SoundManager.Instance.Update();

            //For changing music
			bool stateSwitched = false;
			if(prevState != gameState)
			{
				stateSwitched = true;

				//Exceptions
				if(gameState == GameState.Town)
				{
					if(prevState == GameState.Shop)
					{
						stateSwitched = false;
					}
				}
			}

			prevState = gameState;

			switch(GameManager.Instance.State)
			{

                #region menu
				case GameManager.GameState.Menu:
                    //If the state changed, change the music
					if(stateSwitched)
					{
						SoundManager.Instance.PlaySong("Prelude");
					}

					if(IsKeyPressed(kbState, oldKbState, Keys.W))
					{
						menuOptions.Previous();
					}
					if(IsKeyPressed(kbState, oldKbState, Keys.S))
					{
						menuOptions.Next();
					}

					if(IsKeyPressed(kbState, oldKbState, Keys.Enter))
					{
						switch(menuOptions.Selected)
						{

						//Loading the overworld map
							case 0:
                                //Loads saved town and player information                              
								if(Load())
								{
									MapManager.Instance.LoadMap(GameManager.Instance.CurrentMap.MapName);
								}
								break;

						//Play Game
							case 1:
			                    MapManager.Instance.LoadMap("airedale");
								break;
						}
					}
					break;
                #endregion

                #region town
				case GameManager.GameState.Town:
                    //If the state changed, change the music
					if(stateSwitched)
					{
						SoundManager.Instance.PlaySong("WelcomeToOurTown");
					}

                    //State changes for testing
                    if (!paused && !conversation)
                    {
                        if (IsKeyPressed(kbState, oldKbState, Keys.Escape))
                        {
                            pauseMenu.Selected = 0;
                            paused = true;
                        }
                        //Interactable
                        else if(IsKeyPressed(kbState, oldKbState, Keys.Enter))
						{
							ShopManager.Instance.Current = new Shop(GameManager.Instance.CurrentMap.MapName);
							//We don't care if it returns true or not
							GameManager.Instance.Player.IsColliding(
								GameManager.Instance.CurrentMap.ParallaxLayer,
								TypeOfObject.Interactable);

							GameManager.Instance.Player.IsColliding(
								GameManager.Instance.CurrentMap.ParallaxLayer,
								TypeOfObject.Gate);

							if(GameManager.Instance.Player.IsColliding(
								   GameManager.Instance.CurrentMap.ParallaxLayer,
								   TypeOfObject.NPC))
							{
								dialog = new Message("scroll", false);
								TextObject message = new TextObject(mFont,
								                                    ct.Current.Message,
								                                    new Vector2(dialog.RectX + 150,
								                                                dialog.RectY + 25));
								TextList dialogOptions = new TextList(null,
								                                      new Vector2(dialog.RectX + 200,
								                                                  dialog.RectY + 75));
								dialogOptions.Font = mFont;
								foreach(string s in ct.Current.Responses)
								{
									dialogOptions.Add(s);
								}
								dialog.Options = dialogOptions;
								dialog.Mess = new TextObject(mFont,
								                             ct.Current.Message,
								                             new Vector2(dialog.RectX + 150,
								                                         dialog.RectY + 25));
								dialog = new Message(message, dialogOptions, scroll, dialog.Rect);
							}
						}

						//Player movement
						GameManager.Instance.Player.Move(Keyboard.GetState(), gameTime);

						//Layer movement
						if(GameManager.Instance.Player.MS == Player.MovementState.WalkingRight)
						{
							GameManager.Instance.CurrentMap.MoveLayers();

							if(GameManager.Instance.Player.IsColliding(
								   GameManager.Instance.CurrentMap.ParallaxLayer,
								   TypeOfObject.Solid))
							{
								GameManager.Instance.CurrentMap.MoveLayers(false);
							}
						}
						else if(GameManager.Instance.Player.MS == Player.MovementState.WalkingLeft)
						{
							GameManager.Instance.CurrentMap.MoveLayers(false);

							if(GameManager.Instance.Player.IsColliding(
								   GameManager.Instance.CurrentMap.ParallaxLayer,
								   TypeOfObject.Solid))
							{
								GameManager.Instance.CurrentMap.MoveLayers();
							}
						}
					}
					else if(paused && !conversation)
					{
						if(IsKeyPressed(kbState, oldKbState, Keys.W))
						{
							pauseMenu.Previous();
						}
						if(IsKeyPressed(kbState, oldKbState, Keys.S))
						{
							pauseMenu.Next();
						}
						if(IsKeyPressed(kbState, oldKbState, Keys.Enter))
						{
							switch(pauseMenu.Selected)
							{
							//Resume
								case 0:
									paused = false;
									break;
							//load
								case 1:
									if(Load())
									{
										MapManager.Instance.LoadMap(GameManager.Instance.CurrentMap.MapName);
										paused = false;
									}
									break;

							}
						}
					}
					else
					{

						if(IsKeyPressed(kbState, oldKbState, Keys.Escape))
						{
							if(ct.Current != ct.Root)
							{
								ct.Current = ct.Previous;
								dialog.Options.Clear();
								TextList dialogOptions = new TextList(null,
								                                      new Vector2(dialog.RectX + 200,
								                                                  dialog.RectY + 75));
								dialogOptions.Font = mFont;
								foreach(string s in ct.Current.Responses)
								{
									dialogOptions.Add(s);
								}
								dialog.Options = dialogOptions;
								dialog.Mess = new TextObject(mFont,
								                             ct.Current.Message,
								                             new Vector2(dialog.RectX + 150,
								                                         dialog.RectY + 25));
							}
							else
							{
								conversation = false;
							}

						}
						if(IsKeyPressed(kbState, oldKbState, Keys.W))
						{
							dialog.Options.Previous();
						}
						if(IsKeyPressed(kbState, oldKbState, Keys.S))
						{
							dialog.Options.Next();
						}
						if(IsKeyPressed(kbState, oldKbState, Keys.Enter))
						{
							switch(dialog.Options.Selected)
							{
								case 0:
									if(ct.Current.NextNodes.Count != 0)
									{
										ct.Previous = ct.Current;
										ct.Current = ct.Current.NextNodes[0];

									}
									else
									{
										ConversationEndTrigger();
									}

									break;
								case 1:

									if(ct.Current.NextNodes.Count != 0)
									{
										ct.Current = ct.Current.NextNodes[1];
									}
									else
									{
										ConversationEndTrigger();
									}
									break;
							}
							dialog.Options.Clear();
							TextList dialogOptions = new TextList(null,
							                                      new Vector2(dialog.RectX + 200,
							                                                  dialog.RectY + 75));
							dialogOptions.Font = mFont;
							foreach(string s in ct.Current.Responses)
							{
								dialogOptions.Add(s);
							}
							dialog.Options = dialogOptions;
							dialog.Mess = new TextObject(mFont,
							                             ct.Current.Message,
							                             new Vector2(dialog.RectX + 150,
							                                         dialog.RectY + 25));
						}


					}
					break;
                #endregion

                #region Map
				case GameManager.GameState.Map:
                    //If the state changed, change the music
                    if (stateSwitched)
                    {
                        SoundManager.Instance.PlaySong("MainTheme");
                    }

                    //overworld movement
                    //Checks the movement flags
                    if (IsKeyPressed(kbState, oldKbState, Keys.W))
                    {
                        if (CurrentMap.Tiles[(int)player.mapX, (int)player.mapY].Flags.HasFlag(MovementFlags.UP))
                        {
                            player.mapY--;
                        }
                        RandomEncounter();
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.S))
                    {
                        if (CurrentMap.Tiles[(int)player.mapX, (int)player.mapY].Flags.HasFlag(MovementFlags.DOWN))
                        {
                            player.mapY++;
                        }
                        RandomEncounter();
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.A))
                    {
                        if (CurrentMap.Tiles[(int)player.mapX, (int)player.mapY].Flags.HasFlag(MovementFlags.LEFT))
                        {
                            player.mapX--;
                        }
                        RandomEncounter();
                    }
                    else if (IsKeyPressed(kbState, oldKbState, Keys.D))
                    {
                        if (CurrentMap.Tiles[(int)player.mapX, (int)player.mapY].Flags.HasFlag(MovementFlags.RIGHT))
                        {
                            player.mapX++;
                        }
                        RandomEncounter();
                    }
                    //If the location of the player when they press enter is within the townLoactions list
                    //it will load the map at that location
                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        if (townLocations.Contains(player.MapPos))
                        {
                            if (player.MapPos == townLocations[0])
                            {
                                MapManager.Instance.LoadMap("airedale");
                            }
                            else if (player.MapPos == townLocations[1] && Progress.Instance.Flags.HasFlag(ProgressFlags.TalkedWithElder))
                            {
                                MapManager.Instance.LoadMap("tardide");
                            }
                            else if (player.MapPos == townLocations[2] && Progress.Instance.Flags.HasFlag(ProgressFlags.SecondFragment))
                            {
                                MapManager.Instance.LoadMap("kineallen");
                            }
                            else if (player.MapPos == townLocations[3] && Progress.Instance.Flags.HasFlag(ProgressFlags.ThirdFragment))
                            {
                                MapManager.Instance.LoadMap("whitebridge");
                            }
                            else if (player.MapPos == townLocations[4] && Progress.Instance.Flags.HasFlag(ProgressFlags.FourthFragment))
                            {
                                MapManager.Instance.LoadMap("ironhaven");
                            }
                            else if (player.MapPos == townLocations[5] && Progress.Instance.Flags.HasFlag(ProgressFlags.FifthFragment))
                            {
                                MapManager.Instance.LoadMap("solaris");
                            }
                            conversation = false;
                        }
                    }
                    break;
                #endregion

                #region battle
				case GameManager.GameState.Battle:
                    //If the state changed, change the music
					if(stateSwitched)
					{
						SoundManager.Instance.PlaySong("Fight2");
					}

					BattleManager.Instance.Update(gameTime);
					break;
                #endregion

                #region shop
				case GameManager.GameState.Shop:
					if(IsKeyPressed(kbState, oldKbState, Keys.W))
					{
						ShopManager.Instance.Options.Previous();
					}
					if(IsKeyPressed(kbState, oldKbState, Keys.S))
					{
						ShopManager.Instance.Options.Next();
					}
					if(IsKeyPressed(kbState, oldKbState, Keys.Enter))
					{
						switch(ShopManager.Instance.Options.Selected)
						{
							case (0):
								if(Player.SwordLevel < ShopManager.Instance.Current.Items[0].Level && Player.Gold >= ShopManager.Instance.Current.Items[0].Cost
                                    && GameManager.Instance.Player.Level >= ShopManager.Instance.Current.Items[0].Level)
								{
									GameManager.Instance.Player.Atk += ShopManager.Instance.Current.Items[0].Level*4;
									Player.SwordLevel = ShopManager.Instance.Current.Items[0].Level;
									Player.Gold -= ShopManager.Instance.Current.Items[0].Cost;
								}
								break;
							case (1):
								if(Player.ShieldLevel < ShopManager.Instance.Current.Items[1].Level && Player.Gold >= ShopManager.Instance.Current.Items[1].Cost
                                    && GameManager.Instance.Player.Level >= ShopManager.Instance.Current.Items[1].Level)
								{
									GameManager.Instance.Player.Def += ShopManager.Instance.Current.Items[1].Level*4;
									Player.ShieldLevel = ShopManager.Instance.Current.Items[1].Level;
									Player.Gold -= ShopManager.Instance.Current.Items[1].Cost;
								}

								break;
						}

					}

					if(IsKeyPressed(kbState, oldKbState, Keys.Escape))
					{
						GameManager.Instance.State = GameManager.GameState.Town;
					}
					break;
                #endregion

                #region over
				case GameManager.GameState.Over:
					if(dialog == null || dialog.Mess.Text != "You Died")
					{
						dialog = new Message("scroll", false);
						dialog.Rect = new Rectangle((int)dialog.RectX - 25, 250, dialog.Rect.Width, dialog.Rect.Height);
						TextObject overmsg = new TextObject(mFont,
						                                    "You Died",
						                                    new Vector2(dialog.RectX + 150,
						                                                dialog.RectY + 25));
						TextList overopts = new TextList(null,
						                                 new Vector2(dialog.RectX + 200,
						                                             dialog.RectY + 75));
						overopts.Font = mFont;
						overopts.Add("Return To Menu");
						overopts.Add("Load Most Recent Save");
						dialog.Options = overopts;
						dialog.Mess = new TextObject(font,
						                             "You Died",
						                             new Vector2(dialog.RectX + 150,
						                                         dialog.RectY + 25));
						dialog = new Message(overmsg, overopts, scroll, dialog.Rect);
					}
					if(IsKeyPressed(kbState, oldKbState, Keys.W))
					{
						dialog.Options.Previous();

					}
					if(IsKeyPressed(kbState, oldKbState, Keys.S))
					{
						dialog.Options.Next();
					}
					if(IsKeyPressed(kbState, oldKbState, Keys.Enter))
					{
						switch(dialog.Options.Selected)
						{
							case (0):
								GameManager.Instance.State = GameManager.GameState.Menu;
								break;
							case (1):
								if(Load())
								{
									MapManager.Instance.LoadMap(GameManager.Instance.CurrentMap.MapName);
								}
								break;
						}
					}
					break;
                    #endregion
			}

			oldKbState = kbState;
		}

		public bool IsKeyPressed(KeyboardState current, KeyboardState old, Keys k)
		{
			return (current.IsKeyDown(k) && old.IsKeyUp(k));
		}

		#endregion


		#region Draw

		//Drawing
		public void Draw(SpriteBatch spriteBatch, TextList menuOptions, Message battle, GraphicsDevice graphics)
		{
			switch(GameManager.Instance.State)
			{
				case GameManager.GameState.Menu:
					spriteBatch.Draw(menu, Vector2.Zero);
                    spriteBatch.DrawString(mFont, "Instructions: \nUse WASD to move and \nEnter to interact with doors and NPCs", new Vector2(400,550),Color.Black);
					menuOptions.DrawText(spriteBatch);
					break;

				case GameManager.GameState.Town:
					if(!paused && !conversation)
					{
						graphics.Clear(new Color(140, 100, 0));
						GameManager.Instance.CurrentMap.Draw(spriteBatch, Color.White);
						GameManager.Instance.Player.Draw(spriteBatch, Color.White);
					}
					else if(paused && !conversation)
					{
						graphics.Clear(new Color(35, 25, 0));

						GameManager.Instance.CurrentMap.Draw(spriteBatch, new Color(50, 50, 50));
						GameManager.Instance.Player.Draw(spriteBatch, new Color(50, 50, 50));

                        //Center
						spriteBatch.Draw(scroll, new Rectangle(190, 125, 450, 500), Color.White);

                        //Stats
						spriteBatch.DrawString(mFont, "Stats:", new Vector2(735, 200), Color.Gold);
						spriteBatch.DrawString(mFont, player.GetStats(), new Vector2(710, 275), Color.White);

                        //Left progress stuff
                        spriteBatch.DrawString(mFont, "Fragments:", new Vector2(30, 200), Color.Gold);
                        spriteBatch.DrawString(mFont, Progress.Instance.Fragments.ToString() + " pieces", new Vector2(40, 250), Color.White);
                        spriteBatch.DrawString(mFont, "Equipment:", new Vector2(30, 350), Color.Gold);
                        spriteBatch.DrawString(mFont, player.GetEquipment(), new Vector2(20, 400), Color.White);

                        pauseMenu.Spacing = 100;
						pauseMenu.DrawText(spriteBatch);
					}
					else
					{
						graphics.Clear(new Color(35, 25, 0));
						GameManager.Instance.CurrentMap.Draw(spriteBatch, new Color(50, 50, 50));
						GameManager.Instance.Player.Draw(spriteBatch, new Color(50, 50, 50));
						dialog.Draw(spriteBatch);

					}

					break;

				case GameManager.GameState.Map:
					graphics.Clear(Color.Brown);
					overworld = new Map();
					GameManager.Instance.CurrentMap = overworld;
					GameManager.Instance.CurrentMap.DrawOverworld(spriteBatch, Color.White);
					GameManager.Instance.Player.DrawOverworld(spriteBatch);
                    //m.Draw(spriteBatch);
                    //GameManager.Instance.CurrentMap.DrawOverworld(spriteBatch, Color.White);
					break;

				case GameManager.GameState.Battle:
					if(GameManager.Instance.CurrentMap == overworld)
                    {
                        GameManager.Instance.CurrentMap.DrawOverworld(spriteBatch, Color.DarkSlateGray);
                    }
                    else
                    {
                        GameManager.Instance.CurrentMap.Draw(spriteBatch, new Color(120, 50,50));
                    }
					
					BattleManager.Instance.Draw(spriteBatch);
					break;

				case GameManager.GameState.Shop:
					graphics.Clear(Color.Black);
					ShopManager.Instance.DrawItems(spriteBatch);

					break;
				case GameManager.GameState.Over:
					graphics.Clear(Color.Black);
					if(dialog != null)
					{
						dialog.Draw(spriteBatch);
					}
					break;
			}
		}

		#endregion

		#region Save&Load

		public void Save()
		{
			StreamWriter output = null;

			try
			{
				// Create the writer
				output = new StreamWriter("../../../save.txt");
				output.WriteLine(player.Gold);
				output.WriteLine(Player.Atk);
				output.WriteLine(Player.Def);
				output.WriteLine(Player.MaxHp);
				output.WriteLine(Player.MaxSp);
				output.WriteLine(Player.Spd);
				output.WriteLine(Player.mapX);
                output.WriteLine(Player.mapY);
                output.WriteLine(Player.SwordLevel);
                output.WriteLine(Player.ShieldLevel);
                output.WriteLine(GameManager.Instance.CurrentMap.MapName);
				output.WriteLine(Progress.Instance.ToString());
                output.WriteLine(player.Level);
			}
			catch(Exception e)
			{
				Console.WriteLine("Problem with file: " + e.Message);
			}
			finally
			{
				// Close the file as long as it's actually open
				if(output != null) output.Close();
			}
		}

		public bool Load()
		{
			StreamReader input = null;

			try
			{
				input = new StreamReader("../../../save.txt");
				// Set up some variables for the read
				player.Gold = int.Parse(input.ReadLine());
				Player.Atk = int.Parse(input.ReadLine());
				Player.Def = int.Parse(input.ReadLine());
				Player.MaxHp = int.Parse(input.ReadLine());
				Player.MaxSp = int.Parse(input.ReadLine());
				Player.Spd = int.Parse(input.ReadLine());
				Player.MapPos = new Vector2(int.Parse(input.ReadLine()), int.Parse(input.ReadLine()));
                Player.SwordLevel = int.Parse(input.ReadLine());
                Player.ShieldLevel = int.Parse(input.ReadLine());
				GameManager.Instance.CurrentMap = new Map(input.ReadLine());

				//Load progress
				string flagString = input.ReadLine();
				int flagInt;
				Int32.TryParse(flagString, out flagInt);
				Progress.Instance.Flags = (ProgressFlags)flagInt;

                string fragmentsString = input.ReadLine();
                int fragmentsInt;
                Int32.TryParse(fragmentsString, out fragmentsInt);
                Progress.Instance.Fragments = fragmentsInt;

                Player.Level = int.Parse(input.ReadLine());
                Player.Hp = Player.MaxHp;
            }
			catch(Exception e)
			{
				Console.WriteLine("Error reading file: " + e.Message);
			}
			finally
			{
				if(input != null)
				{
					input.Close();
				}

			}
			if(input != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion
	}
}
