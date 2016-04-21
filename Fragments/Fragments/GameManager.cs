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
            Menu,
            Town,
            Battle,
            Map,
            Pause,
            Shop
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
        private SpriteFont font;
        private SpriteFont mFont;
        private bool paused = false;
        private bool conversation = false;
        private Message dialog;
        private ConversationTree ct;
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
            set
            {
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
        public TextList PauseMenu {
            
            get { return pauseMenu; } set { pauseMenu = value; } }
        public Texture2D ScrollTexture { set { scroll = value; } }

        public SpriteFont Font { set { font = value; } }

        public SpriteFont MFont { set { mFont = value; } }
        
        public ConversationTree CT { set { ct = value; } }

        public bool Conversation { set { conversation = value; } }
        //Constructor
        private GameManager()
        {

            townLocations.Add(new Vector2(9, 12));
            townLocations.Add(new Vector2(8, 2));
            townLocations.Add(new Vector2(9, 12));
            townLocations.Add(new Vector2(9, 12));
            townLocations.Add(new Vector2(9, 12));
            
            pauseMenu = new TextList(
                null,
                Vector2.Zero);

            //Overworld
            overworld = new Map();
        }

        //Methods

        public GameState RunMenu()
        {
            return GameState.Town;
        }

        //Update and check for switches between game states
        public void Update(TextList menuOptions, TextList messageOptions, TextList battleOptions, KeyboardState kbState, KeyboardState oldKbState,GameTime gameTime,SpriteBatch sb)
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
                                overworld = new Map();
                                overworld.Load(overworld.MapName);
                                GameManager.Instance.CurrentMap = overworld;
                                GameManager.Instance.State = GameManager.GameState.Battle;
                                break;

                            //Option 2
                            //Loading the overworld map
                            case 1:
                                //Loads saved town and player information                              
                                if (Load())
                                {
                                    MapManager.Instance.LoadMap(GameManager.Instance.CurrentMap.MapName);
                                }                            
                                break;

                            //Play Game
                            case 2:
                                MapManager.Instance.LoadMap("test");
                                break;
                        }
                    }
                    break;
                    //
                    //
                    //
                    //                     TOWN
                    //
                    //
                    //
                    //
                case GameManager.GameState.Town:
                    //State changes for testing
                    if (!paused && !conversation)
                    {
                        ShopManager.Instance.Current = new Shop(GameManager.Instance.CurrentMap.MapName);
                        if (IsKeyPressed(kbState, oldKbState, Keys.A))
                        {
                            GameManager.Instance.State = GameManager.GameState.Menu;
                        }
                        else if (IsKeyPressed(kbState, oldKbState, Keys.Escape))
                        {
                            pauseMenu.Selected = 0;
                            paused = true;
                        }
                        else if (IsKeyPressed(kbState, oldKbState, Keys.C))
                        {
                            dialog = new Message("scroll", false);
                            conversation = true;
                            
                            TextObject message = new TextObject(mFont, "Message.", new Vector2(dialog.RectX + 150, dialog.RectY + 25));
                            TextList dialogOptions = new TextList(null, new Vector2(dialog.RectX + 200, dialog.RectY + 75));
                            dialogOptions.Font = mFont;
                            dialogOptions.Add("1");
                            dialogOptions.Add("2");
                            dialog = new Message(message, dialogOptions,scroll,dialog.Rect);

                        }
                        //Interactable
                        else if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                        {
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
                                TextObject message = new TextObject(mFont, ct.Current.Message, new Vector2(dialog.RectX + 150, dialog.RectY + 25));
                                TextList dialogOptions = new TextList(null, new Vector2(dialog.RectX + 200, dialog.RectY + 75));
                                dialogOptions.Font = mFont;
                                dialogOptions.Add("Sure");
                                dialogOptions.Add("Nah");
                                dialog = new Message(message, dialogOptions, scroll, dialog.Rect);
                            }
                        }

                        //Player movement
                        GameManager.Instance.Player.Move(Keyboard.GetState(), gameTime);

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
                    }
                    else if(paused && !conversation)
                    {
                        if (IsKeyPressed(kbState, oldKbState, Keys.W))
                        {
                            pauseMenu.Previous();
                        }
                        if (IsKeyPressed(kbState, oldKbState, Keys.S))
                        {
                            pauseMenu.Next();
                        }
                        if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                        {
                            switch (pauseMenu.Selected)
                            {
                                //Resume
                                case 0:
                                    //GameManager.Instance.State = GameManager.Instance.PrevState;
                                    paused = false;
                                    break;
                                //load
                                case 1:
                                    if (Load())
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
                        if (IsKeyPressed(kbState, oldKbState, Keys.Escape))
                        {
                            conversation = false;
                        }
                        if (IsKeyPressed(kbState, oldKbState, Keys.W))
                        {
                            dialog.Options.Previous();
                        }
                        if (IsKeyPressed(kbState, oldKbState, Keys.S))
                        {
                            dialog.Options.Next();
                        }
                        if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                        {
                            switch (dialog.Options.Selected)
                            {
                                case 0:
                                    if (!ct.Current.IsCapNode)
                                    {
                                        ct.Current = ct.Current.NextNodes[0];              
                                    }
                                    else
                                    {
                                        conversation = false;
                                    }
                                                                  
                                    break;
                                case 1:
                                    if (!ct.Current.IsCapNode)
                                    {
                                        ct.Current = ct.Current.NextNodes[1];
                                    }
                                    else
                                    {
                                        conversation = false;
                                    }
                                    break;
                            }
                            if (ct.Current.IsCapNode)
                            {
                                dialog.Options.Clear();
                            }
                            dialog.Mess = new TextObject(mFont, ct.Current.Message, new Vector2(dialog.RectX + 150, dialog.RectY + 25));
                        }


                    }
                    break;

                case GameManager.GameState.Map:
                    if (IsKeyPressed(kbState, oldKbState, Keys.T))
                    {
                        GameManager.Instance.State = GameManager.GameState.Menu;
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.G))
                    {
                        GameManager.Instance.State = GameManager.GameState.Pause;
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.Z))
                    {
                        GameManager.Instance.State = GameManager.GameState.Battle;
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
                            }else if(player.MapPos == townLocations[1])
                            {
                                MapManager.Instance.LoadMap("test1");
                            }
                            
                        }
                    }
                    break;

                case GameManager.GameState.Battle:
                    //BattleManager.Instance.Player = player;
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
                                if (battleOptions.Options[0].Text.Equals("Fight"))
                                {
                                    battleOptions.Clear();
                                    battleOptions.Add("Attack");
                                    battleOptions.Add("Magic");
                                    battleOptions.Add("Defend");
                                }
                                else if (battleOptions.Options[0].Text.Equals("Attack"))
                                {
                                    battleOptions.Clear();
                                    battleOptions.Add("You swung your sword!");
                                    //battle animation
                                    battleOptions.Clear();
                                    battleOptions.Add("Fight");
                                    battleOptions.Add("Run");
                                }
                                break;

                            //Option 2
                            case 1:
                                if (battleOptions.Options[1].Text.Equals("Run"))
                                {
                                    battleOptions.Clear();
                                    battleOptions.Add("You managed to escape!(Press Esc to return to the overworld)");
                                }
                                else if (battleOptions.Options[1].Text.Equals("Magic"))
                                {
                                    battleOptions.Clear();
                                    battleOptions.Add("You used Magic!");
                                    //battle animation
                                    battleOptions.Clear();
                                    battleOptions.Add("Fight");
                                    battleOptions.Add("Run");
                                }
                                break;
                            case 2:
                                battleOptions.Clear();
                                battleOptions.Add("Fight");
                                battleOptions.Add("Run");
                                break;
                        }
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.Escape))
                    {
                        battleOptions.Clear();
                        battleOptions.Add("Fight");
                        battleOptions.Add("Run");
                        overworld = new Map();
                        GameManager.Instance.currentMap = overworld;
                        GameManager.Instance.State = GameManager.GameState.Map;
                    }
                    break;
                case GameManager.GameState.Pause:
                    if (IsKeyPressed(kbState, oldKbState, Keys.W))
                    {
                        pauseMenu.Previous();
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.S))
                    {
                        pauseMenu.Next();
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        switch (pauseMenu.Selected)
                        {
                            //Resume
                            case 0:
                               GameManager.Instance.State = GameManager.Instance.PrevState;                       
                            break;
                            //load
                            case 1:
                                if (Load())
                                {
                                    MapManager.Instance.LoadMap(GameManager.Instance.CurrentMap.MapName);
                                }
                                break;

                        }
                    }
                    break;
                case GameManager.GameState.Shop:
                    if (IsKeyPressed(kbState, oldKbState, Keys.W))
                    {
                        ShopManager.Instance.Options.Previous();
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.S))
                    {
                        ShopManager.Instance.Options.Next();
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.Escape))
                    {
                        GameManager.Instance.State = GameManager.GameState.Town;
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
        public void Draw(SpriteBatch spriteBatch, TextList menuOptions, Message battle, GraphicsDevice graphics, Message convo)
        {
            switch (GameManager.Instance.State)
            {
                case GameManager.GameState.Menu:
                    graphics.Clear(Color.White);
                    menuOptions.DrawText(spriteBatch);
                    break;

                case GameManager.GameState.Town:
                    if (!paused && !conversation)
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
                        spriteBatch.Draw(scroll, new Rectangle(190, 125, 450, 500), Color.White);
                        
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
                    GameManager.Instance.CurrentMap = overworld;
                    GameManager.Instance.CurrentMap.DrawOverworld(spriteBatch, Color.DarkSlateGray);
                    battle.Draw(spriteBatch);
                    break;
                case GameManager.GameState.Shop:
                    graphics.Clear(Color.Black);
                    ShopManager.Instance.DrawItems(spriteBatch);
                    break;
            }
        }
        public void Save()
        {
            StreamWriter output = null;

            try
            {
                // Create the writer
                output = new StreamWriter("../../../save.txt");
                output.WriteLine(Player.Atk);
                output.WriteLine(Player.Def);
                output.WriteLine(Player.MaxHp);
                output.WriteLine(Player.MaxSp);
                output.WriteLine(Player.Spd);
                output.WriteLine(Player.mapX);
                output.WriteLine(Player.mapY);
                output.WriteLine(GameManager.Instance.CurrentMap.MapName);
                

            }
            catch (Exception e)
            {
                Console.WriteLine("Problem with file: " + e.Message);
            }
            finally
            {
                // Close the file as long as it's actually open
                if (output != null)
                    output.Close();
            }
        }
        public bool Load()
        {
            StreamReader input = null;

            try
            {
                input = new StreamReader("../../../save.txt");
                // Set up some variables for the read
                Player.Atk = int.Parse(input.ReadLine());
                Player.Def = int.Parse(input.ReadLine());
                Player.MaxHp = int.Parse(input.ReadLine());
                Player.MaxSp = int.Parse(input.ReadLine());
                Player.Spd = int.Parse(input.ReadLine());
                Player.MapPos = new Vector2(int.Parse(input.ReadLine()), int.Parse(input.ReadLine()));
                GameManager.Instance.CurrentMap = new Map(input.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading file: " + e.Message);
            }
            finally
            {
                if (input != null)
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
    }
}
