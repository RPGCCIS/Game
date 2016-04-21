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
        public DialogueNodes Root
        {
            get
            {
                return rootNode;
            }
            set
            {
                rootNode = value;
            }
        }
        //Constructor
        public ConversationTree(string rootMessage, string[] responses)
        {
            
            //Make the root
            rootNode = new DialogueNodes(rootMessage);
            rootNode.Responses = responses;
            current = rootNode;
            capNodes = new Dictionary<string, DialogueNodes>();
        }

        //Methods
        //This is for adding stuff to the root node
        public void AddNode(string message, string[] responses)
        {          
            rootNode.AddPossibleNext(message,responses);
        }

        public void AddNode(string message, int[] path, string[] responses)
        {
            
            DialogueNodes current = rootNode;

            for (int i = 0; i < path.Length; i++)
            {
                current = current.NextNodes[path[i]];
            }
            current.AddPossibleNext(message,responses);
        }
        //This is for adding stuff to the root node
        public void AddCapNode(string key, string message, string[] responses)
        {
            capNodes.Add(key, rootNode.AddCapNode(message, responses));
        }

        public void AddCapNode(string key, string message, int[] path, string[] responses)
        {
            DialogueNodes current = rootNode;

            for (int i = 0; i < path.Length; i++)
            {
                current = current.NextNodes[path[i]];
            }
            capNodes.Add(key, current.AddCapNode(message, responses));
        }
    }
}
