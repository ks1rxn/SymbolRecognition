using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbLight : MonoBehaviour {

	// Use this for initialization


    public bool setAmbientLight;
    public float ambientLight;


	void Update() {
		{
            RenderSettings.ambientIntensity = this.ambientLight;
        }
    }
 
}
