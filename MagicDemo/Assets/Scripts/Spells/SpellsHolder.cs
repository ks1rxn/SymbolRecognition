using UnityEngine;

public class SpellsHolder : MonoBehaviour {
	[SerializeField]
	private GameObject godsHandPrefab;
	[SerializeField]
	private GameObject fireballPrefab;
	[SerializeField]
	private GameObject enegyActivationPrefab;
	[SerializeField]
	private GameObject enegyThrusterPrefab;

	protected void Awake() { 
		Game.EventService.RegisterListener(typeof(CastGodsHandMessage), OnCastGodsHand);
		Game.EventService.RegisterListener(typeof(SpawnGodsFireballMessage), OnSpawnGodsFireball);
		Game.EventService.RegisterListener(typeof(SpawnEnergyActivationMessage), OnCastEnergyActivation);
		Game.EventService.RegisterListener(typeof(SpawnEnergyThusterMessage), OnCastEnergyThruster);
	}

	private void OnCastGodsHand(IMessage msg) {
		CastGodsHandMessage message = (CastGodsHandMessage) msg;
		Instantiate(godsHandPrefab, message.CastPosition, message.Rotation, transform);
	}

	private void OnSpawnGodsFireball(IMessage msg) {
		SpawnGodsFireballMessage message = (SpawnGodsFireballMessage) msg;
		GameObject go = Instantiate(fireballPrefab, message.CastPosition, Quaternion.identity, transform);
		Rigidbody body = go.GetComponent<Rigidbody>();
		body.velocity = message.Speed * 200;
	}

	private void OnCastEnergyActivation(IMessage msg) {
		SpawnEnergyActivationMessage message = (SpawnEnergyActivationMessage) msg;
		Instantiate(enegyActivationPrefab, message.CastPosition, message.Rotation, transform);
	}

	private void OnCastEnergyThruster(IMessage msg) {
		SpawnEnergyThusterMessage message = (SpawnEnergyThusterMessage) msg;
		Instantiate(enegyThrusterPrefab, message.CastPosition, message.Rotation, transform);
	}

}
