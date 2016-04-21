using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fragments
{
    class ConversationTree
    {
        private DialogueNodes rootNode;
        private DialogueNodes current;
        private Dictionary<string, DialogueNodes> capNodes;

        public Dictionary<string,DialogueNodes> CapNodes
        {
            get { return capNodes; }
        }
        public DialogueNodes Current
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
            }
        }
        //Constructor
        public ConversationTree(string rootMessage)
        {
            //Make the root
            rootNode = new DialogueNodes(rootMessage);
            current = rootNode;
            capNodes = new Dictionary<string, DialogueNodes>();
        }

        //Methods
        //This is for adding stuff to the root node
        public void AddNode(string message)
        {
            rootNode.AddPossibleNext(message);
        }

        public void AddNode(string message, int[] path)
        {
            DialogueNodes current = rootNode;

            for (int i = 0; i < path.Length; i++)
            {
                current = current.NextNodes[path[i]];
            }

            current.AddPossibleNext(message);
        }
        //This is for adding stuff to the root node
        public void AddCapNode(string key, string message)
        {
            capNodes.Add(key, rootNode.AddCapNode(message));
        }

        public void AddCapNode(string key, string message, int[] path)
        {
            DialogueNodes current = rootNode;

            for (int i = 0; i < path.Length; i++)
            {
                current = current.NextNodes[path[i]];
            }

            capNodes.Add(key, current.AddCapNode(message));
        }
    }
}
