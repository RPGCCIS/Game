using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Pause
        }

        //Member Variables
        private static GameManager instance;

        private GameState gameState;
        private GameState prevState;
        private Player player;

        private Dictionary<string, bool> keyEvents;

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
            set {
                prevState = gameState;
                gameState = value;
            }
        }

        public GameState PrevState
        {
            get { return prevState; }
        }

        //Constructor
        private GameManager()
        {
            //EMPTY
        }

        //Method
        public GameState RunMenu()
        {
            return GameState.Town;
        }

        //Drawing
    }
}
