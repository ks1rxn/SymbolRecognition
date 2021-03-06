﻿using System;
using System.Collections.Generic;

public class EventService {
	private IDictionary<Type, IList<Action<IMessage>>> listeners;

	public EventService() {
		listeners = new Dictionary<Type, IList<Action<IMessage>>>();
	}

	public void SendMessage(IMessage message) {
		IList<Action<IMessage>> listeners;
		if (this.listeners.TryGetValue(message.GetType(), out listeners)) {
			foreach (Action<IMessage> listener in listeners) {
				listener(message);
			}
		}
	}

	public void RegisterListener(Type messageType, Action<IMessage> action) {
		IList<Action<IMessage>> listeners;
		if (this.listeners.TryGetValue(messageType, out listeners)) {
			listeners.Add(action);
			return;
		}
		listeners = new List<Action<IMessage>>();
		listeners.Add(action);
		this.listeners.Add(messageType, listeners);
	}

	public void UnregisterListener(Action<IMessage> action) {
		foreach (KeyValuePair<Type, IList<Action<IMessage>>> pair in listeners) {
			foreach (Action<IMessage> actionInList in pair.Value) {
				if (actionInList == action) {
					pair.Value.Remove(actionInList);
					if (pair.Value.Count == 0) {
						listeners.Remove(pair);
					}
					return;
				}
			}
		}
	}

}