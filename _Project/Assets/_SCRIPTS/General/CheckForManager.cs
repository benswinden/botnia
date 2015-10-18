/*
 * Placed on camera
 */ 

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CheckForManager : MonoBehaviour {	


	#region Unity

	void Start() {

        // Check for the manager object, Make one if not found
        if (GameObject.Find("Manager") == null) {

            Instantiate(Resources.Load("_PREFABS/Manager"));
        }
	}


	#endregion


}
