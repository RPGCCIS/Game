using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Fragments
{
	class BattleManager
	{
		private BattleState state;
		private BattleState oldState;
		private bool stateChange;
		private Timer pauseTimer;
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
			state = BattleState.Start;
			pauseTimer = new Timer(1500); //1.5 seconds
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
						title.Dialogue.Add("Run");
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
                        if (oldState == BattleState.Run || oldState == BattleState.Win || oldState == BattleState.Lose)
                        {
                            //This is essentially our "end" state
                            GameManager.Instance.State = GameManager.GameState.Map;
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
        }

        public void Draw(SpriteBatch spritebatch)
        {
                p.Draw(spritebatch);
                e.Draw(spritebatch);
                title.Draw(spritebatch);
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
