using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fragments
{
    class DialogueNodes
    {
        private string message;
        private List<DialogueNodes> nextNodes;
        private bool isCapNode = false;

        public bool IsCapNode
        {
            get { return isCapNode; }
            set
            {
                isCapNode = value;
            }
        }
        //Property 
        public List<DialogueNodes> NextNodes
        {
            get { return nextNodes; }
        }

        public string Message
        {
            get { return message; }
        }

        //Constructor
        public DialogueNodes(string message)
        {
            this.message = message;
            nextNodes = new List<DialogueNodes>();
        }

        //Methods
        public void AddPossibleNext(string message)
        {
            nextNodes.Add(new DialogueNodes(message));
        }

        
        public DialogueNodes AddCapNode(string message)
        {
            DialogueNodes capNode = new DialogueNodes(message);
            capNode.IsCapNode = true;
            nextNodes.Add(capNode);
            return capNode;
        }
    }
}
