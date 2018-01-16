using UnityEngine;

public class SpawnGodsFireballMessage : IMessage {
	private Vector3 castPosition;
	private Vector3 speed;

	public SpawnGodsFireballMessage(Vector3 position, Vector3 speed) {
		castPosition = position;
		this.speed = speed;
	}

	public Vector3 CastPosition {
		get {
			return castPosition;
		}
	}

	public Vector3 Speed {
		get {
			return speed;
		}
	}

}