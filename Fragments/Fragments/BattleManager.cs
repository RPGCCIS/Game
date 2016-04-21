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
        private BattleState state = BattleState.Start;
        private BattleState oldstate;

        //instance of battle manager
        private static BattleManager instance;

        //creates the battle manager
        public static BattleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BattleManager();
                }
                return instance;
            }
        }

        //fields
        private Player p;
        private Enemy e;
        private Message title;
        private KeyboardState kbState;
        private KeyboardState oldKbState;

        //properties to get or set the above variables
        public Player Player
        {
            get { return p; }
            set { p = value; }
        }

        public Enemy Enemy
        {
            get { return e; }
            set { e = value; }
        }

        public Message Title
        {
            get { return title; }
            set { title = value; }
        }

        //several parts are imcomplete, such as magic just dealing a normal amount of attack damage. 
        //Eventually, there should be separate stats, magic and resistance, that will be incorporated into character, as well as different types of magic.
        public void Update()
        {
            kbState = Keyboard.GetState();
            oldstate = state;
            switch (state)
            {
                case BattleState.Start:
                    p.MapPos = new Vector2(12,3);
                    int atk;
                    state = BattleState.Player;
                    break;
                case BattleState.Player:
                    title.Name = "What will you do?";
                    title.Dialogue.Clear();
                    title.Dialogue.Add("Fight");
                    title.Dialogue.Add("Run");
                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        if (title.Dialogue.Selected == 0)
                        {
                            state = BattleState.Fight;
                            Wipe(title);
                        }
                        else if (title.Dialogue.Selected == 1)
                        {
                            state = BattleState.Run;
                            Wipe(title);
                        }
                    }
                    break;
                case BattleState.Fight:
                    title.Name = "What will you do?";
                    title.Dialogue.Clear();
                    title.Dialogue.Add("Attack");
                    title.Dialogue.Add("Magic");
                    title.Dialogue.Add("Defend");
                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        if (title.Dialogue.Selected == 0)
                        {
                            state = BattleState.Attack;
                            Wipe(title);
                        }
                        else if (title.Dialogue.Selected == 1)
                        {
                            state = BattleState.Magic;
                            Wipe(title);
                        }
                        else if (title.Dialogue.Selected == 2)
                        {
                            state = BattleState.Defend;
                            Wipe(title);
                        }
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.Back))
                    {
                        state = BattleState.Player;
                    }
                    break;
                case BattleState.Attack:
                    atk = p.Atk - e.Def;
                    title.Name = "You swing your Sword! You deal " + atk + " damage!";
                    Player.Attack(e);
                    state = BattleState.Enemy;
                    break;
                case BattleState.Magic:
                    atk = p.Atk - e.Def;
                    title.Name = "You cast Magic! You deal " + atk + " damage!";
                    Player.Magic(e);
                    state = BattleState.Enemy;
                    break;
                case BattleState.Defend:
                    title.Name = "You raise your Shield!";
                    Player.Defending = true;
                    state = BattleState.Enemy;
                    break;
                case BattleState.Run:
                    title.Name = "You got away safely!";
                    state = BattleState.Start;   
                    break;
                case BattleState.Enemy:
                    Enemy.Act(title, state);
                    if(Player.Defending == true)
                    {
                        Player.Defending = false;
                    }
                    break;
                case BattleState.Win:
                    title.Name = "You won!";
                    state = BattleState.Start;
                        break;
                case BattleState.Lose:
                    title.Name = "You Lost!";
                    state = BattleState.Start;
                    break;
            }
            if (IsKeyPressed(kbState, oldKbState, Keys.W))
            {
                title.Dialogue.Previous();
            }
            if (IsKeyPressed(kbState, oldKbState, Keys.S))
            {
                title.Dialogue.Next();
            }
            oldKbState = kbState;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            p.Draw(spritebatch);
            e.Draw(spritebatch);
            title.Draw(spritebatch);
            if (oldstate == BattleState.Attack || oldstate == BattleState.Magic || oldstate == BattleState.Defend || oldstate == BattleState.Run || oldstate == BattleState.Enemy || oldstate == BattleState.Win)
            {
                GameManager.Instance.TimePause(5);
                if(oldstate == BattleState.Run)
                {
                    GameManager.Instance.State = GameManager.GameState.Map;
                }
                Wipe(title);
            }
            if(oldstate == BattleState.Start)
            {
                GameManager.Instance.State = GameManager.GameState.Map;
            }
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
}
