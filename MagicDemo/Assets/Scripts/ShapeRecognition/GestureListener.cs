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
		Vector3 center = CalculateSpellCenter();
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
			Vector3 center = CalculateSpellCenter();
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
			CorrectPointsCount();
			Vector3 center = CalculateSpellCenter();
			Vector3 head = Player.instance.headCollider.transform.position;
			Vector3 targetVector = (head - center).normalized;
			Vector3 targetUpVector = targetVector.y < 0 ? targetVector : -targetVector;
			targetUpVector.y = -(targetVector.x * targetUpVector.x + targetVector.z * targetUpVector.z) / targetVector.y;
			Vector3 targetRightVector = Vector3.Cross(targetVector, targetUpVector);
			Vector3.OrthoNormalize(ref targetVector, ref targetUpVector, ref targetRightVector);

			Vector3 row1 = new Vector3(targetRightVector.x, targetUpVector.x, targetVector.x);
			Vector3 row2 = new Vector3(targetRightVector.y, targetUpVector.y, targetVector.y);
			Vector3 row3 = new Vector3(targetRightVector.z, targetUpVector.z, targetVector.z);

			float det = row1.x * row2.y * row3.z + row2.x * row3.y * row1.z + row3.x * row1.y * row2.z - row1.x * row3.y * row2.z - row3.x * row2.y * row1.z - row2.x * row1.y * row3.z;
			Vector3 newRow1 = 1 / det * new Vector3(row3.z * row2.y - row3.y * row2.z, -(row3.z * row1.y - row3.y * row1.z), row2.z * row1.y - row2.y * row1.z);
			Vector3 newRow2 = 1 / det * new Vector3(-(row3.z * row2.x - row3.x * row2.z), row3.z * row1.x - row3.x * row1.z, -(row2.z * row1.x - row2.x * row1.z));
			Vector3 newRow3 = 1 / det * new Vector3(row3.y * row2.x - row3.x * row2.y, -(row3.y * row1.x - row3.x * row1.y), row2.y * row1.x - row2.x * row1.y);

			ProjectPointsToPlane(center, targetVector);
			for (int i = 0; i != points.Count; i++) {
				Vector3 pointVector = points[i] - center;
				Vector3 pointNew = MakeVectorInNewBasis(pointVector, newRow1, newRow2, newRow3);
				points[i] = pointNew;
			}

			SymbolAnalyzerResult result = SymbolAnalyzer.Instance.Analyze(points);
			if (result.IsPassed()) {
				switch (result.SymbolType) {
					case SymbolType.Circle:
						Game.EventService.SendMessage(new CastGodsHandMessage(center, Quaternion.LookRotation(targetVector, targetUpVector)));
						break;
					case SymbolType.S:
						break;
					case SymbolType.Line:
						break;
				}
			}

			ToIdleState();		
		}
	}

	private Vector3 MakeVectorInNewBasis(Vector3 pointVector, Vector3 newRow1, Vector3 newRow2, Vector3 newRow3) {
		Vector3 pointNew = new Vector3(pointVector.x * newRow1.x + pointVector.y * newRow1.y + pointVector.z * newRow1.z, 
					pointVector.x * newRow2.x + pointVector.y * newRow2.y + pointVector.z * newRow2.z,
					pointVector.x * newRow3.x + pointVector.y * newRow3.y + pointVector.z * newRow3.z);
		return pointNew;
	}

	private void CorrectPointsCount() {
		const int correctCount = 128;

		while (points.Count > correctCount) {
			CombineClosestPoints();
		}
		while (points.Count < correctCount) {
			AddExtraPoints();
		}
	}

	private void ProjectPointsToPlane(Vector3 sourcePoint, Vector3 normal) {
		for (int i = 0; i != points.Count; i++) { 
			Vector3 newPoint = points[i] - normal * (Vector3.Dot(points[i] - sourcePoint, normal) / Vector3.Dot(normal, normal));
			points[i] = newPoint;
		}
	}

	private void AddExtraPoints() {
		float maxD = float.MinValue;
		int maxIndex = int.MinValue;
		Vector3? point1 = null;
		Vector3? point2 = null;

		for (int i = 1; i != points.Count; i++) {
			float d = Vector3.Distance(points[i], points[i - 1]);
			if (d > maxD) {
				maxD = d;
				maxIndex = i;
				point1 = points[i];
				point2 = points[i - 1];
			}
		}

		if (point1 == null || point2 == null) {
			Debug.LogError("Too little points to add extra");
			return;
		}

		Vector3 newPoint = (point2.Value - point1.Value) / 2 + point1.Value;
		points.Insert(maxIndex, newPoint);
	}

	private void CombineClosestPoints() {
		float minD = float.MaxValue;
		int minIndex = int.MinValue;
		Vector3? point1 = null;
		Vector3? point2 = null;

		for (int i = 1; i != points.Count; i++) {
			float d = Vector3.Distance(points[i], points[i - 1]);
			if (d < minD) {
				minD = d;
				minIndex = i;
				point1 = points[i];
				point2 = points[i - 1];
			}
		}

		if (point1 == null || point2 == null) {
			Debug.LogError("Too little points to combine");
			return;
		}

		Vector3 newPoint = (point2.Value - point1.Value) / 2 + point1.Value;
		points[minIndex] = newPoint;
		points.RemoveAt(minIndex - 1);
	}

	private Vector3 CalculateSpellCenter() {
		Vector3 center = Vector3.zero;
		for (int i = 0; i != points.Count; i++) {
			center += points[i];
		}
		center /= points.Count;
		return center;
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
