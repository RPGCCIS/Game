using System;
using System.Collections.Generic;

namespace Fragments.Conversation
{
	public class ConversationState
	{
		public delegate void Trigger(ConversationTrigger trigger);

		event Trigger DoTrigger;

		public string message;
		public ConversationTrigger trigger;
		public List<String> options;
		public List<String> optionText;

		public string Message
		{
			get
			{
				return message;
			}
		}

		public List<String> Options
		{
			get
			{
				return options;
			}
		}

		public List<String> OptionText
		{
			get
			{
				return optionText;
			}
		}

		public ConversationState(ConversationTrigger trigger, String message, List<String> options, List<String> optionText)
		{
			this.trigger = trigger;
			this.message = message;
			this.options = options;
			this.optionText = optionText;
		}

		public void OnTrigger()
		{
			if(DoTrigger != null) DoTrigger(trigger);
		}
	}
}

