using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class EnergyThruster : MonoBehaviour {
	private readonly Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ~Hand.AttachmentFlags.SnapOnAttach & ~Hand.AttachmentFlags.DetachOthers;

	[SerializeField]
	private Animator m_animator;

	private void HandHoverUpdate(Hand hand) {
		if (hand.GetStandardInteractionButtonDown()) {
			m_animator.SetBool("TriggerPressed", true);
			m_animator.SetBool("Idle", false);
			hand.HoverLock(GetComponent<Interactable>());
			hand.AttachObject(gameObject, attachmentFlags);
			StartCoroutine(WaitAnimation(hand));
		}
	}

	private IEnumerator WaitAnimation(Hand hand) {
		yield return new WaitForSecondsRealtime(0.5f);
		hand.DetachObject(gameObject);
		hand.HoverUnlock(GetComponent<Interactable>());
		hand.gameObject.GetComponent<GestureListener>().Thruster.gameObject.SetActive(true);
		Destroy(gameObject);
	}

	private void OnHandHoverBegin(Hand hand) {}

	private void OnHandHoverEnd(Hand hand) {}

	private void OnAttachedToHand(Hand hand) {}

	private void OnDetachedFromHand(Hand hand) {}

	private void HandAttachedUpdate(Hand hand) {}

	private void OnHandFocusAcquired(Hand hand) {}

	private void OnHandFocusLost(Hand hand) {}

}