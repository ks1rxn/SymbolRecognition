using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class GodsHand : MonoBehaviour {
	private readonly Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & ~Hand.AttachmentFlags.SnapOnAttach & ~Hand.AttachmentFlags.DetachOthers;
	private Queue<Vector3> m_points;

	private void HandHoverUpdate(Hand hand) {
//		if (hand.GetStandardInteractionButtonDown()) {
//			hand.HoverLock(GetComponent<Interactable>());
//			hand.AttachObject(gameObject, attachmentFlags);
//		} else if (hand.GetStandardInteractionButtonUp()) {
//			hand.DetachObject(gameObject);
//			hand.HoverUnlock(GetComponent<Interactable>());
//		}
	}

	private void OnHandHoverBegin(Hand hand) {
		Game.EventService.SendMessage(new SpawnEnergyActivationMessage(gameObject.transform.position, gameObject.transform.rotation));
		Destroy(gameObject);
	}

	private void OnHandHoverEnd(Hand hand) {}

	private void OnAttachedToHand(Hand hand) {
//		m_points = new Queue<Vector3>(11);
	}

	private void OnDetachedFromHand(Hand hand) {
//		if (m_points != null) {
//			int count = m_points.Count;
//			Vector3 med = Vector3.zero;
//			Vector3 previousPoint = m_points.Dequeue();
//			while (m_points.Count > 0) {
//				Vector3 currentPoint = m_points.Dequeue();
//				med += currentPoint - previousPoint;
//				previousPoint = currentPoint;
//			}
//			med /= count;
//			Game.EventService.SendMessage(new SpawnGodsFireballMessage(previousPoint, med));
//		}
//		m_points = null;
//		Destroy(gameObject);
	}

	private void HandAttachedUpdate(Hand hand) {
//		m_points.Enqueue(transform.position);
//		if (m_points.Count > 10) {
//			m_points.Dequeue();
//		}
	}

	private void OnHandFocusAcquired(Hand hand) {}

	private void OnHandFocusLost(Hand hand) {}

}