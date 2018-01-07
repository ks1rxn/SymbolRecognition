using UnityEngine;

public class SpellsHolder : MonoBehaviour {
	[SerializeField]
	private GameObject m_godsHandPrefab;
	[SerializeField]
	private GameObject m_fireballPrefab;
	[SerializeField]
	private GameObject m_enegyActivationPrefab;
	[SerializeField]
	private GameObject m_enegyThrusterPrefab;

	protected void Awake() {
		Game.EventService.RegisterListener(typeof(CastGodsHandMessage), OnCastGodsHand);
		Game.EventService.RegisterListener(typeof(SpawnGodsFireballMessage), OnSpawnGodsFireball);
		Game.EventService.RegisterListener(typeof(SpawnEnergyActivationMessage), OnCastEnergyActivation);
		Game.EventService.RegisterListener(typeof(SpawnEnergyThusterMessage), OnCastEnergyThruster);
	}

	private void OnCastGodsHand(IMessage msg) {
		CastGodsHandMessage message = (CastGodsHandMessage) msg;
		Instantiate(m_godsHandPrefab, message.CastPosition, message.Rotation, transform);
	}

	private void OnSpawnGodsFireball(IMessage msg) {
		SpawnGodsFireballMessage message = (SpawnGodsFireballMessage) msg;
		GameObject go = Instantiate(m_fireballPrefab, message.CastPosition, Quaternion.identity, transform);
		Rigidbody body = go.GetComponent<Rigidbody>();
		body.velocity = message.Speed * 200;
	}

	private void OnCastEnergyActivation(IMessage msg) {
		SpawnEnergyActivationMessage message = (SpawnEnergyActivationMessage) msg;
		Instantiate(m_enegyActivationPrefab, message.CastPosition, message.Rotation, transform);
	}

	private void OnCastEnergyThruster(IMessage msg) {
		SpawnEnergyThusterMessage message = (SpawnEnergyThusterMessage) msg;
		Instantiate(m_enegyThrusterPrefab, message.CastPosition, message.Rotation, transform);
	}

}
