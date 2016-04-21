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
        private string[] responses = new String[2];
        public string[] Responses
        {
            get { return responses; }
            set { responses = value; }
        }
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
        public void AddPossibleNext(string message,String[] responses)
        {
            nextNodes.Add(new DialogueNodes(message));
            nextNodes[nextNodes.Count - 1].Responses = responses;
        }

        
        public DialogueNodes AddCapNode(string message, String[] responses)
        {
            DialogueNodes capNode = new DialogueNodes(message);
            capNode.IsCapNode = true;
            capNode.Responses = responses;
            nextNodes.Add(capNode);
            return capNode;
        }
    }
}
