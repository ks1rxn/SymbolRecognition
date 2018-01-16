using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HandThruster : MonoBehaviour {
	private Queue<Vector3> points;
	private Hand hand;

	public void Initiate(Hand hand) {
		this.hand = hand;
	}

	protected void OnEnable() {
		points = new Queue<Vector3>(11);
	}

	//todo: disable is called when game shuts down
	protected void OnDisable() {
		if (points != null) {
			int count = points.Count;
			Vector3 med = Vector3.zero;
			Vector3 previousPoint = points.Dequeue();
			while (points.Count > 0) {
				Vector3 currentPoint = points.Dequeue();
				med += currentPoint - previousPoint;
				previousPoint = currentPoint;
			}
			med /= count;
			Game.EventService.SendMessage(new SpawnGodsFireballMessage(previousPoint, med));
		}
		points = null;
	}

	protected void Update() {
		points.Enqueue(transform.position);
		if (points.Count > 10) {
			points.Dequeue();
		}
		if (hand.buttonsListener.GetStandardInteractionButtonUp()) {
			gameObject.SetActive(false);
		}
	}

}
