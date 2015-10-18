using UnityEngine;
using InControl;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {
    

    public GameObject pathRendererPrefab;
    public GameObject floraPrefab;

    public float minDistanceBetweenPoints;
    public float smoothLength;

    bool mouseDown;
    List<Vector2> gestureLinePoints = new List<Vector2>();

    LineRenderer currentPathRenderer;

    // Controller    
    public PlayerActions playerActions { get; set; }
    float controllerDeadzone = 0.1f;


    void Start() {

        playerActions = PlayerActions.CreateWithDefaultBindings();
    }

	void Update() {

        checkInput();

        if (mouseDown)
            whileDragging();
    }


    public void checkInput() {

        if (Input.GetMouseButton(0) && !mouseDown) {

            mouseDown = true;

            var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            // Instantiate line
            GameObject line = Instantiate(pathRendererPrefab, worldPoint, Quaternion.identity) as GameObject;
            currentPathRenderer = line.GetComponent<LineRenderer>();

            gestureLinePoints.Add(worldPoint);
        }
        else if (!Input.GetMouseButton(0) && mouseDown) {

            mouseDown = false;

            if (gestureLinePoints.Count > 5)
                placeFlora();

            currentPathRenderer.GetComponent<GesturePathRenderer>().fade();
            gestureLinePoints.Clear();
        }
    }

    void whileDragging() {

        var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

        if (currentPathRenderer && gestureLinePoints.Count > 0 && Vector3.Distance(gestureLinePoints[gestureLinePoints.Count - 1], worldPoint) > minDistanceBetweenPoints) {

            gestureLinePoints.Add(worldPoint);
            Global.manager.smoothPoint(gestureLinePoints, smoothLength);
            Global.manager.updateLine(currentPathRenderer, gestureLinePoints);
        }
    }

    void placeFlora() {

        GameObject flora = Instantiate(floraPrefab, gestureLinePoints[0], Quaternion.identity) as GameObject;
        flora.GetComponent<Flora>().startMoving(gestureLinePoints);
    }
}
