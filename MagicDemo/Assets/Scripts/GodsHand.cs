using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class GodsHand : MonoBehaviour {

	private readonly Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ~Hand.AttachmentFlags.SnapOnAttach & ~Hand.AttachmentFlags.DetachOthers;
	private Vector3? m_lastPosition;
	private Vector3? m_currentPosition;

	private void HandHoverUpdate(Hand hand) {
		if (hand.GetStandardInteractionButtonDown()) {
			hand.HoverLock(GetComponent<Interactable>());
			hand.AttachObject(gameObject, attachmentFlags);
		} else if (hand.GetStandardInteractionButtonUp()) {
			hand.DetachObject(gameObject);
			hand.HoverUnlock(GetComponent<Interactable>());
		}
	}

	private void OnHandHoverBegin(Hand hand) {}

	private void OnHandHoverEnd(Hand hand) {}

	private void OnAttachedToHand(Hand hand) {
		m_lastPosition = null;
		m_currentPosition = null;
	}

	private void OnDetachedFromHand(Hand hand) {
		if (m_lastPosition != null && m_currentPosition != null) {
			Game.EventService.SendMessage(new SpawnGodsFireballMessage(m_currentPosition.Value, m_currentPosition.Value - m_lastPosition.Value));
		}
		Destroy(gameObject);
	}

	private void HandAttachedUpdate(Hand hand) {
		m_lastPosition = m_currentPosition;
		m_currentPosition = transform.position;
	}

	private void OnHandFocusAcquired(Hand hand) {}

	private void OnHandFocusLost(Hand hand) {}

}