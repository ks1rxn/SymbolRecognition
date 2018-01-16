using UnityEngine;

public class DebugListener : MonoBehaviour {
	[SerializeField]
	private GameObject spellTracker;
	[SerializeField]
	private GameObject planeTracker;


	protected void Awake() {
		Game.EventService.RegisterListener(typeof(DebugSpellTrackerMessage), OnDebugSpellTrackerMessage);
	}

	private void OnDebugSpellTrackerMessage(IMessage msg) {
		DebugSpellTrackerMessage message = (DebugSpellTrackerMessage) msg;
		GameObject prefab = null;
		switch (message.Form) {
			case DebugSpellTrackerForm.Plane:
				prefab = planeTracker;
				break;
			default:
				prefab = spellTracker;
				break;
		}
		GameObject go = Instantiate(prefab, message.TrackPosition, message.Rotation, transform);
		go.transform.localScale = new Vector3(message.Size, message.Size, message.Size);
	}

}
