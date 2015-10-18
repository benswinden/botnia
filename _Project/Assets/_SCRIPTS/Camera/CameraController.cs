using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float shakeAmt = 15F;

    public bool followPlayer = true;
    public GameObject snapFollowTarget;
    public GameObject smoothFollowTarget;
    public float dampTime = 0.15F;   


    private bool timeShake = false;
    private bool shake;

    private Vector3 pos;    
         
    private Vector3 velocity = Vector3.zero;

    void Awake() {

        Global.cameraController = this;
    }

    // Update is called once per frame
    void Update() {
        

        if (snapFollowTarget != null) {
            transform.position = new Vector3(snapFollowTarget.transform.position.x, snapFollowTarget.transform.position.y, transform.position.z);
        }
        else if (smoothFollowTarget && followPlayer) {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(smoothFollowTarget.transform.position);
            Vector3 delta = smoothFollowTarget.transform.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }


        if (shake) {
            // Generate either 1 or -1 twice
            int num1 = 0; if (Random.value < 0.5f) num1 = 1; else num1 = -1;
            int num2 = 0; if (Random.value < 0.5f) num2 = 1; else num2 = -1;

            transform.position = new Vector3(transform.position.x + shakeAmt * num1, transform.position.y + shakeAmt * num2, transform.position.z);

            if (!timeShake)
                shake = false;
        }

        if (transform.position.z != -40)
            transform.position = new Vector3(transform.position.x, transform.position.y, -40);

    }
    
    // Shake once
    public void screenShake(float shakeAmount) {

        shakeAmt = shakeAmount;
        shake = true;
    }
    // Shake for a period of time
    public void screenShake(float shakeAmount, float time) {

        shakeAmt = shakeAmount;
        shake = true;
        timeShake = true;

        StartCoroutine(timeShakeRoutine(time));
    }

    IEnumerator timeShakeRoutine(float time) {

        yield return new WaitForSeconds(time);

        timeShake = false;
        shake = false;
    }
}