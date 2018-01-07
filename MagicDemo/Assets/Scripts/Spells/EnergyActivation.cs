using System.Collections;
using UnityEngine;

public class EnergyActivation : MonoBehaviour {

	protected void OnEnable() {
		StartCoroutine(WaitForAnimation());
	}

	private IEnumerator WaitForAnimation() {
		yield return new WaitForSecondsRealtime(0.83f);
		Game.EventService.SendMessage(new SpawnEnergyThusterMessage(gameObject.transform.position, gameObject.transform.rotation));
		Destroy(gameObject);
	}

}