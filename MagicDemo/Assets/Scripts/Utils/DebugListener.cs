using UnityEngine;

public class DebugListener : MonoBehaviour {
	[SerializeField]
	private GameObject m_spellTracker;
	[SerializeField]
	private GameObject m_planeTracker;


	protected void Awake() {
		Game.EventService.RegisterListener(typeof(DebugSpellTrackerMessage), OnDebugSpellTrackerMessage);
	}

	private void OnDebugSpellTrackerMessage(IMessage msg) {
		DebugSpellTrackerMessage message = (DebugSpellTrackerMessage) msg;
		GameObject prefab = null;
		switch (message.Form) {
			case DebugSpellTrackerForm.Plane:
				prefab = m_planeTracker;
				break;
			default:
				prefab = m_spellTracker;
				break;
		}
		GameObject go = Instantiate(prefab, message.TrackPosition, message.Rotation, transform);
		go.transform.localScale = new Vector3(message.Size, message.Size, message.Size);
	}

}
