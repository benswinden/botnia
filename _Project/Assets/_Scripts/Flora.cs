using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Flora : MonoBehaviour {



    public float turnSpeed = 3;
    public float speed = 5;
    public float maxVelocity = 10;    

    public float minDistancePoints;
    public float distanceToMovePoint = 0.6f;      
	public bool applySmoothing = true;
    public float smoothLength = 4;    

	public int numSamples = 250;
	public float sinusoidalFreq = 25f;
	public float sinusoidalMag = .25f;
	public float sinusoidalSlip = .25f;

    public bool moving = false;

    public GameObject debugCirclePrefab;
    public bool debug;    


    LineRenderer stemPathRenderer;

    List<Vector2> stemPathPoints;                        // Points in its stem line
    List<Vector2> movementPathPoints;                    // Path it's moving along

    int currentPoint = 0;                    // Index of the point i'm currently moving to on the path

    Vector2 target;
    Rigidbody2D rigidbodyComponent;
    
    Vector3 startPosition;    



    void Awake() {

        stemPathRenderer = GetComponent<LineRenderer>();
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        stemPathPoints = new List<Vector2>();
        stemPathPoints.Add(transform.position);
    }

    void Start() {

        startPosition = transform.position;

        GetComponent<Renderer>().sortingLayerName = "Dendrite";
    }

    void FixedUpdate() {

        if (moving)
            move();
    }

    void Update() {
        
        if (Vector2.Distance(stemPathPoints[stemPathPoints.Count - 1], transform.position) > minDistancePoints) {

            
            stemPathPoints.Add(transform.position);

			if (applySmoothing)
                Global.manager.smoothPoint(stemPathPoints, smoothLength);

            Global.manager.updateLine(stemPathRenderer, stemPathPoints);
        }
    }

    void LateUpdate() {

        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
    }
    
    // Called to initialize movement
    public void startMoving(List<Vector2> pointListIn) {

        // Reset these values since otherwise the movment goes crazy        
        rigidbodyComponent.velocity = Vector3.zero;
                
        // Clean up the path list a bit to make the transition from animation to path movement smoother
        int closestPointIndex = 0;
        int firstFarAwayPoint = 0;
        if (pointListIn.Count > 1) {            

            // Throw away all points within a certain distance to the cell body      
            //for (int i = 1; i < pointListIn.Count; i++) {
            //    firstFarAwayPoint = i;
            //    if (Vector3.Distance( parentNeuron.transform.position,pointListIn[i]) > minDistanceFromNeuron ) 
            //        break;                
            //}

            pointListIn = pointListIn.GetRange(firstFarAwayPoint, pointListIn.Count - firstFarAwayPoint);

            // Find to closest point in the provided path to the axons position            
            for (int i = 1; i < pointListIn.Count; i++) {
                
                if (Vector3.Distance(transform.position, pointListIn[i]) < Vector3.Distance(transform.position, pointListIn[closestPointIndex])) {

                    closestPointIndex = i;
                }
            }
        }        

        pointListIn = pointListIn.GetRange(closestPointIndex, pointListIn.Count - closestPointIndex);


            //transform.position = startPosition;
            //transform.rotation = startRotation;

            //pointListIn.Insert(0, startPosition);

            // Clone list
            movementPathPoints = new List<Vector2>();
		/*
        foreach (Vector2 point in points) {
            path.Add(point);
        }
		*/

        // Find the closest point in the pointListIn to our current position and make that the first point to travel to


		// --------------------
		// mike's sinusoidal offset stuff:

		//Debug.Log(pointListIn.Count + " Points in, numSamples="+numSamples);

		// Resample to ensure a healthy number of samples:
		var pointList = new Vector2[numSamples];
		if (pointListIn.Count < numSamples)
		{
			pointList[0] = pointListIn[0];
			int jPrevious = 0;
			for (int i=1; i<pointListIn.Count; i++)
			{
				int j = i * (pointList.Length - 1)/(pointListIn.Count - 1);

				int destLength = j - jPrevious;
				Vector2 valueLength = pointListIn[i] - pointListIn[i-1];
				for (int k = 0; k <= destLength; k++)
					pointList[jPrevious + k] = pointListIn[i-1] + (valueLength * k)/destLength;

				jPrevious = j;
			}
		}
		
		float totalLength = (pointList[0] - pointList[pointList.Length-1]).sqrMagnitude;
		movementPathPoints.Add(pointList[0]);
		for (int pointIndex=1; pointIndex<pointList.Length-1; pointIndex++)
		{
			// Current line direction vector:
			float dir = Mathf.Atan2(pointList[pointIndex].y-pointList[pointIndex-1].y, pointList[pointIndex].x-pointList[pointIndex-1].x);
			
			// Sinusoidal offset, normalized between -1 and +1:
			float offset = Mathf.Sin( (pointList[pointIndex]-pointList[0]).sqrMagnitude / totalLength * (Mathf.PI*2f) * sinusoidalFreq );

			// Add some random forward/backward slip so the sinusoid is not so perfect looking:
			float slip = Random.Range(-sinusoidalSlip,sinusoidalSlip);

			// Add an offset to the new point in a direction of 90 degrees from the current direction:
			// Note: the size of the offset is adjustable with 'sinusoidalMag'
			Vector2 newP = pointList[pointIndex] + (Vector2)( Quaternion.Euler(0,0,(dir*Mathf.Rad2Deg)+90f) * new Vector2(offset * sinusoidalMag, slip) );

			//Debug.Log("newPoints["+(pointIndex-1)+"]=" + pointList[pointIndex-1] + " to newPoints["+pointIndex+"]="+ pointList[pointIndex]+ ": dir="+dir + ", offset="+offset+", slip="+slip+", newP="+newP);
			movementPathPoints.Add(newP);
		}
		// --------------------

        currentPoint = 0;
        target = movementPathPoints[currentPoint];
        moving = true;

        if (debug) {
            foreach (Vector2 point in movementPathPoints) {

                Instantiate(debugCirclePrefab, point, Quaternion.identity);
            }
        }
    }
    
    // Called every tick while moving
    void move() { 

        // Check distance to target, move to the next point if close enough
        if (Vector2.Distance(transform.position, target) < distanceToMovePoint && currentPoint != movementPathPoints.Count - 1) {

            currentPoint++;
            target = movementPathPoints[currentPoint];
        }
        else if (Vector2.Distance(transform.position, target) < distanceToMovePoint && currentPoint == movementPathPoints.Count - 1) {

            //dying = true;

            //deathIndicator.gameObject.SetActive(true);
            //deathIndicator.segments = deathTimer;
            //deathIndicator.init();

            moving = false;
        }

        // Find the correct rotation towards the given point
        Quaternion rotationTowardsTarget = Quaternion.AngleAxis(Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * 180 / Mathf.PI - 90, transform.forward);

        // Rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTowardsTarget, Time.deltaTime * turnSpeed);

        // Add force        
        rigidbodyComponent.AddForce(new Vector2(-transform.right.y * speed, transform.right.x * speed) * Time.deltaTime);
    }

}
