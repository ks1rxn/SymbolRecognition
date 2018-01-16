using UnityEngine;

public class DebugSpellTrackerMessage : IMessage {
	private DebugSpellTrackerForm form;
	private Vector3 trackPosition;
	private Quaternion rotation;
	private float size;

	public DebugSpellTrackerMessage(DebugSpellTrackerForm form, Vector3 position, Quaternion rotation, float size) {
		this.form = form;
		trackPosition = position;
		this.rotation = rotation;
		this.size = size;
	}

	public DebugSpellTrackerForm Form {
		get {
			return form;
		}
	}

	public Vector3 TrackPosition {
		get {
			return trackPosition;
		}
	}

	public Quaternion Rotation {
		get {
			return rotation;
		}
	}

	public float Size {
		get {
			return size;
		}
	}

}

public enum DebugSpellTrackerForm {
	Sphere,
	Plane
}