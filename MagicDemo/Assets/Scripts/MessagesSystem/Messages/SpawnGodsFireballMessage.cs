using UnityEngine;

public class SpawnGodsFireballMessage : IMessage {
	private Vector3 m_castPosition;
	private Vector3 m_speed;

	public SpawnGodsFireballMessage(Vector3 position, Vector3 speed) {
		m_castPosition = position;
		m_speed = speed;
	}

	public Vector3 CastPosition {
		get {
			return m_castPosition;
		}
	}

	public Vector3 Speed {
		get {
			return m_speed;
		}
	}

}