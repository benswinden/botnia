using UnityEngine;
using InControl;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {

	
    InputDevice device;


	void Update() {

        device = InputManager.ActiveDevice;

        checkInput();
    }


    public void checkInput() {

        
    }

}
