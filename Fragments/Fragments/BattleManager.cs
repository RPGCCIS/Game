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
        //enum for every battle situation
        private enum State
        {
            Start,
            Player,
            Fight,
            Attack,
            Magic,
            Defend,
            Run,
            Enemy,
            Win,
        }

        private State state;
        private State oldstate;

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
        Player p;
        Enemy e;
        Message title;
        KeyboardState kbState;
        KeyboardState oldKbState;

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

        public void Update()
        {
            kbState = Keyboard.GetState();
            oldstate = state;
            switch (state)
            {
                case State.Start:
                    //p.MapPos = new Vector2(,);
                    //e = new Enemy(EnemyType.grunt, );
                    break;
                case State.Player:
                    title.Name = "What will you do?";
                    title.Dialogue.Clear();
                    title.Dialogue.Add("Fight");
                    title.Dialogue.Add("Run");
                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        if (title.Dialogue.Selected == 0)
                        {
                            state = State.Fight;
                        }
                        else if (title.Dialogue.Selected == 1)
                        {
                            state = State.Run;
                        }
                    }
                    break;
                case State.Fight:
                    title.Name = "What will you do?";
                    title.Dialogue.Clear();
                    title.Dialogue.Add("Attack");
                    title.Dialogue.Add("Magic");
                    title.Dialogue.Add("Defend");
                    if (IsKeyPressed(kbState, oldKbState, Keys.Enter))
                    {
                        if (title.Dialogue.Selected == 0)
                        {
                            state = State.Attack;
                        }
                        else if (title.Dialogue.Selected == 1)
                        {
                            state = State.Magic;
                        }
                        else if (title.Dialogue.Selected == 2)
                        {
                            state = State.Defend;
                        }
                    }
                    if (IsKeyPressed(kbState, oldKbState, Keys.Back))
                    {
                        state = State.Player;
                    }
                    break;
                case State.Attack:
                    title.Name = "You swing your Sword!";
                    title.Dialogue.Clear();
                    Player.Attack(e);
                    break;
                case State.Magic:
                    title.Name = "You cast Magic!";
                    title.Dialogue.Clear();
                    //Player.Magic();
                    break;
                case State.Defend:
                    title.Name = "You raise your Shield!";
                    title.Dialogue.Clear();
                    //Player.Defend();
                    break;
                case State.Run:
                    title.Name = "You got away safely!";
                    title.Dialogue.Clear();
                    break;
                case State.Enemy:
                    Enemy.Act(title);
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
            if (oldstate == State.Attack || oldstate == State.Magic || oldstate == State.Defend || oldstate == State.Run || oldstate == State.Enemy)
            {
                GameManager.Instance.TimePause(5);
            }
        }

        public bool IsKeyPressed(KeyboardState current, KeyboardState old, Keys k)
        {
            return (current.IsKeyDown(k) && old.IsKeyUp(k));
        }
    }
}
