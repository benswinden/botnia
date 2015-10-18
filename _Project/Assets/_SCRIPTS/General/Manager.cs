using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Manager : MonoBehaviour {


	#region Variables

	//Public


	//Private
    
    bool sleeping;
    float sleepTimer;
    float oldTimescale;

    float prevRealTime;
    float thisRealTime;

	//Properties
    // Delta time that takes timescale into account
    public float deltaTime {
        get {
            if (Time.timeScale > 0f) return Time.deltaTime / Time.timeScale;
            return Time.realtimeSinceStartup - prevRealTime;                        // Checks realtimeSinceStartup again because it may have changed since Update was called
        }
    }

	#endregion


	#region Unity

	void Awake() {

        name = "Manager";                       // Remove "Clone" from object name
        DontDestroyOnLoad(gameObject);
        Global.manager = this;                
	}


	void Update() {

        prevRealTime = thisRealTime;
        thisRealTime = Time.realtimeSinceStartup;

        defaultControls();

        // Sleep
        if (sleeping) {
            if (sleepTimer > 0) {
                sleepTimer -= deltaTime;
            }
            if (sleepTimer <= 0) {
                sleeping = false;
                sleepTimer = 0;
                Time.timeScale = oldTimescale;
            }
        }
	}

	#endregion

    void defaultControls() {

        //Quit
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();  
        }
    }

    //Sleep
    public void Sleep(float duration) {

        if (!sleeping) {
            sleeping = true;
            sleepTimer = duration;
            oldTimescale = Time.timeScale;
            Time.timeScale = 0;
        }
    }    
}
