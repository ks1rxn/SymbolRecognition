using UnityEngine;

public class CastGodsHandMessage : IMessage {
	private Vector3 castPosition;
	private Quaternion rotation;

	public CastGodsHandMessage(Vector3 position, Quaternion rotation) {
		castPosition = position;
		this.rotation = rotation;
	}

	public Vector3 CastPosition {
		get {
			return castPosition;
		}
	}

	public Quaternion Rotation {
		get {
			return rotation;
		}
	}

}