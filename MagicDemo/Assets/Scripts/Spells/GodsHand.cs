using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class GodsHand : MonoBehaviour {
	private readonly Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ~Hand.AttachmentFlags.SnapOnAttach & ~Hand.AttachmentFlags.DetachOthers;
	private Queue<Vector3> points;

	private void HandHoverUpdate(Hand hand) {}

	private void OnHandHoverBegin(Hand hand) {
		Game.EventService.SendMessage(new SpawnEnergyActivationMessage(gameObject.transform.position, gameObject.transform.rotation));
		Destroy(gameObject);
	}

	private void OnHandHoverEnd(Hand hand) {}

	private void OnAttachedToHand(Hand hand) {}

	private void OnDetachedFromHand(Hand hand) {}

	private void HandAttachedUpdate(Hand hand) {}

	private void OnHandFocusAcquired(Hand hand) {}

	private void OnHandFocusLost(Hand hand) {}

}