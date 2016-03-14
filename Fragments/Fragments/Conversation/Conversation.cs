using System;
using System.Collections.Generic;
using System.IO;


namespace Fragments.Conversation
{
	public class Conversation
	{
		private Dictionary<String, ConversationState> states;
		private ConversationState state;

		public Conversation(String filename)
		{
			bool hasNextConversationState = true;
			states = new Dictionary<String, ConversationState>();
			BinaryReader r = new BinaryReader(File.OpenRead(filename));
			while(hasNextConversationState)
			{
				String name = r.ReadString();
				ConversationTrigger trigger = (ConversationTrigger)r.ReadInt32();
				String message = r.ReadString();
				List<String> options = new List<String>();
				List<String> optionText = new List<String>();
				bool hasNextConversationOption = true;
				while(hasNextConversationOption)
				{
					options.Add(r.ReadString());
					optionText.Add(r.ReadString());
					hasNextConversationState = r.ReadBoolean();
				}
				states[name] = new ConversationState(trigger, message, options, optionText);
				hasNextConversationState = r.ReadBoolean();
			}
			state = states["main"];
		}

		public ConversationState GetCurrentState()
		{
			return state;
		}

		public void SetState(String stateName)
		{
			state = states[stateName];
			state.OnTrigger();
		}

		public void MainState()
		{
			state = states["main"];
		}

		public void ExitState()
		{
			state = states["exit"];
		}
	}
}

