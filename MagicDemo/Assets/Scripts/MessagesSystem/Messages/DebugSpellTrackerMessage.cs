using UnityEngine;

public class DebugSpellTrackerMessage : IMessage {
	private DebugSpellTrackerForm m_form;
	private Vector3 m_trackPosition;
	private Quaternion m_rotation;
	private float m_size;

	public DebugSpellTrackerMessage(DebugSpellTrackerForm form, Vector3 position, Quaternion rotation, float size) {
		m_form = form;
		m_trackPosition = position;
		m_rotation = rotation;
		m_size = size;
	}

	public DebugSpellTrackerForm Form {
		get {
			return m_form;
		}
	}

	public Vector3 TrackPosition {
		get {
			return m_trackPosition;
		}
	}

	public Quaternion Rotation {
		get {
			return m_rotation;
		}
	}

	public float Size {
		get {
			return m_size;
		}
	}

}

public enum DebugSpellTrackerForm {
	Sphere,
	Plane
}