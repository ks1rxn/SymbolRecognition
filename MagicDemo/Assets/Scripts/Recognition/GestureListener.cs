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
	private ParticleSystem[] m_drawingIndicator;
	[SerializeField]
	private Animator m_handAnimator;
	[SerializeField]
	private GameObject m_drawingFinger;
	[SerializeField]
	private Player m_player;
	[SerializeField]
	private HandThruster m_thruster;

	#endregion

	private GestureListenerState m_state;
	private List<Vector3> m_points; 

	protected void Awake() {
		m_thruster.Initiate(MyHand);
		ToIdleState();
	}

	protected void Update() {
		switch (m_state) {
			case GestureListenerState.Idle:
				IdleState();
				break;
			case GestureListenerState.Listening:
				ListeningState();
				break;
		}
	}

	private void ToIdleState() {
		m_state = GestureListenerState.Idle;

		foreach (ParticleSystem system in m_drawingIndicator) {
			system.Stop();
			system.Clear();
		}
		m_handAnimator.SetBool("IsTriggered", false);
	}

	private void IdleState() {
		if (MyHand.hoveringInteractable == null && MyHand.GetStandardInteractionButtonDown()) {
			ToListeningState();
		}
	}

	private void ToListeningState() {
		m_state = GestureListenerState.Listening;

		foreach (ParticleSystem system in m_drawingIndicator) {
			system.Play();
		}
		m_handAnimator.SetBool("IsTriggered", true);

		m_points = new List<Vector3>();
	}

	private void ListeningState() {
		m_points.Add(m_drawingFinger.transform.position);

		if (MyHand.GetStandardInteractionButtonUp()) {
			CorrectPointsCount();
			Vector3 center = CalculateSpellCenter();
			Vector3 head = m_player.headCollider.transform.position;
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
			for (int i = 0; i != m_points.Count; i++) {
				Vector3 pointVector = m_points[i] - center;
				Vector3 pointNew = MakeVectorInNewBasis(pointVector, newRow1, newRow2, newRow3);
				m_points[i] = pointNew;
			}

			foreach (Vector3 point in m_points) {
//				Debug.Log(point);
//				Game.EventService.SendMessage(new DebugSpellTrackerMessage(DebugSpellTrackerForm.Sphere, point, Quaternion.identity, 0.1f));
			}

			ShapeRecognizerResult result = ShapeRecognizer.Analyze(m_points);
			if (result.Value <= ShapeRecognizerResult.MinPassValue) {
				switch (result.Type) {
					case ShapeType.Circle:
						Game.EventService.SendMessage(new CastGodsHandMessage(center, Quaternion.LookRotation(targetVector, targetUpVector)));
						break;
					case ShapeType.S:
						break;
					case ShapeType.Line:
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

		while (m_points.Count > correctCount) {
			CombineClosestPoints();
		}
		while (m_points.Count < correctCount) {
			AddExtraPoints();
		}
	}

	private void ProjectPointsToPlane(Vector3 sourcePoint, Vector3 normal) {
		for (int i = 0; i != m_points.Count; i++) { 
			Vector3 newPoint = m_points[i] - normal * (Vector3.Dot(m_points[i] - sourcePoint, normal) / Vector3.Dot(normal, normal));
			m_points[i] = newPoint;
		}
	}

	private void AddExtraPoints() {
		float maxD = float.MinValue;
		int maxIndex = int.MinValue;
		Vector3? point1 = null;
		Vector3? point2 = null;

		for (int i = 1; i != m_points.Count; i++) {
			float d = Vector3.Distance(m_points[i], m_points[i - 1]);
			if (d > maxD) {
				maxD = d;
				maxIndex = i;
				point1 = m_points[i];
				point2 = m_points[i - 1];
			}
		}

		if (point1 == null || point2 == null) {
			Debug.LogError("Too little points to add extra");
			return;
		}

		Vector3 newPoint = (point2.Value - point1.Value) / 2 + point1.Value;
		m_points.Insert(maxIndex, newPoint);
	}

	private void CombineClosestPoints() {
		float minD = float.MaxValue;
		int minIndex = int.MinValue;
		Vector3? point1 = null;
		Vector3? point2 = null;

		for (int i = 1; i != m_points.Count; i++) {
			float d = Vector3.Distance(m_points[i], m_points[i - 1]);
			if (d < minD) {
				minD = d;
				minIndex = i;
				point1 = m_points[i];
				point2 = m_points[i - 1];
			}
		}

		if (point1 == null || point2 == null) {
			Debug.LogError("Too little points to combine");
			return;
		}

		Vector3 newPoint = (point2.Value - point1.Value) / 2 + point1.Value;
		m_points[minIndex] = newPoint;
		m_points.RemoveAt(minIndex - 1);
	}

	private Vector3 CalculateSpellCenter() {
		Vector3 center = Vector3.zero;
		for (int i = 0; i != m_points.Count; i++) {
			center += m_points[i];
		}
		center /= m_points.Count;
		return center;
	}

	public HandThruster Thruster {
		get {
			return m_thruster;
		}
	}

	private enum GestureListenerState {
		Idle,
		Listening
	}

}
