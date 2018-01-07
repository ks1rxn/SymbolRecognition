using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandThruster : MonoBehaviour {
	private Queue<Vector3> m_points;
	private Hand m_hand;

	public void Initiate(Hand hand) {
		m_hand = hand;
	}

	protected void OnEnable() {
		m_points = new Queue<Vector3>(11);
	}

	protected void OnDisable() {
		if (m_points != null) {
			int count = m_points.Count;
			Vector3 med = Vector3.zero;
			Vector3 previousPoint = m_points.Dequeue();
			while (m_points.Count > 0) {
				Vector3 currentPoint = m_points.Dequeue();
				med += currentPoint - previousPoint;
				previousPoint = currentPoint;
			}
			med /= count;
			Game.EventService.SendMessage(new SpawnGodsFireballMessage(previousPoint, med));
		}
		m_points = null;
	}

	protected void Update() {
		m_points.Enqueue(transform.position);
		if (m_points.Count > 10) {
			m_points.Dequeue();
		}
		if (m_hand.GetStandardInteractionButtonUp()) {
			gameObject.SetActive(false);
		}
	}

}
