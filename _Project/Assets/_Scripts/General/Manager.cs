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

    public void smoothPoint(List<Vector2> points, float smoothLength) {

        if (points.Count < smoothLength + 5) {
            return;
        }

        for (var i = 0; i < smoothLength; ++i) {

            var j = points.Count - i - 2;
            var p0 = points[j];
            var p1 = points[j + 1];
            var a = 0.7f;
            var p = new Vector2(p0.x * (1 - a) + p1.x * a, p0.y * (1 - a) + p1.y * a);

            points[j] = p;
        }
    }

    public void updateLine(LineRenderer line, List<Vector2> points) {

        line.SetVertexCount(points.Count - 1);

        for (int pointIndex = 0; pointIndex < points.Count - 1; pointIndex++) {

            line.SetPosition(pointIndex, points[pointIndex]);
        }
    }  

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
