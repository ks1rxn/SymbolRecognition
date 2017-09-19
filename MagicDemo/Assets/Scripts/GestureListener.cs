using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Hand))]
public class GestureListener : MonoBehaviour {
	
	#region Fields

	private Hand m_hand;

	public Hand MyHand {
		get {
			if (m_hand == null) {
				m_hand = GetComponent<Hand>();
			}
			return m_hand;
		}
	}

	[SerializeField]
	private GameObject m_drawingIndicator;

	#endregion

	private GestureListenerState m_state;

	protected void Awake() {
		m_drawingIndicator.SetActive(false);
		m_state = GestureListenerState.Idle;
	}

	protected void Update() {
		switch (m_state) {
			case GestureListenerState.Idle:
				if (MyHand.hoveringInteractable == null && MyHand.GetStandardInteractionButtonDown()) {
					m_drawingIndicator.SetActive(true);
					m_state = GestureListenerState.Listening;
				}
				break;
			case GestureListenerState.Listening:
				if (MyHand.GetStandardInteractionButtonUp()) {
					m_drawingIndicator.SetActive(false);
					Game.EventService.SendMessage(new CastGodsHandMessage(transform.position));
					m_state = GestureListenerState.Idle;
				}
				break;
		}
	}

	private enum GestureListenerState {
		Idle,
		Listening
	}

}
