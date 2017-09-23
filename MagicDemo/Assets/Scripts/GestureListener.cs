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
	private ParticleSystem[] m_drawingIndicator;
	[SerializeField]
	private Animator m_handAnimator;

	#endregion

	private GestureListenerState m_state;

	protected void Awake() {
		foreach (ParticleSystem system in m_drawingIndicator) {
			system.Stop();
		}
		m_handAnimator.SetBool("IsTriggered", false);
		m_state = GestureListenerState.Idle;
	}

	protected void Update() {
		switch (m_state) {
			case GestureListenerState.Idle:
				if (MyHand.hoveringInteractable == null && MyHand.GetStandardInteractionButtonDown()) {
					foreach (ParticleSystem system in m_drawingIndicator) {
						system.Play();
					}
					m_handAnimator.SetBool("IsTriggered", true);
					m_state = GestureListenerState.Listening;
				}
				break;
			case GestureListenerState.Listening:
				if (MyHand.GetStandardInteractionButtonUp()) {
					foreach (ParticleSystem system in m_drawingIndicator) {
						system.Stop();
					}
					m_handAnimator.SetBool("IsTriggered", false);
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
