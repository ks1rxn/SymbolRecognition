using UnityEngine;

public class SpawnEnergyThusterMessage : IMessage {
	private Vector3 m_castPosition;
	private Quaternion m_rotation;

	public SpawnEnergyThusterMessage(Vector3 position, Quaternion rotation) {
		m_castPosition = position;
		m_rotation = rotation;
	}

	public Vector3 CastPosition {
		get {
			return m_castPosition;
		}
	}

	public Quaternion Rotation {
		get {
			return m_rotation;
		}
	}

}