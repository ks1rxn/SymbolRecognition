using UnityEngine;

public class CastGodsHandMessage : IMessage {
	private Vector3 m_castPosition;

	public CastGodsHandMessage(Vector3 position) {
		m_castPosition = position;
	}

	public Vector3 CastPosition {
		get {
			return m_castPosition;
		}
	}

}