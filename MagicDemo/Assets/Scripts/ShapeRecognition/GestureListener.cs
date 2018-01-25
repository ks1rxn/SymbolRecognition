using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GestureListener : MonoBehaviour {
	
	#region Fields

	[SerializeField]
	private Hand hand;
	[SerializeField]
	private ParticleSystem[] drawingIndicator;
	[SerializeField]
	private Animator handAnimator;
	[SerializeField]
	private GameObject drawingFinger;
	[SerializeField]
	private HandThruster thruster;

	#endregion

	private GestureListenerState state;
	private List<Vector3> points; 

	protected void Awake() {
		thruster.Initiate(hand);
		ToIdleState();
	}

	protected void Update() {
		if (hand.GuessCurrentHandType() == Hand.HandType.Right) {
			Player.instance.VrDebugInTopRightCornerOfView.ShowLine(new VRDebugViewerLine(state.ToString(), DebugViewerLineColor.Green));
		} else if (hand.GuessCurrentHandType() == Hand.HandType.Left) {
			Player.instance.VrDebugInTopLeftCornerOfView.ShowLine(new VRDebugViewerLine(state.ToString(), DebugViewerLineColor.Green));
		}

		switch (state) {
			case GestureListenerState.Idle:
				IdleState();
				break;
			case GestureListenerState.Listening:
				ListeningState();
				break;
			case GestureListenerState.ListeningShot:
				ListeningShotState();
				break;
			case GestureListenerState.ListeningGesture:
				ListeningGestureState();
				break;
		}
	}

	private void ToIdleState() {
		state = GestureListenerState.Idle;
		if (points != null) {
			points.Clear();
		}

		foreach (ParticleSystem system in drawingIndicator) {
			system.Stop();
			system.Clear();
		}
		handAnimator.SetBool("IsTriggered", false);
	}

	private void IdleState() {
		if (hand.hoveringInteractable == null && hand.buttonsListener.GetStandardInteractionButtonDown()) {
			ToListeningState();
		}
	}

	private void ToListeningState() {
		state = GestureListenerState.Listening;

		points = new List<Vector3>();

		handAnimator.SetBool("IsTriggered", true);
		foreach (ParticleSystem system in drawingIndicator) {
			system.Play();
		}
	}

	private void ListeningState() {
		points.Add(drawingFinger.transform.position);

		if (points.Count > 20) {
			if (IsItGesture()) {
				ToListeningGestureState();
			} else {
				ToListeningShotState();
			}
		}
		if (hand.buttonsListener.GetStandardInteractionButtonUp()) {
			ToIdleState();
		}
	}

	private bool IsItGesture() {
		Vector3 center = SymbolsHelper.CalculateSymbolCenter(points);
		float distSum = 0;
		foreach (Vector3 point in points) {
			float dist = Vector3.Distance(point, center);
			distSum += dist;
		}
		return distSum > 0.5f;
	}

	private void ToListeningShotState() {
		state = GestureListenerState.ListeningShot;
	}

	private void ListeningShotState() {
		if (hand.buttonsListener.GetStandardInteractionButtonUp()) {
			Vector3 center = SymbolsHelper.CalculateSymbolCenter(points);
			Vector3 head = Player.instance.headCollider.transform.position;
			Vector3 targetVector = (center - head).normalized;

			//todo: to calculate speed vector use hand rotation, not center-head vector
			Game.EventService.SendMessage(new SpawnGodsFireballMessage(center, targetVector * 0.005f * points.Count));

			ToIdleState();
		}
	}

	private void ToListeningGestureState() {
		state = GestureListenerState.ListeningGesture;
	}

	private void ListeningGestureState() {
		points.Add(drawingFinger.transform.position);

		if (hand.buttonsListener.GetStandardInteractionButtonUp()) {
			CustomSymbol customSymbol = CustomSymbolFactory.CreateSymbol(points);
			switch (customSymbol.Type) {
				case SymbolType.Unrecognized:
					break;
				case SymbolType.Circle:
					Game.EventService.SendMessage(new CastGodsHandMessage(customSymbol));
					break;
				case SymbolType.S:
					break;
				case SymbolType.Line:
					break;
			}

			ToIdleState();		
		}
	}

	public HandThruster Thruster {
		get {
			return thruster;
		}
	}

	private enum GestureListenerState {
		Idle,
		Listening,
		ListeningShot,
		ListeningGesture
	}

}
