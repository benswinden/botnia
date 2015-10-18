using UnityEngine;
using InControl;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Player : MonoBehaviour {
    

    public GameObject pathRendererPrefab;

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

        if (Input.GetMouseButtonDown(0) && !mouseDown) {

            mouseDown = true;

            var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

            // Instantiate line
            GameObject line = Instantiate(pathRendererPrefab, worldPoint, Quaternion.identity) as GameObject;
            currentPathRenderer = line.GetComponent<LineRenderer>();

            gestureLinePoints.Add(worldPoint);
        }
        else if (Input.GetMouseButtonUp(0) && mouseDown) {

            mouseDown = false;
            currentPathRenderer.GetComponent<GesturePathRenderer>().fade();
        }
    }

    void whileDragging() {

        var worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        if (currentPathRenderer && gestureLinePoints.Count > 0 && Vector3.Distance(gestureLinePoints[gestureLinePoints.Count - 1], worldPoint) > minDistanceBetweenPoints) {

            gestureLinePoints.Add(worldPoint);
            Global.manager.smoothPoint(gestureLinePoints, smoothLength);
            updateLine(currentPathRenderer, gestureLinePoints);
        }
    }

    void smoothPoint(List<Vector2> points, float smoothLength) {

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

    void updateLine(LineRenderer line, List<Vector2> points) {

        line.SetVertexCount(points.Count - 1);

        for (int pointIndex = 0; pointIndex < points.Count - 1; pointIndex++) {

            line.SetPosition(pointIndex, points[pointIndex]);
        }
    }    
}
