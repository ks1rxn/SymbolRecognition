using UnityEngine;

public class SpawnEnergyThusterMessage : IMessage {
	private Vector3 castPosition;
	private Quaternion rotation;

	public SpawnEnergyThusterMessage(Vector3 position, Quaternion rotation) {
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