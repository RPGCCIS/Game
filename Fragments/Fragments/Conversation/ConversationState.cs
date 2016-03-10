using System;
using System.Collections.Generic;

namespace Fragments.Conversation
{
	public class ConversationState
	{
		public delegate void Trigger(ConversationTrigger trigger);

		public event Trigger OnTrigger;

		string message;
		ConversationTrigger trigger;
		List<String> options;
		List<String> optionText;

		public ConversationState(ConversationTrigger trigger, String message, List<String> options, List<String> optionText)
		{
			this.trigger = trigger;
			this.message = message;
			this.options = options;
			this.optionText = optionText;
		}
	}
}

