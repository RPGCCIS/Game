using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Fragments
{
	class BattleManager
	{
        private ContentManager content;

		private BattleState state;
		private BattleState oldState;
		private bool stateChange;
		private Timer pauseTimer;
        private Texture2D playerTexture;
        private Texture2D enemyTexture;

        private HealthBar playerHealth;
        private HealthBar EnemyHealth;

        public Texture2D PlayerTexture
        {
            set { playerTexture = value; }
        }
        public Texture2D EnemyTexture
        {
            set { enemyTexture = value; }
        }
        //instance of battle manager
        private static BattleManager instance;

		//creates the battle manager
		public static BattleManager Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new BattleManager();
				}

				return instance;
			}
		}

        public ContentManager Content
        {
            set { content = value; }
        }

		public BattleState State
		{
			get
			{
				return state;
			}
			set
			{
				state = value;
			}
		}

		//fields
		private Player p;
		private Enemy e;
		private Message title;
		private KeyboardState kbState;
		private KeyboardState oldKbState;
        private GameManager.GameState homeState;
		#region Properties

		//properties to get or set the above variables
		public Player Player
		{
			get
			{
				return p;
			}
			set
			{
				p = value;
			}
		}

		public Enemy Enemy
		{
			get
			{
				return e;
			}
			set
			{
				e = value;
			}
		}

		public Message Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		#endregion

		//Constructor
		private BattleManager()
		{
			pauseTimer = new Timer(1500); //1.5 seconds
		}

        //Initialize
        public void Initialize(EnemyType et) 
        {
            Random rand = new Random();
            homeState = GameManager.Instance.State;
            GameManager.Instance.State = GameManager.GameState.Battle;
            state = BattleState.Start;

            Enemy createdEnemy = new Enemy(et, 300, 300, 100, 100, null);
            switch (et)
            {
                case EnemyType.grunt:
                    createdEnemy.Texture = content.Load<Texture2D>("Goomba");

                    createdEnemy.Atk    = rand.Next(1, 3);
                    createdEnemy.Def    = rand.Next(0, 2);
                    createdEnemy.MaxHp  = rand.Next(3, 6);
                    createdEnemy.MaxSp  = 0;
                    createdEnemy.Spd    = 6;
                    createdEnemy.Sp     = createdEnemy.MaxSp;
                    createdEnemy.Hp     = createdEnemy.MaxHp;

                    break;

                case EnemyType.boss:
                    createdEnemy.Texture = content.Load<Texture2D>("enemy");

                    createdEnemy.Atk = rand.Next(3, 6);
                    createdEnemy.Def = rand.Next(2, 4);
                    createdEnemy.MaxHp = rand.Next(6, 9);
                    createdEnemy.MaxSp = 0;
                    createdEnemy.Spd = 9;
                    createdEnemy.Sp = createdEnemy.MaxSp;
                    createdEnemy.Hp = createdEnemy.MaxHp;
                    break;

                case EnemyType.final:
                    break;
            }
            e = createdEnemy;
            playerHealth = new HealthBar(p.Hp, p.MaxHp, 150, 200, Game1.universalFont);
            EnemyHealth = new HealthBar(e.Hp, e.MaxHp, 565, 200, Game1.universalFont);
        }

        //several parts are imcomplete, such as magic just dealing a normal amount of attack damage.
        //Eventually, there should be separate stats, magic and resistance, that will be incorporated into character, as well as different types of magic.
        public void Update(GameTime gameTime)
		{
			kbState = Keyboard.GetState();

			if(state != BattleState.Paused)
			{
				oldState = state;
			}

			switch(state)
			{
				case BattleState.Start:
                    //p.MapPos = new Vector2(12, 3);
                    e.Hp = e.MaxHp;
					state = BattleState.Player;
					break;

				case BattleState.Player:

					if(stateChange)
					{
						title.Name = "What will you do?";

						title.Dialogue.Add("Fight");
                        if(e.Type != EnemyType.boss)
                        {
                            title.Dialogue.Add("Run");
                        }
						
					}
                    
					if(IsKeyPressed(kbState, oldKbState, Keys.Enter))
					{
						if(title.Dialogue.Selected == 0)
						{
							state = BattleState.Fight;
						}
						else if(title.Dialogue.Selected == 1)
						{
							state = BattleState.Run;
						}
					}
					break;

				case BattleState.Fight:

                    //Initialization stuff

                    if (stateChange)
                    {
                        title.Name = "What will you do?";

                        title.Dialogue.Add("Attack");
                        title.Dialogue.Add("Defend");
                    }

                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        if (title.Dialogue.Selected == 0)
                        {
                            state = BattleState.Attack;
                        }
                        else if (title.Dialogue.Selected == 1)
                        {
                            state = BattleState.Defend;
                        }
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.Back))
                    {
                        state = BattleState.Player;
                    }
                    break;

                case BattleState.Attack:
                    int rawAtk = p.Atk - e.Def;
                    if (rawAtk < 0)
                    {
                        rawAtk = 0;
                    }
                    int dealtAtk = Player.Attack(e, rawAtk);

                    title.Name = "You swing your Sword! You deal " + dealtAtk + " damage!";
                    state = BattleState.Paused;
                    break;

                case BattleState.Defend:
                    title.Name = "You raise your Shield!";
                    Player.Defending = true;
                    state = BattleState.Paused;
                    break;

                case BattleState.Run:
                    title.Name = "You got away safely!";
                    state = BattleState.Paused;
                      
                    break;

                case BattleState.Enemy:
                    e.Act(title);
                    state = BattleState.Paused;

                    if(Player.Defending == true)
                    {
                        Player.Defending = false;
                    }
                    break;

                case BattleState.Win:
                    Random gen = new Random();
                    
                    title.Name = "You won!";
                    GameManager.Instance.Player.Gold += gen.Next(10, 25);
                    state = BattleState.Paused;
                    
                        break;

                case BattleState.Lose:
                    title.Name = "You Lost!";
                    state = BattleState.Paused;
                    break;

                case BattleState.Paused:
                    if (pauseTimer.Update(gameTime))
                    {
                        if (oldState == BattleState.Run || oldState == BattleState.Win)
                        {
                            //This is essentially our "end" state
                            if(e.Type == EnemyType.boss)
                            {
                                Progress.Instance.Fragments += 1;
                            }
                            GameManager.Instance.State = homeState;
                            state = BattleState.Start;
                            return;
                        }
                        else if(oldState == BattleState.Lose)
                        {
                            GameManager.Instance.State = GameManager.GameState.Over;
                            state = BattleState.Start;
                            return;
                        }
                        else if (oldState == BattleState.Enemy)
                        {
                            if (p.IsAlive())
                            {
                                state = BattleState.Player;
                            }
                            else
                            {
                                state = BattleState.Lose;
                            }
                        }
                        else if (!e.IsAlive())
                        {
                            state = BattleState.Win;
                        }
                        else
                        {
                            state = BattleState.Enemy;
                        }
                    }

                    break;             
            }

            //Input
            if (IsKeyPressed(kbState, oldKbState, Keys.W))
            {
                title.Dialogue.Previous();
            }
            else if (IsKeyPressed(kbState, oldKbState, Keys.S))
            {
                title.Dialogue.Next();
            }

            oldKbState = kbState;

            if (oldState == state)
            {
                stateChange = false;
            }
            else
            {
                stateChange = true;
            }

            //Wipe?
            if (stateChange)
            {
                Wipe(title);
            }

            playerHealth.Health = p.Hp;
            EnemyHealth.Health = e.Hp;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            title.Draw(spritebatch);
            spritebatch.Draw(playerTexture, new Rectangle(175, 250, 150, 200), Color.White);
            spritebatch.Draw(enemyTexture, new Rectangle(575, 250, 150, 200), Color.White);
            playerHealth.Draw(spritebatch, Game1.whiteSquareText);
            EnemyHealth.Draw(spritebatch, Game1.whiteSquareText);
        }

        public bool IsKeyPressed(KeyboardState current, KeyboardState old, Keys k)
        {
            return (current.IsKeyDown(k) && old.IsKeyUp(k));
        }

        public void Wipe(Message m)
        {
            m.Dialogue.Clear();
            m.Dialogue.Selected = 0;
        }
    }

	//				if(stateChange)
	//				{
	//					title.Name = "What will you do?";

	//					title.Dialogue.Add("Attack");
	//					title.Dialogue.Add("Defend");
	//				}

	//				if(IsKeyPressed(kbState, oldKbState, Keys.Enter))
	//				{
	//					if(title.Dialogue.Selected == 0)
	//					{
	//						state = BattleState.Attack;
	//					}
	//					else if(title.Dialogue.Selected == 1)
	//					{
	//						state = BattleState.Defend;
	//					}
	//				}
	//				if(IsKeyPressed(kbState, oldKbState, Keys.Back))
	//				{
	//					state = BattleState.Player;
	//				}
	//				break;

	//			case BattleState.Attack:
	//				int rawAtk = p.Atk - e.Def;
	//				int dealtAtk = Player.Attack(e, rawAtk);
	//				title.Name = "You swing your Sword! You deal " + dealtAtk + " damage!";
	//				state = BattleState.Paused;
	//				break;

	//			case BattleState.Defend:
	//				title.Name = "You raise your Shield!";
	//				Player.Defending = true;
	//				state = BattleState.Paused;
	//				break;

	//			case BattleState.Run:
	//				title.Name = "You got away safely!";
	//				state = BattleState.Paused;   
	//				break;

	//			case BattleState.Enemy:
	//				e.Act(title);
	//				state = BattleState.Paused;

	//				if(Player.Defending == true)
	//				{
	//					Player.Defending = false;
	//				}
	//				break;

	//			case BattleState.Win:
	//				title.Name = "You won!";
	//				state = BattleState.Paused;
	//				break;

	//			case BattleState.Lose:
	//				title.Name = "You Lost!";
	//				state = BattleState.Paused;
	//				break;

	//			case BattleState.Paused:
	//				if(pauseTimer.Update(gameTime))
	//				{
	//					if(oldState == BattleState.Run || oldState == BattleState.Win)
	//					{
	//						//This is essentially our "end" state
	//						GameManager.Instance.State = GameManager.GameState.Map;
	//						state = BattleState.Start;
	//						return;
	//					}
	//					else if(oldState == BattleState.Lose)
	//					{
	//						GameManager.Instance.State = GameManager.GameState.Over;
	//						state = BattleState.Start;
	//						return;
	//					}
	//					else if(oldState == BattleState.Enemy)
	//					{
	//						if(p.IsAlive())
	//						{
	//							state = BattleState.Player;
	//						}
	//						else
	//						{
	//							state = BattleState.Lose;
	//						}
	//					}
	//					else if(!e.IsAlive())
	//					{
	//						state = BattleState.Win;
	//					}
	//					else
	//					{
	//						state = BattleState.Enemy;
	//					}
	//				}

	//				break;             
	//		}

	//		//Input
	//		if(IsKeyPressed(kbState, oldKbState, Keys.W))
	//		{
	//			title.Dialogue.Previous();
	//		}
	//		else if(IsKeyPressed(kbState, oldKbState, Keys.S))
	//		{
	//			title.Dialogue.Next();
	//		}

	//		oldKbState = kbState;

	//		if(oldState == state)
	//		{
	//			stateChange = false;
	//		}
	//		else
	//		{
	//			stateChange = true;
	//		}

	//		//Wipe?
	//		if(stateChange)
	//		{
	//			Wipe(title);
	//		}
	//	}

	//	public void Draw(SpriteBatch spritebatch)
	//	{
	//		p.Draw(spritebatch);
	//		e.Draw(spritebatch);
	//		title.Draw(spritebatch);
	//	}

	//	public bool IsKeyPressed(KeyboardState current, KeyboardState old, Keys k)
	//	{
	//		return (current.IsKeyDown(k) && old.IsKeyUp(k));
	//	}

	//	public void Wipe(Message m)
	//	{
	//		m.Dialogue.Clear();
	//		m.Dialogue.Selected = 0;
	//	}
	//}

}
