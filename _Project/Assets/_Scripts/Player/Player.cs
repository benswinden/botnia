using UnityEngine;
using InControl;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {


    // Controller    
    public PlayerActions playerActions { get; set; }
    float controllerDeadzone = 0.1f;


    void Start() {

        playerActions = PlayerActions.CreateWithDefaultBindings();
    }

	void Update() {

        checkInput();
    }


    public void checkInput() {

        
    }

}
