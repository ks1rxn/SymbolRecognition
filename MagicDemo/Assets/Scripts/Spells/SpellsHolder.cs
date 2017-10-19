using UnityEngine;

public class SpellsHolder : MonoBehaviour {
	[SerializeField]
	private GameObject m_godsHandPrefab;
	[SerializeField]
	private GameObject m_fireballPrefab;

	protected void Awake() {
		Game.EventService.RegisterListener(typeof(CastGodsHandMessage), OnCastGodsHand);
		Game.EventService.RegisterListener(typeof(SpawnGodsFireballMessage), OnSpawnGodsFireball);
	}

	private void OnCastGodsHand(IMessage msg) {
		CastGodsHandMessage message = (CastGodsHandMessage) msg;
		Instantiate(m_godsHandPrefab, message.CastPosition, Quaternion.identity, transform);
	}

	private void OnSpawnGodsFireball(IMessage msg) {
		SpawnGodsFireballMessage message = (SpawnGodsFireballMessage) msg;
		GameObject go = Instantiate(m_fireballPrefab, message.CastPosition, Quaternion.identity, transform);
		Rigidbody body = go.GetComponent<Rigidbody>();
		body.velocity = message.Speed * 200;
	}

}
