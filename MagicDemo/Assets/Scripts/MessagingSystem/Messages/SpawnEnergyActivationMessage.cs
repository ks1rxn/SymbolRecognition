using UnityEngine;

public class SpawnEnergyActivationMessage : IMessage {
	private Vector3 castPosition;
	private Quaternion rotation;

	public SpawnEnergyActivationMessage(Vector3 position, Quaternion rotation) {
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