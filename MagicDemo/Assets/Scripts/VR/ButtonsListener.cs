using UnityEngine;

namespace Valve.VR.InteractionSystem {

	public class ButtonsListener : MonoBehaviour {
		[SerializeField]
		private Hand hand;

		#region Trigger

		public bool GetStandardInteractionButtonDown() {
			if (hand.noSteamVRFallbackCamera) {
				return Input.GetMouseButtonDown(0);
			}
			if (hand.controller != null) {
				return hand.controller.GetHairTriggerDown();
			}
			return false;
		}

		public bool GetStandardInteractionButtonUp() {
			if (hand.noSteamVRFallbackCamera) {
				return Input.GetMouseButtonUp(0);
			}
			if (hand.controller != null) {
				return hand.controller.GetHairTriggerUp();
			}
			return false;
		}

		public bool GetStandardInteractionButton() {
			if (hand.noSteamVRFallbackCamera) {
				return Input.GetMouseButton(0);
			}
			if (hand.controller != null) {
				return hand.controller.GetHairTrigger();
			}
			return false;
		}

		#endregion

		#region Grip

		public bool GetGripButtonDown() {
			if (hand.noSteamVRFallbackCamera) {
				return Input.GetMouseButtonDown(0);
			}
			if (hand.controller != null) {
				return hand.controller.GetPressDown(EVRButtonId.k_EButton_Grip);
			}
			return false;
		}

		public bool GetGripButtonUp() {
			if (hand.noSteamVRFallbackCamera) {
				return Input.GetMouseButtonUp(0);
			}
			if (hand.controller != null) {
				return hand.controller.GetPressUp(EVRButtonId.k_EButton_Grip);
			}
			return false;
		}

		public bool GetGripButton() {
			if (hand.noSteamVRFallbackCamera) {
				return Input.GetMouseButton(0);
			}
			if (hand.controller != null) {
				return hand.controller.GetPress(EVRButtonId.k_EButton_Grip);
			}
			return false;
		}

		#endregion

	}

}